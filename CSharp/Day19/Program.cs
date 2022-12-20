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
            Puzzle2(blueprints);
        }

        private static void Puzzle1(List<Blueprint> blueprints)
        {
            var sum = 0;
            var sw = new Stopwatch();
            sw.Start();
            foreach (var blueprint in blueprints)
            {
                blueprint.MaxMinutes = 24;
                Console.Write($"Blueprint {blueprint.ID}...");
                var score = BluebrintScore(blueprint);
                Console.WriteLine($" score: {score}");
                sum += blueprint.ID * score;
            }
            sw.Stop();
            Console.WriteLine($"Puzzle 1 solution: {sum}, solved in {sw.ElapsedMilliseconds / 1000.0} seconds");
        }

        private static void Puzzle2(List<Blueprint> blueprints)
        {
            var mul = 1;
            var sw = new Stopwatch();
            sw.Start();
            foreach (var blueprint in blueprints.Take(3))
            {
                blueprint.MaxMinutes = 32;
                Console.Write($"Blueprint {blueprint.ID}...");
                var score = BluebrintScore(blueprint);
                Console.WriteLine($" score: {score}");
                mul *= score;
            }
            sw.Stop();
            Console.WriteLine($"Puzzle 12solution: {mul}, solved in {sw.ElapsedMilliseconds / 1000.0} seconds");
        }

        private static int BluebrintScore(Blueprint blueprint)
        {
            var result = blueprint.FindOptimalQuality();
            var geodeOpened = result.Sum(ps => ps.Score(blueprint.MaxMinutes));
            return geodeOpened;
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
                if (!match.Success)
                {
                    throw new ArgumentException($"Wrong input line: {line}");
                }

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
}
