using System;
using System.Diagnostics;

namespace Day15
{
    [DebuggerDisplay("Sensor: {SX},[SY}, beacon {BX},{BY}")]
    class Sensor
    {
        public readonly int SX;
        public readonly int SY;
        public readonly int BX;
        public readonly int BY;
        public readonly int Distance;

        public Sensor(int sx, int sy, int bx, int by)
        {
            SX = sx;
            SY = sy;
            BX = bx;
            BY = by;
            Distance = Math.Abs(this.SX - this.BX) + Math.Abs(this.SY - this.BY);
        }

        public int DistanceTo(Pos pos)
        {
            return Math.Abs(this.SX - pos.X) + Math.Abs(this.SY - pos.Y);
        }
    }
}