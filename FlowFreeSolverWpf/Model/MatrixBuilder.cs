using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FlowFreeSolverWpf.Model
{
    public class MatrixBuilder
    {
        private Grid _grid;
        private int _numColourPairs;
        private int _numColumns;
        private IDictionary<int, Tuple<ColourPair, Path>> _rowIndexToColourPairAndPath;
        private CancellationToken _cancellationToken;

        public bool[,] BuildMatrixFor(Grid grid, CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            _grid = grid;
            _numColourPairs = _grid.ColourPairs.Count();
            _numColumns = _numColourPairs + (_grid.Width * _grid.Height);
            var internalData = new List<IList<bool>>();
            _rowIndexToColourPairAndPath = new Dictionary<int, Tuple<ColourPair, Path>>();

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            _grid.ColourPairs.Select((colourPair, colourPairIndex) =>
                {
                    AddInternalDataRowsForColourPair(internalData, colourPair, colourPairIndex);
                    return 0;
                }).ToList();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            var matrix = new bool[internalData.Count, _numColumns];
            for (var row = 0; row < internalData.Count; row++)
            {
                for (var col = 0; col < _numColumns; col++)
                {
                    matrix[row, col] = internalData[row][col];
                }
            } 
            
            return matrix;
        }

        public Tuple<ColourPair, Path> GetColourPairAndPathForRowIndex(int rowIndex)
        {
            return _rowIndexToColourPairAndPath[rowIndex];
        }

        private void AddInternalDataRowsForColourPair(List<IList<bool>> internalData, ColourPair colourPair, int colourPairIndex)
        {
            var pathFinder = new PathFinder(_cancellationToken);
            var paths = pathFinder.FindAllPaths(_grid, colourPair.StartCoords, colourPair.EndCoords);

            foreach (var path in paths.PathList)
            {
                var internalDataRow = BuildInternalDataRowForColourPairPath(colourPairIndex, path);
                internalData.Add(internalDataRow);
                var rowIndex = internalData.Count - 1;
                _rowIndexToColourPairAndPath.Add(rowIndex, Tuple.Create(colourPair, path));
            }
        }

        private IList<bool> BuildInternalDataRowForColourPairPath(int colourPairIndex, Path path)
        {
            var internalDataRow = new bool[_numColumns];

            internalDataRow[colourPairIndex] = true;

            foreach (var coords in path.CoordsList)
            {
                var gridLocationColumnIndex = _numColourPairs + (_grid.Width * coords.X) + coords.Y;
                internalDataRow[gridLocationColumnIndex] = true;
            }

            return internalDataRow;
        }
    }
}
