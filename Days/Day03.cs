using System;
using System.IO;
using System.Collections.Generic;

namespace AoC2021
{
    class Day03 : Day
    {
        string[] input;

        public override void SetInput(string inputPath)
        {
            StreamReader sr = new StreamReader(inputPath);
            input = sr.ReadToEnd()
                .Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)[0..^1];
        }


        /// <returns>
        ///     '1' if 1 is the most common value,
        ///     '0' if 0 is the most common value,
        ///     'e' if 1 and 0 are equally common.
        /// </returns>
        char MostCommonValue(IEnumerable<string> input, int index)
        {
            int ones = 0, zeros = 0;
            foreach (string s in input)
            {
                if (s[index] == '1')
                    ++ones;
                else
                    ++zeros;
            }

            if (ones == zeros)
                return 'e';
            if (ones > zeros)
                return '1';
            return '0';
        }

        public override string Solve1()
        {
            int gama = 0, epsilon = 0;
            for (int i = 0; i < input[0].Length; ++i)
            {
                gama <<= 1;
                epsilon <<= 1;
                char mcv = MostCommonValue(input, i);
                if (mcv == '1')
                    ++gama;
                else if (mcv == '0')
                    ++epsilon;
            }
            return $"{gama} * {epsilon} = {gama * epsilon}";
        }

        public override string Solve2()
        {
            HashSet<string> o2Set = new HashSet<string>(input);
            HashSet<string> co2Set = new HashSet<string>(input);

            // O2
            for (int i = 0; i < input[0].Length && o2Set.Count > 1; ++i)
            {
                char mcv = MostCommonValue(o2Set, i);
                if (mcv == 'e')
                    mcv = '1';
                o2Set.RemoveWhere(s => s[i] != mcv);
            }

            // CO2
            for (int i = 0; i < input[0].Length && co2Set.Count > 1; ++i)
            {
                char mcv = MostCommonValue(co2Set, i);
                if (mcv == 'e')
                    mcv = '1';
                co2Set.RemoveWhere(s => s[i] == mcv);
            }

            int o2Rating = Convert.ToInt32(System.Linq.Enumerable.ToArray<string>(o2Set)[0], 2);
            int co2Rating = Convert.ToInt32(System.Linq.Enumerable.ToArray<string>(co2Set)[0], 2);

            return $"{o2Rating} * {co2Rating} = {o2Rating * co2Rating}";
        }
    }
}
