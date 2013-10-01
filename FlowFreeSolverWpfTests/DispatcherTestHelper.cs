using System.Windows.Threading;

namespace FlowFreeSolverWpfTests
{
    // http://stackoverflow.com/questions/12033798/why-doesnt-relaycommand-raisecanexecutechanged-work-in-a-unit-test

    internal static class DispatcherTestHelper
    {
        private static readonly DispatcherOperationCallback ExitFrameCallback = ExitFrame;

        public static void ProcessWorkItems(DispatcherPriority minimumPriority)
        {
            var frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(minimumPriority, ExitFrameCallback, frame);
            Dispatcher.PushFrame(frame);
        }

        private static object ExitFrame(object state)
        {
            var frame = (DispatcherFrame)state;
            frame.Continue = false;
            return null;
        }
    }
}
