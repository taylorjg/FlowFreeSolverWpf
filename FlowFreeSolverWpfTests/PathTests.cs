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
            path.AddCoords(new Coords(3, 3));
            Assert.That(path.NumDirectionChanges, Is.EqualTo(0));
        }

        [Test]
        public void NumDirectionChangesIsZeroWhenThePathContainsTwoLocations()
        {
            var path = new Path();
            path.AddCoords(new Coords(3, 3));
            path.AddCoords(new Coords(4, 3));
            Assert.That(path.NumDirectionChanges, Is.EqualTo(0));
        }

        [Test]
        public void NumDirectionChangesIsZeroWhenThePathContainsThreeLocationsInALine()
        {
            var path = new Path();
            path.AddCoords(new Coords(3, 3));
            path.AddCoords(new Coords(4, 3));
            path.AddCoords(new Coords(5, 3));
            Assert.That(path.NumDirectionChanges, Is.EqualTo(0));
        }

        [Test]
        public void NumDirectionChangesIsOneWhenThePathContainsThreeLocationsInASmallRightAngle()
        {
            var path = new Path();
            path.AddCoords(new Coords(3, 3));
            path.AddCoords(new Coords(4, 3));
            path.AddCoords(new Coords(4, 4));
            Assert.That(path.NumDirectionChanges, Is.EqualTo(1));
        }

        [Test]
        public void NumDirectionChangesIsOneWhenThePathContainsThreeLocationsInAnLargeRightAngle()
        {
            var path = new Path();
            path.AddCoords(new Coords(3, 3));
            path.AddCoords(new Coords(4, 3));
            path.AddCoords(new Coords(5, 3));
            path.AddCoords(new Coords(6, 3));
            path.AddCoords(new Coords(6, 4));
            path.AddCoords(new Coords(6, 5));
            path.AddCoords(new Coords(6, 6));
            Assert.That(path.NumDirectionChanges, Is.EqualTo(1));
        }

        [Test]
        public void NumDirectionChangesIsTwoWhenTheDirectionChangesTwice()
        {
            var path = new Path();
            path.AddCoords(new Coords(3, 3));
            path.AddCoords(new Coords(4, 3));
            path.AddCoords(new Coords(5, 3));
            path.AddCoords(new Coords(6, 3));
            path.AddCoords(new Coords(6, 4));
            path.AddCoords(new Coords(6, 5));
            path.AddCoords(new Coords(5, 5));
            path.AddCoords(new Coords(4, 5));
            Assert.That(path.NumDirectionChanges, Is.EqualTo(2));
        }
    }
}
