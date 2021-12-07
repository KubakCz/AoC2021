using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AoC2021
{
    class Day06 : Day
    {
        int[] input;

        public override void SetInput(string inputPath)
        {
            using (StreamReader sr = new StreamReader(inputPath))
            {
                input = sr.ReadLine().Split(',')
                                     .Select<string, int>(s => int.Parse(s))
                                     .ToArray<int>();
            }
        }


        void Simulate(ulong[] fishCounts)
        {
            ulong count0 = fishCounts[0];
            for (int i = 1; i < fishCounts.Length; ++i)
                fishCounts[i - 1] = fishCounts[i];
            fishCounts[6] += count0;
            fishCounts[8] = count0;
        }

        string Solve(int days)
        {
            // Counts of fish of the state 'index'
            ulong[] fishCounts = new ulong[9];
            foreach (int state in input)
                ++fishCounts[state];

            for (int i = 0; i < days; ++i)
                Simulate(fishCounts);

            ulong total = 0;
            foreach (ulong i in fishCounts)
                total += i;
            return total.ToString();
        }

        public override string Solve1() => Solve(80);

        public override string Solve2() => Solve(256);
    }
}
