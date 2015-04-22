using System.Collections.Generic;
using System.Linq;
using FlowFreeSolverWpf.Model;
using NUnit.Framework;

namespace FlowFreeSolverWpfTests
{
    [TestFixture]
    internal class PathTests
    {
        [Test]
        public void NumDirectionChangesIsZeroWhenThePathContainsOneLocation()
        {
            var path = MakePath(
                new[]
                {
                    CoordsFactory.GetCoords(3, 3)
                },
                Direction.Right);
            Assert.That(path.NumDirectionChanges, Is.EqualTo(0));
        }

        [Test]
        public void NumDirectionChangesIsZeroWhenThePathContainsTwoLocations()
        {
            var path = MakePath(
                new[]
                {
                    CoordsFactory.GetCoords(3, 3),
                    CoordsFactory.GetCoords(4, 3)
                },
                Direction.Right);
            Assert.That(path.NumDirectionChanges, Is.EqualTo(0));
        }

        [Test]
        public void NumDirectionChangesIsZeroWhenThePathContainsThreeLocationsInALine()
        {
            var path = MakePath(
                new[]
                {
                    CoordsFactory.GetCoords(3, 3),
                    CoordsFactory.GetCoords(4, 3),
                    CoordsFactory.GetCoords(5, 3)
                },
                Direction.Right);
            Assert.That(path.NumDirectionChanges, Is.EqualTo(0));
        }

        [Test]
        public void NumDirectionChangesIsOneWhenThePathContainsThreeLocationsInASmallRightAngle()
        {
            var path = MakePath(
                new[]
                {
                    CoordsFactory.GetCoords(3, 3),
                    CoordsFactory.GetCoords(4, 3),
                    CoordsFactory.GetCoords(4, 4)
                },
                Direction.Right);
            Assert.That(path.NumDirectionChanges, Is.EqualTo(1));
        }

        [Test]
        public void NumDirectionChangesIsOneWhenThePathContainsThreeLocationsInAnLargeRightAngle()
        {
            var path = MakePath(
                new[]
                {
                    CoordsFactory.GetCoords(3, 3),
                    CoordsFactory.GetCoords(4, 3),
                    CoordsFactory.GetCoords(5, 3),
                    CoordsFactory.GetCoords(6, 3),
                    CoordsFactory.GetCoords(6, 4),
                    CoordsFactory.GetCoords(6, 5),
                    CoordsFactory.GetCoords(6, 6)
                },
                Direction.Right);
            Assert.That(path.NumDirectionChanges, Is.EqualTo(1));
        }

        [Test]
        public void NumDirectionChangesIsTwoWhenTheDirectionChangesTwice()
        {
            var path = MakePath(
                new[]
                {
                    CoordsFactory.GetCoords(3, 3),
                    CoordsFactory.GetCoords(4, 3),
                    CoordsFactory.GetCoords(5, 3),
                    CoordsFactory.GetCoords(6, 3),
                    CoordsFactory.GetCoords(6, 4),
                    CoordsFactory.GetCoords(6, 5),
                    CoordsFactory.GetCoords(5, 5),
                    CoordsFactory.GetCoords(4, 5)
                },
                Direction.Right);
            Assert.That(path.NumDirectionChanges, Is.EqualTo(2));
        }

        private static Path MakePath(IReadOnlyList<Coords> coordsList, Direction initialDirection)
        {
            var path = Path.PathWithStartCoordsAndDirection(coordsList.First(), initialDirection);

            for (var index = 1; index < coordsList.Count; index++)
            {
                var coords1 = coordsList[index - 1];
                var coords2 = coordsList[index];
                var direction = PathUtils.DirectionOfTravel(coords1, coords2);
                path = path.PathWithNewCoordsAndDirection(coords2, direction, 100);
            }

            return path;
        }
    }
}
