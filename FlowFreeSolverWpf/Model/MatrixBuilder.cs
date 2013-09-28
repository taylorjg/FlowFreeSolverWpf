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
        private CancellationToken _cancellationToken;
        private int _numColourPairs;
        private int _numColumns;
        private IDictionary<int, Tuple<ColourPair, Path>> _rowIndexToColourPairAndPath;
        private IList<MatrixRow> _previousCombinedMatrixRows;
        private IList<InternalDataRow> _allAbandonedPaths;

        private class MatrixRow : List<bool>
        {
            public MatrixRow(int numColumns)
            {
                var bools = new bool[numColumns];
                AddRange(bools);
            }
        }

        private class InternalDataRow
        {
            public InternalDataRow(ColourPair colourPair, Path path, MatrixRow matrixRow)
            {
                ColourPair = colourPair;
                Path = path;
                MatrixRow = matrixRow;
            }

            public ColourPair ColourPair { get; private set; }
            public Path Path { get; private set; }
            public MatrixRow MatrixRow { get; private set; }
        }

        public bool[,] BuildMatrixFor(Grid grid, int maxDirectionChanges, CancellationToken cancellationToken)
        {
            _grid = grid;
            _cancellationToken = cancellationToken;

            _numColourPairs = _grid.ColourPairs.Count();
            _numColumns = _numColourPairs + (_grid.GridSize * grid.GridSize);

            var tasks = new List<Task<IList<InternalDataRow>>>();
            var colourPairsWithIndexes = _grid.ColourPairs.Select((colourPair, colourPairIndex) => new { ColourPair = colourPair, ColourPairIndex = colourPairIndex });

            if (_rowIndexToColourPairAndPath == null)
            {
                _rowIndexToColourPairAndPath = new Dictionary<int, Tuple<ColourPair, Path>>();
                _previousCombinedMatrixRows = new List<MatrixRow>();
                _allAbandonedPaths = new List<InternalDataRow>();
            }

            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var item in colourPairsWithIndexes)
            {
                var copyOfItemForCapture = item;
                var abandonedPaths = _allAbandonedPaths
                    .Where(ap => ap.ColourPair == copyOfItemForCapture.ColourPair)
                    .Select(ap => ap.Path)
                    .ToList();
                var task =
                    Task<IList<InternalDataRow>>.Factory.StartNew(
                        () =>
                        BuildInternalDataRowsForColourPair(
                            copyOfItemForCapture.ColourPair,
                            copyOfItemForCapture.ColourPairIndex,
                            abandonedPaths,
                            maxDirectionChanges));
                tasks.Add(task);
            }
            // ReSharper restore LoopCanBeConvertedToQuery

            Task.WaitAll(tasks.Cast<Task>().ToArray());

            _allAbandonedPaths.Clear();
            var combinedMatrixRows = new List<MatrixRow>();
            combinedMatrixRows.AddRange(_previousCombinedMatrixRows);
            foreach (var internalDataRow in tasks.Select(task => task.Result).SelectMany(internalDataRows => internalDataRows))
            {
                if (internalDataRow.Path.IsAbandoned)
                {
                    _allAbandonedPaths.Add(internalDataRow);
                }
                else
                {
                    combinedMatrixRows.Add(internalDataRow.MatrixRow);
                    var rowIndex = combinedMatrixRows.Count - 1;
                    _rowIndexToColourPairAndPath[rowIndex] = Tuple.Create(internalDataRow.ColourPair, internalDataRow.Path);
                }
            }

            _previousCombinedMatrixRows = combinedMatrixRows;

            return ConvertCombinedMatrixRowsToDlxMatrix(combinedMatrixRows);
        }

        public bool ThereAreStillSomeAbandonedPaths()
        {
            return _allAbandonedPaths.Any();
        }

        private bool[,] ConvertCombinedMatrixRowsToDlxMatrix(IList<MatrixRow> combinedMatrixRows)
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

        private IList<InternalDataRow> BuildInternalDataRowsForColourPair(
            ColourPair colourPair,
            int colourPairIndex,
            IList<Path> abandonedPaths,
            int maxDirectionChanges)
        {
            var internalDataRows = new List<InternalDataRow>();

            var pathFinder = new PathFinder(_cancellationToken);
            var paths = pathFinder.FindAllPaths(_grid, colourPair.StartCoords, colourPair.EndCoords, abandonedPaths, maxDirectionChanges);

            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var path in paths.PathList)
            {
                var matrixRow = BuildMatrixRowForColourPairPath(colourPairIndex, path);
                internalDataRows.Add(new InternalDataRow(colourPair, path, matrixRow));
            }
            // ReSharper restore LoopCanBeConvertedToQuery

            return internalDataRows;
        }

        private MatrixRow BuildMatrixRowForColourPairPath(int colourPairIndex, Path path)
        {
            var matrixRow = new MatrixRow(_numColumns);

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
