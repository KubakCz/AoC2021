using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AoC2021
{
    class Day12 : Day
    {
        Dictionary<string, HashSet<string>> inputGraph;

        public override void SetInput(string inputPath)
        {
            using (StreamReader sr = new StreamReader(inputPath))
            {
                IEnumerable<(string, string)> connections = sr.ReadLines(true)
                                                              .Select(str => str.Split('-'))
                                                              .Select(strArray => (strArray[0], strArray[1]));

                inputGraph = new Dictionary<string, HashSet<string>>();
                void insert(string key, string value)
                {
                    if (inputGraph.ContainsKey(key))
                    {
                        inputGraph[key].Add(value);
                    }
                    else
                    {
                        var set = new HashSet<string>();
                        set.Add(value);
                        inputGraph[key] = set;
                    }
                }

                try
                {
                    foreach (var (a, b) in connections)
                    {
                        insert(a, b);
                        insert(b, a);
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    throw new FileFormatException(Path.GetFullPath(inputPath), "There was no '-' on the line!");
                }

                if (!inputGraph.ContainsKey("start") || !inputGraph.ContainsKey("end"))
                    throw new FileFormatException(Path.GetFullPath(inputPath), "Data must include 'start' and 'end' nodes.");
            }
        }

        bool IsLarge(string cave)
        {
            foreach (char c in cave)
                if (!char.IsUpper(c))
                    return false;
            return true;
        }


        List<List<string>> FindPaths(Dictionary<string, HashSet<string>> graph)
        {
            List<List<string>> result = new List<List<string>>();
            FindPaths(graph, "start", new List<string>() { "start" }, result);
            return result;
        }
        void FindPaths(Dictionary<string, HashSet<string>> graph,
                       string current, List<string> currentPath,
                       List<List<string>> result)
        {
            if (current == "end")
            {
                result.Add(new List<string>(currentPath));
                return;
            }

            foreach (string neighbour in graph[current])
            {
                if (IsLarge(neighbour) || !currentPath.Contains(neighbour))
                {
                    currentPath.Add(neighbour);
                    FindPaths(graph, neighbour, currentPath, result);
                    currentPath.RemoveAt(currentPath.Count - 1);
                }
            }
        }


        List<List<string>> FindPaths2(Dictionary<string, HashSet<string>> graph)
        {
            List<List<string>> result = new List<List<string>>();
            FindPaths2(graph, "start", new List<string>() { "start" }, false, result);
            return result;
        }
        void FindPaths2(Dictionary<string, HashSet<string>> graph,
                        string current, List<string> currentPath,
                        bool smallTwice,
                        List<List<string>> result)
        {
            if (current == "end")
            {
                result.Add(new List<string>(currentPath));
                return;
            }

            foreach (string neighbour in graph[current])
            {
                bool smallTwiceChange = smallTwice;
                if (neighbour != "start" && (IsLarge(neighbour) || !currentPath.Contains(neighbour) || (smallTwiceChange = !smallTwice)))
                {
                    currentPath.Add(neighbour);
                    FindPaths2(graph, neighbour, currentPath, smallTwiceChange, result);
                    currentPath.RemoveAt(currentPath.Count - 1);
                }
            }
        }

        public override string Solve1() => FindPaths(inputGraph).Count.ToString();

        public override string Solve2() => FindPaths2(inputGraph).Count.ToString();
    }
}
