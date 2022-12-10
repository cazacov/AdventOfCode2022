using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022, Day 10");

            var input = ReadInput("input.txt");

            var regX = new List<int>();
            var X = 1;
            foreach (var cmd in input)
            {
                if (cmd.Command == Command.Noop)
                {
                    regX.Add(X);
                }
                else
                {
                    regX.Add(X);
                    regX.Add(X);
                    X += cmd.Arg;
                }
            }

            Puzzle1(regX);
            Puzzle2(regX);
        }

        private static void Puzzle2(List<int> regX)
        {
            Console.WriteLine("Puzzle 2:");
            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 40; x++)
                {
                    var index = x + y * 40;
                    if (Math.Abs(regX[index] - (x)) <= 1)
                    {
                        Console.Write(" ");
                        var cl = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("#");
                        Console.ForegroundColor = cl;
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write(" . ");
                    }
                }
                Console.WriteLine();
            }
        }

        private static void Puzzle1(List<int> regX)
        {
            var strength = 0;
            var offset = 20;
            while (offset <= 220)
            {
                var signal = offset * regX[offset - 1];
                strength += signal;
                offset += 40;
            }

            Console.WriteLine($"Puzzle 1: {strength}");
        }

        private static List<Instruction> ReadInput(string fileName)
        {
            var lines = File.ReadAllLines(fileName);
            var result = new List<Instruction>();
            foreach (var line in lines)
            {
                if (line[0] == 'n')
                {
                    result.Add(new Instruction(){Command = Command.Noop});
                }
                else
                {
                    result.Add(new Instruction() { Command = Command.Addx, Arg = Int32.Parse(line.Substring(5)) });
                }
            }
            return result;
        }

        enum Command
        {
            Noop,
            Addx
        }

        class Instruction
        {
            public Command Command;
            public int Arg;
        }
    }
}
