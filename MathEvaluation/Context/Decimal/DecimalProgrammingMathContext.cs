namespace MathEvaluation.Context.Decimal;

/// <summary>
/// The base programming math context supports floor division '//', uses '**' notation for exponentiation, and '%' for modulo operation.
/// </summary>
/// <seealso cref="MathEvaluation.Context.MathContext" />
/// <seealso cref="MathEvaluation.Context.IProgrammingMathContext" />
public class DecimalProgrammingMathContext : MathContext, IProgrammingMathContext
{
    /// <summary>Initializes a new instance of the <see cref="DecimalProgrammingMathContext" /> class.</summary>
    public DecimalProgrammingMathContext()
    {
        static decimal divisionFn(decimal leftOperand, decimal rigntOperand) => leftOperand % rigntOperand;
        BindOperator(divisionFn, '%');
    }
}
