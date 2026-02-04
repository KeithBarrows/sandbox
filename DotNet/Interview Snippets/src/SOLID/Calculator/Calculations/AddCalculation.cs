namespace Calculator.Calculations
{
    public class AddCalculation : ICalculator2OperandOperation
    {
        public double Calculate(double x, double y) => x + y;
    }
}