using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AoC2021
{
    class Day18 : Day
    {
        class Number
        {
            int? value;
            Number l;
            Number r;

            bool IsRegular => value.HasValue;

            public int Magnitude => IsRegular ? value.Value : 3 * l.Magnitude + 2 * r.Magnitude;

            public Number(int val)
            {
                value = val;
            }
            public Number(Number l, Number r)
            {
                this.l = l;
                this.r = r;
            }
            public Number(Number n)
            {
                if (n == null)
                    throw new NullReferenceException();

                if (n.IsRegular)
                    value = n.value;
                else
                {
                    l = new Number(n.l);
                    r = new Number(n.r);
                }
            }

            public void Reduce()
            {
                do
                {
                    Explode(0);
                } while (Split().Item1);
            }

            (int?, int?) Explode(int depth)
            {
                if (IsRegular)
                    return (null, null);

                if (depth == 4)
                    return (l.value, r.value);

                var (ll, lr) = l.Explode(depth + 1);
                if (ll.HasValue && depth == 3)
                    l = new Number(0);
                if (lr.HasValue)
                    r.AddLeft(lr.Value);

                var (rl, rr) = r.Explode(depth + 1);
                if (rl.HasValue && depth == 3)
                    r = new Number(0);
                if (rl.HasValue)
                    l.AddRight(rl.Value);

                return (ll, rr);
            }

            void AddLeft(int n)
            {
                if (IsRegular)
                    value += n;
                else
                    l.AddLeft(n);
            }
            void AddRight(int n)
            {
                if (IsRegular)
                    value += n;
                else
                    r.AddRight(n);
            }

            (bool, Number) Split()
            {
                if (IsRegular)
                {
                    if (value > 9)
                    {
                        Number newL = new Number(value.Value / 2),
                               newR = new Number((int)Math.Ceiling(value.Value / 2f));
                        return (true, new Number(newL, newR));
                    }
                    return (false, this);
                }

                (bool split, l) = l.Split();
                if (split) return (true, this);

                (split, r) = r.Split();
                return (split, this);
            }

            public static Number operator +(Number l, Number r)
            {
                Number n = new Number(new Number(l), new Number(r));
                n.Reduce();
                return n;
            }

            public override string ToString() => IsRegular ? value.Value.ToString() : $"[{l.ToString()},{r.ToString()}]";
        }

        Number[] inputNumbers;

        public override void SetInput(string inputPath)
        {
            using (StreamReader sr = new StreamReader(inputPath))
            {
                try
                {
                    inputNumbers = sr.ReadLines(excludeEmpty: true)
                                     .Select(ParseNumber)
                                     .ToArray();
                }
                catch (ArgumentException e)
                {
                    throw new FileFormatException(inputPath, e.Message, e);
                }
            }
        }

        Number ParseNumber(string number)
        {
            Stack<Number> stack = new Stack<Number>();
            int depth = 0;
            foreach (char c in number)
            {
                if (c == '[' && ++depth > 4)
                    throw new ArgumentException($"'{number}' is not a valid snailfish number.");
                else if (char.IsDigit(c))
                    stack.Push(new Number(c - '0'));
                else if (c == ']')
                {
                    if (--depth < 0 || stack.Count < 2)
                        throw new ArgumentException($"'{number}' is not a valid snailfish number.");
                    Number r = stack.Pop(), l = stack.Pop();
                    stack.Push(new Number(l, r));
                }
            }

            if (stack.Count == 0)
                throw new ArgumentException($"'{number}' is not a valid snailfish number.");
            return stack.Pop();
        }

        public override string Solve1()
        {
            Number n = new Number(inputNumbers[0]);
            for (int i = 1; i < inputNumbers.Length; ++i)
                n += inputNumbers[i];

            return $"{n}.Magnitude: {n.Magnitude}";
        }

        public override string Solve2()
        {
            int maxMag = 0;
            for (int i = 0; i < inputNumbers.Length; ++i)
            {
                for (int j = 0; j < inputNumbers.Length; ++j)
                {
                    if (i != j)
                        maxMag = Math.Max((inputNumbers[i] + inputNumbers[j]).Magnitude, maxMag);
                }
            }
            return maxMag.ToString();
        }
    }
}
