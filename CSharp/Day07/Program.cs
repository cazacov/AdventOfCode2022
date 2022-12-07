using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day07
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022, Day 7");

            var commands = ReadCommands("input.txt");
            var allDirs = ProcessCommands(commands);

            var root = allDirs.First(d => d.Name == "/");
            UpdateSize(root);

            var smallDirs = allDirs.Where(d => d.Size <= 100000);
            Console.WriteLine($"Puzzle 1: {smallDirs.Sum(d => d.Size)}");

            var spaceUsed = root.Size;
            var spaceToFree = spaceUsed - 40000000;

            var dirToDelete = allDirs.OrderBy(d => d.Size).First(d => d.Size > spaceToFree);
            Console.WriteLine($"Puzzle 2: {dirToDelete.Size}");
        }

        private static List<FsDir> ProcessCommands(List<Command> commands)
        {
            var allDirs = new List<FsDir>();

            var root = new FsDir()
            {
                Name = "/"
            };
            allDirs.Add(root);
            var current = root;

            foreach (var command in commands)
            {
                if (command.CommandType == CommandType.Cd)
                {
                    switch (command.Argument)
                    {
                        case "/":
                        {
                            while (current.Parent != null)
                            {
                                current = current.Parent;
                            }
                            break;
                        }
                        case "..":
                            current = current.Parent;
                            break;
                        default:
                            current = current.Dirs.Single(d => d.Name == command.Argument);
                            break;
                    }
                }
                else if (command.CommandType == CommandType.Ls)
                {
                    foreach (var result in command.Results)
                    {
                        if (result.StartsWith("dir "))
                        {
                            var dirName = result[4..];
                            if (current.Dirs.All(d => d.Name != dirName))
                            {
                                var newDir = new FsDir() {Name = dirName, Parent = current};
                                allDirs.Add(newDir);
                                current.Dirs.Add(newDir);
                            }
                        }
                        else
                        {
                            var regex = new Regex(@"(\d+) (\S+)");
                            var match = regex.Match(result);
                            var fileName = match.Groups[2].Value;
                            var fileSize = Int32.Parse(match.Groups[1].Value);

                            if (current.Files.All(f => f.Name != fileName))
                            {
                                current.Files.Add(new FsFile()
                                {
                                    Name = fileName,
                                    Size = fileSize,
                                });
                            }
                        }
                    }
                }
            }
            return allDirs;
        }

        private static void UpdateSize(FsDir dir)
        {
            dir.Dirs.ForEach(UpdateSize);
            dir.Size = dir.Files.Sum(f => f.Size);
            dir.Size += dir.Dirs.Sum(d => d.Size);
        }

        private static List<Command> ReadCommands(string fileName)
        {
            var input = File.ReadAllLines(fileName).ToList();

            var result = new List<Command>();
            Command command = null;
            foreach (var line in input)
            {
                if (line.StartsWith("$"))
                {
                    if (line.StartsWith("$ cd"))
                    {
                        command = new Command(CommandType.Cd, line[5..]);
                    }
                    else if (line.StartsWith("$ ls"))
                    {
                        command = new Command(CommandType.Ls, "");
                    }
                    result.Add(command);
                }
                else
                {
                    command.Results.Add(line);
                }
            }
            return result;
        }
    }
    
    public class FsDir
    {
        public String Name;
        public long Size;
        public List<FsFile> Files = new List<FsFile>();
        public List<FsDir> Dirs = new List<FsDir>();
        public FsDir Parent;
    }

    public class FsFile
    {
        public String Name;
        public long Size;
    }

    public enum CommandType
    {
        Cd,
        Ls
    }

    [DebuggerDisplay("{CommandType} - {Argument}")]
    public class Command
    {
        public CommandType CommandType;
        public string Argument;
        public List<string> Results = new List<string>();

        public Command(CommandType commandType, string argument)
        {
            CommandType = commandType;
            Argument = argument;
        }
    }
}
