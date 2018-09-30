using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.EncoderTask
{
    public class ConsonantsEncoder : IEncoder {
        static List<char> Consonants => new List<char> { 'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'z' };
        public bool Execute (char source, ref StringBuilder target) {
            if (Consonants.Contains (source)) {
                var t = Convert.ToChar(source - 1);
                var q = t.ToString();
                target.Append (q);
                return true;
            }
            return false;
        }
    }
}