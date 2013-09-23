using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DlxLib;
using FlowFreeSolverWpf.Model;

namespace FlowFreeSolverWpf
{
    public class GridSizeItem
    {
        public GridSizeItem(int gridSize)
        {
            GridSize = gridSize;
            GridSizeName = string.Format("{0}x{0}", gridSize);
        }

        public int GridSize { get; private set; }
        public string GridSizeName { get; private set; }
    }

    public partial class MainWindow
    {
        public GridSizeItem[] GridSizeItems { get; private set; }
        public GridSizeItem SelectedGridSizeItem { get; set; }
        private SolvingDialog _solvingDialog;
        private NoSolutionDialog _noSolutionDialog;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            GridSizeItems = new[]
                {
                    new GridSizeItem(5),
                    new GridSizeItem(6),
                    new GridSizeItem(7),
                    new GridSizeItem(8),
                    new GridSizeItem(9),
                    new GridSizeItem(10)
                };
            SelectedGridSizeItem = GridSizeItems[2];

            ContentRendered += (_, __) => ChangeGridSize();
            GridSizeCombo.SelectionChanged += (_, __) =>
                {
                    //ChangeGridSize()
                };

            SolveButton.Click += (_, __) =>
                {
                    GridSizeCombo.IsEnabled = false;
                    SolveButton.IsEnabled = false;
                    ThreadPool.QueueUserWorkItem(state => SolveThePuzzle());
                    _solvingDialog = new SolvingDialog {Owner = this};
                    _solvingDialog.ShowDialog();
                };

            BoardControl.CellClicked += (_, e) =>
                {
                    //System.Windows.MessageBox.Show(string.Format("CellClicked: ({0}, {1})", e.Coords.X, e.Coords.Y));
                };
        }

        private void SolveThePuzzle()
        {
            var grid = new Grid(
                SelectedGridSizeItem.GridSize,
                SelectedGridSizeItem.GridSize,
                BoardControl.GetColourPairs().ToArray());

            var matrixBuilder = new MatrixBuilder();
            var matrix = matrixBuilder.BuildMatrixFor(grid);

            var dlx = new Dlx();
            var solutions = dlx.Solve(matrix).ToList();

            if (solutions.Any())
            {
                var colourPairPaths = solutions.First().RowIndexes.Select(matrixBuilder.GetColourPairAndPathForRowIndex);
                Dispatcher.Invoke(new WaitCallback(DrawTheSolution), colourPairPaths);
            }
            else
            {
                Dispatcher.Invoke(new WaitCallback(HandleNoSolutionFound), Enumerable.Empty<Tuple<ColourPair, Path>>());
            }
        }

        private void DrawTheSolution(object state)
        {
            var colourPairPaths = (IEnumerable<Tuple<ColourPair, Path>>)state;

            foreach (var colourPairPath in colourPairPaths)
            {
                var colourPair = colourPairPath.Item1;
                var path = colourPairPath.Item2;
                BoardControl.DrawPath(colourPair, path);
            }

            GridSizeCombo.IsEnabled = true;
            _solvingDialog.Close();
        }

        private void HandleNoSolutionFound(object state)
        {
            GridSizeCombo.IsEnabled = true;
            SolveButton.IsEnabled = true;
            _solvingDialog.Close();
            _noSolutionDialog = new NoSolutionDialog {Owner = this};
            _noSolutionDialog.ShowDialog();
        }

        private void ChangeGridSize()
        {
            BoardControl.Clear();
            BoardControl.GridSize = SelectedGridSizeItem.GridSize;
            BoardControl.DrawGrid();

            var grid = new Grid(7, 7, new[]
                {
                    new ColourPair(new Coords(6, 6), new Coords(5, 0), "A"),
                    new ColourPair(new Coords(5, 5), new Coords(1, 4), "B"),
                    new ColourPair(new Coords(6, 5), new Coords(4, 1), "C"),
                    new ColourPair(new Coords(3, 3), new Coords(2, 2), "D"),
                    new ColourPair(new Coords(4, 3), new Coords(6, 0), "E"),
                    new ColourPair(new Coords(4, 2), new Coords(5, 1), "F")
                });

            foreach (var colourPair in grid.ColourPairs)
            {
                BoardControl.AddDot(colourPair.StartCoords, colourPair.Tag);
                BoardControl.AddDot(colourPair.EndCoords, colourPair.Tag);
            }
        }
    }
}
