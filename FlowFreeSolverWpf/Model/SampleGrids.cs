namespace FlowFreeSolverWpf.Model
{
    public static class SampleGrids
    {
        private static readonly ColourPair[] ColourPairsFor5X5SampleGrid =
        {
            new ColourPair(CoordsFactory.GetCoords(1, 1), CoordsFactory.GetCoords(2, 3), DotColours.Blue),
            new ColourPair(CoordsFactory.GetCoords(0, 1), CoordsFactory.GetCoords(4, 1), DotColours.Orange),
            new ColourPair(CoordsFactory.GetCoords(0, 2), CoordsFactory.GetCoords(3, 4), DotColours.Red),
            new ColourPair(CoordsFactory.GetCoords(3, 3), CoordsFactory.GetCoords(4, 4), DotColours.Green),
            new ColourPair(CoordsFactory.GetCoords(3, 1), CoordsFactory.GetCoords(4, 0), DotColours.Yellow)
        };

        private static readonly ColourPair[] ColourPairsFor6X6SampleGrid =
        {
            new ColourPair(CoordsFactory.GetCoords(0, 4), CoordsFactory.GetCoords(5, 2), DotColours.Blue),
            new ColourPair(CoordsFactory.GetCoords(3, 4), CoordsFactory.GetCoords(4, 1), DotColours.Orange),
            new ColourPair(CoordsFactory.GetCoords(0, 3), CoordsFactory.GetCoords(1, 4), DotColours.Red),
            new ColourPair(CoordsFactory.GetCoords(0, 2), CoordsFactory.GetCoords(3, 3), DotColours.Green),
            new ColourPair(CoordsFactory.GetCoords(3, 0), CoordsFactory.GetCoords(5, 1), DotColours.Cyan),
            new ColourPair(CoordsFactory.GetCoords(1, 1), CoordsFactory.GetCoords(2, 4), DotColours.Yellow)
        };

        private static readonly ColourPair[] ColourPairsFor7X7SampleGrid =
        {
            new ColourPair(CoordsFactory.GetCoords(6, 6), CoordsFactory.GetCoords(5, 0), DotColours.Blue),
            new ColourPair(CoordsFactory.GetCoords(5, 5), CoordsFactory.GetCoords(1, 4), DotColours.Orange),
            new ColourPair(CoordsFactory.GetCoords(6, 5), CoordsFactory.GetCoords(4, 1), DotColours.Red),
            new ColourPair(CoordsFactory.GetCoords(3, 3), CoordsFactory.GetCoords(2, 2), DotColours.Green),
            new ColourPair(CoordsFactory.GetCoords(4, 3), CoordsFactory.GetCoords(6, 0), DotColours.Cyan),
            new ColourPair(CoordsFactory.GetCoords(4, 2), CoordsFactory.GetCoords(5, 1), DotColours.Yellow)
        };

        private static readonly ColourPair[] ColourPairsFor8X8SampleGrid =
        {
            new ColourPair(CoordsFactory.GetCoords(4, 4), CoordsFactory.GetCoords(6, 3), DotColours.Blue),
            new ColourPair(CoordsFactory.GetCoords(4, 5), CoordsFactory.GetCoords(6, 7), DotColours.Orange),
            new ColourPair(CoordsFactory.GetCoords(1, 5), CoordsFactory.GetCoords(4, 6), DotColours.Red),
            new ColourPair(CoordsFactory.GetCoords(1, 1), CoordsFactory.GetCoords(6, 1), DotColours.Green),
            new ColourPair(CoordsFactory.GetCoords(1, 7), CoordsFactory.GetCoords(2, 4), DotColours.Cyan),
            new ColourPair(CoordsFactory.GetCoords(5, 3), CoordsFactory.GetCoords(6, 2), DotColours.Yellow),
            new ColourPair(CoordsFactory.GetCoords(0, 3), CoordsFactory.GetCoords(3, 4), DotColours.Magenta),
            new ColourPair(CoordsFactory.GetCoords(1, 6), CoordsFactory.GetCoords(6, 5), DotColours.Brown)
        };

        private static readonly ColourPair[] ColourPairsFor9X9SampleGrid =
        {
            new ColourPair(CoordsFactory.GetCoords(1, 3), CoordsFactory.GetCoords(2, 1), DotColours.Blue),
            new ColourPair(CoordsFactory.GetCoords(4, 8), CoordsFactory.GetCoords(4, 2), DotColours.Orange),
            new ColourPair(CoordsFactory.GetCoords(6, 6), CoordsFactory.GetCoords(8, 3), DotColours.Red),
            new ColourPair(CoordsFactory.GetCoords(2, 3), CoordsFactory.GetCoords(7, 4), DotColours.Green),
            new ColourPair(CoordsFactory.GetCoords(6, 5), CoordsFactory.GetCoords(7, 1), DotColours.Cyan),
            new ColourPair(CoordsFactory.GetCoords(2, 2), CoordsFactory.GetCoords(8, 6), DotColours.Yellow),
            new ColourPair(CoordsFactory.GetCoords(1, 6), CoordsFactory.GetCoords(4, 7), DotColours.Magenta),
            new ColourPair(CoordsFactory.GetCoords(5, 8), CoordsFactory.GetCoords(8, 8), DotColours.Brown),
            new ColourPair(CoordsFactory.GetCoords(2, 6), CoordsFactory.GetCoords(4, 4), DotColours.MediumPurple)
        };

        private static readonly ColourPair[] ColourPairsFor10X10SampleGrid =
        {
            new ColourPair(CoordsFactory.GetCoords(5, 9), CoordsFactory.GetCoords(9, 9), DotColours.Blue),
            new ColourPair(CoordsFactory.GetCoords(2, 6), CoordsFactory.GetCoords(6, 6), DotColours.Orange),
            new ColourPair(CoordsFactory.GetCoords(0, 9), CoordsFactory.GetCoords(2, 3), DotColours.Red),
            new ColourPair(CoordsFactory.GetCoords(3, 7), CoordsFactory.GetCoords(8, 5), DotColours.Green),
            new ColourPair(CoordsFactory.GetCoords(1, 6), CoordsFactory.GetCoords(9, 3), DotColours.Cyan),
            new ColourPair(CoordsFactory.GetCoords(0, 2), CoordsFactory.GetCoords(1, 0), DotColours.Yellow),
            new ColourPair(CoordsFactory.GetCoords(5, 3), CoordsFactory.GetCoords(8, 1), DotColours.Magenta),
            new ColourPair(CoordsFactory.GetCoords(1, 1), CoordsFactory.GetCoords(4, 0), DotColours.MediumPurple),
            new ColourPair(CoordsFactory.GetCoords(3, 6), CoordsFactory.GetCoords(6, 5), DotColours.Brown),
            new ColourPair(CoordsFactory.GetCoords(1, 2), CoordsFactory.GetCoords(8, 4), DotColours.Gray),
            new ColourPair(CoordsFactory.GetCoords(2, 7), CoordsFactory.GetCoords(3, 8), DotColours.White),
            new ColourPair(CoordsFactory.GetCoords(1, 5), CoordsFactory.GetCoords(6, 1), DotColours.Lime)
        };

        private static readonly ColourPair[] ColourPairsFor11X11SampleGrid =
        {
            new ColourPair(CoordsFactory.GetCoords(6, 0), CoordsFactory.GetCoords(7, 2), DotColours.Blue),
            new ColourPair(CoordsFactory.GetCoords(8, 5), CoordsFactory.GetCoords(9, 0), DotColours.Orange),
            new ColourPair(CoordsFactory.GetCoords(6, 9), CoordsFactory.GetCoords(10, 8), DotColours.Red),
            new ColourPair(CoordsFactory.GetCoords(5, 9), CoordsFactory.GetCoords(7, 3), DotColours.Green),
            new ColourPair(CoordsFactory.GetCoords(8, 8), CoordsFactory.GetCoords(10, 9), DotColours.Cyan),
            new ColourPair(CoordsFactory.GetCoords(9, 1), CoordsFactory.GetCoords(10, 0), DotColours.Yellow),
            new ColourPair(CoordsFactory.GetCoords(3, 6), CoordsFactory.GetCoords(8, 4), DotColours.Magenta),
            new ColourPair(CoordsFactory.GetCoords(5, 1), CoordsFactory.GetCoords(7, 8), DotColours.MediumPurple),
            new ColourPair(CoordsFactory.GetCoords(2, 3), CoordsFactory.GetCoords(9, 2), DotColours.Brown),
            new ColourPair(CoordsFactory.GetCoords(1, 1), CoordsFactory.GetCoords(6, 1), DotColours.Gray),
            new ColourPair(CoordsFactory.GetCoords(8, 9), CoordsFactory.GetCoords(10, 10), DotColours.White),
            new ColourPair(CoordsFactory.GetCoords(3, 7), CoordsFactory.GetCoords(7, 5), DotColours.Lime)
        };

        private static readonly ColourPair[] ColourPairsFor12X12SampleGrid =
        {
            new ColourPair(CoordsFactory.GetCoords(2, 7), CoordsFactory.GetCoords(6, 7), DotColours.Blue),
            new ColourPair(CoordsFactory.GetCoords(4, 1), CoordsFactory.GetCoords(7, 2), DotColours.Orange),
            new ColourPair(CoordsFactory.GetCoords(1, 1), CoordsFactory.GetCoords(8, 4), DotColours.Red),
            new ColourPair(CoordsFactory.GetCoords(0, 6), CoordsFactory.GetCoords(5, 10), DotColours.Green),
            new ColourPair(CoordsFactory.GetCoords(8, 8), CoordsFactory.GetCoords(9, 2), DotColours.Cyan),
            new ColourPair(CoordsFactory.GetCoords(7, 1), CoordsFactory.GetCoords(11, 6), DotColours.Yellow),
            new ColourPair(CoordsFactory.GetCoords(1, 2), CoordsFactory.GetCoords(10, 6), DotColours.Magenta),
            new ColourPair(CoordsFactory.GetCoords(0, 10), CoordsFactory.GetCoords(8, 7), DotColours.MediumPurple),
            new ColourPair(CoordsFactory.GetCoords(2, 5), CoordsFactory.GetCoords(8, 9), DotColours.Brown),
            new ColourPair(CoordsFactory.GetCoords(1, 10), CoordsFactory.GetCoords(4, 10), DotColours.Gray),
            new ColourPair(CoordsFactory.GetCoords(4, 0), CoordsFactory.GetCoords(10, 4), DotColours.White),
            new ColourPair(CoordsFactory.GetCoords(10, 10), CoordsFactory.GetCoords(10, 8), DotColours.Lime)
        };

        private static readonly ColourPair[] ColourPairsFor13X13SampleGrid =
        {
            new ColourPair(CoordsFactory.GetCoords(1, 1), CoordsFactory.GetCoords(7, 1), DotColours.Blue),
            new ColourPair(CoordsFactory.GetCoords(2, 4), CoordsFactory.GetCoords(7, 4), DotColours.Orange),
            new ColourPair(CoordsFactory.GetCoords(0, 12), CoordsFactory.GetCoords(6, 10), DotColours.Red),
            new ColourPair(CoordsFactory.GetCoords(4, 11), CoordsFactory.GetCoords(12, 3), DotColours.Green),
            new ColourPair(CoordsFactory.GetCoords(10, 10), CoordsFactory.GetCoords(10, 2), DotColours.Cyan),
            new ColourPair(CoordsFactory.GetCoords(9, 5), CoordsFactory.GetCoords(11, 1), DotColours.Yellow),
            new ColourPair(CoordsFactory.GetCoords(1, 9), CoordsFactory.GetCoords(6, 1), DotColours.Magenta),
            new ColourPair(CoordsFactory.GetCoords(0, 0), CoordsFactory.GetCoords(5, 6), DotColours.MediumPurple),
            new ColourPair(CoordsFactory.GetCoords(1, 12), CoordsFactory.GetCoords(7, 12), DotColours.Brown),
            new ColourPair(CoordsFactory.GetCoords(6, 8), CoordsFactory.GetCoords(11, 7), DotColours.Gray),
            new ColourPair(CoordsFactory.GetCoords(3, 4), CoordsFactory.GetCoords(6, 3), DotColours.White),
            new ColourPair(CoordsFactory.GetCoords(8, 7), CoordsFactory.GetCoords(12, 9), DotColours.Lime),
            new ColourPair(CoordsFactory.GetCoords(2, 3), CoordsFactory.GetCoords(7, 3), DotColours.Wheat),
            new ColourPair(CoordsFactory.GetCoords(4, 8), CoordsFactory.GetCoords(5, 10), DotColours.CornflowerBlue)
        };

        private static readonly ColourPair[] ColourPairsFor14X14SampleGrid =
        {
            new ColourPair(CoordsFactory.GetCoords(2, 4), CoordsFactory.GetCoords(6, 4), DotColours.Blue),
            new ColourPair(CoordsFactory.GetCoords(0, 13), CoordsFactory.GetCoords(13, 13), DotColours.Orange),
            new ColourPair(CoordsFactory.GetCoords(2, 3), CoordsFactory.GetCoords(3, 5), DotColours.Red),
            new ColourPair(CoordsFactory.GetCoords(9, 10), CoordsFactory.GetCoords(12, 11), DotColours.Green),
            new ColourPair(CoordsFactory.GetCoords(0, 12), CoordsFactory.GetCoords(2, 5), DotColours.Cyan),
            new ColourPair(CoordsFactory.GetCoords(2, 11), CoordsFactory.GetCoords(4, 10), DotColours.Yellow),
            new ColourPair(CoordsFactory.GetCoords(2, 10), CoordsFactory.GetCoords(2, 2), DotColours.Magenta),
            new ColourPair(CoordsFactory.GetCoords(5, 12), CoordsFactory.GetCoords(10, 12), DotColours.MediumPurple),
            new ColourPair(CoordsFactory.GetCoords(4, 7), CoordsFactory.GetCoords(5, 10), DotColours.Brown),
            new ColourPair(CoordsFactory.GetCoords(0, 0), CoordsFactory.GetCoords(6, 10), DotColours.Gray),
            new ColourPair(CoordsFactory.GetCoords(4, 12), CoordsFactory.GetCoords(12, 9), DotColours.White),
            new ColourPair(CoordsFactory.GetCoords(5, 0), CoordsFactory.GetCoords(8, 10), DotColours.Lime),
            new ColourPair(CoordsFactory.GetCoords(8, 9), CoordsFactory.GetCoords(11, 2), DotColours.Wheat)
        };

        public static GridDescription[] SampleGridDescriptions =
        {
            new GridDescription(5, ColourPairsFor5X5SampleGrid, 1, 4, 5),
            new GridDescription(6, ColourPairsFor6X6SampleGrid, 1, 4, 6),
            new GridDescription(7, ColourPairsFor7X7SampleGrid, 1),
            new GridDescription(8, ColourPairsFor8X8SampleGrid, 1),
            new GridDescription(9, ColourPairsFor9X9SampleGrid, 1),
            new GridDescription(10, ColourPairsFor10X10SampleGrid, 1),
            new GridDescription(11, ColourPairsFor11X11SampleGrid, 1),
            new GridDescription(12, ColourPairsFor12X12SampleGrid, 1),
            new GridDescription(13, ColourPairsFor13X13SampleGrid, 1, 10, 14),
            new GridDescription(14, ColourPairsFor14X14SampleGrid, 1)
        };
    }
}
