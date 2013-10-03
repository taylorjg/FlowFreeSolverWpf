using System.Windows.Threading;
using FakeItEasy;
using FlowFreeSolverWpf.ViewModel;
using NUnit.Framework;

namespace FlowFreeSolverWpfTests
{
    [TestFixture]
    internal class MainWindowViewModelTests
    {
        private IDialogService _fakeDialogService;
        private IDispatcher _fakeDispatcher;
        private IBoardControl _fakeBoardControl;
        private MainWindowViewModel _mainWindowViewModel;

        [SetUp]
        public void SetUp()
        {
            _fakeDialogService = A.Fake<IDialogService>();
            _fakeDispatcher = A.Fake<IDispatcher>();
            _fakeBoardControl = A.Fake<IBoardControl>();
            _mainWindowViewModel = new MainWindowViewModel(_fakeDialogService, _fakeDispatcher, _fakeBoardControl);
        }

        [Test]
        public void ChangingSelectedGridRaisesPropertyChangedEvent()
        {
            var propertyName = string.Empty;
            _mainWindowViewModel.PropertyChanged += (_, e) => propertyName = e.PropertyName;
            _mainWindowViewModel.SelectedGrid = _mainWindowViewModel.GridDescriptions[0];
            DispatcherTestHelper.ProcessWorkItems(DispatcherPriority.Background);
            Assert.That(propertyName, Is.EqualTo("SelectedGrid"));
        }

        [Test]
        public void ChangingSelectedDotColourRaisesPropertyChangedEvent()
        {
            var propertyName = string.Empty;
            _mainWindowViewModel.PropertyChanged += (_, e) => propertyName = e.PropertyName;
            _mainWindowViewModel.SelectedDotColour = _mainWindowViewModel.DotColours[10];
            DispatcherTestHelper.ProcessWorkItems(DispatcherPriority.Background);
            Assert.That(propertyName, Is.EqualTo("SelectedDotColour"));
        }

        [Test]
        public void ChangingStatusMessageRaisesPropertyChangedEvent()
        {
            const string expectedPropertyName = "StatusMessage";
            var propertyName = string.Empty;
            _mainWindowViewModel.PropertyChanged += (_, e) =>
                { if (e.PropertyName == expectedPropertyName) propertyName = e.PropertyName; };
            _mainWindowViewModel.SelectedGridChangedCommand.Execute(null);
            DispatcherTestHelper.ProcessWorkItems(DispatcherPriority.Background);
            Assert.That(propertyName, Is.EqualTo(expectedPropertyName));
        }

        [Test]
        public void SelectingAGridChangesTheBoardControlGridSize()
        {
            _mainWindowViewModel.SelectedGrid = _mainWindowViewModel.GridDescriptions[0];
            _mainWindowViewModel.SelectedGridChangedCommand.Execute(null);
            var expectedGridSize = _mainWindowViewModel.GridDescriptions[0].GridSize;
            A.CallTo(_fakeBoardControl)
             .Where(x => x.Method.Name.Equals("set_GridSize"))
             .WhenArgumentsMatch(x => x[0].Equals(expectedGridSize))
             .MustHaveHappened();
        }

        [Test]
        public void ExecutingTheClearCommandClearsTheBoardControlOfDotsAndPaths()
        {
            _mainWindowViewModel.ClearCommand.Execute(null);
            A.CallTo(() => _fakeBoardControl.ClearDots()).MustHaveHappened();
            A.CallTo(() => _fakeBoardControl.ClearPaths()).MustHaveHappened();
        }

        [Test]
        public void ExecutingTheClearCommandRaisesCanExecuteChangedOnTheSolveCommand()
        {
            var wasRaised = false;
            _mainWindowViewModel.SolveCommand.CanExecuteChanged += (_, __) => wasRaised = true;
            _mainWindowViewModel.ClearCommand.Execute(null);
            DispatcherTestHelper.ProcessWorkItems(DispatcherPriority.Background);
            Assert.That(wasRaised, Is.True, "Expected the mainWindowViewModel.SolveCommand.CanExecuteChanged event to have been raised");
            Assert.That(_mainWindowViewModel.SolveCommand.CanExecute(null), Is.False, "Expected mainWindowViewModel.SolveCommand.CanExecute() to return false");
        }
    }
}
