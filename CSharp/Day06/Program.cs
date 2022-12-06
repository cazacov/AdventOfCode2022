using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day06
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022, Day 6");

            var input = ReadInput("input.txt");

            var line = input[0];

            Console.WriteLine($"Puzzle 1: {FindUniqueBlock(line, 4)}");
            Console.WriteLine($"Puzzle 2: {FindUniqueBlock(line, 14)}");
        }

        private static int FindUniqueBlock(string line, int length)
        {
            for (var i = 0; i < line.Length - length; i++)
            {
                var block = line.Substring(i, length);
                var cnt = block.Distinct().Count();
                if (cnt == length)
                {
                    return i + length;
                }
            }
            return 0;
        }

        private static List<string> ReadInput(string fileName)
        {
            var input = File.ReadAllLines(fileName).ToList();

            return input;
        }
    }
}
