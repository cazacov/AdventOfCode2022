using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace Day24
{
    class Map
    {
        public List<Blizzard> Blizzards = new List<Blizzard>();
        public int MaxX;
        public int MaxY;
        public Pos Start;
        public Pos Finish;
        public int Step { get; private set; }
        private readonly Dictionary<int, HashSet<Pos>> snapshots = new Dictionary<int, HashSet<Pos>>();
        public List<Command> BestPath = new List<Command>();

        public static Map ReadFromFile(string fileName)
        {
            var lines = File.ReadAllLines(fileName);

            var map = new Map()
            {
                MaxX = lines[0].Length - 2,
                MaxY = lines.Length - 2,
            };
            map.Start = new Pos(0, 0);
            map.Finish = new Pos(map.MaxX - 1, map.MaxY - 1);


            for (var y = 0; y < map.MaxY; y++)
            {
                var line = lines[y + 1];
                for (var x = 0; x < map.MaxX; x++)
                {
                    if (line[x + 1] == '.')
                    {
                        continue;
                    }
                    map.Blizzards.Add(new Blizzard(
                        line[x + 1] switch
                        {
                            '^' => 0,
                            '>' => 1,
                            'v' => 2,
                            '<' => 3
                        },
                        x,y));
                }
            }
            map.MakeSnapshot();
            return map;
        }

        static readonly Command[] allCommands = { Command.Wait, Command.Right, Command.Down, Command.Left, Command.Up };

        public void FindPath(Pos current, int step, List<Command> path, ref int upperBound, ref int stepBack)
        {
            if (current == Finish)
            {
                BestPath.Clear();
                BestPath.AddRange(path);
                Console.WriteLine($"Find a path of length {path.Count}");
                upperBound = path.Count - 1;
                return;
            }

            if (step >= upperBound)
            {
                return;
            }

            var safeLocations = this.SafeLocationsAtStep(step + 1);

            var moves = new List<Move>();
            foreach (var command in allCommands)
            {
                var nextPos = current.Go(command);
                if (nextPos.X >= this.MaxX || nextPos.Y >= this.MaxY || nextPos.X < 0 || nextPos.Y < 0)
                {
                    continue;
                }
                if (safeLocations.Contains(nextPos))
                {
                    moves.Add(new Move()
                    {
                        Command = command,
                        Position = nextPos,
                        Cost = path.Count + this.Cost(nextPos, this.Finish)
                    });
                }
            }
            moves.Sort(0, moves.Count, Move.CostComparer);

            foreach (var move in moves)
            {
                path.Add(move.Command);
                FindPath(move.Position, step + 1, path, ref upperBound, ref stepBack);
                path.RemoveAt(path.Count - 1);
            }

            if (step < stepBack)
            {
                stepBack = step;
                Console.WriteLine($"Step back to {step}");
            }
        }


        public void NextStep()
        {
            foreach (var blizzard in Blizzards)
            {
                blizzard.MakeStep(this.MaxX, this.MaxY);
            }
            Step++;
            MakeSnapshot();
        }

        private void MakeSnapshot()
        {
            var safeLocations = new HashSet<Pos>();
            for (var x = 0; x < MaxX; x++)
            {
                for (var y = 0; y < MaxY; y++)
                {
                    safeLocations.Add(new Pos(x, y));
                }
            }
            foreach (var blizzard in Blizzards)
            {
                safeLocations.Remove(blizzard.Position);
            }
            snapshots[Step] = safeLocations;
        }

        public HashSet<Pos> SafeLocationsAtStep(int stepNr)
        {
            while (this.Step < stepNr)
            {
                this.NextStep();
            }
            return snapshots[stepNr];
        }

        public int Cost(Pos from, Pos to)
        {
            return Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
/*
            var dx = from.X - to.X;
            var dy = from.Y - to.Y;
            return (int) (Math.Sqrt(dx * dx + dy * dy) * 10); */
        }
    }

    class Blizzard
    {
        public readonly int Direction;
        public Pos Position;

        public Blizzard(int direction, int x, int y)
        {
            Direction = direction;
            this.Position = new Pos(x, y);
        }

        public Pos MakeStep(int maxX, int maxY)
        {
            var dx = new int[] { 0, 1, 0, -1 };
            var dy = new int[] { -1, 0, 1, 0 };

            var x = (this.Position.X + dx[Direction] + maxX) % maxX;
            var y = (this.Position.Y + dy[Direction] + maxY) % maxY;

            this.Position = new Pos(x, y);

            return this.Position;
        }

        public override string ToString()
        {
            var dirch = new[] { "^", ">", "v", "<" };
            return $"{Position.X},{Position.Y} {dirch[Direction]}";
        }
    }

    public enum Command
    {
        Wait,
        Up,
        Right,
        Down, 
        Left
    }

    [DebuggerDisplay("{X}, {Y}")]
    class Pos
    {
        public readonly int X;
        public readonly int Y;
        private readonly int hashCode;

        public Pos(int x, int y)
        {
            X = x;
            Y = y;
            this.hashCode = x * 100 + y;
        }

        public Pos Go(Command command)
        {
            switch (command)
            {
                case Command.Up:
                    return new Pos(this.X, this.Y - 1);
                case Command.Right:
                    return new Pos(this.X + 1, this.Y);
                case Command.Down:
                    return new Pos(this.X, this.Y + 1);
                case Command.Left:
                    return new Pos(this.X - 1, this.Y);
                case Command.Wait:
                default:
                    return new Pos(this.X, this.Y);
            }
        }

        protected bool Equals(Pos other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            return this.hashCode == (obj as Pos).hashCode;
        }

        public override int GetHashCode()
        {
            return hashCode;
        }

        public static bool operator ==(Pos left, Pos right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Pos left, Pos right)
        {
            return !Equals(left, right);
        }
    }
}