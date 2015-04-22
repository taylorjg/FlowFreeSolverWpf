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
            yield return Path.PathWithStartCoordsAndDirection(colourPair.StartCoords, Direction.Up);
            yield return Path.PathWithStartCoordsAndDirection(colourPair.StartCoords, Direction.Down);
            yield return Path.PathWithStartCoordsAndDirection(colourPair.StartCoords, Direction.Left);
            yield return Path.PathWithStartCoordsAndDirection(colourPair.StartCoords, Direction.Right);
        }

        public IEnumerable<Path> FindAllPaths(
            Grid grid,
            Coords endCoords,
            IEnumerable<Path> paths,
            int maxDirectionChanges)
        {
            return paths.SelectMany(path => FollowPath(grid, endCoords, path, maxDirectionChanges));
        }

        private IEnumerable<Path> FollowPath(
            Grid grid,
            Coords endCoords,
            Path path,
            int maxDirectionChanges)
        {
            _cancellationToken.ThrowIfCancellationRequested();

            var newPaths = new List<Path>();
            var nextCoords = path.GetNextCoords();

            if (nextCoords.Equals(endCoords))
            {
                var newPath = path.PathWithEndCoords(endCoords);
                newPaths.Add(newPath);
                return newPaths;
            }

            if (grid.CoordsAreOffTheGrid(nextCoords) ||
                grid.IsCellOccupied(nextCoords) ||
                path.ContainsCoords(nextCoords))
            {
                return newPaths;
            }

            var directionsToTry = path.Direction.DirectionsToTry();

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = 0; index < directionsToTry.Length; index++)
            {
                var directionToTry = directionsToTry[index];
                var newPath = path.PathWithNewCoordsAndDirection(nextCoords, directionToTry, maxDirectionChanges);

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
