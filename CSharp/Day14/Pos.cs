using System;
using System.Diagnostics;

namespace Day14
{
    [DebuggerDisplay("{X},{Y}")]
    public class Pos
    {
        public int X;
        public int Y;

        public Pos(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        protected bool Equals(Pos other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Pos) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static bool operator ==(Pos left, Pos right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Pos left, Pos right)
        {
            return !Equals(left, right);
        }
    }
}