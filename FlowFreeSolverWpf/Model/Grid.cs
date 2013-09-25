using System.Collections.Generic;

namespace FlowFreeSolverWpf.Model
{
    public class Grid
    {
        public int GridSize { get; private set; }
        public IEnumerable<ColourPair> ColourPairs { get; private set; }
        private readonly string[,] _cells;

        public Grid(int gridSize, params ColourPair[] colourPairs)
        {
            GridSize = gridSize;
            ColourPairs = colourPairs;
            _cells = new string[GridSize, GridSize];

            foreach (var colourPair in colourPairs)
            {
                SetCellContents(colourPair.StartCoords, colourPair);
                SetCellContents(colourPair.EndCoords, colourPair);
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
                coords.X >= GridSize ||
                coords.Y >= GridSize;
        }
    }
}
