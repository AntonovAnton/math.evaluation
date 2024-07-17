namespace MathEvaluation.Context;

/// <summary>
/// The base programming math context supports floor division '//', uses '**' notation for exponentiation, and '%' for modulo operation.
/// </summary>
/// <seealso cref="MathEvaluation.Context.MathContext" />
/// <seealso cref="MathEvaluation.Context.IProgrammingMathContext" />
public class ProgrammingMathContext : MathContext, IProgrammingMathContext
{
    /// <summary>Initializes a new instance of the <see cref="ProgrammingMathContext" /> class.</summary>
    public ProgrammingMathContext()
    {
        static double divisionFn(double leftOperand, double rigntOperand) => leftOperand % rigntOperand;
        BindOperator(divisionFn, '%');
    }
}
