using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AoC2021
{
    class Day16 : Day
    {
        Packet inputPacket;

        static IEnumerable<bool> HexToBin(char c)
        {
            if ((c < '0' || c > '9') && (c < 'A' && c > 'F'))
                throw new ArgumentOutOfRangeException("c", c, "c must be valid hexadecimal char.");

            int n;
            if (char.IsDigit(c))
                n = c - '0';
            else
                n = c - 'A' + 10;

            yield return (n & 0b1000) != 0;
            yield return (n & 0b0100) != 0;
            yield return (n & 0b0010) != 0;
            yield return (n & 0b0001) != 0;
        }

        static int BinToInt(bool[] bin, int start = 0, int? end = null)
        {
            end ??= bin.Length;

            int n = 0;
            for (int i = start; i < end.Value; ++i)
            {
                n <<= 1;
                if (bin[i]) ++n;
            }
            return n;
        }

        public override void SetInput(string inputPath)
        {
            bool[] data;
            using (StreamReader sr = new StreamReader(inputPath))
            {
                try
                {
                    data = sr.ReadLine().SelectMany(HexToBin).ToArray();
                }
                catch (ArgumentOutOfRangeException e)
                {
                    throw new FileFormatException(inputPath, $"'{e.ActualValue}' is not a valid hexadecimal character!");
                }
            }
            inputPacket = new Packet(data);
        }

        struct Packet
        {
            bool[] data;
            public int Start { get; }
            public int Type { get; }
            public int Version { get; }

            long? value = null;
            List<Packet> subpackets = null;
            int end;
            public int End => end;


            public Packet(bool[] data, int start = 0)
            {
                this.data = data;
                Start = start;
                Version = BinToInt(data, start, start + 3);
                Type = BinToInt(data, start + 3, start + 6);

                end = 0;
                if (Type == 4)
                    ParseLiteralValue();
                else
                    ParseOperator();
            }

            void ParseLiteralValue()
            {
                value = 0;
                int i = Start + 1;
                do
                {
                    i += 5;
                    value <<= 4;
                    value += BinToInt(data, i + 1, i + 5);
                } while (data[i]);

                end = i + 5;
            }

            void ParseOperator()
            {
                subpackets = new List<Packet>();
                int i = Start + 6;
                if (data[i++]) // number of sub-packets immediately contained
                {
                    int packetsN = BinToInt(data, i, i + 11);
                    i += 11;
                    for (int j = 0; j < packetsN; ++j)
                    {
                        subpackets.Add(new Packet(data, i));
                        i = subpackets.Last().End;
                    }
                    end = i;
                }
                else // total length in bits
                {
                    int totalLength = BinToInt(data, i, i + 15);
                    i += 15;
                    end = i + totalLength;

                    while (i < end)
                    {
                        subpackets.Add(new Packet(data, i));
                        i = subpackets.Last().End;
                    }

                    if (i != end)
                        throw new ArgumentException("Data can't be parsed!");
                }
            }

            public long Evaluate()
            {
                if (Type == 4)
                    return value.Value;

                List<long> subValues = subpackets.Select(p => p.Evaluate()).ToList();
                switch (Type)
                {
                    case 0:
                        return subValues.Sum();
                    case 1:
                        return subValues.Aggregate((a, b) => a * b);
                    case 2:
                        return subValues.Min();
                    case 3:
                        return subValues.Max();
                    case 5:
                        return subValues[0] > subValues[1] ? 1 : 0;
                    case 6:
                        return subValues[0] < subValues[1] ? 1 : 0;
                    case 7:
                        return subValues[0] == subValues[1] ? 1 : 0;
                    default:
                        throw new NotImplementedException();
                }
            }

            public IEnumerable<Packet> IteratePackets()
            {
                yield return this;
                if (subpackets != null)
                    foreach (var p in subpackets.SelectMany(p => p.IteratePackets()))
                        yield return p;
            }
        }



        public override string Solve1() => inputPacket.IteratePackets()
                                                      .Select(p => p.Version)
                                                      .Sum()
                                                      .ToString();

        public override string Solve2() => inputPacket.Evaluate().ToString();
    }
}
