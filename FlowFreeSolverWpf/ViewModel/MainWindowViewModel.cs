using System.Windows.Input;
using FlowFreeSolverWpf.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace FlowFreeSolverWpf.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private GridDescription _selectedGrid;
        private DotColour _selectedDotColour;
        private string _statusMessage;
        private ICommand _solveCommand;
        private ICommand _clearCommand;
        private ICommand _selectedGridChangedCommand;

        public MainWindowViewModel()
        {
            GridDescriptions = Grids.GridDescriptions;
            DotColours = Model.DotColours.Colours;
            StatusMessage = string.Empty;
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

        public ICommand SolveCommand
        {
            get { return _solveCommand ?? (_solveCommand = new RelayCommand(OnSolve, OnCanSolve)); }
        }

        public ICommand ClearCommand
        {
            get { return _clearCommand ?? (_clearCommand = new RelayCommand(OnClear, OnCanClear)); }
        }

        // http://stackoverflow.com/questions/3131142/mvvm-light-silverlight-using-eventtocommand-with-a-combobox

        public ICommand SelectedGridChangedCommand
        {
            get { return _selectedGridChangedCommand ?? (_selectedGridChangedCommand = new RelayCommand(OnSelectedGridChanged)); }
        }

        private void OnSolve()
        {
        }

        private bool OnCanSolve()
        {
            return true;
        }

        private void OnClear()
        {
        }

        private bool OnCanClear()
        {
            return true;
        }

        private void OnSelectedGridChanged()
        {
        }
    }
}
