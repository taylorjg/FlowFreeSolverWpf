namespace FlowFreeSolverWpf.Model
{
    public class ColourPair
    {
        public ColourPair(Coords startCoords, Coords endCoords, DotColour dotColour)
        {
            StartCoords = startCoords;
            EndCoords = endCoords;
            DotColour = dotColour;
        }

        public Coords StartCoords { get; private set; }
        public Coords EndCoords { get; private set; }
        public DotColour DotColour { get; set; }
    }
}
