using System.Collections.Generic;
using System.Threading;

namespace FlowFreeSolverWpf.Model
{
    public class PathFinder
    {
        private readonly CancellationToken _cancellationToken;

        public PathFinder(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
        }

        public static List<Path> InitialPaths(ColourPair colourPair)
        {
            return new List<Path>
            {
                Path.PathWithStartingPointAndDirection(colourPair.StartCoords, Direction.Up),
                Path.PathWithStartingPointAndDirection(colourPair.StartCoords, Direction.Down),
                Path.PathWithStartingPointAndDirection(colourPair.StartCoords, Direction.Left),
                Path.PathWithStartingPointAndDirection(colourPair.StartCoords, Direction.Right)
            };
        }

        public Paths FindAllPaths(
            Grid grid,
            Coords startCoords,
            Coords endCoords,
            IList<Path> activePaths,
            int maxDirectionChanges)
        {
            var resultantPaths = new Paths();

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
            _cancellationToken.ThrowIfCancellationRequested();

            var nextCoords = activePath.GetNextCoords(activePath.Direction);

            if (nextCoords == endCoords)
            {
                activePath.AddCoords(nextCoords);
                activePath.IsActive = true;
                resultantPaths.AddPath(activePath);
                return;
            }

            if (activePath.ContainsCoords(nextCoords) ||
                grid.CoordsAreOffTheGrid(nextCoords) ||
                grid.IsCellOccupied(nextCoords))
            {
                return;
            }

            var directionsToTry = activePath.Direction.DirectionsToTry();

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = 0; index < directionsToTry.Length; index++)
            {
                var directionToTry = directionsToTry[index];
                var copyOfActivePath = Path.CopyOfPath(activePath);
                copyOfActivePath.AddCoords(nextCoords);
                copyOfActivePath.IsActive = true;
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
                    copyOfActivePath.IsActive = false;
                    resultantPaths.AddPath(copyOfActivePath);
                }
            }
        }
    }
}
