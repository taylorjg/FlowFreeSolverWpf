using System;
using System.Linq;

namespace FlowFreeSolverWpf.Model
{
    public class Path
    {
        private readonly Coords[] _coordsList;
        private readonly int _bits;

        private const int DirectionMask = 0x000000FF;
        private const int NumDirectionChangesMask = 0x0000FF00;
        private const int IsActiveMask = 0x00010000;

        private Path(Coords[] coordsList, Direction direction, int numDirectionChanges, bool isActive)
        {
            _coordsList = coordsList;
            _bits = (Byte) direction | (((Byte) numDirectionChanges) << 8) | ((isActive ? 1 : 0) << 16);
        }

        public static Path PathWithStartCoordsAndDirection(Coords startCoords, Direction direction)
        {
            return new Path(new[] {startCoords}, direction, 0, true);
        }

        public Path PathWithNewCoordsAndDirection(Coords newCoords, Direction direction, int maxDirectionChanges)
        {
            var isActive = PathUtils.IsActiveWithNewCoords(this, newCoords, direction, maxDirectionChanges);
            var newNumDirectionChanges = NumDirectionChanges + (direction != Direction ? 1 : 0);
            return new Path(CoordsList.Concat(new[] { newCoords }).ToArray(), direction, newNumDirectionChanges, isActive);
        }

        public Path PathWithEndCoords(Coords endCoords)
        {
            return new Path(CoordsList.Concat(new[] { endCoords }).ToArray(), Direction, NumDirectionChanges, true);
        }

        public bool ContainsCoords(Coords coords)
        {
            return _coordsList.Any(c => c.Equals(coords));
        }

        public Coords[] CoordsList
        {
            get { return _coordsList; }
        }

        public Direction Direction
        {
            get { return (Direction) (_bits & DirectionMask); }
        }

        public int NumDirectionChanges
        {
            get { return (_bits & NumDirectionChangesMask) >> 8; }
        }

        public bool IsActive
        {
            get { return (_bits & IsActiveMask) != 0; }
        }

        public Coords GetNextCoords()
        {
            return PathUtils.GetNextCoords(_coordsList, Direction);
        }

        public override string ToString()
        {
            return string.Format(
                "CoordsList: {0}; Direction: {1}; IsActive: {2}; NumDirectionChanges: {3}",
                CoordsListToString(),
                Direction,
                IsActive,
                NumDirectionChanges);
        }

        private string CoordsListToString()
        {
            return string.Join(", ", CoordsList.Select(c => c.ToString()).ToArray());
        }
    }
}
