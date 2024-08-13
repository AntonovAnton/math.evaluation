using System;

namespace MathEvaluation.Context.Decimal;

/// <summary>
/// The base programming math context supports floor division '//', uses '**' notation for exponentiation, and '%' for modulo operation.
/// </summary>
/// <seealso cref="MathEvaluation.Context.MathContext" />
public class DecimalProgrammingMathContext : MathContext
{
    /// <summary>Initializes a new instance of the <see cref="DecimalProgrammingMathContext" /> class.</summary>
    public DecimalProgrammingMathContext()
    {
        BindVariable(1m, "true");
        BindVariable(1m, "True");
        BindVariable(1m, "TRUE");

        BindVariable(0m, "false");
        BindVariable(0m, "False");
        BindVariable(0m, "FALSE");

        static decimal modFn(decimal leftOperand, decimal rigntOperand) => leftOperand % rigntOperand;
        BindOperator(modFn, '%');

        static decimal floorDivisionFn(decimal leftOperand, decimal rigntOperand) => Math.Floor(leftOperand / rigntOperand);
        BindOperator(floorDivisionFn, "//");

        static decimal exponentiationFn(decimal leftOperand, decimal rigntOperand) => (decimal)Math.Pow((double)leftOperand, (double)rigntOperand);
        BindOperandOperator(exponentiationFn, "**", (int)EvalPrecedence.Exponentiation);

        static decimal equalToFn(decimal leftOperand, decimal rigntOperand) => leftOperand == rigntOperand ? 1.0m : default;
        BindOperator(equalToFn, '=', (int)EvalPrecedence.Equality);

        static decimal notEqualToFn(decimal leftOperand, decimal rigntOperand) => leftOperand != rigntOperand ? 1.0m : default;
        BindOperator(notEqualToFn, "<>", (int)EvalPrecedence.Equality);

        static decimal greaterThanFn(decimal leftOperand, decimal rigntOperand) => leftOperand > rigntOperand ? 1.0m : default;
        BindOperator(greaterThanFn, '>', (int)EvalPrecedence.Comparison);

        static decimal lessThanFn(decimal leftOperand, decimal rigntOperand) => leftOperand < rigntOperand ? 1.0m : default;
        BindOperator(lessThanFn, '<', (int)EvalPrecedence.Comparison);

        static decimal greaterThanOrEqualToFn(decimal leftOperand, decimal rigntOperand) => leftOperand >= rigntOperand ? 1.0m : default;
        BindOperator(greaterThanOrEqualToFn, ">=", (int)EvalPrecedence.Comparison);

        static decimal lessThanOrEqualToFn(decimal leftOperand, decimal rigntOperand) => leftOperand <= rigntOperand ? 1.0m : default;
        BindOperator(lessThanOrEqualToFn, "<=", (int)EvalPrecedence.Comparison);

        static decimal andFn(decimal leftOperand, decimal rigntOperand) => leftOperand != default && rigntOperand != default ? 1.0m : default;
        BindOperator(andFn, "and", (int)EvalPrecedence.LogicalAnd);
        BindOperator(andFn, "And", (int)EvalPrecedence.LogicalAnd);
        BindOperator(andFn, "AND", (int)EvalPrecedence.LogicalAnd);

        static decimal orFn(decimal leftOperand, decimal rigntOperand) => leftOperand != default || rigntOperand != default ? 1.0m : default;
        BindOperator(orFn, "or", (int)EvalPrecedence.LogicalOr);
        BindOperator(orFn, "Or", (int)EvalPrecedence.LogicalOr);
        BindOperator(orFn, "OR", (int)EvalPrecedence.LogicalOr);

        static decimal xorFn(decimal leftOperand, decimal rigntOperand) => leftOperand != default ^ rigntOperand != default ? 1.0m : default;
        BindOperator(xorFn, "xor", (int)EvalPrecedence.LogicalXor);
        BindOperator(xorFn, "Xor", (int)EvalPrecedence.LogicalXor);
        BindOperator(xorFn, "XOR", (int)EvalPrecedence.LogicalXor);

        static decimal notFn(decimal leftOperand, decimal rigntOperand) => rigntOperand == default ? 1.0m : default;
        BindOperator(notFn, "not", (int)EvalPrecedence.LogicalNot);
        BindOperator(notFn, "Not", (int)EvalPrecedence.LogicalNot);
        BindOperator(notFn, "NOT", (int)EvalPrecedence.LogicalNot);
    }
}
