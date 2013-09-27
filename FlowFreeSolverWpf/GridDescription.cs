using FlowFreeSolverWpf.Model;

namespace FlowFreeSolverWpf
{
    public class GridDescription
    {
        public GridDescription(int gridSize, ColourPair[] samplePuzzle, int initialMaxDirectionChanges)
        {
            GridSize = gridSize;
            GridSizeName = string.Format("{0}x{0}", gridSize);
            SamplePuzzle = samplePuzzle;
            InitialMaxDirectionChanges = initialMaxDirectionChanges;
        }

        public int GridSize { get; private set; }
        public string GridSizeName { get; private set; }
        public ColourPair[] SamplePuzzle { get; private set; }
        public int InitialMaxDirectionChanges { get; set; }
    }
}
