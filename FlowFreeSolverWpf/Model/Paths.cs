using System.Collections.Generic;

namespace FlowFreeSolverWpf.Model
{
    public class Paths
    {
        private readonly IList<Path> _pathList;

        public Paths()
        {
            _pathList = new List<Path>();
        }

        public void AddPath(Path path)
        {
            // TODO: I think this is where the bug was. Is this a good enough fix ?
            // When we were only collecting completed paths, it made sense to check
            // for duplicates. But now we also collect inactive paths and we don't
            // want to check inactive paths for duplicates.

            if (path.IsActive)
            {
                if (!_pathList.Contains(path))
                {
                    _pathList.Add(path);
                }
            }
            else
            {
                _pathList.Add(path);
            }
        }

        public IEnumerable<Path> PathList {
            get { return _pathList; }
        }
    }
}
