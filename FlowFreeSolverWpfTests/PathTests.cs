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
            var path = new Path();
            Assert.That(path.NumDirectionChanges, Is.EqualTo(0));
        }

        [Test]
        public void NumDirectionChangesIsZeroWhenThePathContainsOneLocation()
        {
            var path = new Path();
            path.AddCoords(CoordsFactory.GetCoords(3, 3));
            Assert.That(path.NumDirectionChanges, Is.EqualTo(0));
        }

        [Test]
        public void NumDirectionChangesIsZeroWhenThePathContainsTwoLocations()
        {
            var path = new Path();
            path.AddCoords(CoordsFactory.GetCoords(3, 3));
            path.AddCoords(CoordsFactory.GetCoords(4, 3));
            Assert.That(path.NumDirectionChanges, Is.EqualTo(0));
        }

        [Test]
        public void NumDirectionChangesIsZeroWhenThePathContainsThreeLocationsInALine()
        {
            var path = new Path();
            path.AddCoords(CoordsFactory.GetCoords(3, 3));
            path.AddCoords(CoordsFactory.GetCoords(4, 3));
            path.AddCoords(CoordsFactory.GetCoords(5, 3));
            Assert.That(path.NumDirectionChanges, Is.EqualTo(0));
        }

        [Test]
        public void NumDirectionChangesIsOneWhenThePathContainsThreeLocationsInASmallRightAngle()
        {
            var path = new Path();
            path.AddCoords(CoordsFactory.GetCoords(3, 3));
            path.AddCoords(CoordsFactory.GetCoords(4, 3));
            path.AddCoords(CoordsFactory.GetCoords(4, 4));
            Assert.That(path.NumDirectionChanges, Is.EqualTo(1));
        }

        [Test]
        public void NumDirectionChangesIsOneWhenThePathContainsThreeLocationsInAnLargeRightAngle()
        {
            var path = new Path();
            path.AddCoords(CoordsFactory.GetCoords(3, 3));
            path.AddCoords(CoordsFactory.GetCoords(4, 3));
            path.AddCoords(CoordsFactory.GetCoords(5, 3));
            path.AddCoords(CoordsFactory.GetCoords(6, 3));
            path.AddCoords(CoordsFactory.GetCoords(6, 4));
            path.AddCoords(CoordsFactory.GetCoords(6, 5));
            path.AddCoords(CoordsFactory.GetCoords(6, 6));
            Assert.That(path.NumDirectionChanges, Is.EqualTo(1));
        }

        [Test]
        public void NumDirectionChangesIsTwoWhenTheDirectionChangesTwice()
        {
            var path = new Path();
            path.AddCoords(CoordsFactory.GetCoords(3, 3));
            path.AddCoords(CoordsFactory.GetCoords(4, 3));
            path.AddCoords(CoordsFactory.GetCoords(5, 3));
            path.AddCoords(CoordsFactory.GetCoords(6, 3));
            path.AddCoords(CoordsFactory.GetCoords(6, 4));
            path.AddCoords(CoordsFactory.GetCoords(6, 5));
            path.AddCoords(CoordsFactory.GetCoords(5, 5));
            path.AddCoords(CoordsFactory.GetCoords(4, 5));
            Assert.That(path.NumDirectionChanges, Is.EqualTo(2));
        }
    }
}
