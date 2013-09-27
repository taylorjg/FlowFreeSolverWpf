using System.Windows.Media;

namespace FlowFreeSolverWpf
{
    public class DotColour
    {
        public DotColour(string colourName, Color colour)
        {
            ColourName = colourName;
            Colour = colour;
        }

        public string ColourName { get; private set; }
        public Color Colour { get; private set; }
    }
}
