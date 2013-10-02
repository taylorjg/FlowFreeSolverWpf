using System;

namespace FlowFreeSolverWpf.ViewModel
{
    public class SolutionStats
    {
        public SolutionStats(
            int numMatrixRows,
            int numMatrixCols,
            TimeSpan? matrixBuildingDuration,
            TimeSpan? matrixSolvingDuration,
            int maxDirectionChanges)
        {
            NumMatrixRows = numMatrixRows;
            NumMatrixCols = numMatrixCols;
            MatrixBuildingDuration = matrixBuildingDuration;
            MatrixSolvingDuration = matrixSolvingDuration;
            MaxDirectionChanges = maxDirectionChanges;
        }

        public int NumMatrixRows { get; private set; }
        public int NumMatrixCols { get; private set; }
        public TimeSpan? MatrixBuildingDuration { get; private set; }
        public TimeSpan? MatrixSolvingDuration { get; private set; }
        public int MaxDirectionChanges { get; private set; }
    }
}
