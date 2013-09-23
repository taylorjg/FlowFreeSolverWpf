using System.Windows.Media;
using FlowFreeSolverWpf.Model;

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
            SelectedGridSizeItem = GridSizeItems[2];

            ContentRendered += (_, __) => ChangeGridSize();
            GridSizeCombo.SelectionChanged += (_, __) =>
                {
                    //ChangeGridSize()
                };
        }

        private void ChangeGridSize()
        {
            BoardControl.Clear();
            BoardControl.GridSize = SelectedGridSizeItem.GridSize;
            BoardControl.DrawGrid();

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
    }
}
