using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AoC2021
{
    class Day09 : Day
    {
        int[,] input; //[x,y]

        public override void SetInput(string inputPath)
        {
            using (StreamReader sr = new StreamReader(inputPath))
            {
                string[] lines = sr.ReadToEnd()
                                   .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                input = new int[lines[0].Length, lines.Length];
                for (int i = 0; i < lines.Length; ++i)
                    for (int j = 0; j < lines[i].Length; ++j)
                        input[j, i] = lines[i][j] - '0';

            }
        }

        List<int> AdjacentVals(int x, int y, int[,] input)
        {
            List<int> adjacent = new List<int>();
            if (x > 0)
                adjacent.Add(input[x - 1, y]);
            if (x + 1 < input.GetLength(0))
                adjacent.Add(input[x + 1, y]);
            if (y > 0)
                adjacent.Add(input[x, y - 1]);
            if (y + 1 < input.GetLength(1))
                adjacent.Add(input[x, y + 1]);
            return adjacent;
        }

        public override string Solve1()
        {
            int riskSum = 0;
            for (int x = 0; x < input.GetLength(0); ++x)
            {
                for (int y = 0; y < input.GetLength(1); ++y)
                {
                    if (AdjacentVals(x, y, input).Min() > input[x, y])
                        riskSum += input[x, y] + 1;
                }
            }
            return riskSum.ToString();
        }

        int[,] GenBasinMap(int[,] heigtMap)
        {
            int[,] map = new int[heigtMap.GetLength(0), heigtMap.GetLength(1)];

            List<int> equivalences = new List<int>() { 0 };

            int index = 0;
            for (int y = 0; y < map.GetLength(1); ++y)
            {
                for (int x = 0; x < map.GetLength(0); ++x)
                {
                    if (heigtMap[x, y] == 9)
                        continue;

                    if (x > 0 && y > 0 && map[x - 1, y] != 0 && map[x, y - 1] != 0 && map[x - 1, y] != map[x, y - 1])
                    {
                        int min = Math.Min(map[x - 1, y], map[x, y - 1]);
                        int max = Math.Max(map[x - 1, y], map[x, y - 1]);
                        map[x, y] = min;
                        equivalences[max] = min;
                    }
                    else if (x > 0 && map[x - 1, y] != 0)
                    {
                        map[x, y] = map[x - 1, y];
                    }
                    else if (y > 0 && map[x, y - 1] != 0)
                    {
                        map[x, y] = map[x, y - 1];
                    }
                    else
                    {
                        map[x, y] = ++index;
                        equivalences.Add(index);
                    }
                }
            }

            for (int i = 0; i < equivalences.Count; ++i)
                equivalences[i] = equivalences[equivalences[i]];

            for (int y = 0; y < map.GetLength(1); ++y)
            {
                for (int x = 0; x < map.GetLength(0); ++x)
                {
                    map[x, y] = equivalences[map[x, y]];
                }
            }

            return map;
        }

        Dictionary<int, int> BasinSizes(int[,] basinMap)
        {
            Dictionary<int, int> sizes = new Dictionary<int, int>();
            foreach (int i in basinMap)
                sizes[i] = sizes.GetValueOrDefault(i, 0) + 1;
            sizes.Remove(0);
            return sizes;
        }
        public override string Solve2()
        {
            int[,] map = GenBasinMap(input);
            Dictionary<int, int> sizes = BasinSizes(map);
            return sizes.Values
                        .OrderBy(i => i)
                        .TakeLast(3)
                        .Aggregate((a, b) => a * b)
                        .ToString();
        }
    }
}
