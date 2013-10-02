using System.Windows;

namespace FlowFreeSolverWpf
{
    class WpfDialogService : IDialogService
    {
        private readonly Window _mainWindow;
        private SolvingDialog _solvingDialog;
        private MyMessageBox _myMessageBox;

        public WpfDialogService(Window mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public bool? ShowSolvingDialog()
        {
            _solvingDialog = new SolvingDialog {Owner = _mainWindow};
            return _solvingDialog.ShowDialog();
        }

        public void CloseSolvingDialog(bool? dialogResult)
        {
            if (dialogResult.HasValue)
            {
                _solvingDialog.DialogResult = dialogResult;
            }

            _solvingDialog.Close();
        }

        public bool? ShowMyMessageBox(string messageText)
        {
            _myMessageBox = new MyMessageBox {Owner = _mainWindow, MessageText = messageText};
            return _myMessageBox.ShowDialog();
        }

        public void CloseMyMessageBox(bool? dialogResult)
        {
            if (dialogResult.HasValue)
            {
                _myMessageBox.DialogResult = dialogResult;
            }

            _myMessageBox.Close();
        }
    }
}
