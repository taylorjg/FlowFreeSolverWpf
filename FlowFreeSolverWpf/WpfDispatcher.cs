using System;
using System.Windows.Threading;
using FlowFreeSolverWpf.ViewModel;

namespace FlowFreeSolverWpf
{
    class WpfDispatcher : IDispatcher
    {
        private readonly Dispatcher _dispatcher;

        public WpfDispatcher(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void Invoke(Delegate method, params object[] args)
        {
            _dispatcher.Invoke(method, args);
        }
    }
}
