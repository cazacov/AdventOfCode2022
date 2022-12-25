using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Day24
{
    class Map
    {
        public List<Blizzard> Blizzards = new List<Blizzard>();
        public HashSet<Pos> Walls = new HashSet<Pos>();
        public int Width;
        public int Height;
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
                Width = lines[0].Length,
                Height = lines.Length,
            };

            map.Start = new Pos(lines[0].IndexOf('.'), 0);
            map.Finish = new Pos(lines[map.Height - 1].IndexOf('.'), map.Height - 1);

            for (var y = 0; y < map.Height; y++)
            {
                var line = lines[y];
                for (var x = 0; x < map.Width; x++)
                {
                    if (line[x] == '.')
                    {
                        continue;
                    }
                    if (line[x] == '#')
                    {
                        map.Walls.Add(new Pos(x, y));
                        continue;
                    }

                    map.Blizzards.Add(new Blizzard(
                        line[x] switch
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
                if (nextPos.X == this.Width || nextPos.X == 0)
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
                blizzard.MakeStep(this.Width, this.Height);
            }
            Step++;
            MakeSnapshot();
        }

        private void MakeSnapshot()
        {
            var safeLocations = new HashSet<Pos>();
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    safeLocations.Add(new Pos(x, y));
                }
            }
            safeLocations.ExceptWith(this.Walls);
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

            var x = this.Position.X + dx[Direction];
            if (x == 0)
            {
                x = maxX - 2;
            }
            if (x == maxX - 1)
            {
                x = 1;
            }
            var y = this.Position.Y + dy[Direction];
            if (y == 0)
            {
                y = maxY - 2;
            }
            if (y == maxY - 1)
            {
                y = 1;
            }
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

    [DebuggerDisplay("{X}, {Y}, {T}")]
    class MinPos
    {
        public readonly int X;
        public readonly int Y;
        public readonly int T;

        public MinPos(int x, int y, int t)
        {
            X = x;
            Y = y;
            T = t;
        }

        public MinPos(Pos position, int t)
        {
            X = position.X;
            Y = position.Y;
            T = t;
        }


        protected bool Equals(MinPos other)
        {
            return X == other.X && Y == other.Y && T == other.T;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MinPos) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, T);
        }

        public static bool operator ==(MinPos left, MinPos right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MinPos left, MinPos right)
        {
            return !Equals(left, right);
        }
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