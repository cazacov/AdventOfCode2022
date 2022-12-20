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
            var d = encrypted.Distinct().Count();

            Puzzle1(encrypted);
        }

        private static void Puzzle1(List<int> encrypted)
        {
            var numbers = new List<Num>();

            Console.WriteLine(-2 % 7);
            Console.WriteLine(-8 % 7);

            for (int i = 0; i < encrypted.Count; i++)
            {
                numbers.Add(new Num(i, encrypted[i]));
            }

            for (int i = 0; i < encrypted.Count; i++)
            {
                var index = numbers.FindIndex(x => x.Position == i);
                var elem = numbers[index];

                if (elem.Value > 0)
                {
                    numbers.RemoveAt(index);
                    var newIndex = index + (elem.Value % (encrypted.Count - 1));
                    if (newIndex >= encrypted.Count)
                    {
                        newIndex = newIndex - encrypted.Count + 1;
                    }
                    numbers.Insert(newIndex, elem);
                }
                else if (elem.Value < 0)
                {
                    numbers.RemoveAt(index);
                    var newIndex = index + (elem.Value % (encrypted.Count - 1));
                    if (newIndex <= 0)
                    {
                        newIndex = newIndex + encrypted.Count - 1;
                    }
                    numbers.Insert(newIndex, elem);
                }
                //Console.WriteLine(String.Join(", ", numbers.Select(x => x.Value.ToString())));
            }

            var zeroIndex = numbers.FindIndex(n => n.Value == 0);
            var i1000 = (zeroIndex + 1000) % encrypted.Count;
            var i2000 = (zeroIndex + 2000) % encrypted.Count;
            var i3000 = (zeroIndex + 3000) % encrypted.Count;

            Console.WriteLine($"Puzzle 1: {numbers[i1000].Value + numbers[i2000].Value + numbers[i3000].Value}");

        }

        private static List<int> ReadInput(string fileName)
        {
            var lines = File.ReadAllLines(fileName);
            return lines.Select(x => Int32.Parse(x)).ToList();
        }
    }

    [DebuggerDisplay("{Value}")]
    class Num
    {
        public int Position;
        public int Value;

        public Num(int position, int value)
        {
            Position = position;
            Value = value;
        }

        private sealed class PositionEqualityComparer : IEqualityComparer<Num>
        {
            public bool Equals(Num x, Num y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Position == y.Position;
            }

            public int GetHashCode(Num obj)
            {
                return obj.Position;
            }
        }

        public static IEqualityComparer<Num> PositionComparer { get; } = new PositionEqualityComparer();
    }
}
