using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day19
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022, day 19");
            var blueprints = ReadInput("input.txt");

            Puzzle1(blueprints);
        }

        private static void Puzzle1(List<Blueprint> blueprints)
        {
            var sum = 0;
            foreach (var blueprint in blueprints)
            {
                var quality = BluebrintQuality(blueprint);
                sum += blueprint.ID * quality;
            }
            Console.WriteLine(sum);
        }

        private static int BluebrintQuality(Blueprint blueprint)
        {
            var result = FindOptimalQuality(blueprint);

            var geodeOpened = result.Sum(ps =>
            {
                if (ps.Building == Building.Geode)
                {
                    return 24 - ps.Minute;
                }
                else 
                    return 0;
            });
            foreach (var productionStep in result)
            {
                productionStep.Print();
            }

            Console.WriteLine($"Blueprint {blueprint.ID} can open {geodeOpened} geodes in 24 minutes");
            return geodeOpened;
        }

        private static List<ProductionStep> FindOptimalQuality(Blueprint blueprint)
        {
            var current = new ProductionStep(0, 0, 0, 0)
            {
                OreRobots = 1,
                Ore = -1,
                Building = Building.None
            };

            var result = new List<ProductionStep>();

            for (var minute = 1; minute <= 24; minute++)
            {
                var newStep = current.NextStep(blueprint);


                newStep.OreRobots = current.OreRobots + (current.Building == Building.Ore ? 1 : 0);
                newStep.ClayRobots = current.ClayRobots + (current.Building == Building.Clay ? 1 : 0);
                newStep.ObsidianRobots = current.ObsidianRobots + (current.Building == Building.Obsidian ? 1 : 0);

                // try build geode
                if (newStep.CanBuildGeode(blueprint))
                {
                    newStep.Building = Building.Geode;
                }
                else
                {
                    //var oreScore = blueprint.GeodeOreCost + blueprint.GeodeObsidianCost * blueprint.G 
                    // try optimize ore/obsidian production

                    float obsRobotsNeeded =
                        (float) current.OreRobots * blueprint.GeodeObsidianCost / blueprint.GeodeOreCost;

                    if (obsRobotsNeeded > current.ObsidianRobots)
                    {
                        // Need more obsidian robots
                        if (newStep.CanBuildObsidian(blueprint))
                        {
                            newStep.Building = Building.Obsidian;
                        }
                        else
                        {
                            // try optimize ore/clay production
                            float clayRobotsNeeded = (float) current.OreRobots * blueprint.ObsidianClayCost /
                                                     blueprint.ObsidianOreCost;

                            if (clayRobotsNeeded > current.ClayRobots)
                            {
                                if (newStep.CanBuildClay(blueprint))
                                {
                                    newStep.Building = Building.Clay;
                                }
                            }
                            else
                            {
                                if (newStep.CanBuildOre(blueprint))
                                {
                                    newStep.Building = Building.Ore;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (newStep.CanBuildOre(blueprint))
                        {
                            newStep.Building = Building.Ore;
                        }
                    }
                }

                ;
                result.Add(newStep);
                current = newStep;
            }

            return result;
        }


        private static List<Blueprint> ReadInput(string fileName)
        {
            var lines = File.ReadAllLines(fileName);
            var result = new List<Blueprint>();
            var regex = new Regex(
                @"Blueprint (\d+)\: Each ore robot costs (\d+) ore. Each clay robot costs (\d+) ore. Each obsidian robot costs (\d+) ore and (\d+) clay. Each geode robot costs (\d+) ore and (\d+) obsidian.");
            foreach (var line in lines)
            {
                var match = regex.Match(line);

                result.Add(new Blueprint(
                    Int32.Parse(match.Groups[1].Value),
                    Int32.Parse(match.Groups[2].Value),
                    Int32.Parse(match.Groups[3].Value),
                    Int32.Parse(match.Groups[4].Value),
                    Int32.Parse(match.Groups[5].Value),
                    Int32.Parse(match.Groups[6].Value),
                    Int32.Parse(match.Groups[7].Value)
                    ));
            }
            return result;
        }
    }

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
