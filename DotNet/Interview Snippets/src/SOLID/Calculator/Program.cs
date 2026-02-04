using System;
using Calculator.Calculations;
using Calculator.Display;

namespace Calculator
{
    class Program
    {
        private static Calculator add = new Calculator (new AddCalculation ());
        private static Calculator subtract = new Calculator (new SubtractCalculation ());
        private static Calculator multiply = new Calculator (new MultiplyCalculation ());
        private static Calculator divide = new Calculator (new DivideCalculation ());
        private static Calculator sqrt = new Calculator (new SquareRootCalculation ());
        private static Writer displayMenu = new Writer(new DisplayMenu());
        private static Writer displayNextChoice = new Writer(new DisplayNextChoice());

        static void Main(string[] args)
        {

            var calc = new CalculatorProgram();
            Console.WriteLine("This simple calculator can do the following:");
            displayNextChoice.Show();
            var choice = Console.ReadKey();
            switch(choice.KeyChar){
                case 'a':
                    displayMenu.Show();
                    break;
            }
            var result = (double)0;
            while (choice.KeyChar != 'x')
            {
                choice = Console.ReadKey();
                switch(choice.KeyChar){
                    case 'a':
                        result = add.Calculator2OperandOperation.Calculate(3, 5);
                        break;
                    case 'b':
                        result = subtract.Calculator2OperandOperation.Calculate(3, 5);
                        break;
                    case 'c':
                        result = multiply.Calculator2OperandOperation.Calculate(3, 5);
                        break;
                    case 'd':
                        result = divide.Calculator2OperandOperation.Calculate(3, 5);
                        break;
                    case 'e':
                        result = sqrt.Calculator1OperandOperation.Calculate(16);
                        break;
                }
                displayNextChoice.Show();
                choice = Console.ReadKey();
            }
        }
    }
}
