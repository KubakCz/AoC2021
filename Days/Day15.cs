using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AoC2021
{
    class Day15 : Day
    {
        int[,] inputMap;

        public override void SetInput(string inputPath)
        {
            using (StreamReader sr = new StreamReader(inputPath))
            {
                List<string> lines = sr.ReadLines().ToList();

                inputMap = new int[lines[0].Length, lines.Count];
                foreach (var (i, line) in lines.Enumerate())
                {
                    if (line.Length != inputMap.GetLength(0))
                        throw new FileFormatException(inputPath, "All lines must have the same length!");

                    foreach (var (j, c) in line.Enumerate())
                    {
                        if (!int.TryParse(c.ToString(), out inputMap[j, i]))
                            throw new FileFormatException(inputPath, $"'{c}' is not a valid number!");
                    }
                }
            }
        }

        IEnumerable<(int, int)> Neighbours((int, int) mapSize, (int, int) position)
        {
            var (x, y) = position;
            var (xMax, yMax) = mapSize;

            if (x > 0)
                yield return (x - 1, y);
            if (x < xMax - 1)
                yield return (x + 1, y);
            if (y > 0)
                yield return (x, y - 1);
            if (y < yMax - 1)
                yield return (x, y + 1);
        }

        int FindRisk(Func<(int, int), int> getRisk, (int, int) mapSize, (int, int) start, (int, int) goal)
        {
            int h((int, int) pos) => Math.Abs(goal.Item1 - pos.Item1) + Math.Abs(goal.Item2 - pos.Item2);

            PriorityQueue<(int, int), int> q = new PriorityQueue<(int, int), int>();
            q.Enqueue(start, h(start));

            Dictionary<(int, int), int> risk = new Dictionary<(int, int), int>();
            risk[start] = 0;

            while (q.Count > 0)
            {
                var current = q.Dequeue();
                if (current == goal)
                {
                    return risk[current];
                }


                foreach (var (x, y) in Neighbours(mapSize, current))
                {
                    int newRisk = risk[current] + getRisk((x, y));
                    if (risk.GetValueOrDefault((x, y), int.MaxValue) > newRisk)
                    {
                        risk[(x, y)] = newRisk;
                        q.Enqueue((x, y), newRisk + h((x, y)));
                    }
                }
            }

            throw new ArgumentException("Goal can't be reached!");
        }

        (int, int) MapSize(int[,] map) => (map.GetLength(0), map.GetLength(1));

        public override string Solve1()
        {
            var size = MapSize(inputMap);
            int getRisk((int, int) pos) => inputMap[pos.Item1, pos.Item2];

            return FindRisk(getRisk, size, (0, 0), (size.Item1 - 1, size.Item2 - 1)).ToString();
        }


        public override string Solve2()
        {
            var (tileWidth, tileHeight) = MapSize(inputMap);
            int width = 5 * tileWidth, height = 5 * tileHeight;

            int getRisk((int, int) pos)
            {
                var (x, y) = pos;
                int risk = inputMap[x % tileWidth, y % tileHeight];
                risk += x / tileWidth + y / tileHeight;
                return ((risk - 1) % 9) + 1;
            }

            return FindRisk(getRisk, (width, height), (0, 0), (width - 1, height - 1)).ToString();
        }
    }
}
