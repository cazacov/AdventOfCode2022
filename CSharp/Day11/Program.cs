using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day11
{
    class Program
    {
        private const int Turns = 10000;

        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022, Day 11");

            var monkeys = ReadInput("input.txt");

            var modulo = monkeys.Aggregate(1L, (acc, m) => acc * m.Divisor);
            foreach (var monkey in monkeys)
            {
                monkey.Modulo = modulo;
            }

            for (var i = 0; i < Turns; i++)
            {
                foreach (var monkey in monkeys)
                {
//                    Console.WriteLine($"Monkey {monkey.Index}");
                    while (monkey.Items.Count > 0)
                    {
                        var item = monkey.Items.First();
//                        Console.WriteLine($"Monkey inspects an item with a worry level of {item}");
                        monkey.Items.RemoveAt(0);
                        var newLevel = monkey.Inspect(item);
//                        Console.WriteLine($"\tWorry level {newLevel}");
//                        newLevel /= 3;
//                        Console.WriteLine($"\tNew level {newLevel}");
                        if (newLevel % monkey.Divisor == 0)
                        {
//                            Console.WriteLine("Divisible");
//                            Console.WriteLine($"Item with worry level {newLevel} is thrown to monkey {monkey.MonkeyTrue}");
                            monkeys[monkey.MonkeyTrue].Items.Add(newLevel);
                        }
                        else
                        {
//                            Console.WriteLine("NOT Divisible");
//                            Console.WriteLine($"Item with worry level {newLevel} is thrown to monkey {monkey.MonkeyFalse}");
                            monkeys[monkey.MonkeyFalse].Items.Add(newLevel);
                        }
                    }
//                    Console.WriteLine();
                }

                Console.WriteLine($"Turn {i+1}");
                if (i == Turns - 1)
                {
                    foreach (var monkey in monkeys)
                    {
                        Console.WriteLine($"Monkey {monkey.Index}: inspected {monkey.ItemsInspected} items.");
                    }
                    Console.WriteLine();
                }
            }

            var srt = monkeys.ToList().OrderByDescending(x => x.ItemsInspected).ToList();
            Console.WriteLine($"Level of monkey business: {srt[0].ItemsInspected * srt[1].ItemsInspected}");
        }

        private static List<Monkey> ReadInput(string fileName)
        {
            var lines = File.ReadAllLines(fileName);
            var result = new List<Monkey>();

            var i = 0;
            while (i < lines.Length)
            {
                var monkey = new Monkey();
                monkey.Index = int.Parse(lines[i].Substring(7,1));
                monkey.Items = lines[i + 1].Substring(18).Split(", ", options: StringSplitOptions.RemoveEmptyEntries)
                    .Select(long.Parse)
                    .ToList();
                var operation = lines[i + 2].Substring(19);
                if (operation == "old * old")
                {
                    monkey.Square = 1;
                }
                else if (operation.Contains("*"))
                {
                    monkey.Multiply = int.Parse(operation.Substring(6));
                }
                else
                {
                    monkey.Plus = int.Parse(operation.Substring(6));
                }

                monkey.Divisor = int.Parse(lines[i + 3].Substring(21));
                monkey.MonkeyTrue = int.Parse(lines[i + 4].Substring(29));
                monkey.MonkeyFalse = int.Parse(lines[i + 5].Substring(30));

                result.Add(monkey);
                i += 7; // Lines in a monkey descriptor
            }
            return result;
        }
    }

    [DebuggerDisplay("{Index} div {Divisor} {Square}/{Multiply}/{Plus} -> {MonkeyTrue}/{MonkeyFalse} : {ItemsInspected}")]
    class Monkey
    {
        public int Index;
        public List<long> Items;
        public int Divisor;
        public int Square;
        public int Multiply;
        public int Plus;
        public int MonkeyTrue;
        public int MonkeyFalse;
        public long ItemsInspected;
        public long Modulo = 1;

        public long Inspect(long item)
        {
            this.ItemsInspected++;

            var result = 0L;
            if (this.Square != 0)
            {
                result = item * item;
            } else if (this.Multiply != 0)
            {
                result = item * this.Multiply;
            }
            else
            {
                result = item + this.Plus;
            }
            return result % Modulo;
        }
    }
}
