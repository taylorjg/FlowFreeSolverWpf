using System.Collections.Generic;
using System.Linq;

namespace FlowFreeSolverWpf.Model
{
    public class Grid
    {
        private readonly int _gridSize;
        private readonly IEnumerable<ColourPair> _colourPairs;

        public Grid(int gridSize, params ColourPair[] colourPairs)
        {
            _gridSize = gridSize;
            _colourPairs = colourPairs;
        }

        public int GridSize
        {
            get { return _gridSize; }
        }

        public IEnumerable<ColourPair> ColourPairs
        {
            get { return _colourPairs; }
        }

        public bool IsCellOccupied(Coords coords)
        {
            return _colourPairs.Any(cp => cp.StartCoords == coords || cp.EndCoords == coords);
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
