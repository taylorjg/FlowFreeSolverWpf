using System;

namespace FlowFreeSolverWpf.ViewModel
{
    public interface IDispatcher
    {
        void Invoke(Delegate method, params object[] args);
    }
}
