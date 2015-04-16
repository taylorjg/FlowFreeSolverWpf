using System.Collections.Generic;
using System.Threading;
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
            var grid = new Grid(4,
                new ColourPair(CoordsFactory.GetCoords(0, 3), CoordsFactory.GetCoords(3, 3), DotColours.Blue),
                new ColourPair(CoordsFactory.GetCoords(1, 3), CoordsFactory.GetCoords(2, 3), DotColours.Orange),
                new ColourPair(CoordsFactory.GetCoords(1, 2), CoordsFactory.GetCoords(2, 2), DotColours.Red),
                new ColourPair(CoordsFactory.GetCoords(1, 1), CoordsFactory.GetCoords(2, 1), DotColours.Green));

            var matrixBuilder1 = new MatrixBuilder();
            var matrix1 = matrixBuilder1.BuildMatrix(grid, 100, CancellationToken.None);

            var matrixBuilder2 = new MatrixBuilder();
            List<MatrixRow> matrix2;
            var maxDirectionChanges = 1;

            for (; ; )
            {
                matrix2 = matrixBuilder2.BuildMatrix(grid, maxDirectionChanges, CancellationToken.None);
                if (!matrixBuilder2.ThereAreStillSomeAbandonedPaths()) break;
                maxDirectionChanges++;
            }

            Assert.That(matrix1.Count, Is.EqualTo(matrix2.Count));
            Assert.That(matrix1[0].DlxMatrixRow.Count, Is.EqualTo(matrix2[0].DlxMatrixRow.Count));
        }
    }
}
