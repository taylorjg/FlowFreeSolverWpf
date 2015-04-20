using System.Collections;
using System.Collections.Generic;

namespace FlowFreeSolverWpf.Model
{
    public class MatrixRow
    {
        private readonly ColourPair _colourPair;
        private readonly Path _path;
        private readonly BitArray _dlxRow;

        public MatrixRow(ColourPair colourPair, Path path, BitArray dlxRow)
        {
            _colourPair = colourPair;
            _path = path;
            _dlxRow = dlxRow;
        }

        public ColourPair ColourPair
        {
            get { return _colourPair; }
        }

        public Path Path
        {
            get { return _path; }
        }

        public IEnumerable<bool> DlxRowEnumerable
        {
            get { return new BitArrayEnumerable(_dlxRow); }
        }
    }
}
