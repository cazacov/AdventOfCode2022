using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day04
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022, Day 4");

            var input = ReadInput("input.txt");

            Console.WriteLine(input.Count(x => 
                x.From1 <= x.From2 && x.To1 >= x.To2
                || x.From2 <= x.From1 && x.To2 >= x.To1)
            );

            Console.WriteLine(input.Count(x =>
            {
                var r1 = Enumerable.Range(x.From1, x.To1 - x.From1 + 1);
                var r2 = Enumerable.Range(x.From2, x.To2 - x.From2 + 1);

                var overlaps = r1.Intersect(r2);
                return overlaps.Any();
            }));
        }

        static List<Intervals> ReadInput(string fileName)
        {
            var input = File.ReadAllLines(fileName);

            var result = new List<Intervals>();

            var regex = new Regex(@"(\d+)-(\d+),(\d+)-(\d+)");
            foreach (var line in input)
            {
                var match = regex.Match(line);
                result.Add(new Intervals()
                {
                    From1 = Int32.Parse(match.Groups[1].Value),
                    To1 = Int32.Parse(match.Groups[2].Value),
                    From2 = Int32.Parse(match.Groups[3].Value),
                    To2 = Int32.Parse(match.Groups[4].Value)
                });
            }
            return result;
        }
    }

    class Intervals
    {
        public Int32 From1;
        public Int32 To1;
        public Int32 From2;
        public Int32 To2;
    }
}
