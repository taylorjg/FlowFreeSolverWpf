namespace FlowFreeSolverWpf
{
    public interface IDialogService
    {
        bool? ShowSolvingDialog();
        void CloseSolvingDialog(bool? dialogResult = null);
        bool? ShowMyMessageBox(string messageText);
        void CloseMyMessageBox(bool? dialogResult = null);
    }
}
