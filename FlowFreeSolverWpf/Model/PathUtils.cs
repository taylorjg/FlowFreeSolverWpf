using System;
using System.Linq;

namespace FlowFreeSolverWpf.Model
{
    public static class PathUtils
    {
        public static Coords GetNextCoords(Coords[] coordsList, Direction currentDirection)
        {
            var currentCoords = coordsList.Last();

            switch (currentDirection)
            {
                case Direction.Up:
                    return CoordsFactory.GetCoords(currentCoords.X, currentCoords.Y + 1);

                case Direction.Down:
                    return CoordsFactory.GetCoords(currentCoords.X, currentCoords.Y - 1);

                case Direction.Left:
                    return CoordsFactory.GetCoords(currentCoords.X - 1, currentCoords.Y);

                case Direction.Right:
                    return CoordsFactory.GetCoords(currentCoords.X + 1, currentCoords.Y);

                default:
                    throw new InvalidOperationException("Unknown direction");
            }
        }

        public static int NumDirectionChanges(Coords[] coordsList)
        {
            var numDirectionChanges = 0;

            for (var i = 1; i < coordsList.Length - 1; i++)
            {
                var coords1 = coordsList[i - 1];
                var coords2 = coordsList[i];
                var coords3 = coordsList[i + 1];

                var direction1 = DirectionOfTravel(coords1, coords2);
                var direction2 = DirectionOfTravel(coords2, coords3);

                if (direction1 != direction2)
                {
                    numDirectionChanges++;
                }
            }

            return numDirectionChanges;
        }

        public static bool IsActiveWithNewCoords(Path path, Coords newCoords, Direction newDirection, int maxDirectionChanges)
        {
            var oldDirection = path.Direction;
            var oldNumDirectionChanges = path.NumDirectionChanges;
            var newNumDirectionChanges = oldNumDirectionChanges + ((newDirection != oldDirection) ? 1 : 0);
            return newNumDirectionChanges <= maxDirectionChanges;
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
    }
}
