namespace FlowFreeSolverWpf.Model
{
    public class Coords
    {
        private readonly int _x;
        private readonly int _y;

        public Coords(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public int X
        {
            get { return _x; }
        }

        public int Y
        {
            get { return _y; }
        }

        public override bool Equals(object rhs)
        {
            if (rhs == null) return false;
            if (ReferenceEquals(this, rhs)) return true;
            if (GetType() != rhs.GetType()) return false;
            return CompareFields(rhs as Coords);
        }

        public override int GetHashCode()
        {
            // http://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode

            unchecked // Overflow is fine, just wrap
            {
                var hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", X, Y);
        }

        private bool CompareFields(Coords rhs)
        {
            return (X == rhs.X && Y == rhs.Y);
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
