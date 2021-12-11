using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AoC2021
{
    class Day11 : Day
    {
        int[,] inputEnergyLevels;

        public override void SetInput(string inputPath)
        {
            using (StreamReader sr = new StreamReader(inputPath))
            {
                string[] lines = sr.ReadToEnd()
                                   .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                int width = lines[0].Length, height = lines.Length;

                inputEnergyLevels = new int[width, height];
                for (int row = 0; row < height; ++row)
                {
                    if (lines[row].Length != width)
                        throw new FileFormatException(Path.GetFullPath(inputPath), "All lines must have the same length!");
                    for (int col = 0; col < width; ++col)
                    {
                        if (!int.TryParse(lines[row][col].ToString(), out inputEnergyLevels[col, row]))
                            throw new FileFormatException(Path.GetFullPath(inputPath), "Only characters 0-9 are allowed!");
                    }
                }
            }
        }

        IEnumerable<(int, int)> Adjacent(int x, int y, int width, int height)
        {
            if (width == 0 || height == 0)
                yield break;

            if (x > 0)
                yield return (x - 1, y);
            if (x > 0 && y > 0)
                yield return (x - 1, y - 1);
            if (y > 0)
                yield return (x, y - 1);
            if (y > 0 && x + 1 < width)
                yield return (x + 1, y - 1);
            if (x + 1 < width)
                yield return (x + 1, y);
            if (x + 1 < width && y + 1 < height)
                yield return (x + 1, y + 1);
            if (y + 1 < height)
                yield return (x, y + 1);
            if (y + 1 < height && x > 0)
                yield return (x - 1, y + 1);
        }

        // Return: number of flashes
        int Step(int[,] energyLevels)
        {
            int width = energyLevels.GetLength(0), height = energyLevels.GetLength(1);

            // +1 energy
            for (int i = 0; i < width; ++i)
                for (int j = 0; j < height; ++j)
                    ++energyLevels[i, j];

            // energy from adjacent flashes
            bool[,] flashed = new bool[width, height];
            bool change = true;
            while (change)
            {
                change = false;
                for (int i = 0; i < width; ++i)
                {
                    for (int j = 0; j < height; ++j)
                    {
                        if (energyLevels[i, j] > 9 && !flashed[i, j])
                        {
                            change = true;
                            flashed[i, j] = true;
                            foreach (var (x, y) in Adjacent(i, j, width, height))
                                ++energyLevels[x, y];
                        }
                    }
                }
            }

            // energy of flashed to 0
            for (int i = 0; i < width; ++i)
                for (int j = 0; j < height; ++j)
                    if (energyLevels[i, j] > 9)
                        energyLevels[i, j] = 0;

            // count flashes
            int counter = 0;
            foreach (bool f in flashed)
                if (f)
                    ++counter;

            return counter;
        }

        public override string Solve1()
        {
            int[,] energyLevels = (int[,])inputEnergyLevels.Clone();

            int counter = 0;
            for (int i = 0; i < 100; ++i)
            {
                counter += Step(energyLevels);
            }

            return counter.ToString();
        }

        public override string Solve2()
        {
            int[,] energyLevels = (int[,])inputEnergyLevels.Clone();
            int octopusCount = energyLevels.Length;

            int counter = 1;
            while (Step(energyLevels) != octopusCount)
                ++counter;

            return counter.ToString();
        }
    }
}
