using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day15
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022, day 15");
            var sensors = ReadInput("input.txt");

            Puzzle1(sensors, 2000000);
            var sw = new Stopwatch();
            sw.Start();
            Puzzle2(sensors, 4000000);
            sw.Stop();
            Console.WriteLine($"Solution found in {sw.ElapsedMilliseconds / 1000.0}");
        }
        
        private static void Puzzle1(List<Sensor> sensors, int rowNumber)
        {
            var cannot = new HashSet<int>();
            foreach (var sensor in sensors)
            {
                var distance = sensor.Distance - Math.Abs(rowNumber - sensor.SY);
                if (distance > 0)
                {
                    for (var i = sensor.SX - distance; i <= sensor.SX + distance; i++)
                    {
                        cannot.Add(i);
                    }
                }
            }

            // Remove sensor positions
            foreach (var sensor in sensors)
            {
                if (sensor.BY == rowNumber)
                {
                    cannot.Remove(sensor.BX);
                }
            }
            Console.WriteLine($"Puzzle 1: {cannot.Count}");
        }

        private static void Puzzle2(List<Sensor> sensors, int fieldSize)
        {
            Console.WriteLine("Solving puzzle 2...");

            // Make a list of "interesting points" - those exactly at the Distance + 1 from a sensor
            var interesting = new HashSet<Pos>();

            foreach (var sensor in sensors)
            {
                for (var i = 0; i < sensor.Distance + 1; i++)
                {
                    var x = sensor.SX - sensor.Distance - 1 + i;
                    var y = sensor.SY + i;
                    if (x >= 0 && x <= fieldSize && y >= 0 && y <= fieldSize)
                    {
                        interesting.Add(new Pos(x, y));
                    }

                    x = sensor.SX + i;
                    y = sensor.SY + sensor.Distance + 1 - i;
                    if (x >= 0 && x <= fieldSize && y >= 0 && y <= fieldSize)
                    {
                        interesting.Add(new Pos(x, y));
                    }

                    x = sensor.SX + sensor.Distance + 1 - i;
                    y = sensor.SY - i;
                    if (x >= 0 && x <= fieldSize && y >= 0 && y <= fieldSize)
                    {
                        interesting.Add(new Pos(x, y));
                    }

                    x = sensor.SX - i;
                    y = sensor.SY - sensor.Distance - 1 + i;
                    if (x >= 0 && x <= fieldSize && y >= 0 && y <= fieldSize)
                    {
                        interesting.Add(new Pos(x, y));
                    }
                }
                Console.Write("S");
            }
            Console.WriteLine($"\nInteresting points: {interesting.Count}");
            // Remove interesting points covered by some sensor
            foreach (var sensor in sensors)
            {
                interesting.RemoveWhere(i => sensor.DistanceTo(i) <= sensor.Distance);
                Console.Write("R");
            }
            Console.WriteLine($"\nInteresting points left: {interesting.Count}");

            if (interesting.Count == 1) // There is only one possible distress beacon location
            {
                var first = interesting.First();
                Console.WriteLine($"Distress beacon at position {first.X},{first.Y}");
                Console.WriteLine($"Puzzle 2: {first.X * 4000000L + first.Y}");
            }
            else
            {
                Console.WriteLine("Not found");
            }
        }

        private static List<Sensor> ReadInput(string filename)
        {
            var lines = File.ReadAllLines(filename);
            var result = new List<Sensor>();

            var regex = new Regex(@"Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)");
            foreach (var line in lines)
            {
                var match = regex.Match(line);
                if (match.Success)
                {
                    result.Add(new Sensor(
                        int.Parse(match.Groups[1].Value),
                        int.Parse(match.Groups[2].Value),
                        int.Parse(match.Groups[3].Value),
                        int.Parse(match.Groups[4].Value)
                    ));
                }
            }
            return result;
        }
    }
}