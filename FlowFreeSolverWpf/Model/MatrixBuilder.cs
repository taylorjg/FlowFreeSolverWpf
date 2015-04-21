using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks.Dataflow;

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
            var flattenedMatrixRows = new List<MatrixRow>();

            var transformBlock = new TransformBlock<Tuple<ColourPair, int, List<Path>, int>, List<MatrixRow>>(
                tuple => FindAllPathsForColourPair(
                    tuple.Item1,
                    tuple.Item2,
                    tuple.Item3,
                    tuple.Item4),
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                });

            var actionBlock = new ActionBlock<List<MatrixRow>>(matrixRows => flattenedMatrixRows.AddRange(matrixRows));

            transformBlock.LinkTo(actionBlock, new DataflowLinkOptions {PropagateCompletion = true});

            var tuples = _grid.ColourPairs
                .SelectMany((colourPair, index) =>
                {
                    var paths = (_firstTime
                        ? PathFinder.InitialPaths(colourPair)
                        : _inactivePaths
                            .Where(ap => ap.ColourPair.DotColour == colourPair.DotColour)
                            .Select(ap => ap.Path))
                        .ToList();

                    return paths.Any()
                        ? new[] {Tuple.Create(colourPair, index, paths, maxDirectionChanges)}
                        : Enumerable.Empty<Tuple<ColourPair, int, List<Path>, int>>();
                });

            foreach (var tuple in tuples) transformBlock.Post(tuple);

            transformBlock.Complete();
            actionBlock.Completion.Wait(_cancellationToken);

            _firstTime = false;
            _inactivePaths.Clear();

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

        private List<MatrixRow> FindAllPathsForColourPair(
            ColourPair colourPair,
            int colourPairIndex,
            IEnumerable<Path> activePaths,
            int maxDirectionChanges)
        {
            var pathFinder = new PathFinder(_cancellationToken);
            var paths = pathFinder.FindAllPaths(_grid, colourPair.EndCoords, activePaths, maxDirectionChanges);
            return BuildMatrixRowsForColourPairPaths(colourPair, colourPairIndex, paths);
        }

        private List<MatrixRow> BuildMatrixRowsForColourPairPaths(
            ColourPair colourPair,
            int colourPairIndex,
            Paths paths)
        {
            return paths.PathList
                .Select(path => BuildMatrixRowForColourPairPath(colourPair, colourPairIndex, path))
                .ToList();
        }

        private MatrixRow BuildMatrixRowForColourPairPath(ColourPair colourPair, int colourPairIndex, Path path)
        {
            var dlxRow = new BitArray(_numColumns);

            dlxRow[colourPairIndex] = true;

            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (var coords in path.CoordsList)
            {
                var gridLocationIndex = _numColourPairs + (_grid.GridSize * coords.X) + coords.Y;
                dlxRow[gridLocationIndex] = true;
            }

            return new MatrixRow(colourPair, path, dlxRow);
        }
    }
}
