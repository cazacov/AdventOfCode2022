using System;
using System.Collections.Generic;
using System.Linq;

namespace Day24
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022, day 24");

            var map = Map.ReadFromFile("input.txt");
            Puzzle1(map);
            Puzzle2(map);
        }

        static readonly Command[] allCommands = { Command.Right, Command.Down, Command.Wait, Command.Left, Command.Up };

        private static void Puzzle1(Map map)
        {
            var result = FindPathLength(map, map.Start, map.Finish, 0);
            Console.WriteLine($"Puzzle 1: {result}");
        }

        private static void Puzzle2(Map map)
        {
            var trip1 = FindPathLength(map, map.Start, map.Finish, 0);
            Console.WriteLine($"Trip 1: {trip1}");

            var trip2 = FindPathLength(map, map.Finish, map.Start, trip1);
            Console.WriteLine($"Trip 2: {trip2}");

            var trip3 = FindPathLength(map, map.Start, map.Finish, trip1 + trip2);
            Console.WriteLine($"Trip 2: {trip3}");

            Console.WriteLine($"Puzzle 2: {trip1 + trip2 + trip3}");
        }


        private static int FindPathLength(Map map, Pos start, Pos finish, int startTime)
        {
            var unvisited = new HashSet<MinPos>();
            var visited = new HashSet<MinPos>();
            int upperBound = Int32.MaxValue;

            unvisited.Add(new MinPos(start, startTime));
            do
            {
                var minT = unvisited.Min(x => x.T);
                var current = unvisited.First(x => x.T == minT);

                if (current.T >= upperBound)
                {
                    unvisited.Remove(current);
                    continue;
                }

                var curPos = new Pos(current.X, current.Y);
                var safeLocations = map.SafeLocationsAtStep(current.T + 1);

                foreach (var command in allCommands)
                {
                    var nextPos = curPos.Go(command);

                    if (nextPos == finish)
                    {
                        upperBound = current.T + 1;
                        Console.WriteLine($"Found a path of length {upperBound}");
                    }
                    else
                    {
                        if (safeLocations.Contains(nextPos))
                        {
                            var nextMin = new MinPos(nextPos.X, nextPos.Y, current.T + 1);
                            if (!visited.Contains(nextMin))
                            {
                                unvisited.Add(nextMin);
                            }
                        }
                    }
                }

                visited.Add(current);
                unvisited.Remove(current);
            } while (unvisited.Any());

            return upperBound - startTime;
        }


        /*
        private static void Puzzle1(Map map)
        {
            var path = new List<Command>();

            var safeLocations = map.SafeLocationsAtStep(1);
            for (var y = 0; y < map.Height; y++)
            {
                for (var x = 0; x < map.Width; x++)
                {
                    var pos = new Pos(x, y);

                    if (map.Walls.Contains(pos))
                    {
                        Console.Write("#");
                        continue;
                    }
                    Console.Write(safeLocations.Contains(new Pos(x, y)) ? "." : "?");
                }
                Console.WriteLine();
            }

            // Walk through
            var upperBound = 300;

            do
            {
                path.Clear();
                Console.WriteLine($"trying upper bound {upperBound}");
                var current = map.Start;
                var stepBack = Int32.MaxValue;
                map.FindPath(current, 0, path, ref upperBound, ref stepBack);
                if (map.BestPath.Any())
                {
                    break;
                }
                upperBound = (int) (upperBound * 1.1);
            } while (true);


            Console.WriteLine($"Puzzle 1: Exit reached in { map.BestPath.Count} minutes");

            for (int i = 0; i < map.BestPath.Count; i++)
            {
                Console.WriteLine($"Minute {i+1}, move {map.BestPath[i]}");
            }

            
        } */
    } 

    internal class Move
    {
        private sealed class CostRelationalComparer : IComparer<Move>
        {
            public int Compare(Move x, Move y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (ReferenceEquals(null, y)) return 1;
                if (ReferenceEquals(null, x)) return -1;
                return x.Cost.CompareTo(y.Cost);
            }
        }

        public static IComparer<Move> CostComparer { get; } = new CostRelationalComparer();

        public Command Command;
        public Pos Position;
        public int Cost;
    }


}
