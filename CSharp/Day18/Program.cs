using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day18
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent Of Code 2022, day 18");
            var input = ReadInput("input.txt");

            Puzzle1(input);
            Puzzle2(input);
        }

        private static void Puzzle1(List<Pos> input)
        {
            var map = new HashSet<Pos>(input);
            var surface = FindSurface(map);
            Console.WriteLine($"Puzzle 1: {surface}");
        }

        private static void Puzzle2(List<Pos> input)
        {
            var map = new HashSet<Pos>(input);

            var surface = FindSurface(map);

            // Count air traps
            var xMin = input.Min(_ => _.X);
            var xMax = input.Max(_ => _.X);
            var yMin = input.Min(_ => _.Y);
            var yMax = input.Max(_ => _.Y);

            var trapped = new HashSet<Pos>();
            for (var x = xMin; x <= xMax; x++)
            {
                for (var y = yMin; y <= yMax; y++)
                {
                    var column = input.Where(cube => cube.X == x && cube.Y == y).ToList();
                    if (!column.Any() || column.Count <= 2)
                    {
                        continue;
                    }

                    var zMin = column.Min(_ => _.Z);
                    var zMax = column.Max(_ => _.Z);

                    var interesting = Enumerable
                        .Range(zMin + 1, zMax - zMin - 1)
                        .Where(pos => !column.Contains(new Pos(x, y, pos)))
                        .Select(pos => new Pos(x, y, pos))
                        .ToList();

                    foreach (var cube in interesting)
                    {
                        CheckIfTrapped(cube, map, trapped, xMin, xMax, yMin, yMax, zMin, zMax);
                    }
                }
            }

            var innerSurface = FindSurface(trapped);

            surface -= innerSurface;
            Console.WriteLine($"Puzzle 2: {surface}");
        }

        private static int FindSurface(HashSet<Pos> map)
        {
            var surface = map.Count * 6;
            foreach (var cube in map)
            {
                foreach (var neighbour in cube.Neighbours())
                {
                    if (map.Contains(neighbour))
                    {
                        surface -= 1;
                    }
                }
            }
            return surface;
        }

        private static void CheckIfTrapped(Pos cube, HashSet<Pos> map, HashSet<Pos> trapped, int xMin, int xMax,
            int yMin, int yMax, int zMin, int zMax)
        {
            if (trapped.Contains(cube))
            {
                return;
            }

            var visited = new HashSet<Pos> { cube };
            var unvisited = new HashSet<Pos>(cube.Neighbours().Where(c => !map.Contains(c)));

            while (unvisited.Any())
            {
                var nextCube = unvisited.First();
                if (nextCube.X == xMin || nextCube.X == xMax || nextCube.Y == yMin || nextCube.Y == yMax || nextCube.Z == zMin || nextCube.Z == zMax)
                {
                    // Reached the borders of the map - the current cube and all visited set are not trapped
                    return;
                }

                visited.Add(nextCube);
                unvisited.Remove(nextCube);
                unvisited.UnionWith(nextCube.Neighbours()
                    .Where(c => !map.Contains(c) && !visited.Contains(c)));
            };

            // No free elements left to check, the visited set is completely surrounded by map blocks
            trapped.UnionWith(visited);
        }

        private static List<Pos> ReadInput(string fileName)
        {
            var lines = File.ReadAllLines(fileName);

            var result = new List<Pos>();
            var regex = new Regex(@"(\d+),(\d+),(\d+)");
            foreach (var line in lines)
            {
                var match = regex.Match(line);
                result.Add(new Pos(
                    int.Parse(match.Groups[1].Value),
                    int.Parse(match.Groups[2].Value),
                    int.Parse(match.Groups[3].Value)
                    ));
            }
            return result;
        }
    }
}
