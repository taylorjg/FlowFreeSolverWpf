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
                Direction.Up);
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
                Direction.Up);
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
                Direction.Up);
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
                Direction.Left);
            Assert.That(path.NumDirectionChanges, Is.EqualTo(2));
        }

        private static Path MakePath(IReadOnlyCollection<Coords> coordsList, Direction direction)
        {
            return coordsList
                .Skip(1)
                .Aggregate(
                    Path.PathWithStartCoordsAndDirection(coordsList.First(), direction),
                    (path, newCoords) => path.PathWithNewCoordsAndDirection(newCoords, direction, 100));
        }
    }
}
