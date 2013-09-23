using System;
using System.Collections.Generic;
using System.Linq;

namespace FlowFreeSolverWpf.Model
{
    public class Grid
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public IEnumerable<ColourPair> ColourPairs { get; private set; }
        private readonly string[,] _cells;

        public Grid(int width, int height, params ColourPair[] colourPairs)
        {
            Width = width;
            Height = height;
            ColourPairs = colourPairs;
            _cells = new string[Width, Height];

            foreach (var colourPair in colourPairs)
            {
                SetCellContents(colourPair.StartCoords, colourPair);
                SetCellContents(colourPair.EndCoords, colourPair);
            }
        }

        public Grid(int width, int height, params Tuple<ColourPair, Path>[] colourPairPaths)
        {
            Width = width;
            Height = height;
            ColourPairs = colourPairPaths.Select(cpp => cpp.Item1);
            _cells = new string[Width, Height];

            foreach (var tuple in colourPairPaths)
            {
                var colourPair = tuple.Item1;
                var path = tuple.Item2;

                foreach (var coords in path.CoordsList)
                {
                    SetCellContents(coords, colourPair);
                }
            }
        }

        private void SetCellContents(Coords coords, ColourPair colourPair)
        {
            _cells[coords.X, coords.Y] = colourPair.Tag;
        }

        public bool IsCellOccupied(Coords coords)
        {
            return _cells[coords.X, coords.Y] != null;
        }

        public string GetTagAtCoords(Coords coords)
        {
            return _cells[coords.X, coords.Y];
        }

        public bool CoordsAreOffTheGrid(Coords coords)
        {
            return
                coords.X < 0 ||
                coords.Y < 0 ||
                coords.X >= Width ||
                coords.Y >= Height;
        }
    }
}
