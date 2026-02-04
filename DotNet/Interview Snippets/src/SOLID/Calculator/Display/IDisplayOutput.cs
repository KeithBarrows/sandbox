using System;

namespace Calculator.Display {
    public interface IDisplayOutput {
        void Show ();
    }

    public class Writer : IDisplayOutput {
        public Writer (IDisplayOutput output) {
            Output = output;
        }

        private IDisplayOutput Output { get; }

        public void Show () => Output.Show ();
    }

    public class DisplayMenu : IDisplayOutput {
        public void Show () {
            Console.Clear();
            Console.WriteLine ("---==< Options >==---");
            Console.WriteLine ("a: Add");
            Console.WriteLine ("b: Subtract");
            Console.WriteLine ("c: Multiply");
            Console.WriteLine ("d: Divide");
            Console.WriteLine ("e: Square Root");
            Console.WriteLine ("x: Exit");
            Console.WriteLine ("");
            Console.WriteLine ("please select an option (a, b, c, d, e or x)...");
        }
    }
    public class DisplayNextChoice : IDisplayOutput {
        public void Show () {
            Console.Clear();
            Console.WriteLine ("---==< Options >==---");
            Console.WriteLine ("a: Calculate");
            Console.WriteLine ("x: Exit");
            Console.WriteLine ("");
            Console.WriteLine ("please select an option (a or x)...");
        }
    }
}