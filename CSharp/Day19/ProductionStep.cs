using System;
using System.Diagnostics;

namespace Day19
{
    public enum Building
    {
        None,
        Ore,
        Clay,
        Obsidian,
        Geode
    }

    [DebuggerDisplay("{Minute} {Building}")]
    class ProductionStep
    {
        public int Minute;
        public int Ore;
        public int Clay;
        public int Obsidian;

        public int OreRobots;
        public int ClayRobots;
        public int ObsidianRobots;

        public Building Building;

        private int oreAfter;
        private int clayAfter;
        private int obsidianAfter;

        public void SetBuilding(Blueprint blueprint, Building newValue)
        {
            {
                this.Building = newValue;
                oreAfter = this.Ore + this.OreRobots;
                clayAfter = this.Clay + this.ClayRobots;
                obsidianAfter = this.Obsidian + this.ObsidianRobots;
                switch (newValue)
                {
                    case Building.None:
                        break;
                    case Building.Ore:
                        oreAfter -= blueprint.OreOreCost;
                        break;
                    case Building.Clay:
                        oreAfter -= blueprint.ClayOreCost;
                        break;
                    case Building.Obsidian:
                        oreAfter -= blueprint.ObsidianOreCost;
                        clayAfter -= blueprint.ObsidianClayCost;
                        break;
                    case Building.Geode:
                        oreAfter -= blueprint.GeodeOreCost;
                        obsidianAfter -= blueprint.GeodeObsidianCost;
                        break;
                }
            }
        }

        public ProductionStep(int minute, int ore, int clay, int obsidian)
        {
            Minute = minute;
            Ore = ore;
            Clay = clay;
            Obsidian = obsidian;
        }

        public bool CanBuildOre(Blueprint blueprint)
        {
            return this.Ore >= blueprint.OreOreCost;
        }

        public bool CanBuildClay(Blueprint blueprint)
        {
            return this.Ore >= blueprint.ClayOreCost;
        }

        public bool CanBuildObsidian(Blueprint blueprint)
        {
            return this.Ore >= blueprint.ObsidianOreCost 
                   && this.Clay >= blueprint.ObsidianClayCost;
        }

        public bool CanBuildGeode(Blueprint blueprint)
        {
            return this.Ore >= blueprint.GeodeOreCost 
                   && this.Obsidian >= blueprint.GeodeObsidianCost;
        }

        
        public ProductionStep NextStep()
        {
            var result = new ProductionStep(
                this.Minute + 1,
                this.oreAfter,
                this.clayAfter,
                this.obsidianAfter
            )
            {
                OreRobots = this.OreRobots + (this.Building == Building.Ore ? 1 : 0),
                ClayRobots = this.ClayRobots + (this.Building == Building.Clay ? 1 : 0),
                ObsidianRobots = this.ObsidianRobots + (this.Building == Building.Obsidian ? 1 : 0)
            };
            return result;
        }

        public int Score()
        {
            if (Building == Building.Geode)
            {
                return 24 - this.Minute;
            }
            else
            {
                return 0;
            }
        }

        public void Print()
        {
            var fg = Console.ForegroundColor;

            Console.WriteLine($"== Minute {this.Minute} ==");
            Console.WriteLine($"Stock: ore: {this.Ore},  clay: {this.Clay}, obsidian: {this.Obsidian}");
            Console.WriteLine($"Robots: ore: {this.OreRobots},  clay: {this.ClayRobots}, obsidian: {this.ObsidianRobots}");
            if (this.Building == Building.Geode)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Building: {this.Building}, score {this.Score()}\n");
            }
            else
            {
                Console.WriteLine($"Building: {this.Building}\n");
            }
            Console.ForegroundColor = fg;
        }
    }
}