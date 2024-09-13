using System;
using System.Linq.Expressions;
using MathEvaluation.Extensions;

namespace MathEvaluation.Entities;

/// <summary>
/// The math constant.
/// </summary>
/// <typeparam name="T"></typeparam>
public class MathConstant<T>(string? key, T value) : MathEntity(key)
    where T : struct, IConvertible
{
    /// <inheritdoc />
    public override int Precedence => (int)EvalPrecedence.Constant;

    /// <summary>Gets the value.</summary>
    /// <value>The value.</value>
    public T Value { get; } = value;

    /// <inheritdoc/>
    public override double Evaluate(MathExpression mathExpression, ref int i, char? separator, char? closingSymbol, double value)
    {
        i += Key.Length;
        var result = Value is double v ? v : Convert.ToDouble(Value);
        result = mathExpression.EvaluateExponentiation(ref i, separator, closingSymbol, result);
        return value == default ? result : value * result;
    }

    /// <inheritdoc/>
    public override decimal Evaluate(MathExpression mathExpression, ref int i, char? separator, char? closingSymbol, decimal value)
    {
        i += Key.Length;
        var result = Value is decimal v ? v : Convert.ToDecimal(Value);
        result = mathExpression.EvaluateExponentiationDecimal(ref i, separator, closingSymbol, result);
        return value == default ? result : value * result;
    }

    /// <inheritdoc/>
    public override Expression Build<TResult>(MathExpression mathExpression, ref int i, char? separator, char? closingSymbol, Expression left)
    {
        i += Key.Length;
        Expression right = Expression.Constant(Value);
        right = Value is not TResult ? Expression.Convert(right, typeof(TResult)) : right;
        right = mathExpression.BuildExponentiation<TResult>(ref i, separator, closingSymbol, right);
        return left.IsZero() ? right : Expression.Multiply(left, right).Reduce();
    }
}
