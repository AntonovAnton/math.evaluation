﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MathEvaluation.Entities;

/// <summary>
/// The math operator processes the left and right expressions.
/// </summary>
public class MathCompatibleOperator : MathEntity
{
    private static readonly Dictionary<OperatorType, ExpressionType> ExpressionTypeByOperatorType = [];
    private static readonly Dictionary<OperatorType, EvalPrecedence> EvalPrecedenceByOperatorType = [];

    private readonly bool _isProcessingOperand = false;

    /// <summary>Gets the type of the expression node.</summary>
    /// <value>The type of the expression node.</value>
    public ExpressionType ExpressionType { get; }

    /// <inheritdoc />
    public override int Precedence { get; }

    /// <summary>Gets the type of the operator.</summary>
    /// <value>The type of the operator.</value>
    public OperatorType OperatorType { get; }

    static MathCompatibleOperator()
    {
        foreach (var fieldInfo in typeof(OperatorType).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var attributes = fieldInfo.GetCustomAttributes(typeof(OperatorAttribute), false);
            var operatorAttribute = (OperatorAttribute?)attributes.FirstOrDefault(a => a is OperatorAttribute)
                ?? throw new Exception($"The Operator attribute isn't set for {fieldInfo.Name}.");

            var expressionType = operatorAttribute.ExpressionType;
            var precedence = operatorAttribute.Precedence;

            var operatorType = Enum.Parse<OperatorType>(fieldInfo.Name);

            ExpressionTypeByOperatorType[operatorType] = expressionType;
            EvalPrecedenceByOperatorType[operatorType] = precedence;
        }
    }

    /// <summary>Initializes a new instance of the <see cref="MathOperator{T}" /> class.</summary>
    /// <param name="key">The key (the operator notation).</param>
    /// <param name="operatorType">The specified type of the operator allows improve performance if it matches a C# operator.</param>
    /// <exception cref="ArgumentNullException"/>
    public MathCompatibleOperator(string? key, OperatorType operatorType)
        : base(key)
    {
        OperatorType = operatorType;
        ExpressionType = ExpressionTypeByOperatorType[operatorType];

        var precedence = EvalPrecedenceByOperatorType[operatorType];
        Precedence = (int)precedence;
        _isProcessingOperand = precedence is EvalPrecedence.OperandUnaryOperator or EvalPrecedence.Exponentiation;
    }

    /// <inheritdoc/>
    public override double Evaluate(MathExpression mathExpression, ref int i, char? separator, char? closingSymbol, double left)
    {
        var start = i;
        i += Key.Length;
        var right = _isProcessingOperand
            ? mathExpression.EvaluateOperand(ref i, separator, closingSymbol)
            : mathExpression.Evaluate(ref i, separator, closingSymbol, Precedence);

        if (_isProcessingOperand)
            right = mathExpression.EvaluateExponentiation(ref i, separator, closingSymbol, right);

        return OperatorType switch
        {
            OperatorType.LogicalConditionalOr => left != default || right != default ? 1.0 : default,
            OperatorType.LogicalConditionalAnd => left != default && right != default ? 1.0 : default,
            OperatorType.LogicalOr => left != default || right != default ? 1.0 : default,
            OperatorType.BitwiseOr => (long)left | (long)right,
            OperatorType.LogicalXor => left != default ^ right != default ? 1.0 : default,
            OperatorType.BitwiseXor => (long)left ^ (long)right,
            OperatorType.LogicalAnd => left != default && right != default ? 1.0 : default,
            OperatorType.BitwiseAnd => (long)left & (long)right,
            OperatorType.LogicalNot or OperatorType.LogicalNegation => right == default ? 1.0 : default,
            OperatorType.BitwiseNegation => ~(long)right,
            OperatorType.Equal => left == right ? 1.0 : default,
            OperatorType.NotEqual => left != right ? 1.0 : default,
            OperatorType.LessThan => left < right ? 1.0 : default,
            OperatorType.LessThanOrEqual => left <= right ? 1.0 : default,
            OperatorType.GreaterThan => left > right ? 1.0 : default,
            OperatorType.GreaterThanOrEqual => left >= right ? 1.0 : default,
            OperatorType.Multiply => left * right,
            OperatorType.Divide => left / right,
            OperatorType.Add => left + right,
            OperatorType.Subtract => left - right,
            OperatorType.Modulo => left % right,
            OperatorType.Power => Math.Pow(left, right),
            _ => throw new MathExpressionException($"'{Key}' is not implemented.", start)
        };
    }

