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

        private class InternalDataRow
        {
            public InternalDataRow(ColourPair colourPair, Path path, IList<bool> matrixRow)
            {
                ColourPair = colourPair;
                Path = path;
                MatrixRow = matrixRow;
            }

            public ColourPair ColourPair { get; private set; }
            public Path Path { get; private set; }
            public IList<bool> MatrixRow { get; private set; }
        }

        public bool[,] BuildMatrixFor(Grid grid, int maxDirectionChanges, CancellationToken cancellationToken)
        {
            _grid = grid;
            _maxDirectionChanges = maxDirectionChanges;
            _cancellationToken = cancellationToken;
            _numColourPairs = _grid.ColourPairs.Count();
            _numColumns = _numColourPairs + (_grid.GridSize * grid.GridSize);
            _rowIndexToColourPairAndPath = new Dictionary<int, Tuple<ColourPair, Path>>();

            var tasks = new List<Task<IList<InternalDataRow>>>();
            var colourPairsWithIndexes = _grid.ColourPairs.Select((colourPair, colourPairIndex) => new { ColourPair = colourPair, ColourPairIndex = colourPairIndex });

            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var item in colourPairsWithIndexes)
            {
                var copyOfitem = item;
                var task =
                    Task<IList<InternalDataRow>>.Factory.StartNew(
                        () =>
                        BuildInternalDataRowsForColourPair(copyOfitem.ColourPair, copyOfitem.ColourPairIndex));
                tasks.Add(task);
            }
            // ReSharper restore LoopCanBeConvertedToQuery

            Task.WaitAll(tasks.Cast<Task>().ToArray());

            var combinedMatrixRows = new List<IList<bool>>();
            foreach (var task in tasks)
            {
                var internalDataRows = task.Result;
                foreach (var internalDataRow in internalDataRows)
                {
                    combinedMatrixRows.Add(internalDataRow.MatrixRow);
                    var rowIndex = combinedMatrixRows.Count - 1;
                    _rowIndexToColourPairAndPath[rowIndex] = Tuple.Create(internalDataRow.ColourPair, internalDataRow.Path);
                }
            }

            return ConvertCombinedMatrixRowsToDlxMatrix(combinedMatrixRows);
        }

        private bool[,] ConvertCombinedMatrixRowsToDlxMatrix(List<IList<bool>> combinedMatrixRows)
        {
            var matrix = new bool[combinedMatrixRows.Count,_numColumns];

            for (var row = 0; row < combinedMatrixRows.Count; row++)
            {
                for (var col = 0; col < _numColumns; col++)
                {
                    matrix[row, col] = combinedMatrixRows[row][col];
                }
            }

            return matrix;
        }

        public Tuple<ColourPair, Path> GetColourPairAndPathForRowIndex(int rowIndex)
        {
            return _rowIndexToColourPairAndPath[rowIndex];
        }

        private IList<InternalDataRow> BuildInternalDataRowsForColourPair(ColourPair colourPair, int colourPairIndex)
        {
            var internalDataRows = new List<InternalDataRow>();

            var pathFinder = new PathFinder(_cancellationToken);
            var paths = pathFinder.FindAllPaths(_grid, colourPair.StartCoords, colourPair.EndCoords, _maxDirectionChanges);

            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var path in paths.PathList)
            {
                var matrixRow = BuildMatrixRowForColourPairPath(colourPairIndex, path);
                internalDataRows.Add(new InternalDataRow(colourPair, path, matrixRow));
            }
            // ReSharper restore LoopCanBeConvertedToQuery

            return internalDataRows;
        }

        private IList<bool> BuildMatrixRowForColourPairPath(int colourPairIndex, Path path)
        {
            var matrixRow = new bool[_numColumns];

            matrixRow[colourPairIndex] = true;

            foreach (var coords in path.CoordsList)
            {
                var gridLocationColumnIndex = _numColourPairs + (_grid.GridSize * coords.X) + coords.Y;
                matrixRow[gridLocationColumnIndex] = true;
            }

            return matrixRow;
        }
    }
}
