using System;

namespace FlowFreeSolverWpf.ViewModel
{
    public class SolutionStats
    {
        private readonly int _numMatrixRows;
        private readonly int _numMatrixCols;
        private readonly TimeSpan? _matrixBuildingDuration;
        private readonly TimeSpan? _matrixSolvingDuration;
        private readonly int _maxDirectionChanges;

        public SolutionStats(
            int numMatrixRows,
            int numMatrixCols,
            TimeSpan? matrixBuildingDuration,
            TimeSpan? matrixSolvingDuration,
            int maxDirectionChanges)
        {
            _numMatrixRows = numMatrixRows;
            _numMatrixCols = numMatrixCols;
            _matrixBuildingDuration = matrixBuildingDuration;
            _matrixSolvingDuration = matrixSolvingDuration;
            _maxDirectionChanges = maxDirectionChanges;
        }

        public int NumMatrixRows
        {
            get { return _numMatrixRows; }
        }

        public int NumMatrixCols
        {
            get { return _numMatrixCols; }
        }

        public TimeSpan? MatrixBuildingDuration
        {
            get { return _matrixBuildingDuration; }
        }

        public TimeSpan? MatrixSolvingDuration
        {
            get { return _matrixSolvingDuration; }
        }

        public int MaxDirectionChanges
        {
            get { return _maxDirectionChanges; }
        }
    }
}
