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
        private readonly Action<SolutionStats> _updateSolutionStatsHandler;
        private readonly IDispatcher _dispatcher;
        private readonly CancellationToken _cancellationToken;

        public PuzzleSolver(
            Grid grid,
            GridDescription gridDescription,
            Action<SolutionStats, IEnumerable<Tuple<ColourPair, Path>>> solutionFoundHandler,
            Action<SolutionStats> noSolutionFoundHandler,
            Action<SolutionStats> updateSolutionStatsHandler,
            IDispatcher dispatcher,
            CancellationToken cancellationToken)
        {
            _grid = grid;
            _gridDescription = gridDescription;
            _solutionFoundHandler = solutionFoundHandler;
            _noSolutionFoundHandler = noSolutionFoundHandler;
            _updateSolutionStatsHandler = updateSolutionStatsHandler;
            _dispatcher = dispatcher;
            _cancellationToken = cancellationToken;
        }

        public void SolvePuzzle()
        {
            Task.Factory.StartNew(
                SolvePuzzleInBackground,
                _cancellationToken,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
        }

        private void SolvePuzzleInBackground()
        {
            var matrixBuilder = new MatrixBuilder(_grid, _cancellationToken);
            var dlx = new Dlx(_cancellationToken);
            var maxDirectionChanges = _gridDescription.InitialMaxDirectionChanges;
            var matrix = new List<MatrixRow>();
            var puzzleSolverStats = new PuzzleSolverStats(_dispatcher, _cancellationToken, _updateSolutionStatsHandler);
            Solution solution = null;

            while (solution == null)
            {
                var maxDirectionChangesLocal = maxDirectionChanges;
                var matrixLocal1 = matrix;

                matrix = puzzleSolverStats.MeasureMatrixBuildingTime(
                    () => matrixBuilder.BuildMatrix(maxDirectionChangesLocal), maxDirectionChangesLocal, matrixLocal1);

                var matrixLocal2 = matrix;

                solution = puzzleSolverStats.MeasureMatrixSolvingTime(
                    () => dlx.Solve(matrixLocal2, r => r, r => r.DlxRowEnumerable).FirstOrDefault(), maxDirectionChangesLocal, matrixLocal2);

                if (!matrixBuilder.HasInactivePaths()) break;

                maxDirectionChanges++;
            }

            var solutionStats = puzzleSolverStats.GetFinalStats(matrixBuilder, matrix, solution);

            if (solution != null)
            {
                var colourPairPaths = solution.RowIndexes.Select(matrixBuilder.GetColourPairAndPathForRowIndex);
                _dispatcher.Invoke(_solutionFoundHandler, solutionStats, colourPairPaths);
            }
            else
            {
                _dispatcher.Invoke(_noSolutionFoundHandler, solutionStats);
            }
        }
    }
}
