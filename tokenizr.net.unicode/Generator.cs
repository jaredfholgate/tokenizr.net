using System;
using System.Collections.Generic;

namespace tokenizr.net.unicode
{
    public class Generator
    {
        public List<char> Generate()
        {
            var array = new List<char>();
            for (int i = char.MinValue; i < char.MaxValue; i++)
            {
                array.Add((char)i);
            }
            return array;
        }
    }
}
