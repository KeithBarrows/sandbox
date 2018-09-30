using System.Text;

namespace ConsoleApp1.EncoderTask
{
    public class ReplacementEncoder : IReplacer {
        public bool Execute (char source, char check, char replacement, ref StringBuilder target) {
            if (source.Equals (check)) {
                target.Append (replacement.ToString());
                return true;
            }
            return false;
        }
    }
}