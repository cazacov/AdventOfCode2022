using System.Diagnostics;

namespace Day23
{
    [DebuggerDisplay("{Position.X},{Position.Y}")]
    class Elf
    {
        public Pos Position;
        public Pos Proposal = null;

        public Elf(int x, int y)
        {
            this.Position = new Pos(x, y);
        }
    }
}