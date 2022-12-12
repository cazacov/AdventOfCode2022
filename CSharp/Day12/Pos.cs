using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Day12
{
    [DebuggerDisplay("{X}/{Y}/{Z} - {CanGo.Count} - {Distance}")]
    class Pos
    {
        public int X;
        public int Y;
        public int Z;
        public bool IsStart;
        public bool IsTarget;
        public int Distance;

        public List<Pos> CanGo = new List<Pos>();

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