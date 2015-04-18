using System.Collections.Generic;

namespace FlowFreeSolverWpf.Model
{
    public class MatrixRow
    {
        private readonly ColourPair _colourPair;
        private readonly Path _path;
        private readonly List<bool> _dlxRow;

        public MatrixRow(ColourPair colourPair, Path path, List<bool> dlxRow)
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

        public List<bool> DlxRow
        {
            get { return _dlxRow; }
        }
    }
}
