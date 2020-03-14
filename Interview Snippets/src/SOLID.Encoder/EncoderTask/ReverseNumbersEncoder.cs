using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.EncoderTask
{
    public class ReverseNumbersEncoder : IReverser {
        public string Execute (string source) {
            var nonNumbers = new List<char> ();
            for (var i = 32; i < 126; i++) {
                if (i < 48 || i > 57)
                    nonNumbers.Add ((char) i);
            }

            var numArray = source.ToLower ().Split (nonNumbers.ToArray ()).Where (a => !a.Equals ("")).ToList ();

            string result = string.Empty;
            foreach (var test in numArray) {
                var position = source.IndexOf (test);
                result = $"{result}{source.Substring(0, position)}{Reverse(test)}";
                source = source.Substring (position + test.Length);
            }
            if (source.Length > 0)
                result = $"{result}{source}";
            return result;
        }
        private string Reverse (string source) {
            var charArray = source.ToCharArray ();
            Array.Reverse (charArray);
            return new string (charArray);
        }
    }
}