using System;
using System.Linq;
using System.Threading;

namespace FlowFreeSolverWpf.Model
{
    public class PathFinder
    {
        private CancellationToken _cancellationToken;

        public PathFinder(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
        }

        public Paths FindAllPaths(Grid grid, Coords startCoords, Coords endCoords)
        {
            var paths = new Paths();

            FollowPath(grid, paths, Path.PathWithStartingPoint(startCoords), endCoords, Direction.Up, 0);
            if (_cancellationToken.IsCancellationRequested) return paths;
            FollowPath(grid, paths, Path.PathWithStartingPoint(startCoords), endCoords, Direction.Down, 0);
            if (_cancellationToken.IsCancellationRequested) return paths;
            FollowPath(grid, paths, Path.PathWithStartingPoint(startCoords), endCoords, Direction.Left, 0);
            if (_cancellationToken.IsCancellationRequested) return paths;
            FollowPath(grid, paths, Path.PathWithStartingPoint(startCoords), endCoords, Direction.Right, 0);

            return paths;
        }

        private void FollowPath(Grid grid, Paths paths, Path currentPath, Coords endCoords, Direction direction, int numDirectionChanges)
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

            var allDirections = Enum.GetValues(typeof(Direction)).Cast<Direction>();
            var oppositeDirection = direction.Opposite();
            var directionsToTry = allDirections.Where(d => d != oppositeDirection);

            var maxDirectionChanges = GetMaxDirectionChanges(grid);

            foreach (var directionToTry in directionsToTry)
            {
                var newNumDirectionChanges = numDirectionChanges + (directionToTry != direction ? 1 : 0);
                if (newNumDirectionChanges <= maxDirectionChanges)
                {
                    FollowPath(grid, paths, Path.CopyOfPath(currentPath), endCoords, directionToTry, newNumDirectionChanges);
                }
            }
        }

        private static int GetMaxDirectionChanges(Grid grid)
        {
            var maxDirectionChanges = 10;

            const int Base = 4;

            switch (grid.Width * grid.Height)
            {
                case 25:
                    maxDirectionChanges = Base + 0;
                    break;
                case 36:
                    maxDirectionChanges = Base + 1;
                    break;
                case 49:
                    maxDirectionChanges = Base + 2;
                    break;
                case 64:
                    maxDirectionChanges = Base + 3;
                    break;
                case 81:
                    maxDirectionChanges = Base + 4;
                    break;
                case 100:
                    maxDirectionChanges = Base + 5;
                    break;
            }

            return maxDirectionChanges;
        }
    }
}
