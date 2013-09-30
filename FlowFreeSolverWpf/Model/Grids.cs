namespace FlowFreeSolverWpf.Model
{
    public static class Grids
    {
        private static readonly ColourPair[] Sample5X5Puzzle = new[]
            {
                new ColourPair(new Coords(1, 1), new Coords(2, 3), DotColours.Blue),
                new ColourPair(new Coords(0, 1), new Coords(4, 1), DotColours.Orange),
                new ColourPair(new Coords(0, 2), new Coords(3, 4), DotColours.Red),
                new ColourPair(new Coords(3, 3), new Coords(4, 4), DotColours.Green),
                new ColourPair(new Coords(3, 1), new Coords(4, 0), DotColours.Yellow)
            };

        private static readonly ColourPair[] Sample6X6Puzzle = new[]
            {
                new ColourPair(new Coords(0, 4), new Coords(5, 2), DotColours.Blue),
                new ColourPair(new Coords(3, 4), new Coords(4, 1), DotColours.Orange),
                new ColourPair(new Coords(0, 3), new Coords(1, 4), DotColours.Red),
                new ColourPair(new Coords(0, 2), new Coords(3, 3), DotColours.Green),
                new ColourPair(new Coords(3, 0), new Coords(5, 1), DotColours.Cyan),
                new ColourPair(new Coords(1, 1), new Coords(2, 4), DotColours.Yellow)
            };

        private static readonly ColourPair[] Sample7X7Puzzle = new[]
            {
                new ColourPair(new Coords(6, 6), new Coords(5, 0), DotColours.Blue),
                new ColourPair(new Coords(5, 5), new Coords(1, 4), DotColours.Orange),
                new ColourPair(new Coords(6, 5), new Coords(4, 1), DotColours.Red),
                new ColourPair(new Coords(3, 3), new Coords(2, 2), DotColours.Green),
                new ColourPair(new Coords(4, 3), new Coords(6, 0), DotColours.Cyan),
                new ColourPair(new Coords(4, 2), new Coords(5, 1), DotColours.Yellow)
            };

        private static readonly ColourPair[] Sample8X8Puzzle = new[]
            {
                new ColourPair(new Coords(4, 4), new Coords(6, 3), DotColours.Blue),
                new ColourPair(new Coords(4, 5), new Coords(6, 7), DotColours.Orange),
                new ColourPair(new Coords(1, 5), new Coords(4, 6), DotColours.Red),
                new ColourPair(new Coords(1, 1), new Coords(6, 1), DotColours.Green),
                new ColourPair(new Coords(1, 7), new Coords(2, 4), DotColours.Cyan),
                new ColourPair(new Coords(5, 3), new Coords(6, 2), DotColours.Yellow),
                new ColourPair(new Coords(0, 3), new Coords(3, 4), DotColours.Magenta),
                new ColourPair(new Coords(1, 6), new Coords(6, 5), DotColours.Brown)
            };

        private static readonly ColourPair[] Sample9X9Puzzle = new[]
            {
                new ColourPair(new Coords(1, 3), new Coords(2, 1), DotColours.Blue),
                new ColourPair(new Coords(4, 8), new Coords(4, 2), DotColours.Orange),
                new ColourPair(new Coords(6, 6), new Coords(8, 3), DotColours.Red),
                new ColourPair(new Coords(2, 3), new Coords(7, 4), DotColours.Green),
                new ColourPair(new Coords(6, 5), new Coords(7, 1), DotColours.Cyan),
                new ColourPair(new Coords(2, 2), new Coords(8, 6), DotColours.Yellow),
                new ColourPair(new Coords(1, 6), new Coords(4, 7), DotColours.Magenta),
                new ColourPair(new Coords(5, 8), new Coords(8, 8), DotColours.Brown),
                new ColourPair(new Coords(2, 6), new Coords(4, 4), DotColours.MediumPurple)
            };

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

        private static readonly ColourPair[] Sample11X11Puzzle = new[]
            {
                new ColourPair(new Coords(6, 0), new Coords(7, 2), DotColours.Blue),
                new ColourPair(new Coords(8, 5), new Coords(9, 0), DotColours.Orange),
                new ColourPair(new Coords(6, 9), new Coords(10, 8), DotColours.Red),
                new ColourPair(new Coords(5, 9), new Coords(7, 3), DotColours.Green),
                new ColourPair(new Coords(8, 8), new Coords(10, 9), DotColours.Cyan),
                new ColourPair(new Coords(9, 1), new Coords(10, 0), DotColours.Yellow),
                new ColourPair(new Coords(3, 6), new Coords(8, 4), DotColours.Magenta),
                new ColourPair(new Coords(5, 1), new Coords(7, 8), DotColours.MediumPurple),
                new ColourPair(new Coords(2, 3), new Coords(9, 2), DotColours.Brown),
                new ColourPair(new Coords(1, 1), new Coords(6, 1), DotColours.Gray),
                new ColourPair(new Coords(8, 9), new Coords(10, 10), DotColours.White),
                new ColourPair(new Coords(3, 7), new Coords(7, 5), DotColours.Lime)
            };

        private static readonly ColourPair[] Sample12X12Puzzle = new[]
            {
                new ColourPair(new Coords(2, 7), new Coords(6, 7), DotColours.Blue),
                new ColourPair(new Coords(4, 1), new Coords(7, 2), DotColours.Orange),
                new ColourPair(new Coords(1, 1), new Coords(8, 4), DotColours.Red),
                new ColourPair(new Coords(0, 6), new Coords(5, 10), DotColours.Green),
                new ColourPair(new Coords(8, 8), new Coords(9, 2), DotColours.Cyan),
                new ColourPair(new Coords(7, 1), new Coords(11, 6), DotColours.Yellow),
                new ColourPair(new Coords(1, 2), new Coords(10, 6), DotColours.Magenta),
                new ColourPair(new Coords(0, 10), new Coords(8, 7), DotColours.MediumPurple),
                new ColourPair(new Coords(2, 5), new Coords(8, 9), DotColours.Brown),
                new ColourPair(new Coords(1, 10), new Coords(4, 10), DotColours.Gray),
                new ColourPair(new Coords(4, 0), new Coords(10, 4), DotColours.White),
                new ColourPair(new Coords(10, 10), new Coords(10, 8), DotColours.Lime)
            };

        private static readonly ColourPair[] Sample13X13Puzzle = new[]
            {
                new ColourPair(new Coords(1, 1), new Coords(7, 1), DotColours.Blue),
                new ColourPair(new Coords(2, 4), new Coords(7, 4), DotColours.Orange),
                new ColourPair(new Coords(0, 12), new Coords(6, 10), DotColours.Red),
                new ColourPair(new Coords(4, 11), new Coords(12, 3), DotColours.Green),
                new ColourPair(new Coords(10, 10), new Coords(10, 2), DotColours.Cyan),
                new ColourPair(new Coords(9, 5), new Coords(11, 1), DotColours.Yellow),
                new ColourPair(new Coords(1, 9), new Coords(6, 1), DotColours.Magenta),
                new ColourPair(new Coords(0, 0), new Coords(5, 6), DotColours.MediumPurple),
                new ColourPair(new Coords(1, 12), new Coords(7, 12), DotColours.Brown),
                new ColourPair(new Coords(6, 8), new Coords(11, 7), DotColours.Gray),
                new ColourPair(new Coords(3, 4), new Coords(6, 3), DotColours.White),
                new ColourPair(new Coords(8, 7), new Coords(12, 9), DotColours.Lime),
                new ColourPair(new Coords(2, 3), new Coords(7, 3), DotColours.Wheat),
                new ColourPair(new Coords(4, 8), new Coords(5, 10), DotColours.CornflowerBlue)
            };

        private static readonly ColourPair[] Sample14X14Puzzle = new[]
            {
                new ColourPair(new Coords(2, 4), new Coords(6, 4), DotColours.Blue),
                new ColourPair(new Coords(0, 13), new Coords(13, 13), DotColours.Orange),
                new ColourPair(new Coords(2, 3), new Coords(3, 5), DotColours.Red),
                new ColourPair(new Coords(9, 10), new Coords(12, 11), DotColours.Green),
                new ColourPair(new Coords(0, 12), new Coords(2, 5), DotColours.Cyan),
                new ColourPair(new Coords(2, 11), new Coords(4, 10), DotColours.Yellow),
                new ColourPair(new Coords(2, 10), new Coords(2, 2), DotColours.Magenta),
                new ColourPair(new Coords(5, 12), new Coords(10, 12), DotColours.MediumPurple),
                new ColourPair(new Coords(4, 7), new Coords(5, 10), DotColours.Brown),
                new ColourPair(new Coords(0, 0), new Coords(6, 10), DotColours.Gray),
                new ColourPair(new Coords(4, 12), new Coords(12, 9), DotColours.White),
                new ColourPair(new Coords(5, 0), new Coords(8, 10), DotColours.Lime),
                new ColourPair(new Coords(8, 9), new Coords(11, 2), DotColours.Wheat)
            };

        public static GridDescription[] GridDescriptions = new[]
            {
                new GridDescription(5, Sample5X5Puzzle, 1, 4, 5),
                new GridDescription(6, Sample6X6Puzzle, 1, 4, 6),
                new GridDescription(7, Sample7X7Puzzle, 1),
                new GridDescription(8, Sample8X8Puzzle, 1),
                new GridDescription(9, Sample9X9Puzzle, 1),
                new GridDescription(10, Sample10X10Puzzle, 1),
                new GridDescription(11, Sample11X11Puzzle, 1),
                new GridDescription(12, Sample12X12Puzzle, 8),
                new GridDescription(13, Sample13X13Puzzle, 1, 10, 14),
                new GridDescription(14, Sample14X14Puzzle, 1)
            };
    }
}
