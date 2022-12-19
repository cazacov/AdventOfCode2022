using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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

        public List<ProductionStep> FindOptimalQuality()
        {
            var current = new ProductionStep(0, 0, 0, 0)
            {
                OreRobots = 1,
                Ore = -1,
                Building = Building.None
            };
            current.SetBuilding(this, Building.None);
            var result = new List<ProductionStep>();
            var bestResult = new List<ProductionStep>();
            int bestScore = 0;

            foreach (var minute in Enumerable.Range(1, 23))
            {
                result.Add(new ProductionStep(minute, 0,0,0));
            }
            FindOptimalQuality(current, result, 0, bestResult, ref bestScore);
            return bestResult;
        }

        private void FindOptimalQuality(ProductionStep current, List<ProductionStep> path, int currentScore,
            List<ProductionStep> bestResult, ref int bestScore)
        {
            if (current.Minute == 23)
            {
                if (currentScore <= bestScore)
                {
                    return;
                }
                bestScore = currentScore;
                bestResult.Clear();
                bestResult.AddRange(path.Select(x => x.Clone()));
                return;
            }

            var minutesLeft = 24 - current.Minute;
            var maxExtraScore = (minutesLeft - 1) * (minutesLeft - 2) / 2;
            if (currentScore + maxExtraScore < bestScore)
            {
                return;
            }

            var next = path[current.Minute];
            current.InitNext(next);

            var buildings = new List<Building>();

            if (next.CanBuildGeode(this))
            {
                buildings.Add(Building.Geode);
            }
            else
            {
                if (next.CanBuildObsidian(this))
                {
                    buildings.Add(Building.Obsidian);
                }
                if (next.CanBuildClay(this))
                {
                    buildings.Add(Building.Clay);
                }
                if (next.CanBuildOre(this))
                {
                    buildings.Add(Building.Ore);
                }
                buildings.Add(Building.None);
            }

            foreach (var building in buildings)
            {
                next.SetBuilding(this, building);
                var score = next.Building == Building.Geode ? 24 - next.Minute : 0;
                FindOptimalQuality(next, path, currentScore + score, bestResult, ref bestScore);
            }
        }
    }
}