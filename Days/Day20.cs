using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC2021
{
    class Day20 : Day
    {
        class Image
        {
            HashSet<(int, int)> data = new HashSet<(int, int)>();
            public bool Edge { get; private set; }
            public int Left { get; private set; }
            public int Right { get; private set; }
            public int Top { get; private set; }
            public int Bottom { get; private set; }
            public int LitPixels => data.Count;

            public Image(bool edge)
            {
                Edge = edge;
            }

            public void Add((int, int) pos)
            {
                data.Add(pos);
                if (pos.Item1 < Left)
                    Left = pos.Item1;
                if (pos.Item1 > Right)
                    Right = pos.Item1;
                if (pos.Item2 < Top)
                    Top = pos.Item2;
                if (pos.Item2 > Bottom)
                    Bottom = pos.Item2;
            }

            public bool IsLit((int, int) pos)
            {
                var (x, y) = pos;
                if (x < Left || x > Right || y < Top || y > Bottom)
                    return Edge;
                return data.Contains(pos);
            }

            public override string ToString()
            {
                int width = Right - Left + 4;
                int height = Bottom - Top + 4;
                StringBuilder result = new StringBuilder((width + 1) * height);

                for (int i = Top; i <= Bottom; ++i)
                {
                    for (int j = Left; j <= Right; ++j)
                    {
                        result.Append(IsLit((j, i)) ? '#' : ' ');
                    }
                    result.Append('\n');
                }

                return result.ToString();
            }
        }
        Image inputImage;
        bool[] imageEnhancementAlgorithm;


        public override void SetInput(string inputPath)
        {
            using (StreamReader sr = new StreamReader(inputPath))
            {
                string iea = sr.ReadLine();
                if (iea.Length != (int)Math.Pow(2, 9))
                    throw new FileFormatException(inputPath, $"Length of the image enhancement algorithm must be {imageEnhancementAlgorithm.Length}!");
                imageEnhancementAlgorithm = iea.Select(c => c == '#').ToArray();

                inputImage = new Image(false);
                foreach (var (i, line) in sr.ReadLines(excludeEmpty: true).Enumerate())
                    foreach (var (j, c) in line.Enumerate())
                        if (c == '#')
                            inputImage.Add((j, i));
            }
        }

        IEnumerable<(int, int)> GetNeighbours((int, int) position)
        {
            var (x, y) = position;
            // order matters
            int[] d = new[] { -1, 0, 1 };
            foreach (int dy in d)
                foreach (int dx in d)
                    yield return (x + dx, y + dy);
        }

        int ToInt(IEnumerable<bool> input)
        {
            int result = 0;
            foreach (bool b in input)
            {
                result <<= 1;
                if (b)
                    result += 1;
            }
            return result;
        }

        Image Enhance(Image image, bool[] algorithm)
        {
            Image result = new Image(algorithm[0] ? !image.Edge : image.Edge);
            for (int i = image.Left - 1; i <= image.Right + 1; ++i)
            {
                for (int j = image.Top - 1; j <= image.Bottom + 1; ++j)
                {
                    int index = ToInt(GetNeighbours((i, j)).Select(image.IsLit));
                    if (algorithm[index])
                        result.Add((i, j));
                }
            }
            return result;
        }

        string Solve(int iterations)
        {
            Image img = inputImage;
            for (int i = 0; i < iterations; ++i)
                img = Enhance(img, imageEnhancementAlgorithm);
            return img.LitPixels.ToString();
        }

        public override string Solve1() => Solve(2);

        public override string Solve2() => Solve(50);
    }
}
