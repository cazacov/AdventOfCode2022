using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Threading;

namespace Day10
{
    /// <summary>
    /// Control Flip-Dot display over REST API
    /// https://github.com/cazacov/FlipDot/tree/master/webBoard
    /// </summary>
    class FlipDotDisplay
    {
        private const string BaseUrl = "http://192.168.178.61/";
        private const int DisplayWidth = 140;
        private const int DisplayHeight = 19;

        private const int InputWidth = 40;
        private const int InputHeight = 6;


        public async Task Init()
        {
            using var client = CreateClient();

            var request = new
            {
                line1 = "     Advent of Code 2022",
                line2 = "  Day 10: Cathode-Ray Tube"
            };
            var content = new StringContent(
                Newtonsoft.Json.JsonConvert.SerializeObject(request),
                Encoding.UTF8, "application/json"
            );
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var result = await client.PostAsync("textSmall", content);
        }

        public async Task ShowPuzzle1(int strength)
        {
            using var client = CreateClient();

            var request = new
            {
                line1 = "                Puzzle 1",
                line2 = $"   Signal strength: {strength}"
            };
            var content = new StringContent(
                Newtonsoft.Json.JsonConvert.SerializeObject(request),
                Encoding.UTF8, "application/json"
            );
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var result = await client.PostAsync("textSmall", content);
        }

        public async Task ShowPuzzle2(bool [,] pixels)
        {
            using var client = CreateClient();

            var request = new
            {
                text = "       Puzzle 2",
            };
            var content = new StringContent(
                Newtonsoft.Json.JsonConvert.SerializeObject(request),
                Encoding.UTF8, "application/json"
            );
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var result = await client.PostAsync("textBig", content);

            Thread.Sleep(3);

            for (var y = 0; y < InputHeight; y++)
            {
                for (var x = 0; x < InputWidth; x++)
                {
                    var buffer = new bool[DisplayHeight, DisplayWidth];
                    var yy = y * 3 + 1;
                    var xx = 10 + x * 3 + 1;
                    if (y != InputHeight - 1 || x != InputWidth - 1)
                    {
                        HLine(yy, ref buffer);
                        VLine(xx, ref buffer);
                    }

                    for (var k = 0; k <= y; k++)
                    {
                        for (var l = 0; l <= ((k == y) ? x : InputWidth - 1); l++)
                        {
                            if (pixels[k, l])
                            {
                                for (var m = -1; m <= 1; m++)
                                {
                                    for (var n = -1; n <= 1; n++)
                                    {
                                        buffer[k * 3 + 1 + m, 10 + l * 3 + 1 + n] = true;
                                    }
                                }
                            }
                        }
                    }
                    await ShowFrame(buffer);
                    Thread.Sleep(20);
                }
            }

        }

        private async Task ShowFrame(bool[,] buffer)
        {
            var bytesInRow = (DisplayWidth + 7) / 8;    // 8 bits per pixel, rounded up
            var bits = new byte[DisplayHeight * bytesInRow];

            for (var y = 0; y < DisplayHeight; y++)
            {
                for (var x = 0; x < DisplayWidth; x++)
                {
                    if (buffer[y, x])
                    {
                        bits[bytesInRow * y + x / 8] |= (byte) (0x80 >> (x & 7));
                    }
                }
            }
            
            using var client = CreateClient();
            var request = new
            {
                frameBuffer = Convert.ToBase64String(bits),
            };
            var content = new StringContent(
                Newtonsoft.Json.JsonConvert.SerializeObject(request),
                Encoding.UTF8, "application/json"
            );
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var result = await client.PostAsync("data", content);
        }

        private void HLine(int y, ref bool[,] buffer)
        {
            for (var i = 0; i < DisplayWidth; i++)
            {
                buffer[y, i] = true;
            }
        }

        private void VLine(int x, ref bool[,] buffer)
        {
            for (var i = 0; i < DisplayHeight; i++)
            {
                buffer[i, x] = true;
            }
        }

        private HttpClient CreateClient()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(BaseUrl)
            };
            return client;
        }
    }
}
