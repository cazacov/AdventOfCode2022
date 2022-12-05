using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day05
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022, Day 5");

            var input = ReadInput("input.txt");

            var stacks = new Dictionary<int, List<char>>();
            for (int j = 1; j <= 9; j++)
            {
                stacks[j] = new List<char>();
            }

            for (var i = 0; i < 8; i++)
            {
                var line = input[i];

                for (int j = 0; j < 9; j++)
                {
                    var ch = line.Substring(j * 4 + 1, 1)[0];
                    if (ch != ' ')
                    {
                        stacks[j+1].Add(ch);
                    }
                }
            }

            var li = input.Skip(10);

            var regex = new Regex(@"move (\d+) from (\d+) to (\d+)");
            foreach (var line in li)
            {
                var match = regex.Match(line);
                var count = Int32.Parse(match.Groups[1].Value);
                var from = Int32.Parse(match.Groups[2].Value);
                var to = Int32.Parse(match.Groups[3].Value);

                for (int i = 0; i < count; i++)
                {
                    var ch = stacks[from].First();
                    stacks[to].Insert(0, ch);
                    stacks[from].RemoveAt(0);
                }

            }

            Console.Write("Puzzle 1: ");
            for (int i = 1; i <= 9; i++)
            {
                Console.Write(stacks[i].First());
            }
            Console.WriteLine();
        }

        private static List<string> ReadInput(string fileName)
        {
            var input = File.ReadAllLines(fileName);
            var result = input.ToList();
            return result;
        }
    }
}