    /// <inheritdoc/>
    public override decimal Evaluate(MathExpression mathExpression, ref int i, char? separator, char? closingSymbol, decimal left)
    {
        var start = i;
        i += Key.Length;
        var right = _isProcessingOperand
            ? mathExpression.EvaluateOperandDecimal(ref i, separator, closingSymbol)
            : mathExpression.EvaluateDecimal(ref i, separator, closingSymbol, Precedence);

        if (_isProcessingOperand)
            right = mathExpression.EvaluateExponentiationDecimal(ref i, separator, closingSymbol, right);

        return OperatorType switch
        {
            OperatorType.LogicalConditionalOr => left != default || right != default ? 1.0m : default,
            OperatorType.LogicalConditionalAnd => left != default && right != default ? 1.0m : default,
            OperatorType.LogicalOr => left != default || right != default ? 1.0m : default,
            OperatorType.BitwiseOr => (long)left | (long)right,
            OperatorType.LogicalXor => left != default ^ right != default ? 1.0m : default,
            OperatorType.BitwiseXor => (long)left ^ (long)right,
            OperatorType.LogicalAnd => left != default && right != default ? 1.0m : default,
            OperatorType.BitwiseAnd => (long)left & (long)right,
            OperatorType.LogicalNot or OperatorType.LogicalNegation => right == default ? 1.0m : default,
            OperatorType.BitwiseNegation => ~(long)right,
            OperatorType.Equal => left == right ? 1.0m : default,
            OperatorType.NotEqual => left != right ? 1.0m : default,
            OperatorType.LessThan => left < right ? 1.0m : default,
            OperatorType.LessThanOrEqual => left <= right ? 1.0m : default,
            OperatorType.GreaterThan => left > right ? 1.0m : default,
            OperatorType.GreaterThanOrEqual => left >= right ? 1.0m : default,
            OperatorType.Multiply => left * right,
            OperatorType.Divide => left / right,
            OperatorType.Add => left + right,
            OperatorType.Subtract => left - right,
            OperatorType.Modulo => left % right,
            OperatorType.Power => (decimal)Math.Pow((double)left, (double)right),
            _ => throw new MathExpressionException($"'{Key}' is not implemented.", start)
        };
    }

    /// <inheritdoc/>
    public override Expression Build<TResult>(MathExpression mathExpression, ref int i, char? separator, char? closingSymbol, Expression left)
    {
        i += Key.Length;
        var right = _isProcessingOperand
            ? mathExpression.BuildOperand<TResult>(ref i, separator, closingSymbol)
            : mathExpression.Build<TResult>(ref i, separator, closingSymbol, Precedence);

        if (_isProcessingOperand)
            right = mathExpression.BuildExponentiation<TResult>(ref i, separator, closingSymbol, right);

        return Build<TResult>(left, right);
    }

    private Expression Build<TResult>(Expression left, Expression right)
    {
        if (ExpressionType is ExpressionType.AndAlso or ExpressionType.OrElse)
        {
            left = ConvertToBoolean<TResult>(left);
            right = ConvertToBoolean<TResult>(right);
        }

        if (ExpressionType is ExpressionType.And or ExpressionType.Or or ExpressionType.ExclusiveOr)
        {
            left = ConvertToLong(left);
            right = ConvertToLong(right);
        }

        if (ExpressionType is ExpressionType.Not)
        {
            right = OperatorType is OperatorType.BitwiseNegation
                ? ConvertToLong(right)
                : ConvertToBoolean<TResult>(right);
        }

        if (ExpressionType is ExpressionType.Power)
        {
            left = ConvertToDouble(left);
            right = ConvertToDouble(right);
        }

        var expression = ExpressionType is ExpressionType.Not
            ? Expression.MakeUnary(ExpressionType, right, null).Reduce()
            : Expression.MakeBinary(ExpressionType, left, right).Reduce();

        if (typeof(TResult) == expression.Type)
            return expression;

        return typeof(TResult) == typeof(decimal) && expression.Type == typeof(bool)
            ? Expression.Condition(expression, Expression.Constant(1.0m), Expression.Constant(0.0m))
            : Expression.Convert(expression, typeof(TResult)).Reduce();
    }

    private static Expression ConvertToBoolean<TResult>(Expression expression)
    {
        if (expression.Type != typeof(bool))
            expression = Expression.NotEqual(expression, typeof(TResult) == typeof(decimal)
                ? Expression.Constant(0.0m)
                : Expression.Constant(0.0));

        return expression;
    }

    private static Expression ConvertToLong(Expression expression)
    {
        if (expression.Type != typeof(bool))
            expression = Expression.Convert(expression, typeof(long));

        return expression;
    }

    private static Expression ConvertToDouble(Expression expression)
    {
        if (expression.Type != typeof(double))
            expression = Expression.Convert(expression, typeof(double));

        return expression;
    }
}
