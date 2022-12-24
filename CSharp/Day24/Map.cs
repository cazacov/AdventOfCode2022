using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Day24
{
    class Map
    {
        public List<Blizzard> Blizzards = new List<Blizzard>();
        public int MaxX;
        public int MaxY;
        public Pos Start;
        public Pos Finish;
        public HashSet<Pos> SafeLocations = new HashSet<Pos>();

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
                        map.SafeLocations.Add(new Pos(x, y));
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
            return map;
        }

        public void NextStep()
        {
            this.SafeLocations.Clear();
            for (var x = 0; x < MaxX; x++)
            {
                for (var y = 0; y < MaxY; y++)
                {
                    SafeLocations.Add(new Pos(x, y));
                }
            }

            foreach (var blizzard in Blizzards)
            {
                SafeLocations.Remove(blizzard.MakeStep(this.MaxX, this.MaxY));
            }
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

        public Pos(int x, int y)
        {
            X = x;
            Y = y;
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
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Pos)obj);
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
    }
}