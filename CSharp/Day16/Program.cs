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

            // Some preparation work to speedup calculations
            CalculatePathMap(valves);
            
            // Solve puzzles
            Puzzle1(valves);
            Puzzle2(valves);
        }

        private static void Puzzle1(Dictionary<string, Valve> valves)
        {

            var interesting = valves.Select(p => p.Value).Where(v => v.FlowRate > 0).ToList();

            var bestScore = 0;
            var sw = new Stopwatch();
            sw.Start();
            var current = valves["AA"];
            FindOrders(current, interesting, 30, 0, ExitMaskFor(interesting), 0, ref bestScore);
            sw.Stop();

            Console.WriteLine($"Puzzle 1: {bestScore}, calculated in {sw.ElapsedMilliseconds / 1000.0} seconds");
        }

        private static void Puzzle2(Dictionary<string, Valve> valves)
        {
            var interesting = valves.Select(p => p.Value).Where(v => v.FlowRate > 0).ToList();
            var n = interesting.Count;  

            var maxSum = 0;
            var current = valves["AA"];

            var sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < (1 << (n - 1)); i++)  // n-1 because we need only to check half of variants. See comment below
            {

                var listOne = new List<Valve>();
                var listTwo = new List<Valve>();

                for (var j = 0; j < n - 1; j++)
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
                // Because me and the elephant are exactly same quick in problem solving the game is symmetric
                // To half the number of considered variants we can assume it's me who visits the last interesting node.
                listOne.Add(interesting.Last()); 

                var bestScoreOne = 0;
                var bestScoreTwo = 0;
                FindOrders(current, listOne, 26, 0, ExitMaskFor(listOne), 0, ref bestScoreOne);
                FindOrders(current, listTwo, 26, 0, ExitMaskFor(listTwo), 0, ref bestScoreTwo);

                if (i > 0 &&  i % 1000 == 0)
                {
                    Console.WriteLine(i);
                }

                var sum = bestScoreOne + bestScoreTwo;
                if (sum > maxSum)
                {
                    maxSum = sum;
                    Console.WriteLine($"\t{maxSum}");
                }
            }
            sw.Stop();

            Console.WriteLine($"Puzzle 1: {maxSum}, calculated in {sw.ElapsedMilliseconds / 1000.0} seconds");
        }


        /// <summary>
        /// Find an order to visit interesting valves to get best score
        /// </summary>
        /// <param name="current">Starting valves</param>
        /// <param name="interesting">Valves to visit</param>
        /// <param name="time">Remaining time</param>
        /// <param name="currentPath">Bit mask of already visited valves</param>
        /// <param name="exitMask">Exit mask - one with all bits set</param>
        /// <param name="score">Score so far</param>
        /// <param name="bestScore">Output parameter</param>
        private static void FindOrders(Valve current, List<Valve> interesting, int time, int currentPath, int exitMask,
            int score,
            ref int bestScore)
        {
            foreach (var node in interesting)
            {

                if ((currentPath & node.BitMask) != 0)
                {
                    continue;
                }

                var pathLength = pathMap[current.Name][node.Name];

                var newTime = time - pathLength - 1;
                var newScore = score + newTime * node.FlowRate;

                if (newTime > 0)
                {
                    currentPath |= node.BitMask;
                    if (newScore > bestScore)
                    {
                        bestScore = newScore;
                    }
                    if (currentPath != exitMask)
                    {
                        FindOrders(node, interesting, newTime, currentPath, exitMask, newScore, ref bestScore);
                    }
                    currentPath &= ~node.BitMask;
                }
            }
        }

        private static Dictionary<string, Dictionary<string, int>> pathMap;

        /// <summary>
        /// Pre-calculate cost to move from node A to B and sore it in a static dictionary pathMap
        /// </summary>
        /// <param name="valves"></param>
        private static void CalculatePathMap(Dictionary<string, Valve> valves)
        {
            pathMap = new Dictionary<string, Dictionary<string, int>>();
            foreach (var valve in valves.Values)
            {
                pathMap[valve.Name] = new Dictionary<string, int>();
                foreach (var another in valves.Values)
                {
                    pathMap[valve.Name][another.Name] = FindPath(valve.Name, another.Name, valves).Count;
                }
            }
        }

        /// <summary>
        /// Finds the shortest path between two valves
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="valves"></param>
        /// <returns></returns>
        private static List<string> FindPath(string from, string to, Dictionary<string, Valve> valves)
        {
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

        /// <summary>
        /// Pre-calculate bit masks (to speedup checking if valve was already opened)
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static int ExitMaskFor(List<Valve> list)
        {
            var allBits = 0;
            for (int i = 0; i < list.Count; i++)
            {
                var mask = 1 << i;
                list[i].BitMask = mask;
                allBits |= mask;
            }
            //
            return allBits;
        }
    }
}
