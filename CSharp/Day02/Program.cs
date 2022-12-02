using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var lines = System.IO.File.ReadAllLines(fileName);

            var result = new List<Game>();
            foreach (var line in lines)
            {
                result.Add(new Game(line.Substring(0,1), line.Substring(2,1)));
            }
            return result;
        }
    }

    enum Choice
    {
        Rock,
        Paper,
        Scisors
    }

    enum GameResult
    {
        Lost,
        Draw,
        Won
    }

    class Game
    {
        public Game(string first, string second)
        {
            Dictionary<string, Choice> their = new Dictionary<string, Choice>()
            {
                {
                    "A", Choice.Rock
                },
                {
                    "B", Choice.Paper
                },
                {
                    "C", Choice.Scisors
                },
            };
            Dictionary<string, Choice> our = new Dictionary<string, Choice>()
            {
                {
                    "X", Choice.Rock
                },
                {
                    "Y", Choice.Paper
                },
                {
                    "Z", Choice.Scisors
                },
            };


            this.First = their[first];


            if (second == "Y")
            {
                this.Second = this.First;
            } else
            if (second == "X")
            {
                this.Second = HowToLose(First);
            }
            else
            {
                this.Second = HowToWin(First);
            }

//            this.Second = our[second];
            if (First == Second)
            {
                GameResult = GameResult.Draw;
            }
            else if (First == Choice.Rock && Second == Choice.Paper
                     || First == Choice.Paper && Second == Choice.Scisors
                     || First == Choice.Scisors && Second == Choice.Rock)
                {
                    GameResult = GameResult.Won;
                }
            else
            {
                GameResult = GameResult.Lost;
            }
        }

        private Choice HowToLose(Choice other)
        {
            if (other == Choice.Paper)
            {
                return Choice.Rock;
            } 
            else if (other == Choice.Rock)
            {
                return Choice.Scisors;
            }
            else return Choice.Paper;
        }

        private Choice HowToWin(Choice other)
        {
            if (other == Choice.Paper)
            {
                return Choice.Scisors;
            }
            else if (other == Choice.Rock)
            {
                return Choice.Paper;
            }
            else return Choice.Rock;
        }

        private GameResult GameResult;

        public int Score
        {
            get
            {
                var score = 0;
                switch (Second)
                {
                    case Choice.Rock:
                        score = 1;
                        break;
                    case Choice.Paper:
                        score = 2;
                        break;
                    case Choice.Scisors:
                        score = 3;
                        break;
                }

                switch (GameResult)
                {
                    case GameResult.Draw:
                        score += 3;
                        break;
                    case GameResult.Won:
                        score += 6;
                        break;
                }
                return score;
            }
        }

        public Choice First { get; set; }

        public Choice Second { get; set; }
    }
}
