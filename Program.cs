using System;
using CommandLine;
using System.Collections.Generic;

namespace AoC2021
{
    class Program
    {
        public class Options
        {
            [Option('d', "day",
                Default = 0U,
                HelpText = "Get solutions of this day. If 0, then all days will be solved.")]
            public uint Day { get; set; }

            [Option("directory",
                Default = @"InputData\",
                HelpText = "Directory with inputs.")]
            public string InputDirectory { get; set; }

            [Option('e', "example",
                HelpText = "Solve example input.")]
            public bool Example { get; set; }
        }

        static Day[] days = { new Day01(), new Day02(), new Day03(), new Day04(), new Day05(), new Day06(), new Day07(), new Day08() };

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(RunOptions);
        }

        static void RunOptions(Options o)
        {
            if (o.Day == 0)
                for (uint i = 1U; i <= days.Length; ++i)
                {
                    Console.WriteLine($"Day {i,2:00}");
                    SolveDay(i, GetFileName(i, o.InputDirectory, o.Example));
                    Console.WriteLine();
                }
            else
            {
                Console.WriteLine($"Day {o.Day,2:00}");
                SolveDay(o.Day, GetFileName(o.Day, o.InputDirectory, o.Example));
                Console.WriteLine();
            }
        }

        static string GetFileName(uint i, string inputDirectory, bool example)
            => $"{inputDirectory}{(example ? "example" : "input")}{i,2:00}.txt";

        static void SolveDay(uint i, string inputPath)
        {
            if (i == 0 || i > days.Length)
                throw new ArgumentOutOfRangeException($"{i} is not valid day - use value from 0 to {days.Length}.");

            Day day = days[i - 1];
            day.SetInput(inputPath);
            Console.WriteLine("Problem 1: " + day.Solve1());
            Console.WriteLine("Problem 2: " + day.Solve2());
        }
    }
}
