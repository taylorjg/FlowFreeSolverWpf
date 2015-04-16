using System.Collections.Generic;

namespace FlowFreeSolverWpf.Model
{
    public class MatrixRow
    {
        private readonly ColourPair _colourPair;
        private readonly Path _path;
        private readonly List<bool> _dlxMatrixRow;

        public MatrixRow(ColourPair colourPair, Path path, List<bool> dlxMatrixRow)
        {
            _colourPair = colourPair;
            _path = path;
            _dlxMatrixRow = dlxMatrixRow;
        }

        public ColourPair ColourPair
        {
            get { return _colourPair; }
        }

        public Path Path
        {
            get { return _path; }
        }

        public List<bool> DlxMatrixRow
        {
            get { return _dlxMatrixRow; }
        }
    }
}
