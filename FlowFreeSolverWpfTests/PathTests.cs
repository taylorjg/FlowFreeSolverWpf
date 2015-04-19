using System.Linq;
using FlowFreeSolverWpf.Model;
using NUnit.Framework;

namespace FlowFreeSolverWpfTests
{
    [TestFixture]
    internal class PathTests
    {
        [Test]
        public void NumDirectionChangesIsZeroWhenThePathContainsZeroLocation()
        {
            var path = new Path(Enumerable.Empty<Coords>(), Direction.Up, true);
            Assert.That(path.NumDirectionChanges, Is.EqualTo(0));
        }

        [Test]
        public void NumDirectionChangesIsZeroWhenThePathContainsOneLocation()
        {
            var path = new Path(new[]
            {
                CoordsFactory.GetCoords(3, 3)
            }, Direction.Up, true);
            Assert.That(path.NumDirectionChanges, Is.EqualTo(0));
        }

        [Test]
        public void NumDirectionChangesIsZeroWhenThePathContainsTwoLocations()
        {
            var path = new Path(new[]
            {
                CoordsFactory.GetCoords(3, 3),
                CoordsFactory.GetCoords(4, 3)
            }, Direction.Right, true);
            Assert.That(path.NumDirectionChanges, Is.EqualTo(0));
        }

        [Test]
        public void NumDirectionChangesIsZeroWhenThePathContainsThreeLocationsInALine()
        {
            var path = new Path(new[]
            {
                CoordsFactory.GetCoords(3, 3),
                CoordsFactory.GetCoords(4, 3),
                CoordsFactory.GetCoords(5, 3)
            }, Direction.Right, true);
            Assert.That(path.NumDirectionChanges, Is.EqualTo(0));
        }

        [Test]
        public void NumDirectionChangesIsOneWhenThePathContainsThreeLocationsInASmallRightAngle()
        {
            var path = new Path(new[]
            {
                CoordsFactory.GetCoords(3, 3),
                CoordsFactory.GetCoords(4, 3),
                CoordsFactory.GetCoords(4, 4)
            }, Direction.Up, true);
            Assert.That(path.NumDirectionChanges, Is.EqualTo(1));
        }

        [Test]
        public void NumDirectionChangesIsOneWhenThePathContainsThreeLocationsInAnLargeRightAngle()
        {
            var path = new Path(new[]
            {
                CoordsFactory.GetCoords(3, 3),
                CoordsFactory.GetCoords(4, 3),
                CoordsFactory.GetCoords(5, 3),
                CoordsFactory.GetCoords(6, 3),
                CoordsFactory.GetCoords(6, 4),
                CoordsFactory.GetCoords(6, 5),
                CoordsFactory.GetCoords(6, 6)
            }, Direction.Up, true);
            Assert.That(path.NumDirectionChanges, Is.EqualTo(1));
        }

        [Test]
        public void NumDirectionChangesIsTwoWhenTheDirectionChangesTwice()
        {
            var path = new Path(new[]
            {
                CoordsFactory.GetCoords(3, 3),
                CoordsFactory.GetCoords(4, 3),
                CoordsFactory.GetCoords(5, 3),
                CoordsFactory.GetCoords(6, 3),
                CoordsFactory.GetCoords(6, 4),
                CoordsFactory.GetCoords(6, 5),
                CoordsFactory.GetCoords(5, 5),
                CoordsFactory.GetCoords(4, 5)
            }, Direction.Left, true);
            Assert.That(path.NumDirectionChanges, Is.EqualTo(2));
        }
    }
}
