using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.EncoderTask
{
    public class VowelsEncoder : IEncoder {
        private Dictionary<char, int> Vowels => new Dictionary<char, int> { { 'a', 1 }, { 'e', 2 }, { 'i', 3 }, { 'o', 4 }, { 'u', 5 } };
        public bool Execute (char source, ref StringBuilder target) {
            if (Vowels.ContainsKey (source)) {
                target.Append (Vowels[source].ToString ());
                return true;
            }
            return false;
        }
    }
}