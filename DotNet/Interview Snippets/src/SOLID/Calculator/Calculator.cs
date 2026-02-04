namespace Calculator
{
    public class Calculator
    {
        // ICalculator2OperandOperation is "injected" via the constructor.
        // This guarantees that the Calculator cannot be created without a calculator operation.
        // This makes the Calculator "dependent" on the ICalculatorXOperandOperation.
        public Calculator(ICalculator1OperandOperation calculator1OperandOperation)
        {
            Calculator1OperandOperation = calculator1OperandOperation;
        }
        public Calculator(ICalculator2OperandOperation calculator2OperandOperation)
        {
            Calculator2OperandOperation = calculator2OperandOperation;
        }

        public ICalculator1OperandOperation Calculator1OperandOperation { get; }
        public ICalculator2OperandOperation Calculator2OperandOperation { get; }

        public double Solve(double x)
        {
            // Calculations will be based on the "injected" ICalculator2OperandOperation.
            return Calculator1OperandOperation.Calculate(x);
        }
        public double Solve(double x, double y)
        {
            // Calculations will be based on the "injected" ICalculator2OperandOperation.
            return Calculator2OperandOperation.Calculate(x, y);
        }
    }
}