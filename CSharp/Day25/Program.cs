using System;
using System.IO;
using System.Linq;

namespace Day25
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022, day 25");
            
            var result = File.ReadAllLines("input.txt")
                .Select(Snafu2Int)
                .Sum();
            
            Console.WriteLine($"Puzzle 1: {Int2Snafu(result)}");
        }

        private static string Int2Snafu(long n)
        {
            var result = "";
            do
            {
                var rest = n % 5;
                result = DigitToChar(n % 5) + result;
                n /= 5;
                if (rest > 2)
                {
                    n++;
                }
            } while (n > 0);
            return result;
        }

        private static long Snafu2Int(string snafuString)
        {
            var result = 0L;
            foreach (var ch in snafuString)
            {
                result = result * 5 + CharToDigit(ch);
            }
            return result;
        }

        private static int CharToDigit(char ch)
        {
            return ch switch
            {
                '2' => 2,
                '1' => 1,
                '0' => 0,
                '-' => -1,
                '=' => -2
            };
        }

        private static string DigitToChar(long digit)
        {
            return digit switch
            {
                0 => "0",
                1 => "1",
                2 => "2",
                3 => "=",
                4 => "-",
            };
        }
    }
}
