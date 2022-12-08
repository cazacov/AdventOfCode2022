using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day08
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022, Day 8");

            var input = ReadInput("input.txt");

            Puzzle1(input);
            Puzzle2(input);
        }

        private static void Puzzle1(int[,] input)
        {
            var size = input.GetUpperBound(0) + 1;
            var visible = size * 4 - 4;

            for (int i = 1; i < size - 1; i++)
            {
                for (int j = 1; j < size-1; j++)
                {
                    var h = input[i, j];

                    var up = TreesUp(i, j, ref input);
                    if (up.All(t => t < h))
                    {
                        visible++;
                        continue;
                    }

                    var right = TreesRight(i, j, ref input);
                    if (right.All(t => t < h))
                    {
                        visible++;
                        continue;
                    }

                    var down = TreesDown(i, j, ref input);
                    if (down.All(t => t < h))
                    {
                        visible++;
                        continue;
                    }

                    var left = TreesLeft(i, j, ref input);
                    if (left.All(t => t < h))
                    {
                        visible++;
                        continue;
                    }
                }
            }
            Console.WriteLine($"Visible trees: {visible}");
        }


        private static void Puzzle2(int[,] input)
        {
            var size = input.GetUpperBound(0) + 1;
            var result = new int[size, size];

            for (int i = 1; i < size-1; i++)
            {
                for (int j = 1; j < size-1; j++)
                {
                    var h = input[i, j];

                    var scientificScore = 1;
                    
                    var up = TreesUp(i, j, ref input);
                    var score = up.TakeWhile(t => t < h).Count();
                    if (score < i)
                    {
                        score++;
                    }
                    scientificScore *= score;

                    var left = TreesLeft(i, j, ref input);
                    score = left.TakeWhile(t => t < h).Count();
                    if (score < j)
                    {
                        score++;
                    }
                    scientificScore *= score;

                    var down = TreesDown(i, j, ref input);
                    score = down.TakeWhile(t => t < h).Count();
                    if (score < size - i - 1)
                    {
                        score++;
                    }
                    scientificScore *= score;

                    var right = TreesRight(i, j, ref input);
                    score = right.TakeWhile(t => t < h).Count();
                    if (score < size - j - 1)
                    {
                        score++;
                    }
                    scientificScore *= score;
                    result[i, j] = scientificScore;
                }
            }

            var m = 0;
            foreach (var res in result)
            {
                if (res > m)
                {
                    m = res;
                }
            }
            Console.WriteLine("Max score: " + m);
        }

        private static List<int> TreesUp(int row, int column, ref int[,] input)
        {
            var result = new List<int>();

            row--;
            while (row >= 0)
            {
                result.Add(input[row, column]);
                row--;
            }
            return result;
        }

        private static List<int> TreesDown(int row, int column, ref int[,] input)
        {
            var result = new List<int>();

            row++;
            while (row <= input.GetUpperBound(0))
            {
                result.Add(input[row, column]);
                row++;
            }
            return result;
        }

        private static List<int> TreesLeft(int row, int column, ref int[,] input)
        {
            var result = new List<int>();

            column--;
            while (column >= 0)
            {
                result.Add(input[row, column]);
                column--;
            }
            return result;
        }

        private static List<int> TreesRight(int row, int column, ref int[,] input)
        {
            var result = new List<int>();

            column++;
            while (column <= input.GetUpperBound(0))
            {
                result.Add(input[row, column]);
                column++;
            }
            return result;
        }

        private static int[,] ReadInput(string fileName)
        {
            var lines = File.ReadAllLines(fileName).ToList();
            int size = lines.Count;

            var result = new int[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    result[i, j] = Int32.Parse(lines[i].Substring(j, 1));
                }
            }
            return result;
        }
    }
}
