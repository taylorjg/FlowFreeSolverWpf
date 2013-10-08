using System;
using System.Collections.Generic;
using System.Linq;

namespace FlowFreeSolverWpf.Model
{
    public class Path
    {
        private readonly IList<Coords> _coordsList = new List<Coords>();

        public static Path PathWithStartingPointAndDirection(Coords startingPoint, Direction direction)
        {
            var path = new Path();
            path.AddCoords(startingPoint);
            path.Direction = direction;
            return path;
        }

        public static Path CopyOfPath(Path originalPath)
        {
            var copyOfPath = new Path();
            foreach (var coords in originalPath.CoordsList)
            {
                copyOfPath.AddCoords(coords);
            }
            copyOfPath.IsAbandoned = originalPath.IsAbandoned;
            copyOfPath.Direction = originalPath.Direction;
            return copyOfPath;
        }

        public void AddCoords(Coords coords)
        {
            _coordsList.Add(coords);
        }

        public bool ContainsCoords(Coords coords)
        {
            return _coordsList.Any(c => c == coords);
        }

        public IEnumerable<Coords> CoordsList {
            get { return _coordsList; }
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

        public bool IsAbandoned { get; set; }
        public Direction Direction { get; set; }

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
                "Coords: {0}; Direction: {1}; IsAbandoned: {2}; NumDirectionChanges: {3}",
                PathCoordsToString(),
                Direction,
                IsAbandoned,
                NumDirectionChanges);
        }

        private string PathCoordsToString()
        {
            return string.Join(", ", CoordsList.Select(c => c.ToString()).ToArray());
        }
    }
}
