using System.Text;

namespace ConsoleApp1.EncoderTask
{

    public class Encoder : IEncoder, IReverser, IReplacer {
        public Encoder (IEncoder handler) => EncodeHandler = handler;
        public Encoder (IReverser handler) => ReverseHandler = handler;
        public Encoder (IReplacer handler) => ReplacerHandler = handler;

        private IEncoder EncodeHandler { get; }
        private IReverser ReverseHandler { get; }
        private IReplacer ReplacerHandler { get; }

        public bool Execute (char source, ref StringBuilder target) => EncodeHandler.Execute (source, ref target);
        public string Execute (string source) => ReverseHandler.Execute (source);
        public bool Execute (char source, char check, char replacement, ref StringBuilder target) => ReplacerHandler.Execute (source, check, replacement, ref target);
    }
}