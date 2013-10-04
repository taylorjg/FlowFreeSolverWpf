using System;
using System.Collections.Generic;

namespace FlowFreeSolverWpf.Model
{
    // Flyweight pattern
    // http://en.wikipedia.org/wiki/Flyweight_pattern

    public static class CoordsFactory
    {
        private static readonly IDictionary<Tuple<int, int>, Coords> Cache = new Dictionary<Tuple<int, int>, Coords>();

        public static Coords GetCoords(int x, int y)
        {
            var key = Tuple.Create(x, y);
            if (Cache.ContainsKey(key))
            {
                return Cache[key];
            }

            var coords = new Coords(x, y);
            Cache[key] = coords;

            return coords;
        }

        public static void PrimeCache(int gridSize)
        {
            for (var x = 0; x < gridSize; x++)
            {
                for (var y = 0; y < gridSize; y++)
                {
                    GetCoords(x, y);
                }
            }
        }

        public static int GetCacheSize()
        {
            return Cache.Count;
        }
    }
}
