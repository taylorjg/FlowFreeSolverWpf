using FlowFreeSolverWpf.Model;
using NUnit.Framework;

namespace FlowFreeSolverWpfTests
{
    [TestFixture]
    internal class CoordsFactoryTests
    {
        [Test]
        public void CoordsAreInterned()
        {
            var coords1 = CoordsFactory.GetCoords(1, 2);
            var coords2 = CoordsFactory.GetCoords(1, 2);
            Assert.That(coords1, Is.SameAs(coords2));
        }

        [Test]
        public void CanPrimeCache()
        {
            CoordsFactory.PrimeCache(9);
            var actual = CoordsFactory.GetCacheSize();
            Assert.That(actual, Is.EqualTo(9 * 9));
        }
    }
}
