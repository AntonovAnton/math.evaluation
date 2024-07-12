namespace MathEvaluation.Context.Decimal;

public class DecimalProgrammingMathContext : MathContext, IProgrammingMathContext
{
    public DecimalProgrammingMathContext()
    {
        static decimal divisionFn(decimal leftOperand, decimal rigntOperand) => leftOperand % rigntOperand;
        BindOperator(divisionFn, '%');
    }
}
