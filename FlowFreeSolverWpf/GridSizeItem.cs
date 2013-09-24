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
}
