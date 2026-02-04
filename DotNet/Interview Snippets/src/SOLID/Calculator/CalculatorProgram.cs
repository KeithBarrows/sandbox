using Calculator.Calculations;

namespace Calculator
{
    public class CalculatorProgram
    {
        public void AddSample()
        {
            Calculator calculator = new Calculator(new AddCalculation());
            double result = calculator.Solve(1, 1);
            // Result is 2.
        }

        public void SubtractSample()
        {
            Calculator calculator = new Calculator(new SubtractCalculation());
            double result = calculator.Solve(1, 1);
            // Result is 0.
        }

        public void MultiplySample()
        {
            Calculator calculator = new Calculator(new MultiplyCalculation());
            double result = calculator.Solve(1, 2);
            // Result is 2.
        }

        public void DivideSample()
        {
            Calculator calculator = new Calculator(new DivideCalculation());
            double result = calculator.Solve(10, 5);
            // Result is 2.
        }

        public void SquareRootSample()
        {
            Calculator calculator = new Calculator(new SquareRootCalculation());
            double result = calculator.Solve(25);
            // Result is 5.
        }
    }
}