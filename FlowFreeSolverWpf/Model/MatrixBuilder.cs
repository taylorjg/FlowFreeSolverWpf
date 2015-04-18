using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FlowFreeSolverWpf.Model
{
    public class MatrixBuilder
    {
        private readonly Grid _grid;
        private readonly CancellationToken _cancellationToken;
        private readonly int _numColourPairs;
        private readonly int _numColumns;
        private readonly List<MatrixRow> _activePaths = new List<MatrixRow>();
        private readonly List<MatrixRow> _inactivePaths = new List<MatrixRow>();

        public MatrixBuilder(Grid grid, CancellationToken cancellationToken)
        {
            _grid = grid;
            _cancellationToken = cancellationToken;
            _numColourPairs = _grid.ColourPairs.Count();
            _numColumns = _numColourPairs + (_grid.GridSize * grid.GridSize);
        }

        private bool _firstTime = true;

        public List<MatrixRow> BuildMatrix(int maxDirectionChanges)
        {
            var tasks = _grid.ColourPairs
                .Select((colourPair, index) =>
                {
                    var paths = _firstTime
                        ? PathFinder.InitialPaths(colourPair)
                        : _inactivePaths
                            .Where(ap => ap.ColourPair == colourPair)
                            .Select(ap => ap.Path)
                            .ToList();

                    var thisColourPair = colourPair;
                    var thisIndex = index;

                    return Task.Factory.StartNew(
                        () => BuildMatrixRowsForColourPair(
                            thisColourPair,
                            thisIndex,
                            paths,
                            maxDirectionChanges),
                        _cancellationToken);
                })
                .ToList();

            Task.WaitAll(tasks.Cast<Task>().ToArray());

            _firstTime = false;
            _inactivePaths.Clear();

            var flattenedMatrixRows = tasks.SelectMany(task =>
            {
                if (task.IsCanceled || task.IsFaulted) return Enumerable.Empty<MatrixRow>();
                return task.Result;
            });

            foreach (var matrixRow in flattenedMatrixRows)
            {
                if (matrixRow.Path.IsActive)
                    _activePaths.Add(matrixRow);
                else
                    _inactivePaths.Add(matrixRow);
            }

            return _activePaths;
        }

        public bool HasInactivePaths()
        {
            return _inactivePaths.Any();
        }

        public Tuple<ColourPair, Path> GetColourPairAndPathForRowIndex(int rowIndex)
        {
            var matrixRow = _activePaths[rowIndex];
            return Tuple.Create(matrixRow.ColourPair, matrixRow.Path);
        }

        private List<MatrixRow> BuildMatrixRowsForColourPair(
            ColourPair colourPair,
            int colourPairIndex,
            IList<Path> activePaths,
            int maxDirectionChanges)
        {
            var pathFinder = new PathFinder(_cancellationToken);
            var paths = pathFinder.FindAllPaths(_grid, colourPair.StartCoords, colourPair.EndCoords, activePaths, maxDirectionChanges);

            return paths.PathList
                .Select(path => BuildDlxMatrixRowForPath(colourPair, colourPairIndex, path))
                .ToList();
        }

        private MatrixRow BuildDlxMatrixRowForPath(ColourPair colourPair, int colourPairIndex, Path path)
        {
            var dlxMatrixRow = new List<bool>(Enumerable.Repeat(false, _numColumns));

            dlxMatrixRow[colourPairIndex] = true;

            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (var coords in path.CoordsList)
            {
                var gridLocationColumnIndex = _numColourPairs + (_grid.GridSize * coords.X) + coords.Y;
                dlxMatrixRow[gridLocationColumnIndex] = true;
            }

            return new MatrixRow(colourPair, path, dlxMatrixRow);
        }
    }
}
