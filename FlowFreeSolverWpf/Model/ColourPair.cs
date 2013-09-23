namespace FlowFreeSolverWpf.Model
{
    public class ColourPair
    {

        public ColourPair(Coords startCoords, Coords endCoords, string tag)
        {
            StartCoords = startCoords;
            EndCoords = endCoords;
            Tag = tag;
        }

        public Coords StartCoords { get; private set; }
        public Coords EndCoords { get; private set; }
        public string Tag { get; private set; }
    }
}
