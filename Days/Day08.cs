using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AoC2021
{
    class Day08 : Day
    {
        (HashSet<char>[], HashSet<char>[])[] input;

        public override void SetInput(string inputPath)
        {
            using (StreamReader sr = new StreamReader(inputPath))
            {
                string[] lines = sr.ReadToEnd().Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                input = new (HashSet<char>[], HashSet<char>[])[lines.Length];
                for (int i = 0; i < lines.Length; ++i)
                {
                    string[] parts = lines[i].Split(" | ");
                    input[i] = (parts[0].Split(' ')
                                        .Select<string, HashSet<char>>(s => new HashSet<char>(s))
                                        .ToArray(),
                                parts[1].Split(' ')
                                        .Select<string, HashSet<char>>(s => new HashSet<char>(s))
                                        .ToArray());
                }
            }
        }

        public override string Solve1()
        {
            int counter = 0;
            foreach (var (usp, dov) in input)
                foreach (HashSet<char> digit in dov)
                    if (digit.Count <= 4 || digit.Count == 7)   // 1, 4, 7, 8
                        ++counter;
            return counter.ToString();
        }

        // Key: length
        // Value: List of Sets of chars of codes
        public Dictionary<int, List<HashSet<char>>> GetLenghtDict(HashSet<char>[] usp)
        {
            Dictionary<int, List<HashSet<char>>> lengthDict = new Dictionary<int, List<HashSet<char>>>();
            for (int i = 2; i <= 8; ++i)
                lengthDict[i] = new List<HashSet<char>>();
            foreach (var digit in usp)
                lengthDict[digit.Count].Add(digit);
            return lengthDict;
        }

        // Key: real segment
        // Value: given segment
        Dictionary<char, char> GetSegmentMap(HashSet<char>[] usp)
        {
            Dictionary<int, List<HashSet<char>>> lengthDict = GetLenghtDict(usp);
            Dictionary<char, char> map = new Dictionary<char, char>();

            //         a = 7 {acf}          -      1 {cf}
            List<char> a = lengthDict[3][0].Except(lengthDict[2][0]).ToList();
            if (a.Count != 1)
                throw new Exception();
            map['a'] = a[0];

            //           {abfg} = 0 & 6 & 9
            HashSet<char> abfg = lengthDict[6].Aggregate((d1, d2) => d1.Intersect(d2).ToHashSet()).ToHashSet();

            //         c = 1 {cf}           -      {abfg}
            List<char> c = lengthDict[2][0].Except(abfg).ToList();
            if (c.Count != 1)
                throw new Exception();
            map['c'] = c[0];

            //         f = 7 {acf}          -      {ac}
            List<char> f = lengthDict[3][0].Except(map.Values).ToList();
            if (f.Count != 1)
                throw new Exception();
            map['f'] = f[0];

            //           {adg} = 2 & 3 & 5
            HashSet<char> adg = lengthDict[5].Aggregate((d1, d2) => d1.Intersect(d2).ToHashSet()).ToHashSet();

            //         d = 4 {bcdf}         & {adg}
            List<char> d = lengthDict[4][0].Intersect(adg).ToList();
            if (d.Count != 1)
                throw new Exception();
            map['d'] = d[0];

            //         b = 4 {bcdf}         - {acdf}
            List<char> b = lengthDict[4][0].Except(map.Values).ToList();
            if (b.Count != 1)
                throw new Exception();
            map['b'] = b[0];

            //         g = {adg} - {abcdf}
            List<char> g = adg.Except(map.Values).ToList();
            if (g.Count != 1)
                throw new Exception();
            map['g'] = g[0];

            //         e = 8 {abcdefg}      - {abcdfg}
            List<char> e = lengthDict[7][0].Except(map.Values).ToList();
            if (e.Count != 1)
                throw new Exception();
            map['e'] = e[0];

            return map;
        }

        int Decode(HashSet<char>[] usp, HashSet<char>[] dov)
        {
            Dictionary<char, char> map = GetSegmentMap(usp);
            int result = 0;
            foreach (var digit in dov)
            {
                result *= 10;
                switch (digit.Count)
                {
                    case 2:
                        result += 1;
                        break;
                    case 3:
                        result += 7;
                        break;
                    case 4:
                        result += 4;
                        break;
                    case 5:
                        if (digit.Contains(map['b']))
                            result += 5;
                        else if (digit.Contains(map['e']))
                            result += 2;
                        else
                            result += 3;
                        break;
                    case 6:
                        if (!digit.Contains(map['c']))
                            result += 6;
                        else if (!digit.Contains(map['e']))
                            result += 9;
                        break;
                    case 7:
                        result += 8;
                        break;
                }
            }
            return result;
        }

        public override string Solve2()
        {
            int sum = 0;
            foreach (var (usp, dov) in input)
                sum += Decode(usp, dov);
            return sum.ToString();
        }
    }
}
