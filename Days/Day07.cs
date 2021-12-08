using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AoC2021
{
    class Day07 : Day
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
            Array.Sort(input);
        }

        int CountFuelConst(int target, int[] positions) => positions.Select<int, int>(p => Math.Abs(p - target))
                                                                    .Sum();

        int CountFuelLin(int target, int[] positions)
        {
            int f(int p)
            {
                int diff = Math.Abs(p - target);
                return diff * (diff + 1) / 2;
            }
            return positions.Select<int, int>(f)
                            .Sum();
        }

        // Return (position, fuel)
        (int, int) FindLowest(int[] input, Func<int, int[], int> countFuel)
        {
            int left = 0, right = input.Length;
            while (right - left > 1)
            {
                int mid = (left + right) / 2;
                int midF = countFuel(mid, input);
                int midLeftF = countFuel(mid - 1, input);
                if (midLeftF > midF)
                    left = mid;
                else
                    right = mid;
            }
            return (left, countFuel(left, input));
        }

        public override string Solve1()
        {
            var (min_pos, min_fuel) = FindLowest(input, CountFuelConst);
            return $"Postion: {min_pos}; Fuel: {min_fuel}";
        }

        public override string Solve2()
        {
            var (min_pos, min_fuel) = FindLowest(input, CountFuelLin);
            return $"Postion: {min_pos}; Fuel: {min_fuel}";
        }
    }
}
