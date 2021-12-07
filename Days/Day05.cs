using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AoC2021
{
    class Day05 : Day
    {
        struct Point
        {
            public int x, y;
            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public static bool operator ==(Point a, Point b) => a.x == b.x && a.y == b.y;
            public static bool operator !=(Point a, Point b) => !(a == b);
        }

        struct Line
        {
            public Point a, b;

            public Line(Point a, Point b)
            {
                this.a = a;
                this.b = b;
            }

            public bool IsHorizontal() => a.y == b.y;
            public bool IsVertical() => a.x == b.x;

            public int Left() => Math.Min(a.x, b.x);
            public int Right() => Math.Max(a.x, b.x);
            public int Top() => Math.Min(a.y, b.y);
            public int Bottom() => Math.Max(a.y, b.y);

            public void Render(int[,] canvas)
            {
                int stepX = Math.Sign(b.x - a.x);
                int stepY = Math.Sign(b.y - a.y);

                Point p = a;
                ++canvas[p.x, p.y];
                while (p != b)
                {
                    p.x += stepX;
                    p.y += stepY;
                    ++canvas[p.x, p.y];
                }
            }
        }

        Line[] input;

        public override void SetInput(string inputPath)
        {
            StreamReader sr = new StreamReader(inputPath);
            string[] lines = sr.ReadToEnd().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            input = lines.Select<string, Line>(LineParser)
                         .ToArray<Line>();
        }

        static Line LineParser(string line)
        {
            List<int> coords = line.Split(new[] { " -> ", "," }, StringSplitOptions.None)
                               .Select<string, int>(s => int.Parse(s))
                               .ToList<int>();
            return new Line(new Point(coords[0], coords[1]), new Point(coords[2], coords[3]));
        }

        static Point MaxCorner(Line[] lines)
        {
            Point corner = new Point(0, 0);
            foreach (Line line in lines)
            {
                corner.x = Math.Max(line.Right(), corner.x);
                corner.y = Math.Max(line.Bottom(), corner.y);
            }
            return corner;
        }

        static void PrintCanvas(int[,] canvas)
        {
            for (int y = 0; y < canvas.GetLength(0); ++y)
            {
                for (int x = 0; x < canvas.GetLength(1); ++x)
                {
                    Console.Write(canvas[x, y] > 0 ? $"{canvas[x, y],3}" : "  .");
                }
                Console.WriteLine();
            }
        }

        string Solve(Func<Line, bool> lineFilter)
        {
            Point corner = MaxCorner(input);
            int[,] canvas = new int[corner.x + 1, corner.y + 1];

            foreach (Line line in input.Where<Line>(lineFilter))
                line.Render(canvas);

            int counter = 0;
            foreach (int i in canvas)
                if (i > 1)
                    ++counter;

            return counter.ToString();
        }

        public override string Solve1() => Solve(l => l.IsVertical() || l.IsHorizontal());

        public override string Solve2() => Solve(l => true);
    }
}
