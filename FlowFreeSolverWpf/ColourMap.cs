using System;
using System.Linq;
using System.Windows.Media;

namespace FlowFreeSolverWpf
{
    public static class ColourMap
    {
        public static DotColour[] DotColours = new[]
            {
                new DotColour("A", "Blue", Colors.Blue),
                new DotColour("B", "Orange", Colors.Orange),
                new DotColour("C", "Red", Colors.Red),
                new DotColour("D", "Green", Colors.Green),
                new DotColour("E", "Cyan", Colors.Cyan),
                new DotColour("F", "Yellow", Colors.Yellow),
                new DotColour("G", "Magenta", Colors.Magenta),
                new DotColour("H", "MediumPurple", Colors.MediumPurple),
                new DotColour("I", "Brown", Colors.Brown)
            };

        public static Color MapTagToColour(string tag)
        {
            var dotColour = DotColours.FirstOrDefault(dc => dc.Tag == tag);

            if (dotColour == null)
            {
                throw new InvalidOperationException(string.Format("Unknown tag, '{0}'.", tag));
            }

            return dotColour.Colour;
        }
    }
}
