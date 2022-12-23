using System.Diagnostics;

namespace Day22
{
    [DebuggerDisplay("{Direction} - {Distance}")]
    public class Command
    {
        public int Direction;
        public int Distance;

        public Command(int direction, int distance)
        {
            Direction = direction;
            Distance = distance;
        }
    }
}