using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DlxLib;
using FlowFreeSolverWpf.Model;

namespace FlowFreeSolverWpf.ViewModel
{
    public class PuzzleSolver
    {
        private readonly Grid _grid;
        private readonly GridDescription _gridDescription;
        private readonly Action<SolutionStats, IEnumerable<Tuple<ColourPair, Path>>> _solutionFoundHandler;
        private readonly Action<SolutionStats> _noSolutionFoundHandler;
        private readonly Action<SolutionStats> _cancelledHandler;
        private readonly Action<SolutionStats> _updateSolutionStatsHandler;
        private readonly IDispatcher _dispatcher;
        private readonly CancellationToken _cancellationToken;
        private readonly MatrixBuilder _matrixBuilder = new MatrixBuilder();
        private readonly Dlx _dlx = new Dlx();

        public PuzzleSolver(
            Grid grid,
            GridDescription gridDescription,
            Action<SolutionStats,
            IEnumerable<Tuple<ColourPair, Path>>> solutionFoundHandler,
            Action<SolutionStats> noSolutionFoundHandler,
            Action<SolutionStats> cancelledHandler,
            Action<SolutionStats> updateSolutionStatsHandler,
            IDispatcher dispatcher,
            CancellationToken cancellationToken)
        {
            _grid = grid;
            _gridDescription = gridDescription;
            _solutionFoundHandler = solutionFoundHandler;
            _noSolutionFoundHandler = noSolutionFoundHandler;
            _cancelledHandler = cancelledHandler;
            _updateSolutionStatsHandler = updateSolutionStatsHandler;
            _dispatcher = dispatcher;
            _cancellationToken = cancellationToken;
        }

        public void SolvePuzzle()
        {
            Task.Factory.StartNew(SolvePuzzleInBackground);
        }

        private void SolvePuzzleInBackground()
        {
            var matrix = new bool[0,0];
            TimeSpan? matrixBuildingDuration = null;
            TimeSpan? matrixBuildingSolving = null;
            var solutions = new List<Solution>();

            _dlx.SolutionFound += (_, __) => _dlx.Cancel();

            var maxDirectionChanges = _gridDescription.InitialMaxDirectionChanges;

            for (;;)
            {
                if (_cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                var localMaxDirectionChanges = maxDirectionChanges;
                matrix = MeasureFunctionExecutionTime(
                    () => _matrixBuilder.BuildMatrixFor(_grid, localMaxDirectionChanges, _cancellationToken),
                    ref matrixBuildingDuration);

                _dispatcher.Invoke(
                    _updateSolutionStatsHandler,
                    new SolutionStats(
                        matrix.GetLength(0),
                        matrix.GetLength(1),
                        matrixBuildingDuration,
                        matrixBuildingSolving,
                        maxDirectionChanges));

                if (_cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                var localMatrix = matrix;
                solutions = MeasureFunctionExecutionTime(
                    () => _dlx.Solve(localMatrix).ToList(),
                    ref matrixBuildingSolving);

                _dispatcher.Invoke(
                    _updateSolutionStatsHandler,
                    new SolutionStats(
                        matrix.GetLength(0),
                        matrix.GetLength(1),
                        matrixBuildingDuration,
                        matrixBuildingSolving,
                        maxDirectionChanges));

                if (solutions.Any())
                {
                    break;
                }

                if (!_matrixBuilder.ThereAreStillSomeAbandonedPaths())
                {
                    break;
                }

                maxDirectionChanges++;
            }

            int maxActualDirectionChanges;

            if (solutions.Any())
            {
                maxActualDirectionChanges = solutions
                    .First()
                    .RowIndexes
                    .Max(rowIndex => _matrixBuilder.GetColourPairAndPathForRowIndex(rowIndex).Item2.NumDirectionChanges);
            }
            else
            {
                var matrixRowCount = matrix.GetLength(0);
                maxActualDirectionChanges =
                    Enumerable.Range(0, matrixRowCount)
                    .Max(rowIndex => _matrixBuilder.GetColourPairAndPathForRowIndex(rowIndex).Item2.NumDirectionChanges);
            }

            var solutionStats = new SolutionStats(
                matrix.GetLength(0),
                matrix.GetLength(1),
                matrixBuildingDuration,
                matrixBuildingSolving,
                maxActualDirectionChanges);

            if (_cancellationToken.IsCancellationRequested)
            {
                _dispatcher.Invoke(_cancelledHandler, solutionStats);
            }
            else
            {
                if (solutions.Any())
                {
                    var colourPairPaths = solutions.First().RowIndexes.Select(_matrixBuilder.GetColourPairAndPathForRowIndex);
                    _dispatcher.Invoke(_solutionFoundHandler, solutionStats, colourPairPaths);
                }
                else
                {
                    _dispatcher.Invoke(_noSolutionFoundHandler, solutionStats);
                }
            }
        }

        private static TResult MeasureFunctionExecutionTime<TResult>(Func<TResult> func, ref TimeSpan? cumulativeDuration)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();

            stopwatch.Start();
            var result = func();
            stopwatch.Stop();

            if (cumulativeDuration.HasValue)
                cumulativeDuration += stopwatch.Elapsed;
            else
                cumulativeDuration = stopwatch.Elapsed;

            return result;
        }
    }
}
