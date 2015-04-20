using System.Collections;
using System.Collections.Generic;

namespace FlowFreeSolverWpf.Model
{
    public class BitArrayEnumerable : IEnumerable<bool>
    {
        private readonly BitArray _bitArray;

        public BitArrayEnumerable(BitArray bitArray)
        {
            _bitArray = bitArray;
        }

        public IEnumerator<bool> GetEnumerator()
        {
            return new BitArrayEnumerator(_bitArray);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
