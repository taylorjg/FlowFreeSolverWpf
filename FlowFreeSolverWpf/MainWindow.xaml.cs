using System;
using System.Collections.Generic;
using FlowFreeSolverWpf.Model;
using FlowFreeSolverWpf.ViewModel;

namespace FlowFreeSolverWpf
{
    public partial class MainWindow : IMainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel(
                new WpfDialogService(this), 
                new WpfDispatcher(Dispatcher),
                this,
                BoardControl);
        }

        //private void SetStatusMessageFromSolutionStats(SolutionStats solutionStats, string extraMessage = null)
        //{
        //    var statusMessage = string.Format(
        //        "Matrix size: {0} rows x {1} cols",
        //        solutionStats.NumMatrixRows,
        //        solutionStats.NumMatrixCols);

        //    const string timeSpanFormat = @"hh\:mm\:ss\.fff";

        //    if (solutionStats.MatrixBuildingDuration.HasValue)
        //    {
        //         statusMessage += string.Format("; Matrix build time: {0}", solutionStats.MatrixBuildingDuration.Value.ToString(timeSpanFormat));
        //    }

        //    if (solutionStats.MatrixSolvingDuration.HasValue)
        //    {
        //        statusMessage += string.Format("; Matrix solve time: {0}", solutionStats.MatrixSolvingDuration.Value.ToString(timeSpanFormat));
        //    }

        //    if (!string.IsNullOrEmpty(extraMessage))
        //    {
        //        statusMessage += string.Format(" ({0})", extraMessage);
        //    }

        //    statusMessage += string.Format("; Direction changes: {0}", solutionStats.MaxDirectionChanges);

        //    StatusMessage = statusMessage;
        //}

        public void OnSolveSolutionFound(IEnumerable<Tuple<ColourPair, Path>> colourPairPaths)
        {
            foreach (var colourPairPath in colourPairPaths)
            {
                var colourPair = colourPairPath.Item1;
                var path = colourPairPath.Item2;
                BoardControl.DrawPath(colourPair, path);
            }
        }

        public void OnSolveNoSolutionFound()
        {
        }

        public void OnSolveCancelled()
        {
        }
    }
}
