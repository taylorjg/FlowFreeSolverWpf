using System;

namespace FlowFreeSolverWpf.Model
{
    public static class DirectionExtensions
    {
        public static Direction Opposite(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
                default:
                    throw new InvalidOperationException("Unknown direction");
            }
        }

        public static Direction[] DirectionsToTry(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return DirectionsToTryWhenGoingUp;
                case Direction.Down:
                    return DirectionsToTryWhenGoingDown;
                case Direction.Left:
                    return DirectionsToTryWhenGoingLeft;
                case Direction.Right:
                    return DirectionsToTryWhenGoingRight;
                default:
                    throw new InvalidOperationException("Unknown direction");
            }
        }

        private static readonly Direction[] DirectionsToTryWhenGoingUp =
        {
            Direction.Up,
            Direction.Left,
            Direction.Right
        };

        private static readonly Direction[] DirectionsToTryWhenGoingDown =
        {
            Direction.Down,
            Direction.Left,
            Direction.Right
        };

        private static readonly Direction[] DirectionsToTryWhenGoingLeft =
        {
            Direction.Left,
            Direction.Up,
            Direction.Down
        };

        private static readonly Direction[] DirectionsToTryWhenGoingRight =
        {
            Direction.Right,
            Direction.Up,
            Direction.Down
        };
    }
}
