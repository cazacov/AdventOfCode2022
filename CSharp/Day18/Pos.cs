using System;
using System.Collections.Generic;

namespace Day18
{
    public class Pos
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Z;

        public Pos(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        protected bool Equals(Pos other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
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
            return HashCode.Combine(X, Y, Z);
        }

        public static bool operator ==(Pos left, Pos right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Pos left, Pos right)
        {
            return !Equals(left, right);
        }

        public IEnumerable<Pos> Neighbours()
        {
            yield return new Pos(X+1, Y, Z);
            yield return new Pos(X-1, Y, Z);
            yield return new Pos(X, Y+1, Z);
            yield return new Pos(X, Y-1, Z);
            yield return new Pos(X, Y, Z + 1);
            yield return new Pos(X, Y, Z - 1);
            yield break;
        }
    }
}