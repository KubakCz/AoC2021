using System;
using System.IO;
using System.Collections;

namespace AoC2021
{
    class Day02 : Day
    {
        (char, int)[] input;

        public override void SetInput(string inputPath)
        {
            StreamReader sr = new StreamReader(inputPath);
            string[] lines = sr.ReadToEnd().Split('\n');
            input = new (char, int)[lines.Length - 1];    // exclude last empty line
            for (int i = 0; i < input.Length; ++i)
            {
                string[] tmp = lines[i].Split(' ');
                input[i] = (tmp[0][0], int.Parse(tmp[1]));
            }
        }

        public override string Solve1()
        {
            int horizontal = 0, depth = 0;
            foreach (var (c, v) in input)
            {
                switch (c)
                {
                    case 'u':
                        depth -= v;
                        break;
                    case 'd':
                        depth += v;
                        break;
                    case 'f':
                        horizontal += v;
                        break;
                }
            }
            return $"{horizontal} * {depth} = {horizontal * depth}";
        }

        public override string Solve2()
        {
            int horizontal = 0, depth = 0, aim = 0;
            foreach (var (c, v) in input)
            {
                switch (c)
                {
                    case 'u':
                        aim -= v;
                        break;
                    case 'd':
                        aim += v;
                        break;
                    case 'f':
                        horizontal += v;
                        depth += aim * v;
                        break;
                }
            }
            return $"{horizontal} * {depth} = {horizontal * depth}";
        }
    }
}
