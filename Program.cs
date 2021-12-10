using System;
using CommandLine;
using System.Reflection;

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

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                          .WithParsed<Options>(RunOptions);
        }

        static void RunOptions(Options o)
        {
            if (o.Day == 0)
                for (uint i = 1U; i <= 25; ++i)
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
            try
            {
                Day day;
                day = GetDay(i);
                day.SetInput(inputPath);
                Console.WriteLine("Part 1: " + day.Solve1());
                Console.WriteLine("Part 2: " + day.Solve2());
            }
            catch (TypeLoadException)
            {
                Console.WriteLine($"Day {i,2:00} not implemented!");
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine($"Input file '{inputPath}' not found!");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception occured: {e.ToString()}");
            }
        }

        static Day GetDay(uint i) => (Day)Activator.CreateInstance(null, $"AoC2021.Day{i,2:00}").Unwrap();
    }
}
