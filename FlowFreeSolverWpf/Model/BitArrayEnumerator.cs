using System.Collections;
using System.Collections.Generic;

namespace FlowFreeSolverWpf.Model
{
    public class BitArrayEnumerator : IEnumerator<bool>
    {
        private readonly BitArray _bitArray;
        private int _index = -1;

        public BitArrayEnumerator(BitArray bitArray)
        {
            _bitArray = bitArray;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            return (++_index < _bitArray.Length);
        }

        public void Reset()
        {
            _index = -1;
        }

        public bool Current
        {
            get
            {
                return _bitArray[_index];
            }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
}
