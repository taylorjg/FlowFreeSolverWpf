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
            IList<Path> activePaths,
            int maxDirectionChanges)
        {
            var resultantPaths = new Paths();

            if (!activePaths.Any())
            {
                activePaths.Add(Path.PathWithStartingPointAndDirection(startCoords, Direction.Up));
                activePaths.Add(Path.PathWithStartingPointAndDirection(startCoords, Direction.Down));
                activePaths.Add(Path.PathWithStartingPointAndDirection(startCoords, Direction.Left));
                activePaths.Add(Path.PathWithStartingPointAndDirection(startCoords, Direction.Right));
            }

            foreach (var activePath in activePaths)
            {
                FollowPath(
                    grid,
                    resultantPaths,
                    activePath,
                    endCoords,
                    maxDirectionChanges);
            }

            return resultantPaths;
        }

        private void FollowPath(
            Grid grid,
            Paths resultantPaths,
            Path activePath,
            Coords endCoords,
            int maxDirectionChanges)
        {
            if (_cancellationToken.IsCancellationRequested) return;

            var nextCoords = activePath.GetNextCoords(activePath.Direction);

            if (nextCoords == endCoords)
            {
                activePath.AddCoords(nextCoords);
                activePath.IsAbandoned = false;
                resultantPaths.AddPath(activePath);
                return;
            }

            if (activePath.ContainsCoords(nextCoords) ||
                grid.CoordsAreOffTheGrid(nextCoords) ||
                grid.IsCellOccupied(nextCoords))
            {
                return;
            }

            var oppositeDirection = activePath.Direction.Opposite();
            var directionsToTry = AllDirections.Where(direction => direction != oppositeDirection);

            foreach (var directionToTry in directionsToTry)
            {
                var copyOfActivePath = Path.CopyOfPath(activePath);
                copyOfActivePath.AddCoords(nextCoords);
                copyOfActivePath.IsAbandoned = false;
                copyOfActivePath.Direction = directionToTry;

                if (copyOfActivePath.NumDirectionChanges <= maxDirectionChanges)
                {
                    FollowPath(
                        grid,
                        resultantPaths,
                        copyOfActivePath,
                        endCoords,
                        maxDirectionChanges);
                }
                else
                {
                    copyOfActivePath.IsAbandoned = true;
                    resultantPaths.AddPath(copyOfActivePath);
                }
            }
        }
    }
}
