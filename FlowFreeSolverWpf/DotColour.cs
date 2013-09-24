using System.Windows.Media;

namespace FlowFreeSolverWpf
{
    public class DotColour
    {
        public DotColour(string tag, string colourName, Color colour)
        {
            Tag = tag;
            ColourName = colourName;
            Colour = colour;
        }

        public string Tag { get; private set; }
        public string ColourName { get; private set; }
        public Color Colour { get; private set; }
    }
}
