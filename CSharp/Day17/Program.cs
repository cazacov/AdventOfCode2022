using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day17
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var input = File.ReadAllLines("input.txt");
            Puzzle1(input.First());
        }

        private static void Puzzle1(string input)
        {
            var stackHeight = 0;
            var jetIndex = 0;
            var map = new List<int[]>();
            for (var i = 0; i < stackHeight + 4; i++)
            {
                map.Add(new int[7]);
            }

            var upper = Int32.MaxValue - 1;

            var loop = 0;
            var head = 0;
            var headStack = 0;
            var cycleLength = 0;
            var cycleStack = 0;
            var history = new Dictionary<int, int>();
            var history2 = new Dictionary<int, int>();
            long toSkip = 0;
            bool loopFound = false;

            for (var i = 0; i < upper; i++)
            {
                if (i % 5 == 0)
                {
                    if (!loopFound && history.ContainsKey(jetIndex))
                    {
                        loopFound = true;
                        head = history[jetIndex];
                        headStack = history2[head];
                        cycleLength = loop - head;
                        cycleStack = stackHeight - headStack;

                        head *= 5;
                        cycleLength *= 5;

                        toSkip = ((1000000000000L - head) / cycleLength) - 1;
                        var r = (1000000000000L - head) % cycleLength;
                        upper = i + (int)r;
                        if (r == 0)
                        {
                            break;
                        }
                    }
                    else
                    {
                        history[jetIndex] = loop;
                        history2[loop] = stackHeight;
                    }
                    loop++;
                }

                var rock = new Rock(i % 5)
                {
                    X = 2,
                    Y = stackHeight + 3
                };
                do
                {
                    var jetDir = input[jetIndex];
                    jetIndex++;
                    if (jetIndex == input.Length)
                    {
                        jetIndex = 0;
                    }
                    if (jetDir == '<')
                    {
                        if (rock.X == 0)
                        {
                        }
                        else
                        {
                            rock.X -= 1;
                            if (Overlaps(rock, map))
                            {
                                rock.X += 1;
                            }
                            else
                            {
                            }
                        }
                    }
                    else if (jetDir == '>')
                    {
                        if (rock.X >= 7 - rock.W)
                        {
                        }
                        else
                        {
                            rock.X += 1;
                            if (Overlaps(rock, map))
                            {
                                rock.X -= 1;
                            }
                            else
                            {
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Wrong jet direction");
                    }
                    rock.Y -= 1;
                    if (Overlaps(rock, map))
                    {
                        rock.Y += 1;
//                        Console.WriteLine($"Rock {rock.Type} hit ground at {rock.X},{rock.Y}");
                        while (map.Count <= rock.Y + rock.H + 4)
                        {
                            map.Add(new int[7]);
                        }

                        if (stackHeight < rock.Y + rock.H)
                        {
                            stackHeight = rock.Y + rock.H;
//                            Console.WriteLine($"New stack height: {stackHeight}");
                        }

                        for (int xx = 0; xx < rock.W; xx++)
                        {
                            for (var yy = 0; yy < rock.H; yy++)
                            {
                                if (rock.HasRock(xx, yy))
                                {
                                    map[rock.Y + yy][rock.X + xx] = rock.Type + 1;
                                }
                            }
                        }
                        break;
                    }
                    else
                    {
//                        Console.WriteLine(" DOWN");
                    }
                } while (true);
            }
            ShowMap(map, map.Count - 1);
            Console.WriteLine("~~~~~~~~~");
            ShowMap(map, 19);
            ///            var sh = map[stackHeight - 1];
            Console.WriteLine($"Puzzle 1: {stackHeight}");


            var result = (long) stackHeight + (toSkip) * cycleStack;


            Console.WriteLine(result);
        }

        private static void ShowMap(List<int[]> map, int start)
        {
            var idx = map.Count - 1;

            var cl = Console.ForegroundColor;
            for (int i = 0; i < 20; i++)
            {
                if (start - i < 0 || start - i > map.Count - 1)
                {
                    continue;
                }
                Console.Write('|');
                for (int j = 0; j < 7; j++)
                {
                    if (map[start - i][j] > 0)
                    {
                        switch (map[start - i][j])
                        {
                            case 1:
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            case 2:
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            case 3:
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case 4:
                                Console.ForegroundColor = ConsoleColor.Blue;
                                break;
                            case 5:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                        }
                        Console.Write('#');
                    }
                    else
                    {
                        Console.ForegroundColor = cl;
                        Console.Write(".");
                    }
                }
                Console.ForegroundColor = cl;
                Console.WriteLine('|');
            }
        }

        private static bool Overlaps(Rock rock, List<int[]> map)
        {
            if (rock.Y < 0)
            {
                return true;
            }
            for (int xx = 0; xx < rock.W; xx++)
            {
                for (var yy = 0; yy < rock.H; yy++)
                {
                    if (rock.Y + yy < map.Count)
                    {
                        if (map[rock.Y + yy][rock.X + xx] > 0 && rock.HasRock(xx, yy))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }

    class Rock
    {
        public readonly int Type;
        public int X;
        public int Y;
        public readonly int W;
        public readonly int H;

        public Rock(int type)
        {
            Type = type;
            switch (type)
            {
                case 0:
                    W = 4;
                    H = 1;
                    break;
                case 1:
                case 2:
                    W = 3;
                    H = 3;
                    break;
                case 3:
                    W = 1;
                    H = 4;
                    break;
                case 4:
                    W = 2;
                    H = 2;
                    break;
            }
        }

        public bool HasRock(int x, int y)
        {
            switch (Type)
            {
                case 0:
                case 3:
                case 4:
                    return x >= 0 && x < W && y >= 0 && y < H;
                case 1:
                    switch (y)
                    {
                        case 0:
                        case 2:
                            return x == 1;
                        case 1:
                            return x >= 0 && x < W;
                        default:
                            return false;
                    }
                case 2:
                    switch (y)
                    {
                        case 0:
                            return x >= 0 && x < W;
                        case 1:
                        case 2:
                            return x == 2;
                        default:
                            return false;
                    }
                default:
                    return false;
            }
        }
    }
}
