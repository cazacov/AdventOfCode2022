using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022, Day 14");

            // Puzzle 1
            var input = ReadInput("input.txt");

            Console.WriteLine($"Puzzle 1: {Puzzle(input, true)}");
            Console.WriteLine($"Puzzle 2: {Puzzle(input, false)}");
        }

        private static int Puzzle(List<Pos> input, bool isAbyss)
        {
            var minX = input.Min(b => b.X);
            var maxX = input.Max(b => b.X);
            var maxY = input.Max(b => b.Y);

            var hash = new HashSet<Pos>(input);
            if (!isAbyss)
            {
                // Add bottom rock
                for (var i = 0; i < maxY + 2 + 2; i++)
                {
                    hash.Add(new Pos(500 + i, maxY + 2));
                    hash.Add(new Pos(500 - i, maxY + 2));
                }
            }
            var map = hash.ToDictionary(x => x, x => BlockType.Rock);

            var flowingIntoTheAbyss = false;
            do
            {
                var sand = new Pos(500, 0);
                if (map.ContainsKey(sand))
                {
                    break;
                }
                do
                {
                    if (!map.ContainsKey(new Pos(sand.X, sand.Y + 1)))
                    {
                        // falls down
                        sand.Y += 1;
                    }
                    else if (!map.ContainsKey(new Pos(sand.X - 1, sand.Y + 1)))
                    {
                        // falls down-left
                        sand.Y += 1;
                        sand.X -= 1;
                    }
                    else if (!map.ContainsKey(new Pos(sand.X + 1, sand.Y + 1)))
                    {
                        // falls down-right
                        sand.Y += 1;
                        sand.X += 1;
                    }
                    else
                    {
                        // comes to rest
                        map[sand] = BlockType.Sand;
                        break;
                    }

                    if (isAbyss)
                    {
                        if (sand.X < minX || sand.X > maxX || sand.Y > maxY)
                        {
                            flowingIntoTheAbyss = true;
                        }
                    }
                } while (!flowingIntoTheAbyss);
            } while (!flowingIntoTheAbyss);

            return map.Count(b => b.Value == BlockType.Sand);
        }

        private static List<Pos> ReadInput(string fileName)
        {
            var lines = File.ReadAllLines(fileName);
            var result = new List<Pos>();

            foreach (var line in lines)
            {
                var res = new List<Pos>();
                var blocks = line.Split(" -> ", StringSplitOptions.RemoveEmptyEntries);
                var current = Parse(blocks[0]);
                res.Add(current);
                for (var i = 1; i < blocks.Length; i++)
                {
                    var another = Parse(blocks[i]);
                    res.AddRange(BlockLine(current, another));
                    current = another;
                }
                result.AddRange(res);
            }
            return result;
        }

        private static IEnumerable<Pos> BlockLine(Pos current, Pos another)
        {
            var dx= Math.Sign(another.X - current.X);
            var dy = Math.Sign(another.Y - current.Y);
            var distance = Math.Abs(current.X - another.X) + Math.Abs(current.Y - another.Y);

            var result = new List<Pos>();
            while (distance > 0)
            {
                
                var newPos = new Pos(current.X + dx, current.Y + dy);
                result.Add(newPos);
                current = newPos;
                distance--;
            }
            return result;
        }

        private static Pos Parse(string block)
        {
            var regex = new Regex(@"(\d+),(\d+)");
            var match = regex.Match(block);
            return new Pos(
                int.Parse(match.Groups[1].Value),
                int.Parse(match.Groups[2].Value));
        }
    }
}
