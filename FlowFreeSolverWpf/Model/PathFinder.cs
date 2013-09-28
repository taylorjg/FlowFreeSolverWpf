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

        public Paths FindAllPaths(
            Grid grid,
            Coords startCoords,
            Coords endCoords,
            IList<Path> abandonedPaths,
            int maxDirectionChanges)
        {
            var paths = new Paths();

            if (abandonedPaths.Any())
            {
                foreach (var abandonedPath in abandonedPaths)
                {
                    if (!abandonedPath.LastDirection.HasValue) continue;

                    var oppositeDirection = abandonedPath.LastDirection.Value.Opposite();
                    var directionsToTry = AllDirections.Where(d => d != oppositeDirection);

                    foreach (var direction in directionsToTry)
                    {
                        var copyOfAbandonedPath = Path.CopyOfPath(abandonedPath);
                        FollowPath(grid, paths, copyOfAbandonedPath, endCoords, direction, maxDirectionChanges, copyOfAbandonedPath.NumDirectionChanges);
                    }
                }
            }
            else
            {
                foreach (var direction in AllDirections)
                {
                    FollowPath(grid, paths, Path.PathWithStartingPoint(startCoords), endCoords, direction, maxDirectionChanges, 0);
                }
            }

            return paths;
        }

        private void FollowPath(
            Grid grid,
            Paths paths,
            Path currentPath,
            Coords endCoords,
            Direction direction,
            int maxDirectionChanges,
            int numDirectionChanges)
        {
            if (_cancellationToken.IsCancellationRequested) return;

            var nextCoords = currentPath.GetNextCoords(direction);

            if (nextCoords.Equals(endCoords))
            {
                currentPath.AddCoords(nextCoords);
                currentPath.IsAbandoned = false;
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
                    FollowPath(
                        grid,
                        paths,
                        Path.CopyOfPath(currentPath),
                        endCoords,
                        directionToTry,
                        maxDirectionChanges,
                        newNumDirectionChanges);
                }
                else
                {
                    currentPath.IsAbandoned = true;
                    currentPath.LastDirection = direction;
                    currentPath.NumDirectionChanges = newNumDirectionChanges;
                    paths.AddPath(currentPath);
                }
            }
        }
    }
}
