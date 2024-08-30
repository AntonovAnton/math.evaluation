using System;
using MathEvaluation.Entities;

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

        static decimal modFn(decimal left, decimal right) => left % right;
        BindOperator(modFn, '%');

        static decimal floorDivisionFn(decimal left, decimal right) => Math.Floor(left / right);
        BindOperator(floorDivisionFn, "//");

        BindOperandsOperator(Math.Pow, "**", (int)EvalPrecedence.Exponentiation);

        static decimal equalToFn(decimal left, decimal right) => left == right ? 1.0m : default;
        BindOperator(equalToFn, '=', (int)EvalPrecedence.Equality);

        static decimal notEqualToFn(decimal left, decimal right) => left != right ? 1.0m : default;
        BindOperator(notEqualToFn, "<>", (int)EvalPrecedence.Equality);

        static decimal greaterThanFn(decimal left, decimal right) => left > right ? 1.0m : default;
        BindOperator(greaterThanFn, '>', (int)EvalPrecedence.RelationalOperator);

        static decimal lessThanFn(decimal left, decimal right) => left < right ? 1.0m : default;
        BindOperator(lessThanFn, '<', (int)EvalPrecedence.RelationalOperator);

        static decimal greaterThanOrEqualToFn(decimal left, decimal right) => left >= right ? 1.0m : default;
        BindOperator(greaterThanOrEqualToFn, ">=", (int)EvalPrecedence.RelationalOperator);

        static decimal lessThanOrEqualToFn(decimal left, decimal right) => left <= right ? 1.0m : default;
        BindOperator(lessThanOrEqualToFn, "<=", (int)EvalPrecedence.RelationalOperator);

        static decimal andFn(decimal left, decimal right) => left != default && right != default ? 1.0m : default;
        BindOperator(andFn, "and", (int)EvalPrecedence.LogicalAnd);
        BindOperator(andFn, "And", (int)EvalPrecedence.LogicalAnd);
        BindOperator(andFn, "AND", (int)EvalPrecedence.LogicalAnd);

        static decimal orFn(decimal left, decimal right) => left != default || right != default ? 1.0m : default;
        BindOperator(orFn, "or", (int)EvalPrecedence.LogicalOr);
        BindOperator(orFn, "Or", (int)EvalPrecedence.LogicalOr);
        BindOperator(orFn, "OR", (int)EvalPrecedence.LogicalOr);

        static decimal xorFn(decimal left, decimal right) => left != default ^ right != default ? 1.0m : default;
        BindOperator(xorFn, "xor", (int)EvalPrecedence.LogicalXor);
        BindOperator(xorFn, "Xor", (int)EvalPrecedence.LogicalXor);
        BindOperator(xorFn, "XOR", (int)EvalPrecedence.LogicalXor);

        static decimal notFn(decimal left, decimal right) => right == default ? 1.0m : default;
        BindOperator(notFn, "not", (int)EvalPrecedence.LogicalNot);
        BindOperator(notFn, "Not", (int)EvalPrecedence.LogicalNot);
        BindOperator(notFn, "NOT", (int)EvalPrecedence.LogicalNot);
    }
}
