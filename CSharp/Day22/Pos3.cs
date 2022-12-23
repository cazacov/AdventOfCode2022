using System;
using System.Diagnostics;

namespace Day22
{
    [DebuggerDisplay("{X},{Y},{Z}")]
    public class Pos3
    {
        public int X;
        public int Y;
        public int Z;

        public Pos3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        protected bool Equals(Pos3 other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Pos3) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        public static bool operator ==(Pos3 left, Pos3 right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Pos3 left, Pos3 right)
        {
            return !Equals(left, right);
        }

        public Pos3 Add(Pos3 other)
        {
            return new Pos3(this.X + other.X, this.Y + other.Y, this.Z + other.Z);
        }

        public Pos3 Negate()
        {
            return new Pos3(-X, -Y, -Z);
        }

        public Pos3 CrossProduct(Pos3 other)
        {
            return new Pos3(
                this.Z * other.Y - this.Y * other.Z,
                this.X * other.Z - this.Z * other.X,
                this.Y * other.X - this.X * other.Y);
        }
    }
}