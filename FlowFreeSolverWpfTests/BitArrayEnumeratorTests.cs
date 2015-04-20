using System.Collections;
using System.Linq;
using FlowFreeSolverWpf.Model;
using NUnit.Framework;

namespace FlowFreeSolverWpfTests
{
    [TestFixture]
    class BitArrayEnumeratorTests
    {
        [Test]
        public void Test1()
        {
            const int size = 156;
            var bitArray = new BitArray(size);
            Assert.That(bitArray.Length, Is.EqualTo(size));
            Assert.That(bitArray.Count, Is.EqualTo(size));
            var enumerable = new BitArrayEnumerable(bitArray);
            Assert.That(enumerable.Count(), Is.EqualTo(size));
        }
    }
}
