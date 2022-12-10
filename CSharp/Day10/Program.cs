using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Day10
{
    class Program
    {
        private static FlipDotDisplay display;

        static async Task Main(string[] args)
        {
            display = new FlipDotDisplay();
            await display.Init();
            Thread.Sleep(3000);

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

            await Puzzle1(regX);
            await Puzzle2(regX);
        }
        private static async Task Puzzle1(List<int> regX)
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
            await display.ShowPuzzle1(strength);
            Thread.Sleep(3000);
        }

        private static async Task Puzzle2(List<int> regX)
        {
            var result = new bool[6, 40];

            Console.WriteLine("Puzzle 2:");
            for (var y = 0; y < 6; y++)
            {
                for (var x = 0; x < 40; x++)
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
                        result[y, x] = true;
                    }
                    else
                    {
                        Console.Write(" . ");
                        result[y, x] = false;
                    }
                }
                Console.WriteLine();
            }
            await display.ShowPuzzle2(result);
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
