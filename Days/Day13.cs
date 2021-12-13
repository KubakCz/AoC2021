using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC2021
{
    class Day13 : Day
    {
        List<(int, int)> inputPoints;
        List<(char, int)> inputFolds;

        public override void SetInput(string inputPath)
        {
            using (StreamReader sr = new StreamReader(inputPath))
            {
                inputPoints = new List<(int, int)>();
                inputFolds = new List<(char, int)>();

                IEnumerator<string> lines = sr.ReadLines().GetEnumerator();

                while (lines.MoveNext() && lines.Current != "")
                {
                    string[] tmp = lines.Current.Split(',');
                    if (tmp.Length != 2)
                        throw new FileFormatException(inputPath, $"'{lines.Current}' is not a valid point!");
                    int a, b;
                    if (!int.TryParse(tmp[0], out a))
                        throw new FileFormatException(inputPath, $"'{tmp[0]}' is not valid number!");
                    if (!int.TryParse(tmp[1], out b))
                        throw new FileFormatException(inputPath, $"'{tmp[1]}' is not valid number!");
                    inputPoints.Add((a, b));
                }
                while (lines.MoveNext() && lines.Current != "")
                {
                    string[] tmp = lines.Current.Split('=');
                    if (tmp.Length != 2)
                        throw new FileFormatException(inputPath, $"'{lines.Current}' is not a valid fold!");
                    char c = tmp[0].Last();
                    if (c != 'y' && c != 'x')
                        throw new FileFormatException(inputPath, $"'{c}' is not a valid axis!");
                    int a;
                    if (!int.TryParse(tmp[1], out a))
                        throw new FileFormatException(inputPath, $"'{tmp[1]}' is not a valid number!");
                    inputFolds.Add((c, a));
                }
            }
        }


        (int, int) foldTransform((int, int) point, char axis, int foldCoordinate)
        {
            var (x, y) = point;
            if (axis == x)
            {
                if (x > foldCoordinate)
                    return (2 * (x - foldCoordinate), y);
                return point;
            }
            else
            {
                if (y > foldCoordinate)
                    return (x, 2 * (y - foldCoordinate));
                return point;
            }
        }

        IEnumerable<(int, int)> Fold(IEnumerable<(int, int)> points, (char, int) fold)
        {
            var (foldAxis, foldCoordinate) = fold;

            (int, int) foldTransform((int, int) point)
            {
                var (x, y) = point;
                if (foldAxis == 'x')
                {
                    if (x > foldCoordinate)
                        return (2 * foldCoordinate - x, y);
                    return point;
                }
                else
                {
                    if (y > foldCoordinate)
                        return (x, 2 * foldCoordinate - y);
                    return point;
                }
            }

            return points.Select(foldTransform).Distinct();
        }

        public override string Solve1()
        {
            return Fold(inputPoints, inputFolds[0]).Count().ToString();
        }

        string Render(HashSet<(int, int)> points)
        {
            int xMax = 0, yMax = 0;
            foreach (var (x, y) in points)
            {
                xMax = Math.Max(xMax, x);
                yMax = Math.Max(yMax, y);
            }

            StringBuilder sb = new StringBuilder();
            for (int y = 0; y < yMax + 1; ++y)
            {
                for (int x = 0; x < xMax + 1; ++x)
                {
                    sb.Append(points.Contains((x, y)) ? '#' : ' ');
                }
                sb.Append('\n');
            }
            return sb.ToString();
        }

        public override string Solve2()
        {
            IEnumerable<(int, int)> points = inputPoints;
            foreach (var fold in inputFolds)
                points = Fold(points, fold);
            return "\n" + Render(points.ToHashSet());
        }
    }
}
