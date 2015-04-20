using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DlxLib;
using FlowFreeSolverWpf.Model;

namespace FlowFreeSolverWpf.ViewModel
{
    public class PuzzleSolverStats
    {
        private readonly IDispatcher _dispatcher;
        private readonly CancellationToken _cancellationToken;
        private readonly Action<SolutionStats> _updateSolutionStatsHandler;
        private TimeSpan? _matrixBuildingDuration;
        private TimeSpan? _matrixSolvingDuration;
        private SolutionStats _solutionStats;

        public PuzzleSolverStats(
            IDispatcher dispatcher,
            CancellationToken cancellationToken,
            Action<SolutionStats> updateSolutionStatsHandler)
        {
            _dispatcher = dispatcher;
            _cancellationToken = cancellationToken;
            _updateSolutionStatsHandler = updateSolutionStatsHandler;
            _solutionStats = new SolutionStats(0, 0, null, null, 0);
        }

        public TimeSpan? MatrixBuildingDuration
        {
            get { return _matrixBuildingDuration; }
        }

        public TimeSpan? MatrixSolvingDuration
        {
            get { return _matrixSolvingDuration; }
        }

        public SolutionStats GetFinalStats(MatrixBuilder matrixBuilder, List<MatrixRow> matrix, Solution solution)
        {
            var rowIndexes = (solution != null) ? solution.RowIndexes : Enumerable.Range(0, matrix.Count);
            var maxActualDirectionChanges = rowIndexes.Max(rowIndex => matrixBuilder.GetColourPairAndPathForRowIndex(rowIndex).Item2.NumDirectionChanges);;

            return new SolutionStats(
                matrix.Count,
                matrix.Any() ? matrix.First().DlxRowEnumerable.Count() : 0,
                MatrixBuildingDuration,
                MatrixSolvingDuration,
                maxActualDirectionChanges);
        }

        public TResult MeasureMatrixBuildingTime<TResult>(Func<TResult> func, int maxDirectionChanges, ICollection<MatrixRow> matrix)
        {
            return MeasureExecutionTime(func, maxDirectionChanges, matrix, ref _matrixBuildingDuration);
        }

        public TResult MeasureMatrixSolvingTime<TResult>(Func<TResult> func, int maxDirectionChanges, ICollection<MatrixRow> matrix)
        {
            return MeasureExecutionTime(func, maxDirectionChanges, matrix, ref _matrixSolvingDuration);
        }

        private TResult MeasureExecutionTime<TResult>(Func<TResult> func, int maxDirectionChanges, ICollection<MatrixRow> matrix, ref TimeSpan? duration)
        {
            _cancellationToken.ThrowIfCancellationRequested();

            var stopwatch = new System.Diagnostics.Stopwatch();

            stopwatch.Start();
            var result = func();
            stopwatch.Stop();

            if (duration.HasValue)
                duration += stopwatch.Elapsed;
            else
                duration = stopwatch.Elapsed;

            _solutionStats = new SolutionStats(
                matrix.Count,
                matrix.Any() ? matrix.First().DlxRowEnumerable.Count() : 0,
                MatrixBuildingDuration,
                MatrixSolvingDuration,
                maxDirectionChanges);

            _dispatcher.Invoke(_updateSolutionStatsHandler, _solutionStats);

            return result;
        }
    }
}
