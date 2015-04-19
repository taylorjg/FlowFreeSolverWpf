namespace FlowFreeSolverWpf.Model
{
    public static class CoordsFactory
    {
        public static Coords GetCoords(int x, int y)
        {
            return new Coords(x, y);
        }

        public static void PrimeCache(int gridSize)
        {
        }

        public static int GetCacheSize()
        {
            return 0;
        }
    }
}
