﻿using System;
using System.Linq.Expressions;
using MathEvaluation.Entities;

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

        static double modFn(double left, double right) => left % right;
        BindOperator(modFn, '%', (int)EvalPrecedence.Basic, ExpressionType.Modulo);

        static double floorDivisionFn(double left, double right) => Math.Floor(left / right);
        BindOperator(floorDivisionFn, "//");

        BindOperandsOperator(Math.Pow, "**", (int)EvalPrecedence.Exponentiation);

        static double equalToFn(double left, double right) => left == right ? 1.0 : default;
        BindOperator(equalToFn, '=', (int)EvalPrecedence.Equality, ExpressionType.Equal);

        static double notEqualToFn(double left, double right) => left != right ? 1.0 : default;
        BindOperator(notEqualToFn, "<>", (int)EvalPrecedence.Equality, ExpressionType.NotEqual);

        static double greaterThanFn(double left, double right) => left > right ? 1.0 : default;
        BindOperator(greaterThanFn, '>', (int)EvalPrecedence.RelationalOperator, ExpressionType.GreaterThan);

        static double lessThanFn(double left, double right) => left < right ? 1.0 : default;
        BindOperator(lessThanFn, '<', (int)EvalPrecedence.RelationalOperator, ExpressionType.LessThan);

        static double greaterThanOrEqualToFn(double left, double right) => left >= right ? 1.0 : default;
        BindOperator(greaterThanOrEqualToFn, ">=", (int)EvalPrecedence.RelationalOperator, ExpressionType.GreaterThanOrEqual);

        static double lessThanOrEqualToFn(double left, double right) => left <= right ? 1.0 : default;
        BindOperator(lessThanOrEqualToFn, "<=", (int)EvalPrecedence.RelationalOperator, ExpressionType.LessThanOrEqual);

        static double andFn(double left, double right) => left != default && right != default ? 1.0 : default;
        BindOperator(andFn, "and", (int)EvalPrecedence.LogicalAnd, ExpressionType.AndAlso);
        BindOperator(andFn, "And", (int)EvalPrecedence.LogicalAnd, ExpressionType.AndAlso);
        BindOperator(andFn, "AND", (int)EvalPrecedence.LogicalAnd, ExpressionType.AndAlso);

        static double orFn(double left, double right) => left != default || right != default ? 1.0 : default;
        BindOperator(orFn, "or", (int)EvalPrecedence.LogicalOr, ExpressionType.OrElse);
        BindOperator(orFn, "Or", (int)EvalPrecedence.LogicalOr, ExpressionType.OrElse);
        BindOperator(orFn, "OR", (int)EvalPrecedence.LogicalOr, ExpressionType.OrElse);

        static double xorFn(double left, double right) => left != default ^ right != default ? 1.0 : default;
        BindOperator(xorFn, "xor", (int)EvalPrecedence.LogicalXor, ExpressionType.ExclusiveOr);
        BindOperator(xorFn, "Xor", (int)EvalPrecedence.LogicalXor, ExpressionType.ExclusiveOr);
        BindOperator(xorFn, "XOR", (int)EvalPrecedence.LogicalXor, ExpressionType.ExclusiveOr);

        static double notFn(double left, double right) => right == default ? 1.0 : default;
        BindOperator(notFn, "not", (int)EvalPrecedence.LogicalNot, ExpressionType.Not);
        BindOperator(notFn, "Not", (int)EvalPrecedence.LogicalNot, ExpressionType.Not);
        BindOperator(notFn, "NOT", (int)EvalPrecedence.LogicalNot, ExpressionType.Not);
    }
}
