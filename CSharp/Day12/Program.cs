using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day12
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022, Day 12");
            var heightMap = ReadInput("input.txt");

            // Puzzle 1
            var start = heightMap.Single(p => p.IsStart);
            var shortestDistance = ShortestDistance(heightMap , start);
            Console.WriteLine($"Puzzle 1: {shortestDistance}");

            // Puzzle 2
            var lowestSquares = heightMap .Where(p => p.Z == 0).ToList();
            shortestDistance = int.MaxValue;
            foreach (var node in lowestSquares)
            {
                var dist = ShortestDistance(heightMap , node);
                if (dist < int.MaxValue)
                {
                    Console.Write($"\tDistance from {node.X}/{node.Y} to target: {dist}");
                    if (dist < shortestDistance)
                    {
                        shortestDistance = dist;
                        Console.WriteLine(" shorter path found!");
                    }
                    else
                    {
                        Console.WriteLine();
                    }
                }
            }
            Console.WriteLine($"Puzzle 2: {shortestDistance}");
        }

        /// <summary>
        /// Dijkstra's algorithm for finding the shortest paths between nodes in a graph
        /// </summary>
        /// <param name="input"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        private static int ShortestDistance(List<Pos> input, Pos start)
        {
            var target = input.First(p => p.IsTarget);
            var unvisited = new HashSet<Pos>();
            input.ForEach(p =>
            {
                if (p != start)
                {
                    p.Distance = int.MaxValue;
                    unvisited.Add(p);
                }
                else
                {
                    p.Distance = 0;
                }
            });

            var current = start;
            while (unvisited.Any())
            {
                if (current.IsTarget)
                {
                    break;
                }

                var nextDistance = current.Distance + 1;
                foreach (var pos in current.CanGo)
                {
                    if (pos.Distance > nextDistance)
                    {
                        pos.Distance = nextDistance;
                    }
                }

                unvisited.Remove(current);
                current = unvisited.OrderBy(p => p.Distance).FirstOrDefault();
                if (current.Distance == Int32.MaxValue)
                {   
                    // There is no way to target
                    break;
                }
            }

            return target.Distance;
        }

        private static List<Pos> ReadInput(string fileName)
        {
            var lines = File.ReadAllLines(fileName);

            var result = new List<Pos>();
            var map = new Pos[lines.Length, lines[0].Length];

            var y = 0;
            foreach (var line in lines)
            {
                var x = 0;
                foreach (var ch in line)
                {
                    var pos = new Pos()
                    {
                        X = x,
                        Y = y,
                    };
                    switch (ch)
                    {
                        case 'S':
                            pos.Z = 0;
                            pos.IsStart = true;
                            break;
                        case 'E':
                            pos.Z = 25;
                            pos.IsTarget = true;
                            break;
                        default:
                            pos.Z = ch - 'a';
                            break;
                    }
                    map[y, x] = pos;
                    result.Add(pos);
                    x++;
                }
                y++;
            }

            // Calculate accessible neighbors
            foreach (var pos in result)
            {
                var up = result.FirstOrDefault(p => p.X == pos.X && p.Y == pos.Y - 1);
                if (up != null && (up.Z == pos.Z || up.Z == pos.Z + 1 || up.Z < pos.Z))
                {
                    pos.CanGo.Add(up);
                }
                var down = result.FirstOrDefault(p => p.X == pos.X && p.Y == pos.Y + 1);
                if (down != null && (down.Z == pos.Z || down.Z == pos.Z + 1 || down.Z < pos.Z))
                {
                    pos.CanGo.Add(down);
                }
                var left = result.FirstOrDefault(p => p.X == pos.X - 1 && p.Y == pos.Y);
                if (left != null && (left.Z == pos.Z || left.Z == pos.Z + 1 || left.Z < pos.Z))
                {
                    pos.CanGo.Add(left);
                }
                var right = result.FirstOrDefault(p => p.X == pos.X + 1 && p.Y == pos.Y);
                if (right != null && (right.Z == pos.Z || right.Z == pos.Z + 1 || right.Z < pos.Z))
                {
                    pos.CanGo.Add(right);
                }
            }
            return result;
        }
    }
}
