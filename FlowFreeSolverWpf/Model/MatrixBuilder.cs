using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FlowFreeSolverWpf.Model
{
    using Matrix = List<MatrixRow>;
    using DlxMatrixRow = List<bool>;

    public class MatrixBuilder
    {
        private Grid _grid;
        private CancellationToken _cancellationToken;
        private int _numColourPairs;
        private int _numColumns;
        private bool _initialised;
        private readonly Matrix _abandonedMatrix = new Matrix();
        private readonly Matrix _currentMatrix = new Matrix();

        // TODO: add a constructor taking grid and cancellationToken ???
        public List<MatrixRow> BuildMatrix(Grid grid, int maxDirectionChanges, CancellationToken cancellationToken)
        {
            _grid = grid;
            _cancellationToken = cancellationToken;

            _numColourPairs = _grid.ColourPairs.Count();
            _numColumns = _numColourPairs + (_grid.GridSize * grid.GridSize);

            var tasks = new List<Task<Matrix>>();
            var processAbandonedRowsOnly = true;

            if (!_initialised)
            {
                _initialised = true;
                processAbandonedRowsOnly = false;
            }

            var taskFactory = new TaskFactory<Matrix>();

            var colourPairsWithIndices = _grid.ColourPairs.Select((colourPair, index) => new
            {
                ColourPair = colourPair,
                Index = index
            });

            foreach (var item in colourPairsWithIndices)
            {
                var copyOfItemForCapture = item;
                IList<Path> paths = new List<Path>();
                if (processAbandonedRowsOnly)
                {
                    paths = _abandonedMatrix
                        .Where(ap => ap.ColourPair == copyOfItemForCapture.ColourPair)
                        .Select(ap => ap.Path)
                        .ToList();
                    if (!paths.Any())
                    {
                        continue;
                    }
                }
                var task = taskFactory.StartNew(
                    () => BuildMatrixRowsForColourPair(
                        copyOfItemForCapture.ColourPair,
                        copyOfItemForCapture.Index,
                        paths,
                        maxDirectionChanges));
                tasks.Add(task);
            }

            Task.WaitAll(tasks.Cast<Task>().ToArray());

            _abandonedMatrix.Clear();

            foreach (var matrixRow in tasks.SelectMany(task => task.Result))
            {
                if (matrixRow.Path.IsAbandoned)
                    _abandonedMatrix.Add(matrixRow);
                else
                    _currentMatrix.Add(matrixRow);
            }

            return _currentMatrix;
        }

        public bool ThereAreStillSomeAbandonedPaths()
        {
            return _abandonedMatrix.Any();
        }

        public Tuple<ColourPair, Path> GetColourPairAndPathForRowIndex(int rowIndex)
        {
            var matrixRow = _currentMatrix[rowIndex];
            return Tuple.Create(matrixRow.ColourPair, matrixRow.Path);
        }

        private Matrix BuildMatrixRowsForColourPair(
            ColourPair colourPair,
            int colourPairIndex,
            IList<Path> abandonedPaths,
            int maxDirectionChanges)
        {
            var matrixRows = new Matrix();

            var pathFinder = new PathFinder(_cancellationToken);
            var paths = pathFinder.FindAllPaths(_grid, colourPair.StartCoords, colourPair.EndCoords, abandonedPaths, maxDirectionChanges);

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var path in paths.PathList)
            {
                var dlxMatrixRow = BuildDlxMatrixRowForColourPairPath(colourPairIndex, path);
                matrixRows.Add(new MatrixRow(colourPair, path, dlxMatrixRow));
            }

            return matrixRows;
        }

        private DlxMatrixRow BuildDlxMatrixRowForColourPairPath(int colourPairIndex, Path path)
        {
            var dlxMatrixRow = new DlxMatrixRow(Enumerable.Repeat(false, _numColumns));

            dlxMatrixRow[colourPairIndex] = true;

            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (var coords in path.CoordsList)
            {
                var gridLocationColumnIndex = _numColourPairs + (_grid.GridSize * coords.X) + coords.Y;
                dlxMatrixRow[gridLocationColumnIndex] = true;
            }

            return dlxMatrixRow;
        }
    }
}
