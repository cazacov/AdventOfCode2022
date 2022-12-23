using System;
using System.Diagnostics;

namespace Day22
{
    public enum Type
    {
        Empty,
        Wall,
        Free
    }

    [DebuggerDisplay("{X},{Y}")]
    public class Pos
    {
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

        public readonly int X;
        public readonly int Y;

        public Pos(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Type Type;

        public Pos3 Left;
        public Pos3 Down;
        public Pos3 Right;
        public Pos3 OnCube;
        public Pos3 Normal { get; set; }
    }
}