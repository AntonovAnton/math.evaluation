using MathEvaluation.Entities;
using System;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MathEvaluation.Context;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// The base programming math context supports floor division '//', uses '**' notation for exponentiation, and '%' for modulo operation.
/// </summary>
/// <seealso cref="MathEvaluation.Context.MathContext" />
public class DecimalProgrammingMathContext : MathContext
{
    /// <summary>Initializes a new instance of the <see cref="DecimalProgrammingMathContext" /> class.</summary>
    public DecimalProgrammingMathContext()
    {
        BindConstant(1d, "true");
        BindConstant(1d, "True");
        BindConstant(1d, "TRUE");

        BindConstant(0d, "false");
        BindConstant(0d, "False");
        BindConstant(0d, "FALSE");

        BindOperator('%', OperatorType.Modulo);

        static decimal floorDivisionFn(decimal left, decimal right) => Math.Floor(left / right);
        BindOperator(floorDivisionFn, "//");

        static double powFn(double x, double y) => Math.Pow(x, y);
        BindOperandsOperator(powFn, "**", (int)EvalPrecedence.Exponentiation);

        BindOperator("=", OperatorType.Equal);
        BindOperator("<>", OperatorType.NotEqual);

        BindOperator('>', OperatorType.GreaterThan);
        BindOperator('<', OperatorType.LessThan);
        BindOperator(">=", OperatorType.GreaterThanOrEqual);
        BindOperator("<=", OperatorType.LessThanOrEqual);

        BindOperator("and", OperatorType.LogicalAnd);
        BindOperator("And", OperatorType.LogicalAnd);
        BindOperator("AND", OperatorType.LogicalAnd);

        BindOperator("or", OperatorType.LogicalOr);
        BindOperator("Or", OperatorType.LogicalOr);
        BindOperator("OR", OperatorType.LogicalOr);

        BindOperator("xor", OperatorType.LogicalXor);
        BindOperator("Xor", OperatorType.LogicalXor);
        BindOperator("XOR", OperatorType.LogicalXor);

        BindOperator("not", OperatorType.LogicalNot);
        BindOperator("Not", OperatorType.LogicalNot);
        BindOperator("NOT", OperatorType.LogicalNot);
    }
}
