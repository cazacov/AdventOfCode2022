using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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
            var current = valves["AA"];
            var interesting = valves.Select(p => p.Value).Where(v => v.FlowRate > 0).ToList();
            var accessible = interesting.Where(v => FindPath("AA", v.Name, valves).Count < 25).ToList();
        }

        private static void Puzzle1(Dictionary<string, Valve> valves)
        {

            var current = valves["AA"];
            var interesting = valves.Select(p => p.Value).Where(v => v.FlowRate > 0).ToList();
            var visitedNames = new List<string>();


            var bestOrder = new List<string>();
            int bestScore = 0;
            var sw = new Stopwatch();
            sw.Start();
            FindOrders(current, interesting, 26, new List<string>() {}, valves, 0, ref bestScore, bestOrder);
            sw.Stop();
            var ell = sw.ElapsedMilliseconds;

            var orderBest = new List<string>
            {
                "DD", "BB", "JJ", "HH", "EE", "CC"
            };
            var sc = OrderScore(orderBest, valves);

            var order = bestOrder;


            int time = 30;
            
            var moves = new List<Move>();
            while (time > 0)
            {
                // Take nearest
                var nextNodeName = order.FirstOrDefault();
                Valve nextNode = null;

                if (order.Any())
                {
                    nextNode = valves[nextNodeName];
                    order.RemoveAt(0);
                }
                else
                {
                    nextNode = null;
                }

                /*
                var unopenedInteresting =
                    interesting.Where(v => !moves.Any(m => m.GoTo == v.Name && m.DoOpen)).ToList();

                nextNode = 
                    unopenedInteresting
                    .OrderByDescending(v =>
                        {
                            var path = FindPath(current.Name, v.Name, valves);
                            var score =  (time  - path.Count - 1) * v.FlowRate;
                            return score;
                        })
                    .FirstOrDefault(); 
                */
                if (nextNode == null)
                {
                    time--;
                    continue;
                }

                var path = FindPath(current.Name, nextNode.Name, valves);
                {
                    if (path.Count < time - 1)
                    {
                        Console.WriteLine($"Heading to the valve {nextNode.Name}");

                        foreach (var node in path)
                        {
                            time--;
                            moves.Add(new Move()
                            {
                                DoOpen = false,
                                GoFrom = current.Name,
                                GoTo = node,
                                TimeLeft = time
                            });
                            Console.WriteLine($"Go from node {current.Name} to {node}. Time left {time}");
                            current = valves[node];
                        }

                        moves.Last().DoOpen = true;
                        time--;
                        moves.Last().TimeLeft = time;
                        Console.WriteLine(
                            $"Opening valve {current.Name} with the flow {current.FlowRate}. Time left {time}");
                    }
                }
            }

            var score = moves.Where(m => m.DoOpen).Select(m => valves[m.GoTo].FlowRate * m.TimeLeft).Sum();
            Console.WriteLine($"Released pressure: {score}");
            Console.WriteLine($"There are {interesting.Count} valves with some flow");

            var cl = Console.ForegroundColor;
            foreach (var move in moves)
            {
                Console.ForegroundColor = move.DoOpen ? ConsoleColor.Yellow : cl;
                Console.Write($"{move.GoTo} ");
            }
            Console.ForegroundColor = cl;
            Console.WriteLine();
        }

        private static int OrderScore(List<string> order, Dictionary<string, Valve> valves)
        {
            var time = 30;
            var score = 0;

            var current = valves["AA"];
            foreach (var node in order)
            {
                var path = FindPath(current.Name, node, valves);
                time -= path.Count + 1;
                score += valves[node].FlowRate * time;
                current = valves[node];
            }
            return score;
        }

        private static void FindOrders(Valve current, List<Valve> interesting, int time, List<string> currentPath,
            Dictionary<string, Valve> valves,
            int score,
            ref int bestScore, List<string> bestOrder)
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
//                        bestOrder.Clear();
//                        bestOrder.AddRange(currentPath);
                    }
                    if (currentPath.Count < interesting.Count)
                    {
                        FindOrders(node, interesting, newTime, currentPath, valves, newScore, ref bestScore, bestOrder);
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
            foreach (var valve in valves)
            {
                unvisited.Add(valve.Key);
                distances[valve.Key] = 1000;
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

    class Move
    {
        public string GoFrom;
        public string GoTo;
        public bool DoOpen;
        public int TimeLeft;
    }

    [DebuggerDisplay("{Name} - {FlowRate}")]
    class Valve
    {
        protected bool Equals(Valve other)
        {
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Valve) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public static bool operator ==(Valve left, Valve right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Valve left, Valve right)
        {
            return !Equals(left, right);
        }

        public string Name;
        public int FlowRate;
        public List<string> LeadsToNames = new List<string>();
        public List<Valve> LeadsToValves = new List<Valve>();

        public Valve(string name, int flowRate)
        {
            Name = name;
            FlowRate = flowRate;
        }
    }
}
