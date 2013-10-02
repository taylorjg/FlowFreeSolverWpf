using FlowFreeSolverWpf.ViewModel;

namespace FlowFreeSolverWpf
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel(
                new WpfDialogService(this), 
                new WpfDispatcher(Dispatcher),
                BoardControl);
        }
    }
}
