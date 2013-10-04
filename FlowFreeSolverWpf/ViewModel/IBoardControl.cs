using System;
using FlowFreeSolverWpf.Model;

namespace FlowFreeSolverWpf.ViewModel
{
    public interface IBoardControl
    {
        event EventHandler<CellClickedEventArgs> CellClicked;
        int GridSize { get; set; }
        void ClearAll();
        void ClearDots();
        void ClearPaths();
        void ClearGridLines();
        void DrawGrid();
        void AddDot(Coords coords, DotColour dotColour);
        void RemoveDot(Coords coords);
        void DrawPath(ColourPair colourPair, Model.Path path);
    }
}
