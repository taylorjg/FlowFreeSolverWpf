using System;
using System.Diagnostics;
using System.Windows.Threading;

namespace FlowFreeSolverWpf.View
{
    public partial class SolvingDialog
    {
        private readonly DispatcherTimer _dispatcherTimer = new DispatcherTimer();
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public SolvingDialog()
        {
            InitializeComponent();

            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(10);
            _dispatcherTimer.Tick += UpdateElapsedTime;
            _dispatcherTimer.Start();
            _stopwatch.Start();

            Closed += (sender, args) =>
                {
                    _dispatcherTimer.Stop();
                    _stopwatch.Stop();
                };
        }

        private void UpdateElapsedTime(object sender, EventArgs eventArgs)
        {
            ElapsedTime.Content = _stopwatch.Elapsed.ToString(@"hh\:mm\:ss\.fff");
        }
    }
}
