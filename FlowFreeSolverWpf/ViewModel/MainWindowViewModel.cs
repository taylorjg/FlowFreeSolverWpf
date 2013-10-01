using System.Windows.Input;
using FlowFreeSolverWpf.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace FlowFreeSolverWpf.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IBoardControl _boardControl;
        private GridDescription _selectedGrid;
        private DotColour _selectedDotColour;
        private string _statusMessage;
        private ICommand _loadedCommand;
        private ICommand _solveCommand;
        private ICommand _clearCommand;
        private ICommand _selectedGridChangedCommand;

        public MainWindowViewModel(IBoardControl boardControl)
        {
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
        }

        public ICommand LoadedCommand
        {
            get { return _loadedCommand ?? (_loadedCommand = new RelayCommand(OnLoaded)); }
        }

        public ICommand SolveCommand
        {
            get { return _solveCommand ?? (_solveCommand = new RelayCommand(OnSolve, OnCanSolve)); }
        }

        public ICommand ClearCommand
        {
            get { return _clearCommand ?? (_clearCommand = new RelayCommand(OnClear)); }
        }

        // http://stackoverflow.com/questions/3131142/mvvm-light-silverlight-using-eventtocommand-with-a-combobox

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
            System.Windows.MessageBox.Show("OnSolve");
        }

        private bool OnCanSolve()
        {
            // return _boardControl.GetColourPairs(SelectedGrid) != null;
            return true;
        }

        private void OnClear()
        {
            _boardControl.ClearDots();
            _boardControl.ClearPaths();
            ClearStatusMessage();
        }

        private void OnSelectedGridChanged()
        {
            _boardControl.GridSize = SelectedGrid.GridSize;
            ClearStatusMessage();
        }

        private void ClearStatusMessage()
        {
            StatusMessage = string.Empty;
        }
    }
}
