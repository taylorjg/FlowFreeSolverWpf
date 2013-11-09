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

            // We also cache all coords that are just off the edge of the board.
            // Therefore, in addition to (0, 0) through (0, 8), we also cache (0, -1) and (0, 9).
            // And similarly for all the other rows and columns.
            Assert.That(actual, Is.EqualTo(11 * 11));
        }
    }
}
