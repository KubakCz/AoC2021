using System;

namespace AoC2021
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = @"C:\Users\jakub\Programming\AoC2021\InputData\example01.txt";
            if (args.Length != 0)
                inputPath = args[0];

            Day d = new Day01(inputPath);
            Console.WriteLine(d.Solve1());
            Console.WriteLine(d.Solve2());
        }
    }
}
