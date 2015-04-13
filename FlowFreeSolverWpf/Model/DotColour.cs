namespace FlowFreeSolverWpf.Model
{
    public class DotColour
    {
        private readonly string _colourName;

        public DotColour(string colourName)
        {
            _colourName = colourName;
        }

        public string ColourName
        {
            get { return _colourName; }
        }
    }
}
