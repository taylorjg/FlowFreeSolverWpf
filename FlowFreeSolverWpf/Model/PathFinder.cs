using System.Collections.Generic;
using System.Linq;
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

        public static IEnumerable<Path> InitialPaths(ColourPair colourPair)
        {
            yield return Path.PathWithStartingPointAndDirection(colourPair.StartCoords, Direction.Up);
            yield return Path.PathWithStartingPointAndDirection(colourPair.StartCoords, Direction.Down);
            yield return Path.PathWithStartingPointAndDirection(colourPair.StartCoords, Direction.Left);
            yield return Path.PathWithStartingPointAndDirection(colourPair.StartCoords, Direction.Right);
        }

        public Paths FindAllPaths(
            Grid grid,
            Coords endCoords,
            IEnumerable<Path> activePaths,
            int maxDirectionChanges)
        {
            var flattenedPaths = activePaths.SelectMany(activePath => FollowPath(grid, endCoords, activePath, maxDirectionChanges));
            var paths = new Paths();
            foreach (var path in flattenedPaths) paths.AddPath(path);
            return paths;
        }

        private IEnumerable<Path> FollowPath(
            Grid grid,
            Coords endCoords,
            Path activePath,
            int maxDirectionChanges)
        {
            _cancellationToken.ThrowIfCancellationRequested();

            var newPaths = new List<Path>();

            var nextCoords = activePath.GetNextCoords(activePath.Direction);

            if (nextCoords == endCoords)
            {
                var newPath = activePath.PathWithNewCoordsAndDirection(nextCoords, activePath.Direction, true);
                newPaths.Add(newPath);
                return newPaths;
            }

            if (activePath.ContainsCoords(nextCoords) ||
                grid.CoordsAreOffTheGrid(nextCoords) ||
                grid.IsCellOccupied(nextCoords))
            {
                return newPaths;
            }

            var directionsToTry = activePath.Direction.DirectionsToTry();

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = 0; index < directionsToTry.Length; index++)
            {
                var directionToTry = directionsToTry[index];
                var oldNumDirectionChanges = activePath.NumDirectionChanges;
                var newNumDirectionChanges = oldNumDirectionChanges + ((directionToTry != activePath.Direction) ? 1 : 0);
                var isActive = (newNumDirectionChanges <= maxDirectionChanges);
                var newPath = activePath.PathWithNewCoordsAndDirection(nextCoords, directionToTry, isActive);

                if (newPath.IsActive)
                {
                    var recursiveNewPaths = FollowPath(
                        grid,
                        endCoords,
                        newPath,
                        maxDirectionChanges);
                    newPaths.AddRange(recursiveNewPaths);
                }
                else
                {
                    newPaths.Add(newPath);
                }
            }

            return newPaths;
        }
    }
}
