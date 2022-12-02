using System;
using System.Collections.Generic;
using System.Linq;

namespace Day02
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022, Day 2");
            
            var input = ReadInput("input.txt");
            Console.WriteLine(input.Sum(x => x.Score));
        }

        private static List<Game> ReadInput(string fileName)
        {
            return System.IO.File.ReadAllLines(fileName)
                .Select(line => 
                    new Game(line[0], line[2])
                )
                .ToList();
        }
    }

    class Game
    {
        public int Their;
        public int My;

        public Game(char first, char second)
        {
            Their = (int) first - (int) 'A';

            // Puzzle 1 
            //My = (int)second - (int)'X';

            // Puzzle 2
            switch (second)
            {
                case 'X':   // Lose
                    My = (Their + 2) % 3;
                    break;
                case 'Y':   // Draw
                    My = Their;
                    break;
                case 'Z':   // Win
                    My = (Their + 1) % 3;
                    break;
            }
        }

        public int Score
        {
            get
            {
                var score = My + 1;
                if (My == Their)    // Draw
                {
                    score += 3;
                }
                else if (My == (Their + 1) % 3) // Win
                {
                    score += 6;
                }
                return score;
            }
        }
    }
}
