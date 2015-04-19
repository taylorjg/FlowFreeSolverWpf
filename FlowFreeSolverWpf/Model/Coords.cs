namespace FlowFreeSolverWpf.Model
{
    public struct Coords
    {
        private readonly int _value;

        public Coords(int x, int y)
        {
            _value = x << 16 | y;
        }

        public int X
        {
            get { return _value >> 16; }
        }

        public int Y
        {
            get { return _value & 0x0000FFFF; }
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

        public static bool operator ==(Coords coords1, Coords coords2)
        {
            return Equals(coords1, coords2);
        }

        public static bool operator !=(Coords coords1, Coords coords2)
        {
            return !Equals(coords1, coords2);
        }
    }
}
