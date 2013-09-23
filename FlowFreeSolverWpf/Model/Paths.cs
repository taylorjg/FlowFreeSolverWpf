using System.Collections.Generic;
using System.Linq;

namespace FlowFreeSolverWpf.Model
{
    public class Paths
    {
        private readonly IList<Path> _pathList = new List<Path>();

        public void AddPath(Path path)
        {
            if (!ContainsPath(path))
            {
                _pathList.Add(path);
            }
        }

        public bool ContainsPath(Path path)
        {
            return _pathList.Any(p => p.CoordsList.SequenceEqual(path.CoordsList));
        }

        public IEnumerable<Path> PathList {
            get { return _pathList; }
        }
    }
}
