using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DlxLib;
using FlowFreeSolverWpf.Model;
using NUnit.Framework;

namespace FlowFreeSolverWpfTests
{
    [TestFixture]
    internal class MatrixBuilderTests
    {
        [Test]
        public void SolvingASmallGridWithLargeInitialMaxDirectionChangesAndDynamicMaxDirectionChangesResultsInSameSizeMatrixes()
        {
            // "ABBA"
            // " CC "
            // " DD "
            // "    "
            var grid = new Grid(4, new[]
                {
                    new ColourPair(new Coords(0, 3), new Coords(3, 3), DotColours.Blue),
                    new ColourPair(new Coords(1, 3), new Coords(2, 3), DotColours.Orange),
                    new ColourPair(new Coords(1, 2), new Coords(2, 2), DotColours.Red),
                    new ColourPair(new Coords(1, 1), new Coords(2, 1), DotColours.Green)
                });

            var matrixBuilder1 = new MatrixBuilder();
            var matrix1 = matrixBuilder1.BuildMatrixFor(grid, 100, CancellationToken.None);

            var matrixBuilder2 = new MatrixBuilder();
            bool[,] matrix2;
            var maxDirectionChanges = 1;
            //var dlx = new Dlx();
            //dlx.SolutionFound += (_, __) => dlx.Cancel();
            //var solutions = new List<Solution>();

            for (; ; )
            {
                matrix2 = matrixBuilder2.BuildMatrixFor(grid, maxDirectionChanges, CancellationToken.None);
                DumpMatrix(string.Format("matrix2 - maxDirectionChanges: {0}", maxDirectionChanges), matrix2, matrixBuilder2);
                //solutions = dlx.Solve(matrix2).ToList();
                //if (solutions.Any())
                //{
                //    break;
                //}
                if (!matrixBuilder2.ThereAreStillSomeAbandonedPaths())
                {
                    break;
                }
                maxDirectionChanges++;
            }

            DumpMatrix("matrix1", matrix1, matrixBuilder1);
            DumpMatrix(string.Format("matrix2 - maxDirectionChanges: {0}", maxDirectionChanges), matrix2, matrixBuilder2);

            Assert.That(matrix2.GetLength(1), Is.EqualTo(matrix1.GetLength(1)));
            Assert.That(matrix2.GetLength(0), Is.EqualTo(matrix1.GetLength(0)));
        }

        private static void DumpMatrix(string message, bool[,] matrix, MatrixBuilder matrixBuilder)
        {
            System.Diagnostics.Debug.WriteLine(message);
            for (var rowIndex = 0; rowIndex < matrix.GetLength(0); rowIndex++)
            {
                var colourPairAndPath = matrixBuilder.GetColourPairAndPathForRowIndex(rowIndex);
                System.Diagnostics.Debug.WriteLine(
                    "{0}: {1} {2}",
                    rowIndex,
                    colourPairAndPath.Item1.DotColour.ColourName,
                    string.Join(", ", colourPairAndPath.Item2.CoordsList.Select(c => c.ToString()).ToArray()));
            }
            System.Diagnostics.Debug.WriteLine(string.Empty);
        }
    }
}
