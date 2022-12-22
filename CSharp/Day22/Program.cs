using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day22
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of code 2022, day 22");
            var map = new Dictionary<Pos, Type>();
            var commands = new List<Command>();
            ReadInput("input.txt", map, commands);

            Puzzle1(map, commands);
        }

        private static void Puzzle1(Dictionary<Pos, Type> map, List<Command> commands)
        {
            int[] dx = new[] {1, 0, -1, 0};
            int[] dy = new[] { 0, 1, 0, -1 };


            var x = map.Where(p => p.Key.Y == 0 && p.Value == Type.Free).Min(p => p.Key.X);
            var current = new Pos(x, 0);
            var direction = 0;

            foreach (var command in commands)
            {
                direction = (direction + command.Direction + 4) % 4;
                var distance = command.Distance;

                Console.WriteLine($"{current.X},{current.Y} \tgo direction {direction} distance {command.Distance}");
                
                // Go
                while (distance-- > 0)
                {
                    var nextPos = new Pos(current.X + dx[direction], current.Y + dy[direction]);
                    if (!map.ContainsKey(nextPos) || map[nextPos] == Type.Empty)
                    {
                        switch (direction)
                        {
                            case 0:
                                var minX = map.Where(p => p.Key.Y == current.Y && p.Value != Type.Empty).Min(p => p.Key.X);
                                nextPos = new Pos(minX, current.Y);
                                break;
                            case 2:
                                var maxX = map.Where(p => p.Key.Y == current.Y && p.Value != Type.Empty).Max(p => p.Key.X);
                                nextPos = new Pos(maxX, current.Y);
                                break;
                            case 1:
                                var minY = map.Where(p => p.Key.X == current.X && p.Value != Type.Empty).Min(p => p.Key.Y);
                                nextPos = new Pos(current.X, minY);
                                break;
                            case 3:
                                var maxY = map.Where(p => p.Key.X == current.X && p.Value != Type.Empty).Max(p => p.Key.Y);
                                nextPos = new Pos(current.X, maxY);
                                break;
                        }
                        Console.WriteLine($"Wrapping from {current.X},{current.Y} to {nextPos.X},{nextPos.Y}");
                    }

                    if (map[nextPos] == Type.Wall)
                    {
                        Console.WriteLine($"Hit wall, staying at {current.X}, {current.Y}");
                        break;
                    }
                    else
                    {
                        current = nextPos;
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine($"End position {current.X},{current.Y} direction {direction}");

            var row = current.Y + 1;
            var column = current.X + 1;
            Console.WriteLine($"Puzzle 1: {row * 1000 + column * 4 + direction}");
        }

        private static void ReadInput(string fileName, Dictionary<Pos, Type> map, List<Command> commands)
        {
            map.Clear();
            commands.Clear();

            var lines = File.ReadAllLines(fileName);

            var y = 0;
            while (true)
            {
                var line = lines[y];
                if (String.IsNullOrWhiteSpace(line))
                {
                    break;
                }

                for (int x = 0; x < line.Length; x++)
                {
                    switch (line[x])
                    {
                        case ' ':
                            map[new Pos(x, y)] = Type.Empty;
                            break;
                        case '.':
                            map[new Pos(x, y)] = Type.Free;
                            break;
                        case '#':
                            map[new Pos(x, y)] = Type.Wall;
                            break;
                    }
                }
                y++;
            }

            var commandLine = lines[y + 1];
            ParseCommandLine(commands, commandLine);
        }

        private static void ParseCommandLine(List<Command> commands, string commandLine)
        {
            var i = 0;
            while (Char.IsDigit(commandLine[i]))
            {
                i++;
            }

            commands.Add(new Command(0, int.Parse(commandLine.Substring(0, i))));
            do
            {
                var direction = 0;
                if (commandLine[i] == 'R')
                {
                    direction = 1;
                }

                if (commandLine[i] == 'L')
                {
                    direction = -1;
                }

                var start = i + 1;
                i++;
                while (i < commandLine.Length && Char.IsDigit(commandLine[i]))
                {
                    i++;
                }

                commands.Add(new Command(direction, int.Parse(commandLine.Substring(start, i - start))));
            } while (i < commandLine.Length);
        }
    }

    public enum Type
    {
        Empty,
        Wall,
        Free
    }


    [DebuggerDisplay("{Direction} - {Distance}")]
    public class Command
    {
        public int Direction;
        public int Distance;

        public Command(int direction, int distance)
        {
            Direction = direction;
            Distance = distance;
        }
    }

    [DebuggerDisplay("{X},{Y}")]
    public class Pos
    {
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

        public readonly int X;
        public readonly int Y;

        public Pos(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
