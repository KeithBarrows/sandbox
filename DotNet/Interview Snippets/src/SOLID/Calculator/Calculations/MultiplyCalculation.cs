namespace Calculator.Calculations {
    public class MultiplyCalculation : ICalculator2OperandOperation {
        public double Calculate (double x, double y) => x * y;
    }
}