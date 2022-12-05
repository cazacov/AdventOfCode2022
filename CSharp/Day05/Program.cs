using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Int32;

namespace Day05
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022, Day 5");

            ReadInput(out var stacks, out var moves);
            foreach (var move in moves)
            {
                stacks[move.To].InsertRange(0, stacks[move.From].Take(move.Count).Reverse());
                stacks[move.From].RemoveRange(0, move.Count);
            }
            PrintAnswer("Puzzle 1: ", stacks);

            ReadInput(out stacks, out moves);
            foreach (var move in moves)
            {
                stacks[move.To].InsertRange(0, stacks[move.From].Take(move.Count));
                stacks[move.From].RemoveRange(0, move.Count);
            }
            PrintAnswer("Puzzle 2: ", stacks);
        }

        private static void PrintAnswer(string title, Dictionary<int, List<char>> stacks)
        {
            Console.Write(title);
            for (int i = 0; i < stacks.Count; i++)
            {
                Console.Write(stacks[i + 1].First());
            }
            Console.WriteLine();
        }

        private static void ReadInput(out Dictionary<int, List<char>> stacks, out List<Movement> moves)
        {
            var input = File.ReadAllLines("input.txt");

            // Read header
            stacks = new Dictionary<int, List<char>>();
            var headerLines = 0;
            foreach (var line in input)
            {
                headerLines++;
                if (char.IsDigit(line[1]))
                {
                    headerLines += 1;
                    break;
                }

                for (var j = 0; j < (line.Length + 1) / 4; j++)
                {
                    if (!stacks.ContainsKey(j + 1))
                    {
                        stacks[j + 1] = new List<char>();
                    }

                    var ch = line.Substring(j * 4 + 1, 1)[0];
                    if (ch != ' ')
                    {
                        stacks[j + 1].Add(ch);
                    }
                }
            }

            // read moves
            var moveLines = input.Skip(headerLines);
            moves = new List<Movement>();
            var regex = new Regex(@"move (\d+) from (\d+) to (\d+)");
            foreach (var line in moveLines)
            {
                var match = regex.Match(line);
                moves.Add(new Movement()
                {
                    Count = Parse(match.Groups[1].Value), 
                    From = Parse(match.Groups[2].Value), 
                    To = Parse(match.Groups[3].Value)
                });
            }
        }
    }

    internal class Movement
    {
        public int Count { get; set; }
        public int From { get; set; }
        public int To { get; set; }
    }
}
