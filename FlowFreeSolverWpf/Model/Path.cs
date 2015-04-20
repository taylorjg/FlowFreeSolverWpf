using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
//using System.Collections;

namespace FlowFreeSolverWpf.Model
{
    public class Path
    {
        private readonly Coords[] _coordsList;
        private readonly Direction _direction;
        private readonly bool _isActive;
        //private readonly BitArray _bitArray;

        private Path(Coords[] coordsList, Direction direction, bool isActive)
        {
            _coordsList = coordsList;
            _direction = direction;
            _isActive = isActive;
        }

        public static Path PathWithStartCoordsAndDirection(Coords startCoords, Direction direction)
        {
            return new Path(new[] {startCoords}, direction, true);
        }

        public Path PathWithNewCoordsAndDirection(Coords newCoords, Direction direction, int maxDirectionChanges)
        {
            var isActive = PathUtils.IsActiveWithNewCoords(this, newCoords, direction, maxDirectionChanges);
            return new Path(CoordsList.Concat(new[] { newCoords }).ToArray(), direction, isActive);
        }

        public Path PathWithEndCoords(Coords endCoords)
        {
            return new Path(CoordsList.Concat(new[] {endCoords}).ToArray(), Direction, true);
        }

        public bool ContainsCoords(Coords coords)
        {
            return _coordsList.Any(c => c.Equals(coords));
        }

        public IReadOnlyList<Coords> CoordsList
        {
            get { return new ReadOnlyCollection<Coords>(_coordsList); }
        }

        public Direction Direction
        {
            get { return _direction; }
        }

        public bool IsActive
        {
            get { return _isActive; }
        }

        public Coords GetNextCoords(Direction direction)
        {
            return PathUtils.GetNextCoords(_coordsList, Direction);
        }

        public int NumDirectionChanges {
            get
            {
                return PathUtils.NumDirectionChanges(_coordsList);
            }
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
