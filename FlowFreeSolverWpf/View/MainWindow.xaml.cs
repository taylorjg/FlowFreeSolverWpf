using FlowFreeSolverWpf.ViewModel;

namespace FlowFreeSolverWpf.View
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
