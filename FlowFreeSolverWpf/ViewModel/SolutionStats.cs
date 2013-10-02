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

        public int NumMatrixRows { get; set; }
        public int NumMatrixCols { get; set; }
        public TimeSpan? MatrixBuildingDuration { get; set; }
        public TimeSpan? MatrixSolvingDuration { get; set; }
        public int MaxDirectionChanges { get; set; }
    }
}
