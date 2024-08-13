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
        BindVariable(1d, "true");
        BindVariable(1d, "True");
        BindVariable(1d, "TRUE");

        BindVariable(0d, "false");
        BindVariable(0d, "False");
        BindVariable(0d, "FALSE");

        static double modFn(double leftOperand, double rigntOperand) => leftOperand % rigntOperand;
        BindOperator(modFn, '%');

        static double floorDivisionFn(double leftOperand, double rigntOperand) => Math.Floor(leftOperand / rigntOperand);
        BindOperator(floorDivisionFn, "//");

        static double exponentiationFn(double leftOperand, double rigntOperand) => Math.Pow(leftOperand, rigntOperand);
        BindOperandOperator(exponentiationFn, "**", (int)EvalPrecedence.Exponentiation);

        static double equalToFn(double leftOperand, double rigntOperand) => leftOperand == rigntOperand ? 1.0 : default;
        BindOperator(equalToFn, '=', (int)EvalPrecedence.Equality);

        static double notEqualToFn(double leftOperand, double rigntOperand) => leftOperand != rigntOperand ? 1.0 : default;
        BindOperator(notEqualToFn, "<>", (int)EvalPrecedence.Equality);

        static double greaterThanFn(double leftOperand, double rigntOperand) => leftOperand > rigntOperand ? 1.0 : default;
        BindOperator(greaterThanFn, '>', (int)EvalPrecedence.Comparison);

        static double lessThanFn(double leftOperand, double rigntOperand) => leftOperand < rigntOperand ? 1.0 : default;
        BindOperator(lessThanFn, '<', (int)EvalPrecedence.Comparison);

        static double greaterThanOrEqualToFn(double leftOperand, double rigntOperand) => leftOperand >= rigntOperand ? 1.0 : default;
        BindOperator(greaterThanOrEqualToFn, ">=", (int)EvalPrecedence.Comparison);

        static double lessThanOrEqualToFn(double leftOperand, double rigntOperand) => leftOperand <= rigntOperand ? 1.0 : default;
        BindOperator(lessThanOrEqualToFn, "<=", (int)EvalPrecedence.Comparison);

        static double andFn(double leftOperand, double rigntOperand) => leftOperand != default && rigntOperand != default ? 1.0 : default;
        BindOperator(andFn, "and", (int)EvalPrecedence.LogicalAnd);
        BindOperator(andFn, "And", (int)EvalPrecedence.LogicalAnd);
        BindOperator(andFn, "AND", (int)EvalPrecedence.LogicalAnd);

        static double orFn(double leftOperand, double rigntOperand) => leftOperand != default || rigntOperand != default ? 1.0 : default;
        BindOperator(orFn, "or", (int)EvalPrecedence.LogicalOr);
        BindOperator(orFn, "Or", (int)EvalPrecedence.LogicalOr);
        BindOperator(orFn, "OR", (int)EvalPrecedence.LogicalOr);

        static double xorFn(double leftOperand, double rigntOperand) => leftOperand != default ^ rigntOperand != default ? 1.0 : default;
        BindOperator(xorFn, "xor", (int)EvalPrecedence.LogicalXor);
        BindOperator(xorFn, "Xor", (int)EvalPrecedence.LogicalXor);
        BindOperator(xorFn, "XOR", (int)EvalPrecedence.LogicalXor);

        static double notFn(double leftOperand, double rigntOperand) => rigntOperand == default ? 1.0 : default;
        BindOperator(notFn, "not", (int)EvalPrecedence.LogicalNot);
        BindOperator(notFn, "Not", (int)EvalPrecedence.LogicalNot);
        BindOperator(notFn, "NOT", (int)EvalPrecedence.LogicalNot);
    }
}
