using System;
using System.Collections.Generic;
using FlowFreeSolverWpf.Model;

namespace FlowFreeSolverWpf
{
    public interface IBoardControl
    {
        event EventHandler<CellClickedEventArgs> CellClicked;
        int GridSize { get; set; }
        IList<ColourPair> GetColourPairs();
        void DrawGrid();
        void ClearAll();
        void ClearDots();
        void ClearPaths();
        void ClearGridLines();
        void AddDot(Coords coords, DotColour dotColour);
        void RemoveDot(Coords coords);
        bool IsDotAt(Coords coords);
        void DrawPath(ColourPair colourPair, Model.Path path);
    }
}
