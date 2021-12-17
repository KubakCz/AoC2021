using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC2021
{
    class Day14 : Day
    {
        Dictionary<(char, char), char> inputRules;
        Dictionary<(char, char), long> inputPairs;
        string inputTemplate;

        public override void SetInput(string inputPath)
        {
            using (StreamReader sr = new StreamReader(inputPath))
            {
                inputTemplate = sr.ReadLine();
                if (inputTemplate == null)
                    throw new FileFormatException(inputPath, "Expected polymer template!");
                inputPairs = new Dictionary<(char, char), long>();
                for (int i = 0; i < inputTemplate.Length - 1; ++i)
                {
                    (char, char) ab = (inputTemplate[i], inputTemplate[i + 1]);
                    inputPairs[ab] = inputPairs.GetValueOrDefault(ab, 0) + 1;
                }

                sr.ReadLine();

                inputRules = new Dictionary<(char, char), char>();
                foreach (string line in sr.ReadLines(excludeEmpty: true))
                {
                    string[] pairInsertion = line.Split(" -> ");
                    if (pairInsertion.Length != 2 || pairInsertion[0].Length != 2 || pairInsertion[1].Length != 1)
                        throw new FileFormatException(inputPath, $"'{line}' is not a valid pair insertion!");

                    char la = pairInsertion[0][0], lb = pairInsertion[0][1], r = pairInsertion[1][0];
                    inputRules[(la, lb)] = r;
                }
            }
        }


        Dictionary<(char, char), long> Step(Dictionary<(char, char), long> polymerPairs, Dictionary<(char, char), char> rules)
        {
            Dictionary<(char, char), long> result = new Dictionary<(char, char), long>();

            foreach (var ((a, b), count) in polymerPairs)
            {
                char r = rules[(a, b)];
                (char, char) ar = (a, r), rb = (r, b);
                result[ar] = result.GetValueOrDefault(ar, 0) + count;
                result[rb] = result.GetValueOrDefault(rb, 0) + count;
            }

            return result;
        }

        Dictionary<char, long> ElementCounts(Dictionary<(char, char), long> polymerPairs, string template)
        {
            Dictionary<char, long> result2 = new Dictionary<char, long>();
            foreach (var ((a, b), count) in polymerPairs)
            {
                result2[a] = result2.GetValueOrDefault(a, 0) + count;
                result2[b] = result2.GetValueOrDefault(b, 0) + count;
            }

            Dictionary<char, long> result = new Dictionary<char, long>(result2.Count);
            foreach (var (k, v) in result2)
                result[k] = v / 2;

            ++result[template.First()];
            ++result[template.Last()];

            return result;
        }

        (T, T) MinMax<T>(IEnumerable<T> collection) where T : IComparable<T>
        {
            T min = collection.First();
            T max = collection.First();
            foreach (T elem in collection)
            {
                if (elem.CompareTo(min) < 0)
                    min = elem;
                if (elem.CompareTo(max) > 0)
                    max = elem;
            }
            return (min, max);
        }

        string Solve(int steps)
        {
            Dictionary<(char, char), long> polymerPairs = inputPairs;
            for (int i = 0; i < steps; ++i)
                polymerPairs = Step(polymerPairs, inputRules);

            var elemCounts = ElementCounts(polymerPairs, inputTemplate);
            var (min, max) = MinMax(elemCounts.Values);
            return (max - min).ToString();
        }


        public override string Solve1() => Solve(10);
        public override string Solve2() => Solve(40);
    }
}
