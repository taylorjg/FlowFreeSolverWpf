using System.Collections;
using NUnit.Framework;

namespace FlowFreeSolverWpfTests
{
    [TestFixture]
    class BitArrayExploratoryTests
    {

        [Test]
        public void BasicTest()
        {
            var ba = new BitArray(156);
            var b0Before = ba[0];
            ba[0] = true;
            var b0After = ba[0];
            Assert.That(b0Before, Is.False);
            Assert.That(b0After, Is.True);
        }

        [Test]
        public void ForEachTest()
        {
            var ba = new BitArray(156);
            ba[0] = true;
            var count = 0;
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var b in ba) count += (bool)b ? 1 : 0;
            Assert.That(count, Is.EqualTo(1));
        }
    }
}
