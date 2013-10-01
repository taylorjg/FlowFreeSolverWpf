using FakeItEasy;
using FlowFreeSolverWpf;
using FlowFreeSolverWpf.ViewModel;
using NUnit.Framework;

namespace FlowFreeSolverWpfTests
{
    [TestFixture]
    internal class MainWindowViewModelTests
    {
        [Test]
        public void ExecutingSelectedGridChangedCommandShouldTellTheBoardControlToChangeGridSize()
        {
            var fakeBoardControl = A.Fake<IBoardControl>();
            var mainWindowViewModel = new MainWindowViewModel(fakeBoardControl);
            mainWindowViewModel.SelectedGridChangedCommand.Execute(null);
            A.CallTo(fakeBoardControl).Where(x => x.Method.Name.Equals("set_GridSize")).WithAnyArguments().MustHaveHappened(); 
            A.CallTo(fakeBoardControl).Where(x => x.Method.Name.Equals("set_GridSize")).WhenArgumentsMatch(x => x[0].Equals(7)).MustHaveHappened(); 
        }
    }
}
