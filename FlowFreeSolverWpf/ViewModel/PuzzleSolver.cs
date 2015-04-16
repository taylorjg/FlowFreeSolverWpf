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
            Task.Factory.StartNew(SolvePuzzleInBackground, _cancellationToken);
        }

        private void SolvePuzzleInBackground()
        {
            var dlx = new Dlx(_cancellationToken);
            var matrix = new List<MatrixRow>();
            var maxDirectionChanges = _gridDescription.InitialMaxDirectionChanges;
            TimeSpan? matrixBuildingDuration = null;
            TimeSpan? matrixSolvingDuration = null;
            Solution firstSolution = null;

            for (;;)
            {
                if (_cancellationToken.IsCancellationRequested) break;

                var localMaxDirectionChanges = maxDirectionChanges;

                matrix = MeasureFunctionExecutionTime(
                    () => _matrixBuilder.BuildMatrix(_grid, localMaxDirectionChanges, _cancellationToken),
                    ref matrixBuildingDuration);

                _dispatcher.Invoke(
                    _updateSolutionStatsHandler,
                    new SolutionStats(
                        matrix.Count,
                        matrix[0].DlxMatrixRow.Count,
                        matrixBuildingDuration,
                        matrixSolvingDuration,
                        maxDirectionChanges));

                if (_cancellationToken.IsCancellationRequested) break;

                var localMatrix = matrix;
                firstSolution = MeasureFunctionExecutionTime(
                    () => dlx.Solve(localMatrix, r => r, r => r.DlxMatrixRow).FirstOrDefault(),
                    ref matrixSolvingDuration);

                _dispatcher.Invoke(
                    _updateSolutionStatsHandler,
                    new SolutionStats(
                        matrix.Count,
                        matrix[0].DlxMatrixRow.Count,
                        matrixBuildingDuration,
                        matrixSolvingDuration,
                        maxDirectionChanges));

                if (firstSolution != null) break;
                if (!_matrixBuilder.ThereAreStillSomeAbandonedPaths()) break;

                maxDirectionChanges++;
            }

            int maxActualDirectionChanges;

            if (firstSolution != null)
            {
                maxActualDirectionChanges = 
                    firstSolution
                    .RowIndexes
                    .Max(rowIndex => _matrixBuilder.GetColourPairAndPathForRowIndex(rowIndex).Item2.NumDirectionChanges);
            }
            else
            {
                var matrixRowCount = matrix.Count;
                maxActualDirectionChanges =
                    Enumerable.Range(0, matrixRowCount)
                    .Max(rowIndex => _matrixBuilder.GetColourPairAndPathForRowIndex(rowIndex).Item2.NumDirectionChanges);
            }

            var solutionStats = new SolutionStats(
                matrix.Count,
                matrix[0].DlxMatrixRow.Count,
                matrixBuildingDuration,
                matrixSolvingDuration,
                maxActualDirectionChanges);

            if (_cancellationToken.IsCancellationRequested)
            {
                _dispatcher.Invoke(_cancelledHandler, solutionStats);
            }
            else
            {
                if (firstSolution != null)
                {
                    var colourPairPaths = firstSolution.RowIndexes.Select(_matrixBuilder.GetColourPairAndPathForRowIndex);
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
