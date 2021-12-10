using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AoC2021
{
    class Day10 : Day
    {
        string[] input;

        public override void SetInput(string inputPath)
        {
            using (StreamReader sr = new StreamReader(inputPath))
            {
                input = sr.ReadToEnd().Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        Dictionary<char, char> brackets = new Dictionary<char, char>()
        {
            {'(', ')'},
            {'[', ']'},
            {'{', '}'},
            {'<', '>'}
        };

        Dictionary<char, int> illegalScoreDict = new Dictionary<char, int>()
        {
            {')', 3},
            {']', 57},
            {'}', 1197},
            {'>', 25137}
        };

        // Return: illegal char, or null if line is correct
        char? FindError(string line)
        {
            Stack<char> stack = new Stack<char>();
            foreach (char c in line)
            {
                if (brackets.ContainsKey(c))
                    stack.Push(c);
                else if (stack.Count == 0 || brackets[stack.Pop()] != c)
                    return c;
            }
            return null;
        }

        public override string Solve1()
        {
            int score = 0;
            foreach (string line in input)
            {
                char? c = FindError(line);
                if (c.HasValue)
                    score += illegalScoreDict[c.Value];
            }
            return score.ToString();
        }


        Dictionary<char, int> autocompleteScoreDict = new Dictionary<char, int>()
        {
            {')', 1},
            {']', 2},
            {'}', 3},
            {'>', 4}
        };

        IEnumerable<char> Complete(string line)
        {
            Stack<char> stack = new Stack<char>();
            foreach (char c in line)
            {
                if (brackets.ContainsKey(c))
                    stack.Push(c);
                else if (stack.Count == 0 || brackets[stack.Pop()] != c)
                    return null;
            }

            return stack.Select(b => brackets[b]);
        }

        public override string Solve2()
        {
            List<long> scores = new List<long>();
            foreach (string line in input)
            {
                IEnumerable<char> complete = Complete(line);
                if (complete != null)
                    scores.Add(complete.Aggregate<char, long>(0, (score, c) => score * 5 + autocompleteScoreDict[c]));

            }
            scores.Sort();
            return scores[scores.Count / 2].ToString();
        }
    }
}
