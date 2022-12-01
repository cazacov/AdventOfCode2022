using System;
using System.Collections.Generic;
using System.Linq;
using static System.Int32;

namespace AdventOfCode2022.Day01
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022, Day 1");
            var input = ReadInput("input.txt");
            Console.WriteLine(input.Max());
            Console.WriteLine(input.OrderByDescending(_ => _).Take(3).Sum());
        }

        private static List<int> ReadInput(string fileName)
        {
            var lines = System.IO.File.ReadAllLines(fileName);

            var result = new List<int>();

            int currentElv = 0;
            foreach (var line in lines)
            {
                if (String.IsNullOrWhiteSpace(line))
                {
                    result.Add(currentElv);
                    currentElv = 0;
                }
                else
                {
                    currentElv += Parse(line);
                }
            }
            result.Add(currentElv);
            return result;
        }
    }
}
