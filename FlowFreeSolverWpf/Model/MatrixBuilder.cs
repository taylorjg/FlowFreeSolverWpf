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

            var tasks = new List<Task<IList<Tuple<ColourPair, Path, IList<bool>>>>>();
            var colourPairsWithIndexes = _grid.ColourPairs.Select((colourPair, colourPairIndex) => new { ColourPair = colourPair, ColourPairIndex = colourPairIndex });

            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var item in colourPairsWithIndexes)
            {
                var copyOfitem = item;
                var task =
                    Task<IList<Tuple<ColourPair, Path, IList<bool>>>>.Factory.StartNew(
                        () =>
                        BuildInternalDataRowsForColourPair(copyOfitem.ColourPair, copyOfitem.ColourPairIndex));
                tasks.Add(task);
            }
            // ReSharper restore LoopCanBeConvertedToQuery

            Task.WaitAll(tasks.Cast<Task>().ToArray());

            foreach (var task in tasks)
            {
                var result = task.Result;
                foreach (var resultItem in result)
                {
                    var colourPair = resultItem.Item1;
                    var path = resultItem.Item2;
                    var internalDataRow = resultItem.Item3;
                    internalData.Add(internalDataRow);
                    var rowIndex = internalData.Count - 1;
                    _rowIndexToColourPairAndPath[rowIndex] = Tuple.Create(colourPair, path);
                }
            }

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

        private IList<Tuple<ColourPair, Path, IList<bool>>> BuildInternalDataRowsForColourPair(ColourPair colourPair, int colourPairIndex)
        {
            var result = new List<Tuple<ColourPair, Path, IList<bool>>>();

            var pathFinder = new PathFinder(_cancellationToken);
            var paths = pathFinder.FindAllPaths(_grid, colourPair.StartCoords, colourPair.EndCoords, _maxDirectionChanges);

            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var path in paths.PathList)
            {
                var internalDataRow = BuildInternalDataRowForColourPairPath(colourPairIndex, path);
                result.Add(Tuple.Create(colourPair, path, internalDataRow));
            }
            // ReSharper restore LoopCanBeConvertedToQuery

            return result;
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
