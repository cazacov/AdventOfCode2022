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

            // Enter the map
            do
            {
                map.NextStep();
                if (map.SafeLocationsAtStep(map.Step).Contains(map.Start))
                {
                    path.Add(Command.Down);
                    break;
                }
                path.Add(Command.Wait);
            } while (true);

            // Walk through
            var upperBound = 300;

            var initStep = map.Step;

            do
            {
                path = path.Take(initStep).ToList();
                Console.WriteLine($"trying upper bound {upperBound}");
                var current = map.Start;
                var stepBack = Int32.MaxValue;
                map.FindPath(current, initStep, path, ref upperBound, ref stepBack);
                if (map.BestPath.Any())
                {
                    break;
                }
                upperBound = (int) (upperBound * 1.1);
            } while (true);

            // Exit the map
            map.BestPath.Add(Command.Down);

            Console.WriteLine($"Puzzle 1: Exit reached in { map.BestPath.Count} minutes");
            var safeLocations = map.SafeLocationsAtStep(map.BestPath.Count);

            for (var y = 0; y < map.MaxY; y++)
            {
                for (var x = 0; x < map.MaxX; x++)
                {
                    Console.Write(safeLocations.Contains(new Pos(x, y)) ? "." : "#");
                }
                Console.WriteLine();
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
