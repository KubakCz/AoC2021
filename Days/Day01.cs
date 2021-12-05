using System;
using System.IO;
using System.Collections;

namespace AoC2021
{
    class Day01 : Day
    {
        int[] input;

        public override void SetInput(string inputPath)
        {
            StreamReader sr = new StreamReader(inputPath);
            string[] lines = sr.ReadToEnd().Split('\n');
            input = new int[lines.Length - 1];    // exclude last empty line
            for (int i = 0; i < input.Length; ++i)
                input[i] = int.Parse(lines[i]);
        }

        public override string Solve1()
        {
            int counter = 0;
            for (int i = 1; i < input.Length; ++i)
                if (input[i] > input[i - 1])
                    ++counter;
            return counter.ToString();
        }

        public override string Solve2()
        {
            int counter = 0;
            int window = input[0] + input[1] + input[2];
            for (int i = 3; i < input.Length; ++i)
            {
                int newWindow = window - input[i - 3] + input[i];
                if (newWindow > window)
                    ++counter;
                window = newWindow;
            }
            return counter.ToString();
        }
    }
}
