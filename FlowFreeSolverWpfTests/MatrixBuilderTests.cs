using System.Collections.Generic;
using System.Linq;
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
            // "BOOB"
            // " RR "
            // " GG "
            // "    "
            var grid = new Grid(4,
                new ColourPair(CoordsFactory.GetCoords(0, 3), CoordsFactory.GetCoords(3, 3), DotColours.Blue),
                new ColourPair(CoordsFactory.GetCoords(1, 3), CoordsFactory.GetCoords(2, 3), DotColours.Orange),
                new ColourPair(CoordsFactory.GetCoords(1, 2), CoordsFactory.GetCoords(2, 2), DotColours.Red),
                new ColourPair(CoordsFactory.GetCoords(1, 1), CoordsFactory.GetCoords(2, 1), DotColours.Green));

            var matrixBuilder1 = new MatrixBuilder(grid, CancellationToken.None);
            var matrix1 = matrixBuilder1.BuildMatrix(100);

            var matrixBuilder2 = new MatrixBuilder(grid, CancellationToken.None);
            var matrix2 = new List<MatrixRow>();

            foreach (var maxDirectionChanges in Enumerable.Range(1, 100))
            {
                matrix2 = matrixBuilder2.BuildMatrix(maxDirectionChanges);
                if (!matrixBuilder2.HasStalledPaths()) break;
            }

            var comparer = new MatrixRowEqualityComparer();
            var distinctRowCount1 = matrix1.Distinct(comparer).Count();
            var distinctRowCount2 = matrix2.Distinct(comparer).Count();
            Assert.That(distinctRowCount1, Is.EqualTo(distinctRowCount2));
        }

        private class MatrixRowEqualityComparer : IEqualityComparer<MatrixRow>
        {
            public bool Equals(MatrixRow x, MatrixRow y)
            {
                return x.DlxRowEnumerable.SequenceEqual(y.DlxRowEnumerable);
            }

            public int GetHashCode(MatrixRow obj)
            {
                return 0;
            }
        }
    }
}
