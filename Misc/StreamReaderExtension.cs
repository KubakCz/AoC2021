using System.IO;
using System.Collections.Generic;

namespace AoC2021
{
    public static class StreamReaderExtension
    {
        /// <summary>
        /// Reads the whole file line by line.
        /// Allows using IEnumerable methods without need to read the whole file to the memory.
        /// </summary>
        public static IEnumerable<string> ReadLines(this StreamReader sr, bool excludeEmpty = false)
        {
            string line;
            while ((line = sr.ReadLine()) != null)
                if (!excludeEmpty || line != "")
                    yield return line;
        }
    }
}