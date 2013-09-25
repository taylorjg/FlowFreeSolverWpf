using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FlowFreeSolverWpf.Model
{
    public class PathFinder
    {
        private static readonly IEnumerable<Direction> AllDirections = Enum.GetValues(typeof(Direction)).Cast<Direction>();
        private CancellationToken _cancellationToken;

        public PathFinder(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
        }

        public Paths FindAllPaths(Grid grid, Coords startCoords, Coords endCoords, int maxDirectionChanges)
        {
            var paths = new Paths();

            foreach (var direction in AllDirections)
            {
                if (_cancellationToken.IsCancellationRequested) return paths;
                FollowPath(grid, paths, Path.PathWithStartingPoint(startCoords), endCoords, direction, maxDirectionChanges, 0);
            }

            return paths;
        }

        private void FollowPath(Grid grid, Paths paths, Path currentPath, Coords endCoords, Direction direction, int maxDirectionChanges, int numDirectionChanges)
        {
            if (_cancellationToken.IsCancellationRequested) return;

            var nextCoords = currentPath.GetNextCoords(direction);

            if (nextCoords.Equals(endCoords))
            {
                currentPath.AddCoords(nextCoords);
                paths.AddPath(currentPath);
                return;
            }

            if (currentPath.ContainsCoords(nextCoords))
            {
                return;
            }

            if (grid.CoordsAreOffTheGrid(nextCoords))
            {
                return;
            }

            if (grid.IsCellOccupied(nextCoords))
            {
                return;
            }

            currentPath.AddCoords(nextCoords);

            var oppositeDirection = direction.Opposite();
            var directionsToTry = AllDirections.Where(d => d != oppositeDirection);

            foreach (var directionToTry in directionsToTry)
            {
                var newNumDirectionChanges = numDirectionChanges + (directionToTry != direction ? 1 : 0);
                if (newNumDirectionChanges <= maxDirectionChanges)
                {
                    FollowPath(grid, paths, Path.CopyOfPath(currentPath), endCoords, directionToTry, maxDirectionChanges, newNumDirectionChanges);
                }
            }
        }
    }
}
