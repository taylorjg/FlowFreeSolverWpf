namespace FlowFreeSolverWpf.Model
{
    public class GridDescription
    {
        private readonly int _gridSize;
        private readonly string _gridSizeName;
        private readonly ColourPair[] _samplePuzzle;
        private readonly int _initialMaxDirectionChanges;
        private readonly int _minColourPairs;
        private readonly int _maxColourPairs;

        public GridDescription(
            int gridSize,
            ColourPair[] samplePuzzle,
            int initialMaxDirectionChanges,
            int minColourPairs = 1,
            int maxColourPairs = 100)
        {
            _gridSize = gridSize;
            _samplePuzzle = samplePuzzle;
            _initialMaxDirectionChanges = initialMaxDirectionChanges;
            _minColourPairs = minColourPairs;
            _maxColourPairs = maxColourPairs;
            _gridSizeName = string.Format("{0}x{0}", gridSize);
        }

        public int GridSize
        {
            get { return _gridSize; }
        }

        public string GridSizeName
        {
            get { return _gridSizeName; }
        }

        public ColourPair[] SamplePuzzle
        {
            get { return _samplePuzzle; }
        }

        public int InitialMaxDirectionChanges
        {
            get { return _initialMaxDirectionChanges; }
        }

        public int MinColourPairs
        {
            get { return _minColourPairs; }
        }

        public int MaxColourPairs
        {
            get { return _maxColourPairs; }
        }
    }
}
