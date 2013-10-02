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
        private readonly Action<SolutionStats, IEnumerable<Tuple<ColourPair, Path>>> _solutionFoundHandler;
        private readonly Action<SolutionStats> _noSolutionFoundHandler;
        private readonly Action<SolutionStats> _cancelledHandler;
        private readonly IDispatcher _dispatcher;
        private readonly CancellationToken _cancellationToken;
        private readonly MatrixBuilder _matrixBuilder = new MatrixBuilder();
        private readonly Dlx _dlx = new Dlx();

        public PuzzleSolver(
            Grid grid,
            Action<SolutionStats,
            IEnumerable<Tuple<ColourPair, Path>>> solutionFoundHandler,
            Action<SolutionStats> noSolutionFoundHandler,
            Action<SolutionStats> cancelledHandler,
            IDispatcher dispatcher,
            CancellationToken cancellationToken)
        {
            _grid = grid;
            _solutionFoundHandler = solutionFoundHandler;
            _noSolutionFoundHandler = noSolutionFoundHandler;
            _cancelledHandler = cancelledHandler;
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

            // TODO: this should come from SelectedGrid.InitialMaxDirectionChanges
            var maxDirectionChanges = 1;

            for (;;)
            {
                if (_cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                var stopwatch = new System.Diagnostics.Stopwatch();

                stopwatch.Reset();
                stopwatch.Start();
                matrix = _matrixBuilder.BuildMatrixFor(_grid, maxDirectionChanges, _cancellationToken);
                stopwatch.Stop();

                if (!matrixBuildingDuration.HasValue)
                {
                    matrixBuildingDuration = stopwatch.Elapsed;
                }
                else
                {
                    matrixBuildingDuration += stopwatch.Elapsed;
                }

                if (_cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                stopwatch.Reset();
                stopwatch.Start();
                solutions = _dlx.Solve(matrix).ToList();
                stopwatch.Stop();

                if (!matrixBuildingSolving.HasValue)
                {
                    matrixBuildingSolving = stopwatch.Elapsed;
                }
                else
                {
                    matrixBuildingSolving += stopwatch.Elapsed;
                }

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

            var solutionStats = new SolutionStats(
                matrix.GetLength(0),
                matrix.GetLength(1),
                matrixBuildingDuration,
                matrixBuildingSolving,
                maxDirectionChanges);

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
    }
}
