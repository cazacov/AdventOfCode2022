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
        }

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

            
        }
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
