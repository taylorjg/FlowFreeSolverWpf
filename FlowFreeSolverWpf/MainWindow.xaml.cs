﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using DlxLib;
using FlowFreeSolverWpf.Model;

namespace FlowFreeSolverWpf
{
    public partial class MainWindow : INotifyPropertyChanged
    {
        public GridDescription[] GridDescriptions { get; private set; }
        public GridDescription SelectedGrid { get; set; }
        public DotColour[] DotColours { get; private set; }
        public DotColour SelectedDotColour { get; set; }

        private string _statusMessage;
        public string StatusMessage
        {
            get
            {
                return _statusMessage;
            }
            private set
            {
                if (value == _statusMessage)
                {
                    return;
                }

                _statusMessage = value;
                RaisePropertyChanged("StatusMessage");
            }
        }

        private SolvingDialog _solvingDialog;
        private MatrixBuilder _matrixBuilder;
        private SolutionStats _solutionStats;
        private Dlx _dlx;
        private CancellationTokenSource _cancellationTokenSource;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            GridDescriptions = Grids.GridDescriptions;
            SelectedGrid = GridDescriptions[2];

            DotColours = ColourMap.DotColours;
            SelectedDotColour = DotColours[0];

            StatusMessage = string.Empty;

            ContentRendered += (_, __) => ChangeGridSize();
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

                    BoardControl.ClearPaths();

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
                    }
                };

            ClearButton.Click += (_, __) =>
                {
                    BoardControl.ClearDots();
                    BoardControl.ClearPaths();
                    SolveButton.IsEnabled = true;
                    StatusMessage = string.Empty;
                };

            BoardControl.CellClicked += (_, e) =>
                {
                    if (BoardControl.IsDotAt(e.Coords))
                    {
                        BoardControl.RemoveDot(e.Coords);
                    }
                    else
                    {
                        BoardControl.AddDot(e.Coords, SelectedDotColour);
                    }
                };
        }

        private void SolveThePuzzle(IEnumerable<ColourPair> colourPairs)
        {
            StatusMessage = string.Empty;

            var grid = new Grid(SelectedGrid.GridSize, colourPairs.ToArray());

            var matrix = new bool[0,0];
            TimeSpan? matrixBuildingDuration = null;
            TimeSpan? matrixBuildingSolving = null;
            var solutions = new List<Solution>();

            if (!_cancellationTokenSource.IsCancellationRequested)
            {
                var stopwatch = new System.Diagnostics.Stopwatch();

                _matrixBuilder = new MatrixBuilder();
                stopwatch.Reset();
                stopwatch.Start();
                matrix = _matrixBuilder.BuildMatrixFor(grid, SelectedGrid.InitialMaxDirectionChanges, _cancellationTokenSource.Token);
                stopwatch.Stop();
                matrixBuildingDuration = stopwatch.Elapsed;

                if (!_cancellationTokenSource.IsCancellationRequested)
                {
                    _dlx = new Dlx();

                    // We only want the first solution - don't waste time trying to find further solutions.
                    _dlx.SolutionFound += (_, __) => _dlx.Cancel();

                    stopwatch.Reset();
                    stopwatch.Start();
                    solutions = _dlx.Solve(matrix).ToList();
                    stopwatch.Stop();
                    matrixBuildingSolving = stopwatch.Elapsed;
                }
            }

            _solutionStats = new SolutionStats(
                matrix.GetLength(0),
                matrix.GetLength(1),
                matrixBuildingDuration,
                matrixBuildingSolving);

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
            SolveButton.IsEnabled = true;
            ClearButton.IsEnabled = true;
            _solvingDialog.DialogResult = true;
            _solvingDialog.Close();

            SetStatusMessageFromSolutionStats(_solutionStats);
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

            SetStatusMessageFromSolutionStats(_solutionStats, "no solution found");
        }

        private void HandleCancellation(object state)
        {
            GridSizeCombo.IsEnabled = true;
            SolveButton.IsEnabled = true;
            ClearButton.IsEnabled = true;

            var myMessageBox = new MyMessageBox { Owner = this, MessageText = "You cancelled the solving process before completion!" };
            myMessageBox.ShowDialog();

            SetStatusMessageFromSolutionStats(_solutionStats, "cancelled by user");
        }

        private void SetStatusMessageFromSolutionStats(SolutionStats solutionStats, string extraMessage = null)
        {
            var statusMessage = string.Format(
                "Matrix size: {0} rows x {1} cols",
                solutionStats.NumMatrixRows,
                solutionStats.NumMatrixCols);

            const string timeSpanFormat = @"hh\:mm\:ss\.fff";

            if (solutionStats.MatrixBuildingDuration.HasValue)
            {
                 statusMessage += string.Format("; Matrix build time: {0}", solutionStats.MatrixBuildingDuration.Value.ToString(timeSpanFormat));
            }

            if (solutionStats.MatrixSolvingDuration.HasValue)
            {
                statusMessage += string.Format("; Matrix solve time: {0}", solutionStats.MatrixSolvingDuration.Value.ToString(timeSpanFormat));
            }

            if (!string.IsNullOrEmpty(extraMessage))
            {
                statusMessage += string.Format(" ({0})", extraMessage);
            }

            StatusMessage = statusMessage;
        }

        private void ChangeGridSize()
        {
            BoardControl.ClearAll();
            SolveButton.IsEnabled = true;
            StatusMessage = string.Empty;
            BoardControl.GridSize = SelectedGrid.GridSize;
            BoardControl.DrawGrid();
            PreLoadSamplePuzzle(SelectedGrid);
        }

        private void PreLoadSamplePuzzle(GridDescription gridDescription)
        {
            foreach (var colourPair in gridDescription.SamplePuzzle)
            {
                BoardControl.AddDot(colourPair.StartCoords, colourPair.DotColour);
                BoardControl.AddDot(colourPair.EndCoords, colourPair.DotColour);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
