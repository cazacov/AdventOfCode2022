using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day21
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022, day 21");
            var input = ReadInput("input.txt");

            Puzzle1(input);
            Puzzle2(input);
        }

        private static void Puzzle1(Dictionary<string, Node> input)
        {
            var root = input["root"];

            while (!root.HasValue)
            {
                var canCalculate = input.Values.Where(n => n.CanCalculate()).ToList();
                foreach (var node in canCalculate)
                {
                    node.Calculate();
                }
            }
            Console.WriteLine($"Puzzle 1: {root.Value}");
        }

        private static void Puzzle2(Dictionary<string, Node> input)
        {
            var root = input["root"];
            var variable = input["humn"];

            while (!root.HasValue)
            {
                var canCalculate = input.Values.Where(n => n.CanCalculate()).ToList();
                foreach (var node in canCalculate)
                {
                    node.Calculate();
                }
            }

            if (root.first.DependsOn(variable))
            {
                root.first.Value = root.second.Value;
                ReverseOperation(root.first, variable);
            }
            else
            {
                root.second.Value = root.first.Value;
                ReverseOperation(root.second, variable);
            }
            Console.WriteLine($"Puzzle 2: {variable.Value}");
        }

        private static void ReverseOperation(Node node, Node targetNode)
        {
            if (node == targetNode)
            {
                return;
            }

            var dependsFirst = node.first.DependsOn(targetNode);
            var dependsSecond = node.second.DependsOn(targetNode);
            if (dependsFirst && dependsSecond)
            {
                throw new NotImplementedException();
            }

            if (dependsFirst)
            {
                switch (node.Operation)
                {
                    case Operation.Add:
                        node.first.Value = node.Value - node.second.Value;
                        break;
                    case Operation.Sub:
                        node.first.Value = node.Value + node.second.Value;
                        break;
                    case Operation.Mul:
                        node.first.Value = node.Value / node.second.Value;
                        break;
                    case Operation.Div:
                        node.first.Value = node.Value * node.second.Value;
                        break;
                }
                ReverseOperation(node.first, targetNode);
            }
            else
            {
                switch (node.Operation)
                {
                    case Operation.Add:
                        node.second.Value = node.Value - node.first.Value;
                        break;
                    case Operation.Sub:
                        node.second.Value = node.first.Value - node.Value;
                        break;
                    case Operation.Mul:
                        node.second.Value = node.Value / node.first.Value;
                        break;
                    case Operation.Div:
                        node.second.Value = node.first.Value / node.Value;
                        break;
                }
                ReverseOperation(node.second, targetNode);
            }
        }

        private static Dictionary<string, Node> ReadInput(string fileName)
        {
            var lines = File.ReadAllLines(fileName);
            var result = new List<Node>();

            var regexConst = new Regex(@"(\S+): (\d+)");
            var regexOp = new Regex(@"(\S+): (\S+) (\S+) (\S+)");

            foreach (var line in lines)
            {
                if (Char.IsDigit(line[6]))
                {
                    var match = regexConst.Match(line);
                    result.Add(new Node(match.Groups[1].Value, int.Parse(match.Groups[2].Value)));
                }
                else
                {
                    var match = regexOp.Match(line);
                    var op = Operation.Constant;
                    switch (match.Groups[3].Value)
                    {
                        case "+":
                            op = Operation.Add;
                            break;
                        case "-":
                            op = Operation.Sub;
                            break;
                        case "*":
                            op = Operation.Mul;
                            break;
                        case "/":
                            op = Operation.Div;
                            break;
                    }
                    result.Add(new Node(
                        match.Groups[1].Value,
                        op,
                        match.Groups[2].Value,
                        match.Groups[4].Value));
                }
            }

            var dict = result.ToDictionary(x => x.Code, y => y);
            foreach (var node in result.Where(x => x.Operation != Operation.Constant))
            {
                node.first = dict[node.firstCode];
                node.second = dict[node.secondCode];
            }
            return dict;
        }
    }
}
