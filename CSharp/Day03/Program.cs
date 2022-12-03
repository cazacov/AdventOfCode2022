using System;
using System.Collections.Generic;
using System.Linq;

namespace Day03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022, Day 3");

            var input = ReadInput("input.txt");

            Console.WriteLine("Puzzle 1");

            var result = input.Sum(ruck =>
            {
                var first = ruck[..(ruck.Length / 2)];
                var second = ruck[(ruck.Length / 2)..];

                var firstItems = new HashSet<char>(first);
                var secondItems = new HashSet<char>(second);

                firstItems.IntersectWith(secondItems);
                return firstItems.Sum(Score);
            });
            Console.WriteLine(result);


            Console.WriteLine("Puzzle 2");
            var groups = input
                .Select((x, i) => new {Index = i, Ruck = x})
                .GroupBy(x => x.Index / 3);

            result = groups.Sum(group =>
            {
                var badge = new HashSet<char>(group.First().Ruck);
                foreach (var ruck in group.Select(x => x.Ruck))
                {
                    badge.IntersectWith(new HashSet<char>(ruck));
                }
                return Score(badge.First());
            });
            Console.WriteLine(result);
        }

        public static int Score(char ch)
        {
            if (ch >= 'a' && ch <= 'z')
            {
                return ch - 'a' + 1;
            }
            else
            {
                return ch - 'A' + 27;
            }
        }

        private static List<String> ReadInput(string fileName)
        {
            return System.IO.File.ReadAllLines(fileName).ToList();
        }
    }
}
