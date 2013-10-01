using System;
using System.Collections.Generic;
using FlowFreeSolverWpf.Model;

namespace FlowFreeSolverWpf
{
    public interface IMainWindow
    {
        void OnSolveSolutionFound(SolutionStats solutionStats, IEnumerable<Tuple<ColourPair, Path>> colourPairPaths);
        void OnSolveNoSolutionFound(SolutionStats solutionStats);
        void OnSolveCancelled(SolutionStats solutionStats);
    }
}
