using System.Windows.Media;

namespace FlowFreeSolverWpf
{
    public static class ColourMap
    {
        public static DotColour[] DotColours = new[]
            {
                new DotColour("Blue", Colors.Blue),
                new DotColour("Orange", Colors.Orange),
                new DotColour("Red", Colors.Red),
                new DotColour("Green", Colors.Green),
                new DotColour("Cyan", Colors.Cyan),
                new DotColour("Yellow", Colors.Yellow),
                new DotColour("Magenta", Colors.Magenta),
                new DotColour("MediumPurple", Colors.MediumPurple),
                new DotColour("Brown", Colors.Brown),
                new DotColour("Gray", Colors.Gray),
                new DotColour("White", Colors.White),
                new DotColour("Lime", Colors.Lime)
            };

        public static DotColour Blue { get { return DotColours[0]; } }
        public static DotColour Orange { get { return DotColours[1]; } }
        public static DotColour Red { get { return DotColours[2]; } }
        public static DotColour Green { get { return DotColours[3]; } }
        public static DotColour Cyan { get { return DotColours[4]; } }
        public static DotColour Yellow { get { return DotColours[5]; } }
        public static DotColour Magenta { get { return DotColours[6]; } }
        public static DotColour MediumPurple { get { return DotColours[7]; } }
        public static DotColour Brown { get { return DotColours[8]; } }
        public static DotColour Gray { get { return DotColours[9]; } }
        public static DotColour White { get { return DotColours[10]; } }
        public static DotColour Lime { get { return DotColours[11]; } }
    }
}
