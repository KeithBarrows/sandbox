namespace Calculator.Calculations {
    public class SubtractCalculation : ICalculator2OperandOperation {
        public double Calculate (double x, double y) => x - y;
    }
}