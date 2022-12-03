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

            var total = 0;
            foreach (var rueck in input)
            {
                var first = rueck.Substring(0, rueck.Length / 2);
                var second = rueck.Substring(rueck.Length / 2);

                String common = "";
                foreach (var ch in first)
                {
                    if (second.Contains(ch))
                    {
                        common += ch;
                    }
                }

                string counted = "";

                int score = 0;
                foreach (var ch in common)
                {
                    if (counted.Contains(ch))
                    {
                        continue;
                    }
                    if (ch >= 'a' && ch <= 'z')
                    {
                        score += ch - 'a' + 1;
                    }
                    else
                    {
                        score += ch - 'A' + 27;
                    }

                    counted += ch;
                }
                Console.WriteLine($"Score of {rueck} = {score}");
                total += score;
            }
            Console.WriteLine(total);
        }

        private static List<String> ReadInput(string fileName)
        {
            return System.IO.File.ReadAllLines(fileName).ToList();
        }
    }
}
