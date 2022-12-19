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
        public Building Building = Building.None;
        public int Ore;
        public int Clay;
        public int Obsidian;
        public int OreRobots;
        public int ClayRobots;
        public int ObsidianRobots;

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

        private int ClayCost(Building building, Blueprint blueprint)
        {
            switch (building)
            {
                case Building.None:
                case Building.Ore:
                case Building.Clay:
                case Building.Geode:
                    return 0;
                case Building.Obsidian:
                    return blueprint.ObsidianClayCost;
                default:
                    return 0;
            }
        }

        private int OreCost(Building building, Blueprint blueprint)
        {
            switch (building)
            {
                case Building.None:
                    return 0;
                case Building.Ore:
                    return blueprint.OreOreCost;
                case Building.Clay:
                    return blueprint.ClayOreCost;
                case Building.Obsidian:
                    return blueprint.ObsidianOreCost;
                case Building.Geode:
                    return blueprint.GeodeOreCost;
                default:
                    return 0;
            }
        }


        private int ObsidianCost(Building building, Blueprint blueprint)
        {
            switch (building)
            {
                case Building.None:
                case Building.Ore:
                case Building.Clay:
                case Building.Obsidian:
                    return 0;
                case Building.Geode:
                    return blueprint.GeodeObsidianCost;
                default:
                    return 0;
            }
        }

        public ProductionStep NextStep(Blueprint blueprint)
        {
            var result = new ProductionStep(
                this.Minute + 1,
                this.Ore + this.OreRobots - OreCost(this.Building, blueprint),
                this.Clay + this.ClayRobots - ClayCost(this.Building, blueprint),
                this.Obsidian + this.ObsidianRobots - ObsidianCost(this.Building, blueprint)
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