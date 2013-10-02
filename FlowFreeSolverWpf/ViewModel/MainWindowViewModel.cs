using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using FlowFreeSolverWpf.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace FlowFreeSolverWpf.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;
        private readonly IDispatcher _dispatcher;
        private readonly IMainWindow _mainWindow;
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

        public MainWindowViewModel(IDialogService dialogService, IDispatcher dispatcher, IMainWindow mainWindow, IBoardControl boardControl)
        {
            _dialogService = dialogService;
            _dispatcher = dispatcher;
            _mainWindow = mainWindow;
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
                RaisePropertyChanged("SelectedGrid");
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
                RaisePropertyChanged("SelectedDotColour");
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
                RaisePropertyChanged("StatusMessage");
            }
        }

        public void CellClicked(Coords coords)
        {
            if (_boardControl.IsDotAt(coords))
                _boardControl.RemoveDot(coords);
            else
                _boardControl.AddDot(coords, SelectedDotColour);
            BoardControlHasChanged();
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
            var colourPairs = _boardControl.GetColourPairs();
            var grid = new Grid(SelectedGrid.GridSize, colourPairs.ToArray());
            _cancellationTokenSource = new CancellationTokenSource();
            var puzzleSolver = new PuzzleSolver(
                grid,
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
                var colourPairs = _boardControl.GetColourPairs();
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
            _mainWindow.OnSolveSolutionFound(colourPairPaths);
            _dialogService.CloseSolvingDialog(true);
        }

        private void OnSolveNoSolutionFound(SolutionStats solutionStats)
        {
            _mainWindow.OnSolveNoSolutionFound();
            _dialogService.CloseSolvingDialog(true);
            _dialogService.ShowMyMessageBox("Sorry - no solution was found!");
        }

        private void OnSolveCancelled(SolutionStats solutionStats)
        {
            _mainWindow.OnSolveCancelled();
            _dialogService.ShowMyMessageBox("You cancelled the solving process before completion!");
        }

        private void OnClear()
        {
            _boardControl.ClearDots();
            _boardControl.ClearPaths();
            ClearStatusMessage();
            BoardControlHasChanged();
        }

        private void OnSelectedGridChanged()
        {
            _boardControl.GridSize = SelectedGrid.GridSize;
            PreLoadSamplePuzzle();
            ClearStatusMessage();
            BoardControlHasChanged();
        }

        private void PreLoadSamplePuzzle()
        {
            foreach (var colourPair in SelectedGrid.SamplePuzzle)
            {
                _boardControl.AddDot(colourPair.StartCoords, colourPair.DotColour);
                _boardControl.AddDot(colourPair.EndCoords, colourPair.DotColour);
            }
        }

        private void ClearStatusMessage()
        {
            StatusMessage = string.Empty;
        }

        private void BoardControlHasChanged()
        {
            if (_solveCommand != null)
            {
                _solveCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
