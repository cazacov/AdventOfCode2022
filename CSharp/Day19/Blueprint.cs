using System.Diagnostics;

namespace Day19
{
    [DebuggerDisplay("Blueprint {ID}")]
    class Blueprint
    {
        public readonly int ID;
        public readonly int OreOreCost;
        public readonly int ClayOreCost;
        public readonly int ObsidianOreCost;
        public readonly int ObsidianClayCost;
        public readonly int GeodeOreCost;
        public readonly int GeodeObsidianCost;

        public Blueprint(int id, int oreOreCost, int clayOreCost, int obsidianOreCost, int obsidianClayCost, int geodeOreCost, int geodeObsidianCost)
        {
            ID = id;
            OreOreCost = oreOreCost;
            ClayOreCost = clayOreCost;
            ObsidianOreCost = obsidianOreCost;
            ObsidianClayCost = obsidianClayCost;
            GeodeOreCost = geodeOreCost;
            GeodeObsidianCost = geodeObsidianCost;
        }
    }
}