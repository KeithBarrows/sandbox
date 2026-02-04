using System;

namespace Calculator.Calculations {
    public class SquareRootCalculation : ICalculator1OperandOperation {
        public double Calculate (double x) => Math.Sqrt (x);
    }
}