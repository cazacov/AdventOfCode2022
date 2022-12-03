using System;
using System.Collections.Generic;
using System.Linq;

namespace Day03
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022, Day 3");

            var input = ReadInput("input.txt");

            Console.WriteLine("Puzzle 1");
            var total = 0;
            foreach (var ruck in input)
            {
                var first = ruck[..(ruck.Length / 2)];
                var second = ruck[(ruck.Length / 2)..];

                var firstItems = new HashSet<char>(first);
                var secondItems = new HashSet<char>(second);

                firstItems.IntersectWith(secondItems);

                var score = firstItems.Sum(Score);
                total += score;
            }
            Console.WriteLine(total);

            Console.WriteLine("Puzzle 2");
            total = 0;
            var groupCounter = 0;
            HashSet<char> badge = null;
            foreach(var ruck in input)
            {
                if (groupCounter++ == 0)
                {
                    badge = new HashSet<char>(ruck);
                    continue;
                }

                badge.IntersectWith(new HashSet<char>(ruck));

                if (groupCounter == 3)
                {
                    total += Score(badge.First());
                    groupCounter = 0;
                }
            }
            Console.WriteLine(total);
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
