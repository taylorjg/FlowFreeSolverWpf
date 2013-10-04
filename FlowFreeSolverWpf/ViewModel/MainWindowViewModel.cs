using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Input; // for ICommand only
using FlowFreeSolverWpf.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace FlowFreeSolverWpf.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;
        private readonly IDispatcher _dispatcher;
        private readonly IBoardControl _boardControl;
        private GridDescription _selectedGrid;
        private DotColour _selectedDotColour;
        private string _statusMessage;
        private RelayCommand _loadedCommand;
        private RelayCommand _solveCommand;
        private RelayCommand _cancelCommand;
        private RelayCommand _clearCommand;
        private RelayCommand _selectedGridChangedCommand;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly IDictionary<Coords, DotColour> _coordsToDots = new Dictionary<Coords, DotColour>();

        public MainWindowViewModel(IDialogService dialogService, IDispatcher dispatcher, IBoardControl boardControl)
        {
            _dialogService = dialogService;
            _dispatcher = dispatcher;
            _boardControl = boardControl;
            _boardControl.CellClicked += (_, e) => CellClicked(e.Coords);
            GridDescriptions = Grids.GridDescriptions;
            DotColours = Model.DotColours.Colours;
            SelectedGrid = GridDescriptions[2];
            SelectedDotColour = DotColours[0];
            ClearStatusMessage();
        }

        public GridDescription[] GridDescriptions { get; private set; }
        public DotColour[] DotColours { get; private set; }

        public GridDescription SelectedGrid
        {
            get
            {
                return _selectedGrid;
            }
            set
            {
                _selectedGrid = value;
                RaisePropertyChanged(() => SelectedGrid);
            }
        }

        public DotColour SelectedDotColour
        {
            get
            {
                return _selectedDotColour;
            }
            set
            {
                _selectedDotColour = value;
                RaisePropertyChanged(() => SelectedDotColour);
            }
        }

        public string StatusMessage
        {
            get
            {
                return _statusMessage;
            }
            private set
            {
                _statusMessage = value;
                RaisePropertyChanged(() => StatusMessage);
            }
        }

        public void CellClicked(Coords coords)
        {
            if (IsDotAt(coords))
                RemoveDot(coords);
            else
                AddDot(coords, SelectedDotColour);
        }

        public ICommand LoadedCommand
        {
            get { return _loadedCommand ?? (_loadedCommand = new RelayCommand(OnLoaded)); }
        }

        public ICommand SolveCommand
        {
            get { return _solveCommand ?? (_solveCommand = new RelayCommand(OnSolve, OnCanSolve)); }
        }

        public ICommand CancelCommand
        {
            get { return _cancelCommand ?? (_cancelCommand = new RelayCommand(OnCancel)); }
        }

        public ICommand ClearCommand
        {
            get { return _clearCommand ?? (_clearCommand = new RelayCommand(OnClear)); }
        }

        public ICommand SelectedGridChangedCommand
        {
            get { return _selectedGridChangedCommand ?? (_selectedGridChangedCommand = new RelayCommand(OnSelectedGridChanged)); }
        }

        private void OnLoaded()
        {
            OnSelectedGridChanged();
        }

        private void OnSolve()
        {
            _boardControl.ClearPaths();
            ClearStatusMessage();
            var colourPairs = GetColourPairs();
            var grid = new Grid(SelectedGrid.GridSize, colourPairs.ToArray());
            _cancellationTokenSource = new CancellationTokenSource();
            var puzzleSolver = new PuzzleSolver(
                grid,
                SelectedGrid,
                OnSolveSolutionFound,
                OnSolveNoSolutionFound,
                OnSolveCancelled,
                _dispatcher,
                _cancellationTokenSource.Token);
            puzzleSolver.SolvePuzzle();

            var dialogResult = _dialogService.ShowSolvingDialog();
            if (dialogResult.HasValue && !dialogResult.Value)
            {
                _cancellationTokenSource.Cancel();

                // TODO: also, we should cancel Dlx - via PuzzleSolver ?
                //if (_dlx != null)
                //{
                //    _dlx.Cancel();
                //}
            }
        }

        private bool OnCanSolve()
        {
            try
            {
                var colourPairs = GetColourPairs();
                var numColourPairs = colourPairs.Count;
                return (numColourPairs >= SelectedGrid.MinColourPairs && numColourPairs <= SelectedGrid.MaxColourPairs);
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        private void OnCancel()
        {
            _cancellationTokenSource.Cancel();
        }

        private void OnSolveSolutionFound(SolutionStats solutionStats, IEnumerable<Tuple<ColourPair, Path>> colourPairPaths)
        {
            DrawSolution(colourPairPaths);
            SetStatusMessageFromSolutionStats(solutionStats);
            _dialogService.CloseSolvingDialog(true);
        }

        public void DrawSolution(IEnumerable<Tuple<ColourPair, Path>> colourPairPaths)
        {
            foreach (var colourPairPath in colourPairPaths)
            {
                var colourPair = colourPairPath.Item1;
                var path = colourPairPath.Item2;
                _boardControl.DrawPath(colourPair, path);
            }
        }

        private void OnSolveNoSolutionFound(SolutionStats solutionStats)
        {
            _dialogService.CloseSolvingDialog(true);
            _dialogService.ShowMyMessageBox("Sorry - no solution was found!");
            SetStatusMessageFromSolutionStats(solutionStats /*, "no solution found" */);
        }

        private void OnSolveCancelled(SolutionStats solutionStats)
        {
            _dialogService.ShowMyMessageBox("You cancelled the solving process before completion!");
            SetStatusMessageFromSolutionStats(solutionStats /*, "cancelled" */);
        }

        private void OnClear()
        {
            _boardControl.ClearDots();
            _boardControl.ClearPaths();
            ClearStatusMessage();
            BoardControlHasChanged();
            SetSelectedGridStatusMessage();
        }

        private void OnSelectedGridChanged()
        {
            _coordsToDots.Clear();
            _boardControl.GridSize = SelectedGrid.GridSize;
            PreLoadSamplePuzzle();
            BoardControlHasChanged();
            SetSelectedGridStatusMessage();
        }

        private void PreLoadSamplePuzzle()
        {
            foreach (var colourPair in SelectedGrid.SamplePuzzle)
            {
                AddDot(colourPair.StartCoords, colourPair.DotColour);
                AddDot(colourPair.EndCoords, colourPair.DotColour);
            }
        }

        private void ClearStatusMessage()
        {
            StatusMessage = string.Empty;
        }

        private void SetSelectedGridStatusMessage()
        {
            StatusMessage = string.Format(
                "There must be {0} to {1} pairs of dots",
                SelectedGrid.MinColourPairs,
                SelectedGrid.MaxColourPairs);
        }

        private void BoardControlHasChanged()
        {
            if (_solveCommand != null)
            {
                _solveCommand.RaiseCanExecuteChanged();
            }
        }

        private void SetStatusMessageFromSolutionStats(SolutionStats solutionStats /*, string extraMessage = null */)
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

            //if (!string.IsNullOrEmpty(extraMessage))
            //{
            //    statusMessage += string.Format(" ({0})", extraMessage);
            //}

            statusMessage += string.Format("; Direction changes: {0}", solutionStats.MaxDirectionChanges);

            StatusMessage = statusMessage;
        }

        private void AddDot(Coords coords, DotColour dotColour)
        {
            _boardControl.AddDot(coords, dotColour);
            _coordsToDots.Add(coords, dotColour);
            BoardControlHasChanged();
        }

        private void RemoveDot(Coords coords)
        {
            _boardControl.RemoveDot(coords);
            _coordsToDots.Remove(coords);
            BoardControlHasChanged();
        }

        private bool IsDotAt(Coords coords)
        {
            return _coordsToDots.ContainsKey(coords);
        }

        private IList<ColourPair> GetColourPairs()
        {
            var dotsGroupedByTag = _coordsToDots.GroupBy(kvp => kvp.Value,
                                                         kvp => new {Coords = kvp.Key})
                                                .ToList();

            if (dotsGroupedByTag.Any(grouping => grouping.Count() != 2))
            {
                throw new InvalidOperationException("Dots must be in pairs!");
            }

            return dotsGroupedByTag.Select(grouping =>
            {
                var startCoords = grouping.ElementAt(0).Coords;
                var endCoords = grouping.ElementAt(1).Coords;
                var dotColour = grouping.Key;
                return new ColourPair(startCoords, endCoords, dotColour);
            }).ToList();
        }
    }
}
