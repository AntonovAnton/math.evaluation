using MathEvaluation.Entities;
using System;

namespace MathEvaluation.Context;

/// <summary>
/// The base programming math context supports floor division '//', uses '**' notation for exponentiation, and '%' for modulo operation. 
/// For a complete list of features and supported functions, please refer to the documentation at <see href="https://github.com/AntonovAnton/math.evaluation"/>.
/// </summary>
/// <seealso cref="MathEvaluation.Context.MathContext" />
public class ProgrammingMathContext : MathContext
{
    /// <summary>Initializes a new instance of the <see cref="ProgrammingMathContext" /> class.</summary>
    public ProgrammingMathContext()
    {
        BindConstant(1d, "true");
        BindConstant(1d, "True");
        BindConstant(1d, "TRUE");

        BindConstant(0d, "false");
        BindConstant(0d, "False");
        BindConstant(0d, "FALSE");

        BindOperator('%', OperatorType.Modulo);

        static double floorDivisionFn(double left, double right) => Math.Floor(left / right);
        BindOperator(floorDivisionFn, "//");

        BindOperator("**", OperatorType.Power);

        BindOperator('=', OperatorType.Equal);
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

        static double iifFn(double[] args) => args[0] != default
            ? args.Length > 1 ? args[1] : 1d
            : args.Length > 2 ? args[2] : args.Length > 3 ? throw new ArgumentOutOfRangeException("Count of args > 3") : 0d;

        BindFunction(iifFn, "iif");
        BindFunction(iifFn, "Iif");
        BindFunction(iifFn, "IIF");
    }
}
