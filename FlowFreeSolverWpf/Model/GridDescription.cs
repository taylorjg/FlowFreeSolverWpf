namespace FlowFreeSolverWpf.Model
{
    public class GridDescription
    {
        public GridDescription(
            int gridSize,
            ColourPair[] samplePuzzle,
            int initialMaxDirectionChanges,
            int minColourPairs = 1,
            int maxColourPairs = 100)
        {
            GridSize = gridSize;
            GridSizeName = string.Format("{0}x{0}", gridSize);
            SamplePuzzle = samplePuzzle;
            InitialMaxDirectionChanges = initialMaxDirectionChanges;
            MinColourPairs = minColourPairs;
            MaxColourPairs = maxColourPairs;
        }

        public int GridSize { get; private set; }
        public string GridSizeName { get; private set; }
        public ColourPair[] SamplePuzzle { get; private set; }
        public int InitialMaxDirectionChanges { get; set; }
        public int MinColourPairs { get; private set; }
        public int MaxColourPairs { get; private set; }
    }
}
