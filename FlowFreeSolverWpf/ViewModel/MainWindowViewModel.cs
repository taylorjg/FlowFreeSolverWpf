using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Threading;
using System.Windows.Input; // needed for ICommand only
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

            GridDescriptions = SampleGrids.SampleGridDescriptions;
            DotColours = Model.DotColours.Colours;
            SelectedGrid = GridDescriptions.First();
            SelectedDotColour = DotColours.First();
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
            {
                RemoveDot(coords);
            }
            else
            {
                if (_coordsToDots.Values.Count(dotColour => dotColour == SelectedDotColour) < 2)
                {
                    AddDot(coords, SelectedDotColour);
                }
                else
                {
                    // http://stackoverflow.com/questions/3044438/what-should-i-use-to-replace-the-winapi-beep-function
                    // http://stackoverflow.com/questions/5756855/c-sharp-play-sound-with-one-line-of-c-sharp-code

                    // TODO: embed the .wav file as a resource...
                    var simpleSound = new SoundPlayer(@"C:\Users\taylojo\Documents\Visual Studio 2010\Projects\FlowFreeSolverWpf\FlowFreeSolverWpf\Sounds\IllegalMove.wav");
                    simpleSound.Play();
                }
            }
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
            ClearPaths();
            SetSolvingStatusMessage();

            var colourPairs = GetColourPairs();
            var grid = new Grid(SelectedGrid.GridSize, colourPairs.ToArray());

            _cancellationTokenSource = new CancellationTokenSource();
            var puzzleSolver = new PuzzleSolver(
                grid,
                SelectedGrid,
                OnSolveSolutionFound,
                OnSolveNoSolutionFound,
                SetStatusMessageFromSolutionStats,
                _dispatcher,
                _cancellationTokenSource.Token);
            puzzleSolver.SolvePuzzle();

            var dialogResult = _dialogService.ShowSolvingDialog();
            if (dialogResult.HasValue && !dialogResult.Value)
            {
                _cancellationTokenSource.Cancel();
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

        private void OnSolveSolutionFound(SolutionStats solutionStats, IEnumerable<MatrixRow> colourPairPaths)
        {
            DrawSolution(colourPairPaths);
            SetStatusMessageFromSolutionStats(solutionStats);
            _dialogService.CloseSolvingDialog(true);
        }

        public void DrawSolution(IEnumerable<MatrixRow> colourPairPaths)
        {
            foreach (var colourPairPath in colourPairPaths)
            {
                var colourPair = colourPairPath.ColourPair;
                var path = colourPairPath.Path;
                _boardControl.DrawPath(colourPair, path);
            }
        }

        private void OnSolveNoSolutionFound(SolutionStats solutionStats)
        {
            _dialogService.CloseSolvingDialog(true);
            _dialogService.ShowMyMessageBox("Sorry - no solution was found!");
            SetStatusMessageFromSolutionStats(solutionStats /*, "no solution found" */);
        }

        private void OnClear()
        {
            ClearDots();
            ClearPaths();
        }

        private void OnSelectedGridChanged()
        {
            ClearDots();
            _boardControl.GridSize = SelectedGrid.GridSize;
            CoordsFactory.PrimeCache(SelectedGrid.GridSize);
            PreLoadSamplePuzzle();
            SetGridRequirementsStatusMessage();
            BoardControlHasChanged();
        }

        private void PreLoadSamplePuzzle()
        {
            foreach (var colourPair in SelectedGrid.SamplePuzzle)
            {
                AddDot(colourPair.StartCoords, colourPair.DotColour);
                AddDot(colourPair.EndCoords, colourPair.DotColour);
            }
        }

        private void BoardControlHasChanged()
        {
            if (_solveCommand != null)
            {
                _solveCommand.RaiseCanExecuteChanged();
            }

            UpdateStatusMessage();
        }

        private void UpdateStatusMessage()
        {
            if (SolveCommand.CanExecute(null))
                SetReadyToSolveStatusMessage();
            else
            {
                SetGridRequirementsStatusMessage();
            }
        }

        private void SetGridRequirementsStatusMessage()
        {
            StatusMessage = string.Format(
                "Please enter between {0} and {1} different coloured pairs of dots",
                SelectedGrid.MinColourPairs,
                SelectedGrid.MaxColourPairs);
        }

        private void SetReadyToSolveStatusMessage()
        {
            StatusMessage = "Ready to solve";
        }

        private void SetSolvingStatusMessage()
        {
            StatusMessage = "Solving...";
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

        private void ClearDots()
        {
            _boardControl.ClearDots();
            _coordsToDots.Clear();
            BoardControlHasChanged();
        }

        private void ClearPaths()
        {
            _boardControl.ClearPaths();
            SetGridRequirementsStatusMessage();
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
