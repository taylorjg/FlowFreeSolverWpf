using System;

namespace FlowFreeSolverWpf.Model
{
    public struct Coords
    {
        private readonly Int16 _value;

        public Coords(int x, int y)
        {
            _value = Convert.ToInt16(x << 8 | y);
        }

        public int X
        {
            get { return _value >> 8; }
        }

        public int Y
        {
            get
            {
                return unchecked((Int16)((_value & 0xFF) << 8) >> 8);
            }
        }

        public override bool Equals(object rhs)
        {
            if (rhs is Coords) return _value == ((Coords)rhs)._value;
            return false;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", X, Y);
        }
    }
}
