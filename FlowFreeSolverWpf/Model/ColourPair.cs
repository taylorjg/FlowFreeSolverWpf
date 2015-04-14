namespace FlowFreeSolverWpf.Model
{
    public class ColourPair
    {
        private readonly Coords _startCoords;
        private readonly Coords _endCoords;
        private readonly DotColour _dotColour;

        public ColourPair(Coords startCoords, Coords endCoords, DotColour dotColour)
        {
            _startCoords = startCoords;
            _endCoords = endCoords;
            _dotColour = dotColour;
        }

        public Coords StartCoords
        {
            get { return _startCoords; }
        }

        public Coords EndCoords
        {
            get { return _endCoords; }
        }

        public DotColour DotColour
        {
            get { return _dotColour; }
        }
    }
}
