using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day09
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022, Day 9");

            var input = ReadInput("input.txt");

            HashSet<Pos> visited = new HashSet<Pos>();

            var head = new Pos(0, 0);
            var tail = new Pos(0, 0);

            visited.Add(tail);

            foreach (var move in input)
            {
                for (var i = 0; i < move.Distance; i++)
                {
                    head.MoveTo(move.Direction);
                    tail.Follow(head);
                    visited.Add(tail);
                    Console.WriteLine($"{move.Direction} {head.X}/{head.Y} - {tail.X}/{tail.Y}");
                }
            }
            Console.WriteLine(visited.Count);
        }

        private static List<Move> ReadInput(string fileName)
        {
            var result = new List<Move>();

            var lines = File.ReadAllLines(fileName);

            var regex = new Regex(@"(\S+) (\d+)");
            foreach (var line in lines)
            {
                var match = regex.Match(line);
                result.Add(new Move()
                {
                    Direction = match.Groups[1].Value,
                    Distance = Int32.Parse(match.Groups[2].Value),
                });
            }
            return result;
        }
    }

    [DebuggerDisplay("{X}-{Y}")]
    class Pos
    {
        public int X;
        public int Y;

        public Pos(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void MoveTo(string direction)
        {
            switch (direction)
            {
                case "R":
                    X += 1;
                    break;
                case "L":
                    X -= 1;
                    break;
                case "U":
                    Y += 1;
                    break;
                case "D":
                    Y -= 1;
                    break;
            }


        }

        protected bool Equals(Pos other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Pos) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static bool operator ==(Pos left, Pos right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Pos left, Pos right)
        {
            return !Equals(left, right);
        }

        public void Follow(Pos head)
        {
            if (head == this)
            {
                return;
            }

            if (Math.Abs(head.X - this.X) <= 1
                && Math.Abs(head.Y - this.Y) <= 1)
            {
                return;
            }

            if (head.X == this.X)
            {
                this.Y += Math.Sign(head.Y - this.Y);
            }
            else if (head.Y == this.Y)
            {
                this.X += Math.Sign(head.X - this.X);
            }
            else
            {
                this.X += Math.Sign(head.X - this.X);
                this.Y += Math.Sign(head.Y - this.Y);
            }
        }
    }

    class Move
    {
        public string Direction;
        public int Distance;
    }
}

