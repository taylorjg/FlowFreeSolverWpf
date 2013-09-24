﻿using System;
using System.Linq;

namespace FlowFreeSolverWpf.Model
{
    public class PathFinder
    {
        public static Paths FindAllPaths(Grid grid, Coords startCoords, Coords endCoords)
        {
            var paths = new Paths();

            FollowPath(grid, paths, Path.PathWithStartingPoint(startCoords), endCoords, Direction.Up, 0);
            FollowPath(grid, paths, Path.PathWithStartingPoint(startCoords), endCoords, Direction.Down, 0);
            FollowPath(grid, paths, Path.PathWithStartingPoint(startCoords), endCoords, Direction.Left, 0);
            FollowPath(grid, paths, Path.PathWithStartingPoint(startCoords), endCoords, Direction.Right, 0);

            return paths;
        }

        private static void FollowPath(Grid grid, Paths paths, Path currentPath, Coords endCoords, Direction direction, int numDirectionChanges)
        {
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

            var maxDirectionChanges = 10;

            switch (grid.Width * grid.Height)
            {
                case 25:
                    maxDirectionChanges = 5;
                    break;
                case 36:
                    maxDirectionChanges = 6;
                    break;
                case 49:
                    maxDirectionChanges = 7;
                    break;
                case 64:
                    maxDirectionChanges = 8;
                    break;
                case 81:
                    maxDirectionChanges = 9;
                    break;
                case 100:
                    maxDirectionChanges = 10;
                    break;
            }

            foreach (var directionToTry in directionsToTry)
            {
                var newNumDirectionChanges = numDirectionChanges + (directionToTry != direction ? 1 : 0);
                if (newNumDirectionChanges <= maxDirectionChanges)
                {
                    FollowPath(grid, paths, Path.CopyOfPath(currentPath), endCoords, directionToTry, newNumDirectionChanges);
                }
            }
        }
    }
}
