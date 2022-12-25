using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day25
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2022, day 25");
            var input = ReadInput("input.txt");
            var s = input.Sum();
            Console.WriteLine($"{Int2Snafu(s)}");
        }

        private static string Int2Snafu(long n)
        {
            if (n > -3 && n < 3)
            {
                return DigitToChar(n);
            }

            var p = 1;
            var pHalf = Power(p) / 2;
            while (n < -pHalf || n > pHalf)
            {
                p++;
                pHalf = Power(p) / 2;
            }

            var n1 = n + pHalf;
            var digit = n1 / Power(p - 1) - 2;
            var rest = n - digit * Power(p - 1);
            var result = DigitToChar(digit);

            p-=2;
            if (p > 0)
            {
                pHalf = Power(p) / 2;
                while (-pHalf <= rest && rest <= pHalf)
                {
                    result += '0';
                    p--;
                    if (p == 0)
                    {
                        break;
                    }
                    pHalf = Power(p) / 2;
                }
            }
            return result + Int2Snafu(rest);
        }

        private static List<long> ReadInput(string fileName)
        {
            var lines = File.ReadAllLines(fileName);
            return lines
                .Select(Snafu2Int)
                .ToList();
        }

        private static long Snafu2Int(string line)
        {
            var result = 0L;
            var nn = 1L;
            for (var i = 0; i < line.Length; i++)
            {
                var ch = line[line.Length - i - 1];

                var n = CharToDigit(ch);
                result += n * nn;
                nn *= 5;
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
                -1 => "-",
                -2 => "=",
            };
        }

        private static Dictionary<long, long> cache = new Dictionary<long, long>();
        private static long Power(long d)
        {
            if (cache.ContainsKey(d))
            {
                return cache[d];
            }

            if (d == 0)
            {
                cache[0] = 1;
                return 1L;
            }
            else
            {
                var res = 5 * Power(d - 1);
                cache[d] = res;
                return res;
            }
        }
    }
}
