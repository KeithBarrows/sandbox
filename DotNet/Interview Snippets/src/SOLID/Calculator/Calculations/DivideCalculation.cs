namespace Calculator.Calculations {
    public class DivideCalculation : ICalculator2OperandOperation {
        public double Calculate (double x, double y) => x / y;
    }
}