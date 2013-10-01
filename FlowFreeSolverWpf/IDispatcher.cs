using System;

namespace FlowFreeSolverWpf
{
    public interface IDispatcher
    {
        void Invoke(Delegate method, params object[] args);
    }
}
