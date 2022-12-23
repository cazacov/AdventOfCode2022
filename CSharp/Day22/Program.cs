using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day22
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of code 2022, day 22");
            var map = new Dictionary<Pos, Pos>();
            var commands = new List<Command>();
            ReadInput("input.txt", map, commands);

            Puzzle1(map, commands);
            Puzzle2(map, commands);
        }

        private static void Puzzle2(Dictionary<Pos, Pos> map, List<Command> commands)
        {
            var cubeSize = (int)Math.Sqrt(map.Values.Count(p => p.Type != Type.Empty) / 6);

            var map3D = Build3dMap(map, cubeSize);

            var start = map[new Pos(map.Where(p => p.Key.Y == 0 && p.Value.Type != Type.Empty).Min(p => p.Key.X), 0)];
            var current3d = start.OnCube;
            var current2d = map3D[current3d];
            var dir3d = new Pos3(1, 0, 0);

            foreach (var command in commands)
            {
                var normal = current2d.Normal;
                var rightDir = dir3d.CrossProduct(normal);
                dir3d = command.Direction switch
                {
                    1 => rightDir,
                    -1 => rightDir.Negate(),
                    _ => dir3d
                };

                var distance = command.Distance;
                while (distance-- > 0)
                {
                    var newDir = dir3d;
                    var newNormal = normal;
                    var nextPos3d = current3d.Add(dir3d);
                    if (!map3D.ContainsKey(nextPos3d))
                    {
                        var t = newDir.Negate();
                        newDir = newNormal;
                        newNormal = t;
                        nextPos3d = nextPos3d.Add(newDir);
                    }
                    var nextPos2d = map3D[nextPos3d];
                    if (nextPos2d.Type == Type.Wall)
                    {
                        break;
                    }

                    dir3d = newDir;
                    normal = newNormal;
                    current3d = nextPos3d;
                    current2d = nextPos2d;
                }
            }
            var row = current2d.Y + 1;
            var column = current2d.X + 1;

            var newF = map3D[current3d.Add(dir3d)];
            var dx = newF.X - current2d.X;
            var dy = newF.Y - current2d.Y;

            var dir = DecodeDirection(dx, dy);

            Console.WriteLine($"Puzzle 2: {row * 1000 + column * 4 + dir}");
        }

        private static int DecodeDirection(int dx, int dy)
        {
            switch (dx)
            {
                case 1 when dy == 0:
                    return 0;
                case 0 when dy == 1:
                    return 1;
                case 0 when dy == -1:
                    return 3;
                case -1 when dy == 0:
                    return 2;
                default:
                    throw new NotImplementedException();
            }
        }

        private static Dictionary<Pos3, Pos> Build3dMap(Dictionary<Pos, Pos> map, int cubeSize)
        {
            var map3D = new Dictionary<Pos3, Pos>();
            var start = map[new Pos(map.Where(p => p.Key.Y == 0 && p.Value.Type != Type.Empty).Min(p => p.Key.X), 0)];
            start.OnCube = new Pos3(0, 0, -1);
            start.Normal = new Pos3(0, 0, 1);
            start.Right = new Pos3(1, 0, 0);
            start.Down = new Pos3(0, 1, 0);
            start.Left = new Pos3(-1, 0, 0);
            map3D[start.OnCube] = start;

            var visited2D = new HashSet<Pos>();

            var unprocessed = new HashSet<Pos>
            {
                start
            };
            while (unprocessed.Any())
            {
                var current = unprocessed.First();
                Pos newNode = null;
                Pos3 newNode3d = null;
                var right2D = new Pos(current.X + 1, current.Y);
                if (map.ContainsKey(right2D) && map[right2D].Type != Type.Empty)
                {
                    if (!visited2D.Contains(right2D))
                    {
                        newNode = map[right2D];
                        newNode3d = current.OnCube.Add(current.Right);
                        if (newNode.X / cubeSize == current.X / cubeSize)
                        {
                            newNode.Left = current.Left;
                            newNode.Down = current.Down;
                            newNode.Right = current.Right;
                            newNode.Normal = current.Normal;
                            newNode.OnCube = newNode3d;
                        }
                        else
                        {
                            newNode.Right = current.Normal;
                            newNode.Down = current.Down;
                            newNode.Left = current.Normal.Negate();
                            newNode.Normal = current.Right.Negate();
                            newNode.OnCube = newNode3d.Add(newNode.Right);
                        }

                        map3D[newNode.OnCube] = newNode;
                        unprocessed.Add(newNode);
                    }
                }

                var left2D = new Pos(current.X - 1, current.Y);
                if (map.ContainsKey(left2D) && map[left2D].Type != Type.Empty)
                {
                    if (!visited2D.Contains(left2D))
                    {
                        newNode = map[left2D];
                        newNode3d = current.OnCube.Add(current.Left);
                        if (newNode.X / cubeSize == current.X / cubeSize)
                        {
                            newNode.Left = current.Left;
                            newNode.Down = current.Down;
                            newNode.Right = current.Right;
                            newNode.Normal = current.Normal;
                            newNode.OnCube = newNode3d;
                        }
                        else
                        {
                            newNode.Right = current.Normal.Negate();
                            newNode.Down = current.Down;
                            newNode.Left = current.Normal;
                            newNode.Normal = current.Right;
                            newNode.OnCube = newNode3d.Add(newNode.Left);
                        }

                        map3D[newNode.OnCube] = newNode;
                        unprocessed.Add(newNode);
                    }
                }

                var down2D = new Pos(current.X, current.Y + 1);
                if (map.ContainsKey(down2D) && map[down2D].Type != Type.Empty)
                {
                    if (!visited2D.Contains(down2D))
                    {
                        newNode = map[down2D];
                        newNode3d = current.OnCube.Add(current.Down);
                        if (newNode.Y / cubeSize == current.Y / cubeSize)
                        {
                            newNode.Left = current.Left;
                            newNode.Down = current.Down;
                            newNode.Right = current.Right;
                            newNode.Normal = current.Normal;
                            newNode.OnCube = newNode3d;
                        }
                        else
                        {
                            newNode.Right = current.Right;
                            newNode.Down = current.Normal;
                            newNode.Left = current.Left;
                            newNode.Normal = current.Down.Negate();
                            newNode.OnCube = newNode3d.Add(newNode.Down);
                        }

                        map3D[newNode.OnCube] = newNode;
                        unprocessed.Add(newNode);
                    }
                }

                unprocessed.Remove(current);
                visited2D.Add(current);
            }

            return map3D;
        }

        private static void Puzzle1(Dictionary<Pos, Pos> map, List<Command> commands)
        {
            var dx = new[] {1, 0, -1, 0};
            var dy = new[] { 0, 1, 0, -1 };


            var x = map.Where(p => p.Key.Y == 0 && p.Value.Type == Type.Free).Min(p => p.Key.X);
            var current = new Pos(x, 0);
            var direction = 0;

            foreach (var command in commands)
            {
                direction = (direction + command.Direction + 4) % 4;
                var distance = command.Distance;

                //Console.WriteLine($"{current.X},{current.Y} \tgo direction {direction} distance {command.Distance}");
                
                // Go
                while (distance-- > 0)
                {
                    var nextPos = new Pos(current.X + dx[direction], current.Y + dy[direction]);
                    if (!map.ContainsKey(nextPos) || map[nextPos].Type == Type.Empty)
                    {
                        switch (direction)
                        {
                            case 0:
                                var minX = map.Where(p => p.Key.Y == current.Y && p.Value.Type != Type.Empty).Min(p => p.Key.X);
                                nextPos = new Pos(minX, current.Y);
                                break;
                            case 2:
                                var maxX = map.Where(p => p.Key.Y == current.Y && p.Value.Type != Type.Empty).Max(p => p.Key.X);
                                nextPos = new Pos(maxX, current.Y);
                                break;
                            case 1:
                                var minY = map.Where(p => p.Key.X == current.X && p.Value.Type != Type.Empty).Min(p => p.Key.Y);
                                nextPos = new Pos(current.X, minY);
                                break;
                            case 3:
                                var maxY = map.Where(p => p.Key.X == current.X && p.Value.Type != Type.Empty).Max(p => p.Key.Y);
                                nextPos = new Pos(current.X, maxY);
                                break;
                        }
                        //Console.WriteLine($"Wrapping from {current.X},{current.Y} to {nextPos.X},{nextPos.Y}");
                    }

                    if (map[nextPos].Type == Type.Wall)
                    {
                        //Console.WriteLine($"Hit wall, staying at {current.X}, {current.Y}");
                        break;
                    }
                    else
                    {
                        current = nextPos;
                    }
                }
                //Console.WriteLine();
            }

            Console.WriteLine($"End position {current.X},{current.Y} direction {direction}");

            var row = current.Y + 1;
            var column = current.X + 1;
            Console.WriteLine($"Puzzle 1: {row * 1000 + column * 4 + direction}");
        }

        private static void ReadInput(string fileName, Dictionary<Pos, Pos> map, List<Command> commands)
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
                    var newPos = new Pos(x, y);
                    
                    switch (line[x])
                    {
                        case ' ':
                            newPos.Type = Type.Empty;
                            break;
                        case '.':
                            newPos.Type = Type.Free;
                            break;
                        case '#':
                            newPos.Type = Type.Wall;
                            break;
                    }
                    map[new Pos(x, y)] = newPos;
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
}
