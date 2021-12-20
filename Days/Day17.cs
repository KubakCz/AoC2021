using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AoC2021
{
    class Day17 : Day
    {
        int xMin, xMax,
            yMin, yMax;

        public override void SetInput(string inputPath)
        {
            using (StreamReader sr = new StreamReader(inputPath))
            {
                string line = sr.ReadLine();
                string[] segments = line.Split(new[] { ": ", ", " }, StringSplitOptions.None);
                if (segments.Length != 3)
                    throw new FileFormatException(inputPath, $"'{line}' is not a valid input!");

                string[] x = segments[1].Split(new[] { "=", ".." }, StringSplitOptions.None);
                string[] y = segments[2].Split(new[] { "=", ".." }, StringSplitOptions.None);
                if (segments.Length != 3 || x[0] != "x" || y[0] != "y"
                        || !int.TryParse(x[1], out xMin) || !int.TryParse(x[2], out xMax)
                        || xMin > xMax
                        || !int.TryParse(y[1], out yMin) || !int.TryParse(y[2], out yMax)
                        || yMin > yMax)
                    throw new FileFormatException(inputPath, $"'{line}' is not a valid input!");
            }
        }

        // v_x, so probe stops after targetXMin, but as close as possible
        //
        // v_x (v_x + 1)                           -1 + sqrt(1 + 8 targetXMin)
        // ------------- >= targetXMin  =>  v_x >= ---------------------------
        //      2                                              2
        int minVx(int targetXMin) => (int)Math.Ceiling((-1 + Math.Sqrt(1 + 8 * targetXMin)) / 2);

        // if v_y > 0, probe will go through y = 0 and velocity will be updated to -v_y - 1 
        // therefore, if targetYMin < 0, v_y >= -targetYMin - 1 will always miss
        int maxVy(int targetYMin) => -targetYMin - 1;

        // return all steps, where x will by inside of [xMin, xMax] interval.
        IEnumerable<int> StepsXInRange(int v_x, int xMin, int xMax)
        {
            if (v_x < Math.Sqrt(2 * xMin) - 1.0 / 2.0)
                yield break;

            int tmp = 2 * v_x + 1;
            for (int t = (int)Math.Ceiling((tmp - Math.Sqrt(tmp * tmp - 8 * xMin)) / 2); posX(v_x, t) <= xMax; ++t)
                yield return t;
        }

        // position (initial velocity, number of steps)
        int posX(int v_x, int t) => t < v_x ? t * (2 * v_x - t + 1) / 2 : v_x * (v_x + 1) / 2;
        int posY(int v_y, int t) => t * (2 * v_y - t + 1) / 2;

        public override string Solve1()
        {
            if (xMin < 0 || yMin >= 0)
                throw new NotImplementedException("This case is not implemented!");

            int v_x = minVx(xMin);
            int v_y = maxVy(yMin);
            if (v_x > xMax || v_y < v_x)
                throw new NotImplementedException("This case is not implemented!");

            return $"v = ({v_x}, {v_y}) => max y = {v_y * (v_y + 1) / 2}";
        }

        public override string Solve2()
        {
            if (xMin < 0 || yMin >= 0)
                throw new NotImplementedException("This case is not implemented!");

            int minV_x = minVx(xMin), maxV_x = xMax;
            int maxV_y = maxVy(yMin), minV_y = yMin;

            int counter = 0;
            for (int v_x = minV_x; v_x <= maxV_x; ++v_x)
            {
                HashSet<int> set = new HashSet<int>();
                foreach (int t in StepsXInRange(v_x, xMin, xMax))
                {
                    if (posY(maxV_y, t) < yMin)
                        break;
                    for (int v_y = minV_y; v_y <= maxV_y; ++v_y)
                    {
                        int y = posY(v_y, t);
                        if (y >= yMin && y <= yMax)
                            set.Add(v_y);
                    }
                }
                counter += set.Count;
            }

            return counter.ToString();
        }
    }
}
