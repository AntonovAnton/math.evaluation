using System;
using System.Linq.Expressions;
using MathEvaluation.Entities;

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

        static decimal modFn(decimal left, decimal right) => left % right;
        BindOperator(modFn, '%', (int)EvalPrecedence.Basic, ExpressionType.Modulo);

        static decimal floorDivisionFn(decimal left, decimal right) => Math.Floor(left / right);
        BindOperator(floorDivisionFn, "//");

        static double powFn(double x, double y) => Math.Pow(x, y);
        BindOperandsOperator(powFn, "**", (int)EvalPrecedence.Exponentiation);

        static decimal equalToFn(decimal left, decimal right) => left == right ? 1.0m : default;
        BindOperator(equalToFn, '=', (int)EvalPrecedence.Equality, ExpressionType.Equal);

        static decimal notEqualToFn(decimal left, decimal right) => left != right ? 1.0m : default;
        BindOperator(notEqualToFn, "<>", (int)EvalPrecedence.Equality, ExpressionType.NotEqual);

        static decimal greaterThanFn(decimal left, decimal right) => left > right ? 1.0m : default;
        BindOperator(greaterThanFn, '>', (int)EvalPrecedence.RelationalOperator, ExpressionType.GreaterThan);

        static decimal lessThanFn(decimal left, decimal right) => left < right ? 1.0m : default;
        BindOperator(lessThanFn, '<', (int)EvalPrecedence.RelationalOperator, ExpressionType.LessThan);

        static decimal greaterThanOrEqualToFn(decimal left, decimal right) => left >= right ? 1.0m : default;
        BindOperator(greaterThanOrEqualToFn, ">=", (int)EvalPrecedence.RelationalOperator, ExpressionType.GreaterThanOrEqual);

        static decimal lessThanOrEqualToFn(decimal left, decimal right) => left <= right ? 1.0m : default;
        BindOperator(lessThanOrEqualToFn, "<=", (int)EvalPrecedence.RelationalOperator, ExpressionType.LessThanOrEqual);

        static decimal andFn(decimal left, decimal right) => left != default && right != default ? 1.0m : default;
        BindOperator(andFn, "and", (int)EvalPrecedence.LogicalAnd, ExpressionType.AndAlso);
        BindOperator(andFn, "And", (int)EvalPrecedence.LogicalAnd, ExpressionType.AndAlso);
        BindOperator(andFn, "AND", (int)EvalPrecedence.LogicalAnd, ExpressionType.AndAlso);

        static decimal orFn(decimal left, decimal right) => left != default || right != default ? 1.0m : default;
        BindOperator(orFn, "or", (int)EvalPrecedence.LogicalOr, ExpressionType.OrElse);
        BindOperator(orFn, "Or", (int)EvalPrecedence.LogicalOr, ExpressionType.OrElse);
        BindOperator(orFn, "OR", (int)EvalPrecedence.LogicalOr, ExpressionType.OrElse);

        static decimal xorFn(decimal left, decimal right) => left != default ^ right != default ? 1.0m : default;
        BindOperator(xorFn, "xor", (int)EvalPrecedence.LogicalXor, ExpressionType.ExclusiveOr);
        BindOperator(xorFn, "Xor", (int)EvalPrecedence.LogicalXor, ExpressionType.ExclusiveOr);
        BindOperator(xorFn, "XOR", (int)EvalPrecedence.LogicalXor, ExpressionType.ExclusiveOr);

        static decimal notFn(decimal left, decimal right) => right == default ? 1.0m : default;
        BindOperator(notFn, "not", (int)EvalPrecedence.LogicalNot, ExpressionType.Not);
        BindOperator(notFn, "Not", (int)EvalPrecedence.LogicalNot, ExpressionType.Not);
        BindOperator(notFn, "NOT", (int)EvalPrecedence.LogicalNot, ExpressionType.Not);
    }
}
