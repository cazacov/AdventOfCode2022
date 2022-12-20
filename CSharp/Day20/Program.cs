using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day20
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022, day 20");

            var encrypted = ReadInput("input.txt");
            
            var numbers = encrypted.Select((number, position) => new Num(position, number)).ToList();
            Console.WriteLine($"Puzzle 1: {Puzzle(numbers, 1)}");

            const long encryptionKey = 811589153L;
            numbers = encrypted.Select((number, position) => new Num(position, encryptionKey * number)).ToList();
            Console.WriteLine($"Puzzle 2: {Puzzle(numbers, 10)}");
        }

        private static long Puzzle(List<Num> numbers, int iterations)
        {
            var n = numbers.Count;

            for (var j = 0; j < iterations; j++)
            {
                for (var i = 0; i < n; i++)
                {
                    var index = numbers.FindIndex(x => x.Position == i);
                    var elem = numbers[index];

                    if (elem.Value > 0)
                    {
                        numbers.RemoveAt(index);
                        var newIndex = index + (elem.Value % (n - 1));
                        if (newIndex >= n)
                        {
                            newIndex = newIndex - n + 1;
                        }
                        numbers.Insert((int)newIndex, elem);
                    }
                    else if (elem.Value < 0)
                    {
                        numbers.RemoveAt(index);
                        var newIndex = index + (elem.Value % (n - 1));
                        if (newIndex <= 0)
                        {
                            newIndex = newIndex + n - 1;
                        }
                        numbers.Insert((int)newIndex, elem);
                    }
                }
            }

            var zeroIndex = numbers.FindIndex(x => x.Value == 0);
            var i1000 = (zeroIndex + 1000) % n;
            var i2000 = (zeroIndex + 2000) % n;
            var i3000 = (zeroIndex + 3000) % n;

            return numbers[i1000].Value + numbers[i2000].Value + numbers[i3000].Value;
        }

        private static List<int> ReadInput(string fileName)
        {
            var lines = File.ReadAllLines(fileName);
            return lines.Select(int.Parse).ToList();
        }
    }

    [DebuggerDisplay("{Value}")]
    class Num
    {
        public int Position;
        public long Value;

        public Num(int position, long value)
        {
            Position = position;
            Value = value;
        }
    }
}
