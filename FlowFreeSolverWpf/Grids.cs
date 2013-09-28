using System.Linq;
using FlowFreeSolverWpf.Model;

namespace FlowFreeSolverWpf
{
    public static class Grids
    {
        private static readonly ColourPair[] Sample5X5Puzzle = Enumerable.Empty<ColourPair>().ToArray();
        private static readonly ColourPair[] Sample6X6Puzzle = Enumerable.Empty<ColourPair>().ToArray();

        private static readonly ColourPair[] Sample7X7Puzzle = new[]
            {
                    new ColourPair(new Coords(6, 6), new Coords(5, 0), DotColours.Blue),
                    new ColourPair(new Coords(5, 5), new Coords(1, 4), DotColours.Orange),
                    new ColourPair(new Coords(6, 5), new Coords(4, 1), DotColours.Red),
                    new ColourPair(new Coords(3, 3), new Coords(2, 2), DotColours.Green),
                    new ColourPair(new Coords(4, 3), new Coords(6, 0), DotColours.Cyan),
                    new ColourPair(new Coords(4, 2), new Coords(5, 1), DotColours.Yellow)
            };

        private static readonly ColourPair[] Sample8X8Puzzle = Enumerable.Empty<ColourPair>().ToArray();
        private static readonly ColourPair[] Sample9X9Puzzle = Enumerable.Empty<ColourPair>().ToArray();

        private static readonly ColourPair[] Sample10X10Puzzle = new[]
            {
                    new ColourPair(new Coords(5, 9), new Coords(9, 9), DotColours.Blue),
                    new ColourPair(new Coords(2, 6), new Coords(6, 6), DotColours.Orange),
                    new ColourPair(new Coords(0, 9), new Coords(2, 3), DotColours.Red),
                    new ColourPair(new Coords(3, 7), new Coords(8, 5), DotColours.Green),
                    new ColourPair(new Coords(1, 6), new Coords(9, 3), DotColours.Cyan),
                    new ColourPair(new Coords(0, 2), new Coords(1, 0), DotColours.Yellow),
                    new ColourPair(new Coords(5, 3), new Coords(8, 1), DotColours.Magenta),
                    new ColourPair(new Coords(1, 1), new Coords(4, 0), DotColours.MediumPurple),
                    new ColourPair(new Coords(3, 6), new Coords(6, 5), DotColours.Brown),
                    new ColourPair(new Coords(1, 2), new Coords(8, 4), DotColours.Gray),
                    new ColourPair(new Coords(2, 7), new Coords(3, 8), DotColours.White),
                    new ColourPair(new Coords(1, 5), new Coords(6, 1), DotColours.Lime)
            };

        private static readonly ColourPair[] Sample11X11Puzzle = Enumerable.Empty<ColourPair>().ToArray();
        private static readonly ColourPair[] Sample12X12Puzzle = Enumerable.Empty<ColourPair>().ToArray();
        private static readonly ColourPair[] Sample13X13Puzzle = Enumerable.Empty<ColourPair>().ToArray();
        private static readonly ColourPair[] Sample14X14Puzzle = Enumerable.Empty<ColourPair>().ToArray();

        public static GridDescription[] GridDescriptions = new[]
            {
                new GridDescription(5, Sample5X5Puzzle, 4),
                new GridDescription(6, Sample6X6Puzzle, 4),
                new GridDescription(7, Sample7X7Puzzle, 5),
                new GridDescription(8, Sample8X8Puzzle, 6),
                new GridDescription(9, Sample9X9Puzzle, 7),
                new GridDescription(10, Sample10X10Puzzle, 7),
                new GridDescription(11, Sample11X11Puzzle, 8),
                new GridDescription(12, Sample12X12Puzzle, 8),
                new GridDescription(13, Sample13X13Puzzle, 8),
                new GridDescription(14, Sample14X14Puzzle, 8)
            };
    }
}
