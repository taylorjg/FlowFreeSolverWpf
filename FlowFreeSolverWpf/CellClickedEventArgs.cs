﻿using System;
using FlowFreeSolverWpf.Model;

namespace FlowFreeSolverWpf
{
    public class CellClickedEventArgs : EventArgs
    {
        public CellClickedEventArgs(Coords coords)
        {
            Coords = coords;
        }

        public Coords Coords { get; private set; }
    }
}
