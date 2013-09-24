namespace FlowFreeSolverWpf
{
    public partial class MyMessageBox
    {
        public MyMessageBox()
        {
            InitializeComponent();
            DataContext = this;

            OkButton.Click += (_, __) =>
                {
                    DialogResult = true;
                    Close();
                };
        }

        public string MessageText { get; set; }
    }
}
