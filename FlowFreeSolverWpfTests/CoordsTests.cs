using FlowFreeSolverWpf.Model;
using NUnit.Framework;

namespace FlowFreeSolverWpfTests
{
    [TestFixture]
    public class CoordsTests
    {
        [Test]
        public void Test1()
        {
            var c = new Coords(1, 2);
            Assert.That(c.X, Is.EqualTo(1));
            Assert.That(c.Y, Is.EqualTo(2));
        }

        [Test, Ignore("Currently, this test fails because c.Y returns 255 instead of -1")]
        public void Test2()
        {
            var c = new Coords(-1, -1);
            Assert.That(c.X, Is.EqualTo(-1));
            Assert.That(c.Y, Is.EqualTo(-1));
        }
    }
}
