using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Day13
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022, Day 13");

            // Puzzle 1
            var input = ReadPairs("input.txt");
            var puzzle1Result = input
                .Where(x => Compare(x.Left, x.Right) < 0)
                .Sum(x => x.Index);

            Console.WriteLine($"Puzzle 1: {puzzle1Result}");

            // Puzzle 2
            var allPackets = ReadAll("input.txt");

            var divider1 = JsonConvert.DeserializeObject<JToken>("[[2]]");
            var divider2 = JsonConvert.DeserializeObject<JToken>("[[6]]");
            allPackets.Add(divider1);
            allPackets.Add(divider2);

            allPackets.Sort(Compare);

            var idx1 = allPackets.IndexOf(divider1);
            var idx2 = allPackets.IndexOf(divider2);
            Console.WriteLine($"Puzzle 2: {(idx1 + 1) * (idx2 + 1)}");
        }

        private static List<JToken> ReadAll(string fileName)
        {
            var lines = File.ReadAllLines(fileName);

            return lines
                .Where(line => !String.IsNullOrWhiteSpace(line))
                .Select(line => JsonConvert.DeserializeObject<JToken>(line))
                .ToList();
        }

        private static List<Pair> ReadPairs(string filename)
        {
            var lines = File.ReadAllLines(filename);
            
            var result = new List<Pair>();

            var lineNumber = 0;
            var pairIndex = 1;
            while (lineNumber < lines.Length)
            {
                var first = lines[lineNumber];
                var second = lines[lineNumber+1];

                var left = JsonConvert.DeserializeObject<JToken>(first);
                var right = JsonConvert.DeserializeObject<JToken>(second);
                result.Add(new Pair() { Left = left, Right = right, Index = pairIndex, LeftSource = first, RightSource = second});

                lineNumber += 3;
                pairIndex += 1;
            }
            return result;
        }

        public static int Compare(JToken first, JToken second)
        {
            if (first is JValue firstValue && second is JValue secondValue)
            {
                // If both values are integers, the lower integer should come first
                return firstValue.CompareTo(secondValue);
            }
            else if (first is JArray firstArray && second is JArray secondArray)
            {
                // If both values are lists
                for (var i = 0; i < firstArray.Count; i++)
                {
                    if (secondArray.Count < i + 1)
                    {
                        // If the right list runs out of items first
                        return 1;
                    }
                    var cmp = Compare(firstArray[i], secondArray[i]);
                    if (cmp != 0)
                    {
                        return cmp;
                    }
                }

                if (firstArray.Count != secondArray.Count)
                {
                    // If the left list runs out of items first
                    return -1;
                }
                // Arrays have same length and all elements are equal
                return 0;   
            }
            else    // If exactly one value is an integer
            {
                if (first is JValue firstInteger)
                {
                    return Compare(new JArray(first), second);
                }
                else
                {
                    return Compare(first, new JArray(second));
                }
            }
        }
    }
}

public class Pair
{
    public JToken Left;
    public JToken Right;
    public string LeftSource;
    public string RightSource;
    public int Index;
}



