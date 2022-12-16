using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day16
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022, day 16");
            var valves = ReadInput("input.txt");

            Puzzle1(valves);
            Puzzle2(valves);
        }

        private static void Puzzle2(Dictionary<string, Valve> valves)
        {
            var interesting = valves.Select(p => p.Value).Where(v => v.FlowRate > 0).ToList();
            var n = interesting.Count;

            var maxSum = 0;
            var current = valves["AA"];



            for (var i = 0; i < (1 << n); i++)
            {

                var listOne = new List<Valve>();
                var listTwo = new List<Valve>();

                for (var j = 0; j < n; j++)
                {
                    if ((i & (1 << j)) != 0)
                    {
                        listOne.Add(interesting[j]);
                    }
                    else
                    {
                        listTwo.Add(interesting[j]);
                    }
                }

                var bestScoreOne = 0;
                var bestScoreTwo = 0;
                FindOrders(current, listOne, 26, new List<string>() { }, valves, 0, ref bestScoreOne);
                FindOrders(current, listTwo, 26, new List<string>() { }, valves, 0, ref bestScoreTwo);

                if (i % 1000 == 0)
                {
                    Console.WriteLine(i);
                }

                var sum = bestScoreOne + bestScoreTwo;
                if (sum > maxSum)
                {
                    Console.WriteLine(sum);
                    maxSum = sum;
                }
            }
            Console.WriteLine($"Puzzle 2: {maxSum}");
        }

        private static void Puzzle1(Dictionary<string, Valve> valves)
        {

            var current = valves["AA"];
            var interesting = valves.Select(p => p.Value).Where(v => v.FlowRate > 0).ToList();

            for (int i = 0; i < interesting.Count; i++)
            {
                interesting[i].BitMask = 1 << (i);
            }

            var bestOrder = new List<string>();
            var bestScore = 0;
            var sw = new Stopwatch();
            sw.Start();
            FindOrders(current, interesting, 30, new List<string>() {}, valves, 0, ref bestScore);
            sw.Stop();
            var ell = sw.ElapsedMilliseconds;


            Console.WriteLine($"Puzzle 1: {bestScore}");
        }


        private static void FindOrders(Valve current, List<Valve> interesting, int time, List<string> currentPath,
            Dictionary<string, Valve> valves,
            int score,
            ref int bestScore)
        {
            foreach (var node in interesting)
            {

                if (currentPath.Contains(node.Name))
                {
                    continue;
                }

                var path = FindPath(current.Name, node.Name, valves);

                var newTime = time - path.Count - 1;
                var newScore = score + newTime * node.FlowRate;

                if (newTime > 0)
                {
                    currentPath.Add(node.Name);
                    if (newScore > bestScore)
                    {
                        bestScore = newScore;
                    }
                    if (currentPath.Count < interesting.Count)
                    {
                        FindOrders(node, interesting, newTime, currentPath, valves, newScore, ref bestScore);
                    }
                    currentPath.RemoveAt(currentPath.Count - 1);
                }
            }
        }


        private static Dictionary<string, List<string>> cache = new Dictionary<string, List<string>>();

        private static List<string> FindPath(string from, string to, Dictionary<string, Valve> valves)
        {
            var cacheKey = $"{from}-{to}";
            if (cache.ContainsKey(cacheKey))
            {
                return cache[cacheKey];
            }

            var current = valves[from];
            var unvisited = new HashSet<string>();
            var distances = new Dictionary<string, int>();
            foreach (var valve in valves.Keys)
            {
                unvisited.Add(valve);
                distances[valve] = 1000;
            }
            distances[current.Name] = 0;

            while (current != null)
            {
                if (current.Name == to)
                {
                    break;
                }
                foreach (var node in current.LeadsToValves)
                {
                    if (distances[node.Name] > distances[current.Name] + 1)
                    {
                        distances[node.Name] = distances[current.Name] + 1;
                    }
                }
                unvisited.Remove(current.Name);
                var newName = unvisited.OrderBy(x => distances[x]).FirstOrDefault();
                if (newName != null)
                {
                    current = valves[newName];
                }
                else
                {
                    current = null;
                }
            }

            var result = new List<string>();
            while (current.Name != from)
            {
                result.Insert(0, current.Name);
                current = current.LeadsToValves.OrderBy(x => distances[x.Name]).First();
            }

            var r1 = new List<string>();
            r1.AddRange(result);
            cache[cacheKey] = r1;

            result.Reverse();
            var r2 = new List<string>();
            r2.AddRange(result);
            cache[$"{to}-{from}"] = r2;

            return result;
        }





        private static Dictionary<string, Valve> ReadInput(string fileName)
        {
            var lines = File.ReadAllLines(fileName);
            var result = new List<Valve>();

            var regex = new Regex(@"Valve (\S+) has flow rate=(\d+); tunnel");
            foreach (var line in lines)
            {
                var match = regex.Match(line);

                var valve = new Valve(match.Groups[1].Value, int.Parse(match.Groups[2].Value));

                var r = line.Substring(match.Groups[0].Length + 16);

                valve.LeadsToNames = r
                    .Split(", ", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
                result.Add(valve);
            }

            var dict = result.ToDictionary(x => x.Name, x => x);
            foreach (var valve in result)
            {
                valve.LeadsToValves = valve.LeadsToNames.Select(v => dict[v]).ToList();
            }
            return dict;
        }
    }
}
