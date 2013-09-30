namespace FlowFreeSolverWpf.Model
{
    public class DotColours
    {
        public static DotColour[] Colours = new[]
            {
                new DotColour("Blue"),
                new DotColour("Orange"),
                new DotColour("Red"),
                new DotColour("Green"),
                new DotColour("Cyan"),
                new DotColour("Yellow"),
                new DotColour("Magenta"),
                new DotColour("Medium Purple"),
                new DotColour("Brown"),
                new DotColour("Gray"),
                new DotColour("White"),
                new DotColour("Lime"),
                new DotColour("Wheat")
            };

        public static DotColour Blue { get { return Colours[0]; } }
        public static DotColour Orange { get { return Colours[1]; } }
        public static DotColour Red { get { return Colours[2]; } }
        public static DotColour Green { get { return Colours[3]; } }
        public static DotColour Cyan { get { return Colours[4]; } }
        public static DotColour Yellow { get { return Colours[5]; } }
        public static DotColour Magenta { get { return Colours[6]; } }
        public static DotColour MediumPurple { get { return Colours[7]; } }
        public static DotColour Brown { get { return Colours[8]; } }
        public static DotColour Gray { get { return Colours[9]; } }
        public static DotColour White { get { return Colours[10]; } }
        public static DotColour Lime { get { return Colours[11]; } }
        public static DotColour Wheat { get { return Colours[12]; } }
    }
}
