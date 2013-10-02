using System;
using System.Collections.Generic;
using FlowFreeSolverWpf.Model;

namespace FlowFreeSolverWpf
{
    public interface IMainWindow
    {
        void OnSolveSolutionFound(IEnumerable<Tuple<ColourPair, Path>> colourPairPaths);
        void OnSolveNoSolutionFound();
        void OnSolveCancelled();
    }
}
