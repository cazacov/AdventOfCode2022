using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day23
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022, day 23");
            Puzzle1();
            Puzzle2();
        }

        private static void Puzzle1()
        {
            var elves = ReadInput("input.txt");
            RunElves(elves, 10);
            
            var minX = elves.Min(e => e.Position.X);
            var maxX = elves.Max(e => e.Position.X);
            var minY = elves.Min(e => e.Position.Y);
            var maxY = elves.Max(e => e.Position.Y);
            var square = (maxX - minX + 1) * (maxY - minY + 1);
            Console.WriteLine($"Puzzle 1: There are {square - elves.Count} empty ground tiles");
        }

        private static void Puzzle2()
        {
            var elves = ReadInput("input.txt");
            var rounds = RunElves(elves, long.MaxValue);
            Console.WriteLine($"Puzzle 2: Elves do not move at round {rounds + 1}");
        }

        private static long RunElves(List<Elf> elves, long maxRounds)
        {
            var map = elves.ToDictionary(e => e.Position);

            var checkMasks = new int[]
            {
                0xE0, 0x0E, 0x83, 0x38
            };
            var dirX = new int[]
            {
                0, 0, -1, 1
            };
            var dirY = new int[]
            {
                -1, 1, 0, 0
            };

            
            for (var round = 0L; round < maxRounds; round++)
            {
                var proposals = new Dictionary<Pos, int>();
                foreach (var elf in elves)
                {
                    elf.Proposal = null;
                    var neighbours = NeighbourMask(elf.Position, map);
                    if (neighbours == 0)
                    {
                        continue;
                    }

                    for (var j = 0; j < 4; j++)
                    {
                        var checkDir = (int)((round + j) % 4);
                        var checkMask = checkMasks[checkDir];
                        if ((neighbours & checkMask) == 0)
                        {
                            elf.Proposal = new Pos(elf.Position.X + dirX[checkDir], elf.Position.Y + dirY[checkDir]);

                            if (!proposals.ContainsKey(elf.Proposal))
                            {
                                proposals[elf.Proposal] = 1;
                            }
                            else
                            {
                                proposals[elf.Proposal] += 1;
                            }
                            break;
                        }
                    }
                }

                var hasMoved = false;
                foreach (var elf in elves)
                {
                    if (elf.Proposal == null)
                    {
                        continue;
                    }
                    if (proposals[elf.Proposal] > 1)
                    {
                        continue;
                    }

                    elf.Position = elf.Proposal;
                    hasMoved = true;
                }
                if (!hasMoved)
                {
                    return round;
                }
                map = elves.ToDictionary(e => e.Position);
            }
            return maxRounds;
        }

        private static int NeighbourMask(Pos elf, Dictionary<Pos, Elf> map)
        {
            var result = 0;

            if (map.ContainsKey(new Pos(elf.X - 1, elf.Y - 1)))
            {
                result |= 0x80;
            }
            if (map.ContainsKey(new Pos(elf.X, elf.Y - 1)))
            {
                result |= 0x40;
            }
            if (map.ContainsKey(new Pos(elf.X + 1, elf.Y - 1)))
            {
                result |= 0x20;
            }
            if (map.ContainsKey(new Pos(elf.X + 1, elf.Y)))
            {
                result |= 0x10;
            }
            if (map.ContainsKey(new Pos(elf.X + 1, elf.Y + 1)))
            {
                result |= 0x08;
            }
            if (map.ContainsKey(new Pos(elf.X, elf.Y + 1)))
            {
                result |= 0x04;
            }
            if (map.ContainsKey(new Pos(elf.X - 1, elf.Y + 1)))
            {
                result |= 0x02;
            }
            if (map.ContainsKey(new Pos(elf.X - 1, elf.Y)))
            {
                result |= 0x01;
            }
            return result;
        }

        private static List<Elf> ReadInput(string fileName)
        {
            var result = new List<Elf>();

            var lines = File.ReadAllLines(fileName);
            var y = 0;
            foreach (var line in lines)
            {
                for (var x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#')
                    {
                        result.Add(new Elf(x, y));
                    }
                }
                y++;
            }
            return result;
        }
    }
}
