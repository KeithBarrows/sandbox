using System.Text;

namespace ConsoleApp1.EncoderTask
{
    public interface IReplacer {
        bool Execute (char source, char check, char replacement, ref StringBuilder target);
    }
}