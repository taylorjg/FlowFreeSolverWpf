using System.Windows.Media;

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
            SelectedGridSizeItem = GridSizeItems[0];

            ContentRendered += (_, __) => ChangeGridSize();
            GridSizeCombo.SelectionChanged += (_, __) => ChangeGridSize();
        }

        private void ChangeGridSize()
        {
            BoardControl.Clear();
            BoardControl.GridSize = SelectedGridSizeItem.GridSize;
            BoardControl.DrawGrid();
            BoardControl.AddDot(Colors.Blue, 3, 4);
            BoardControl.AddDot(Colors.Red, 4, 4);
        }
    }
}
