using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FlowFreeSolverWpf.Model
{
    public class Path
    {
        private readonly ReadOnlyCollection<Coords> _coordsList;
        private readonly Direction _direction;
        private readonly bool _isActive;

        public Path(IEnumerable<Coords> coordsList, Direction direction, bool isActive)
        {
            _coordsList = new ReadOnlyCollection<Coords>(coordsList.ToList());
            _direction = direction;
            _isActive = isActive;
        }

        public static Path PathWithStartingPointAndDirection(Coords startingPoint, Direction direction)
        {
            return new Path(new[]{startingPoint}, direction, true);
        }

        public Path PathWithNewCoordsAndDirection(Coords newCoords, Direction direction, bool isActive)
        {
            return new Path(CoordsList.Concat(new[] {newCoords}), direction, isActive);
        }

        public bool ContainsCoords(Coords coords)
        {
            return _coordsList.Any(c => c == coords);
        }

        public IEnumerable<Coords> CoordsList {
            get { return _coordsList; }
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
            var lastCoords = _coordsList.Last();

            switch (direction)
            {
                case Direction.Up:
                    return CoordsFactory.GetCoords(lastCoords.X, lastCoords.Y + 1);

                case Direction.Down:
                    return CoordsFactory.GetCoords(lastCoords.X, lastCoords.Y - 1);

                case Direction.Left:
                    return CoordsFactory.GetCoords(lastCoords.X - 1, lastCoords.Y);

                case Direction.Right:
                    return CoordsFactory.GetCoords(lastCoords.X + 1, lastCoords.Y);

                default:
                    throw new InvalidOperationException("Unknown direction");
            }
        }

        public int NumDirectionChanges {
            get
            {
                var numDirectionChanges = 0;
                for (var i = 1; i < _coordsList.Count - 1; i++)
                {
                    var coords1 = _coordsList[i - 1];
                    var coords2 = _coordsList[i];
                    var coords3 = _coordsList[i + 1];
                    var direction1 = DirectionOfTravel(coords1, coords2);
                    var direction2 = DirectionOfTravel(coords2, coords3);
                    if (direction1 != direction2)
                    {
                        numDirectionChanges++;
                    }
                }
                return numDirectionChanges;
            }
        }

        private static Direction DirectionOfTravel(Coords coords1, Coords coords2)
        {
            var absX = Math.Abs(coords1.X - coords2.X);
            var absY = Math.Abs(coords1.Y - coords2.Y);

            if (absX + absY != 1)
            {
                throw new InvalidOperationException("Coords are not neighnours!");
            }

            if (coords1.X == coords2.X)
            {
                return coords1.Y < coords2.Y ? Direction.Up : Direction.Down;
            }

            return coords1.X < coords2.X ? Direction.Right : Direction.Left;
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
