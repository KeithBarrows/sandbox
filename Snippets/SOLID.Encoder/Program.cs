using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleApp1.EncoderTask;

/**
 ** vowels are replaced with number: a -> 1, e -> 2, i -> 3, o -> 4, and u -> 5
 ** consonants are replaced with previous letter: b -> a, c -> b, d -> c, etc.
 ** y is replaced with space
 ** space is replaced with y
 ** numbers are replaced with their reverse: 1 -> 1, 23 -> 32, 1234 -> 4321
 ** other characters remain unchanged (punctuation, etc.)
 ** all output should be lower case, regardless of input case ("Hello World" should produce the same result as "hello world")
 **/
namespace ConsoleApp1 {
    class Program {
        private static EncoderTask.Encoder _numberEncoder = new EncoderTask.Encoder (new ReverseNumbersEncoder ());
        private static EncoderTask.Encoder _vowelEncoder = new EncoderTask.Encoder (new VowelsEncoder ());
        private static EncoderTask.Encoder _consonantEncoder = new EncoderTask.Encoder (new ConsonantsEncoder ());
        private static EncoderTask.Encoder _replacerEncoder = new EncoderTask.Encoder (new ReplacementEncoder ());

        static void Main (string[] args) {
            var source = @"Hello 321 World! 12345 Keith321 yy ";
            RunEncoder(source);
            source = @"Loaded 'C:\Program Files\dotnet\shared\Microsoft.NETCore.App\2.1.4\System.Runtime.dll'. Skipped loading symbols. Module is optimized and the debugger option 'Just My Code' is enabled.";
            RunEncoder(source);
            source = @"abcdefghijklmnopqrstuvwxyz 0123456789";
            RunEncoder(source);
            source = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse lobortis eget dolor sit amet vestibulum. Aliquam non viverra orci. Vestibulum nec erat vitae massa pellentesque mollis in non ligula. Praesent quis justo non leo vestibulum finibus sit amet vitae augue. Suspendisse ligula leo, euismod non fermentum id, porttitor accumsan risus. Curabitur ut arcu ipsum. Quisque feugiat sapien eget blandit tincidunt. Vivamus pretium quam et odio facilisis accumsan. Integer vitae rutrum justo. Sed eros metus, luctus mollis felis convallis, dignissim laoreet nisl. Aliquam elementum pretium arcu. Sed accumsan congue ligula, quis placerat mi efficitur ut. Aliquam sed ligula urna. Sed sodales quis felis a ornare. In gravida dapibus diam semper vehicula. Mauris fringilla sem felis, ut dictum est congue quis.";
            RunEncoder(source);
        }

        static string Encoder (string source) {
            StringBuilder result = new StringBuilder ();
            source = _numberEncoder.Execute(source.ToLower());
            foreach (var character in source.ToCharArray ()) {
                if (_vowelEncoder.Execute (character, ref result)) continue;
                if (_consonantEncoder.Execute (character, ref result)) continue;
                if (_replacerEncoder.Execute (character, 'y', ' ', ref result)) continue;
                if (_replacerEncoder.Execute (character, ' ', 'y', ref result)) continue;
                result.Append (character);
            }
            return result.ToString ();
        }

        static void RunEncoder(string source){
            var start = DateTime.Now;
            var result = Encoder (source);
            var end = DateTime.Now;
            var elapsed = end - start;
            Console.WriteLine ("----------------------------------------------------------------------");
            Console.WriteLine (source);
            Console.WriteLine ("----------------------------------------------------------------------");
            Console.WriteLine (result);
            Console.WriteLine ("----------------------------------------------------------------------");
            Console.WriteLine ($"Elapsed time: {elapsed}");
            Console.WriteLine ("Hit any key to continue...");
            Console.ReadKey ();
            Console.WriteLine(" ");
            Console.WriteLine(" ");
        }
    }
}