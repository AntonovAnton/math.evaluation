namespace MathEvaluation.Context;

public class ProgrammingMathContext : MathContext, IProgrammingMathContext
{
    public ProgrammingMathContext()
    {
        static double divisionFn(double leftOperand, double rigntOperand) => leftOperand % rigntOperand;
        BindOperator(divisionFn, '%');
    }
}
