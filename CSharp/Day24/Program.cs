using System;
using System.Collections.Generic;

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

            while (!map.SafeLocations.Contains(map.Start))
            {
                path.Add(Command.Wait);
                map.NextStep();
            }
            path.Add(Command.Down);
            var current = map.Start;

            /* magic happens here */

            path.Add(Command.Down);

            Console.WriteLine($"Puzzle 1: Exit reached in {path.Count} minutes");
        }
    }
}
