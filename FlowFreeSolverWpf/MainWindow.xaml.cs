using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DlxLib;
using FlowFreeSolverWpf.Model;

namespace FlowFreeSolverWpf
{
    public partial class MainWindow
    {
        public GridSizeItem[] GridSizeItems { get; private set; }
        public GridSizeItem SelectedGridSizeItem { get; set; }
        public DotColour[] DotColours { get; private set; }
        public DotColour SelectedDotColour { get; set; }
        private SolvingDialog _solvingDialog;
        private MatrixBuilder _matrixBuilder;
        private Dlx _dlx;
        private CancellationTokenSource _cancellationTokenSource;

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

            DotColours = ColourMap.DotColours;
            SelectedDotColour = DotColours[0];

            ContentRendered += (_, __) =>
                {
                    ChangeGridSize();
                };

            GridSizeCombo.SelectionChanged += (_, __) => ChangeGridSize();

            SolveButton.Click += (_, __) =>
                {
                    IList<ColourPair> colourPairs;
                    try
                    {
                        colourPairs = BoardControl.GetColourPairs();
                    }
                    catch (InvalidOperationException ex)
                    {
                        var myMessageBox = new MyMessageBox { Owner = this, MessageText = ex.Message };
                        myMessageBox.ShowDialog();
                        return;
                    }

                    GridSizeCombo.IsEnabled = false;
                    SolveButton.IsEnabled = false;
                    ClearButton.IsEnabled = false;

                    _cancellationTokenSource = new CancellationTokenSource();

                    ThreadPool.QueueUserWorkItem(state => SolveThePuzzle(colourPairs));

                    _solvingDialog = new SolvingDialog {Owner = this};
                    var dialogResult = _solvingDialog.ShowDialog();
                    if (dialogResult.HasValue && !dialogResult.Value)
                    {
                        _cancellationTokenSource.Cancel();
                        if (_dlx != null)
                        {
                            _dlx.Cancel();
                        }
                        //System.Windows.MessageBox.Show("Cancelled!");
                    }

                    // TODO: when should we clean up _cancellationTokenSource ?
                };

            ClearButton.Click += (_, __) =>
                {
                    BoardControl.ClearDotsAndPaths();
                    SolveButton.IsEnabled = true;
                };

            BoardControl.CellClicked += (_, e) =>
                {
                    if (BoardControl.IsDotAt(e.Coords))
                    {
                        BoardControl.RemoveDot(e.Coords);
                    }
                    else
                    {
                        BoardControl.AddDot(e.Coords, SelectedDotColour.Tag);
                    }
                };
        }

        private void PreLoad7X7Puzzle()
        {
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

        private void SolveThePuzzle(IEnumerable<ColourPair> colourPairs)
        {
            var grid = new Grid(
                SelectedGridSizeItem.GridSize,
                SelectedGridSizeItem.GridSize,
                colourPairs.ToArray());

            _matrixBuilder = new MatrixBuilder();
            var matrix = _matrixBuilder.BuildMatrixFor(grid, _cancellationTokenSource.Token);

            _dlx = new Dlx();
            var solutions = _dlx.Solve(matrix).ToList();

            if (_cancellationTokenSource.IsCancellationRequested)
            {
                Dispatcher.Invoke(new WaitCallback(HandleCancellation), Enumerable.Empty<Tuple<ColourPair, Path>>());
            }
            else
            {
                if (solutions.Any())
                {
                    var colourPairPaths = solutions.First().RowIndexes.Select(_matrixBuilder.GetColourPairAndPathForRowIndex);
                    Dispatcher.Invoke(new WaitCallback(DrawTheSolution), colourPairPaths);
                }
                else
                {
                    Dispatcher.Invoke(new WaitCallback(HandleNoSolutionFound), Enumerable.Empty<Tuple<ColourPair, Path>>());
                }
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
            ClearButton.IsEnabled = true;
            _solvingDialog.DialogResult = true;
            _solvingDialog.Close();
        }

        private void HandleNoSolutionFound(object state)
        {
            GridSizeCombo.IsEnabled = true;
            SolveButton.IsEnabled = true;
            ClearButton.IsEnabled = true;

            _solvingDialog.DialogResult = true;
            _solvingDialog.Close();

            var myMessageBox = new MyMessageBox {Owner = this, MessageText = "Sorry - no solution was found!"};
            myMessageBox.ShowDialog();
        }

        private void HandleCancellation(object state)
        {
            GridSizeCombo.IsEnabled = true;
            SolveButton.IsEnabled = true;
            ClearButton.IsEnabled = true;

            var myMessageBox = new MyMessageBox { Owner = this, MessageText = "You cancelled the solving process before completion!" };
            myMessageBox.ShowDialog();
        }

        private void ChangeGridSize()
        {
            BoardControl.Clear();
            BoardControl.GridSize = SelectedGridSizeItem.GridSize;
            BoardControl.DrawGrid();

            if (SelectedGridSizeItem.GridSize == 7)
            {
                PreLoad7X7Puzzle();
            }
        }
    }
}
