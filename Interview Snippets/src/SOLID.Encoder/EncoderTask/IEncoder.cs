using System.Text;

namespace ConsoleApp1.EncoderTask
{
    public interface IEncoder {
        bool Execute (char source, ref StringBuilder target);
    }
}