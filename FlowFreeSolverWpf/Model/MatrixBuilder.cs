using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FlowFreeSolverWpf.Model
{
    public class MatrixBuilder
    {
        private Grid _grid;
        private int _maxDirectionChanges;
        private CancellationToken _cancellationToken;
        private int _numColourPairs;
        private int _numColumns;
        private IDictionary<int, Tuple<ColourPair, Path>> _rowIndexToColourPairAndPath;

        public bool[,] BuildMatrixFor(Grid grid, int maxDirectionChanges, CancellationToken cancellationToken)
        {
            _grid = grid;
            _maxDirectionChanges = maxDirectionChanges;
            _cancellationToken = cancellationToken;
            _numColourPairs = _grid.ColourPairs.Count();
            _numColumns = _numColourPairs + (_grid.GridSize * grid.GridSize);
            var internalData = new List<IList<bool>>();
            _rowIndexToColourPairAndPath = new Dictionary<int, Tuple<ColourPair, Path>>();

            var colourPairsWithIndexes = _grid.ColourPairs.Select((colourPair, colourPairIndex) => new { ColourPair = colourPair, ColourPairIndex = colourPairIndex });
            Parallel.ForEach(colourPairsWithIndexes, cpwi => AddInternalDataRowsForColourPair(internalData, cpwi.ColourPair, cpwi.ColourPairIndex));

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
            var paths = pathFinder.FindAllPaths(_grid, colourPair.StartCoords, colourPair.EndCoords, _maxDirectionChanges);

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
                var gridLocationColumnIndex = _numColourPairs + (_grid.GridSize * coords.X) + coords.Y;
                internalDataRow[gridLocationColumnIndex] = true;
            }

            return internalDataRow;
        }
    }
}
