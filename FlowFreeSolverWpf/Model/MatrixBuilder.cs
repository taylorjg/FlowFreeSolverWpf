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
        private readonly List<MatrixRow> _currentMatrix = new List<MatrixRow>();
        private readonly List<Path> _stalledPaths;

        public MatrixBuilder(Grid grid, CancellationToken cancellationToken)
        {
            _grid = grid;
            _cancellationToken = cancellationToken;
            _numColourPairs = _grid.ColourPairs.Count();
            _numColumns = _numColourPairs + (_grid.GridSize * grid.GridSize);
            _stalledPaths = _grid.ColourPairs.SelectMany(PathFinder.InitialPaths).ToList();
        }

        public List<MatrixRow> BuildMatrix(int maxDirectionChanges)
        {
            var flattenedMatrixRows = new List<MatrixRow>();
            var flattenedStalledPaths = new List<Path>();

            var transformBlock = new TransformBlock<Tuple<ColourPair, int, List<Path>, int>, Tuple<List<MatrixRow>, List<Path>>>(
                tuple => FindAllPathsForColourPair(
                    tuple.Item1,
                    tuple.Item2,
                    tuple.Item3,
                    tuple.Item4),
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                });

            var actionBlock = new ActionBlock<Tuple<List<MatrixRow>, List<Path>>>(tuple =>
            {
                flattenedMatrixRows.AddRange(tuple.Item1);
                flattenedStalledPaths.AddRange(tuple.Item2);
            });

            transformBlock.LinkTo(actionBlock, new DataflowLinkOptions {PropagateCompletion = true});

            var tuples = _grid.ColourPairs
                .SelectMany((colourPair, index) =>
                {
                    var paths = GetStalledPathsForColourPair(colourPair);
                    return paths.Any()
                        ? new[] {Tuple.Create(colourPair, index, paths, maxDirectionChanges)}
                        : Enumerable.Empty<Tuple<ColourPair, int, List<Path>, int>>();
                });

            foreach (var tuple in tuples) transformBlock.Post(tuple);

            transformBlock.Complete();
            actionBlock.Completion.Wait(_cancellationToken);

            _currentMatrix.AddRange(flattenedMatrixRows);

            _stalledPaths.Clear();
            _stalledPaths.AddRange(flattenedStalledPaths);

            return _currentMatrix;
        }

        private List<Path> GetStalledPathsForColourPair(ColourPair colourPair)
        {
            return _stalledPaths.Where(path => path.CoordsList.First().Equals(colourPair.StartCoords)).ToList();
        }

        public bool HasStalledPaths()
        {
            return _stalledPaths.Any();
        }

        public MatrixRow GetColourPairAndPathForRowIndex(int rowIndex)
        {
            return _currentMatrix[rowIndex];
        }

        private Tuple<List<MatrixRow>, List<Path>> FindAllPathsForColourPair(
            ColourPair colourPair,
            int colourPairIndex,
            IEnumerable<Path> activePaths,
            int maxDirectionChanges)
        {
            var pathFinder = new PathFinder(_cancellationToken);
            var paths = pathFinder.FindAllPaths(_grid, colourPair.EndCoords, activePaths, maxDirectionChanges);
            var partitionedPaths = paths.ToLookup(p => p.IsActive);
            var completedPaths = partitionedPaths[true].ToList();
            var stalledPaths = partitionedPaths[false].ToList();
            var matrixRows = BuildMatrixRowsForColourPairPaths(colourPair, colourPairIndex, completedPaths);
            return Tuple.Create(matrixRows, stalledPaths);
        }

        private List<MatrixRow> BuildMatrixRowsForColourPairPaths(
            ColourPair colourPair,
            int colourPairIndex,
            IEnumerable<Path> paths)
        {
            return paths
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
