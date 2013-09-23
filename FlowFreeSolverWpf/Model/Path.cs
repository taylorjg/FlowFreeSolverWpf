using System;
using System.Collections.Generic;
using System.Linq;

namespace FlowFreeSolverWpf.Model
{
    public class Path
    {
        private readonly IList<Coords> _coordsList = new List<Coords>();

        public static Path PathWithStartingPoint(Coords startingPoint)
        {
            var path = new Path();
            path.AddCoords(startingPoint);
            return path;
        }

        public static Path CopyOfPath(Path originalPath)
        {
            var copyOfPath = new Path();
            foreach (var coords in originalPath.CoordsList)
            {
                copyOfPath.AddCoords(coords);
            }
            return copyOfPath;
        }

        public void AddCoords(Coords coords)
        {
            _coordsList.Add(coords);
        }

        public bool ContainsCoords(Coords coords)
        {
            return _coordsList.Any(c => c.Equals(coords));
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
                    return new Coords(lastCoords.X, lastCoords.Y + 1);

                case Direction.Down:
                    return new Coords(lastCoords.X, lastCoords.Y - 1);

                case Direction.Left:
                    return new Coords(lastCoords.X - 1, lastCoords.Y);

                case Direction.Right:
                    return new Coords(lastCoords.X + 1, lastCoords.Y);

                default:
                    throw new InvalidOperationException("Unknown direction");
            }
        }
    }
}
