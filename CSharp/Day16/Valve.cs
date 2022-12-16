using System.Collections.Generic;
using System.Diagnostics;

namespace Day16
{
    [DebuggerDisplay("{Name} - {FlowRate}")]
    class Valve
    {
        protected bool Equals(Valve other)
        {
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Valve) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public static bool operator ==(Valve left, Valve right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Valve left, Valve right)
        {
            return !Equals(left, right);
        }

        public string Name;
        public int FlowRate;
        public List<string> LeadsToNames = new List<string>();
        public List<Valve> LeadsToValves = new List<Valve>();
        public int BitMask;

        public Valve(string name, int flowRate)
        {
            Name = name;
            FlowRate = flowRate;
        }
    }
}