using MathEvaluation.Extensions;
using System;
using System.Linq.Expressions;

namespace MathEvaluation.Entities;

/// <summary>
/// The math variable uses as a parameter.
/// </summary>
/// <typeparam name="T"></typeparam>
public class MathVariable<T>(string? key, T value) : MathEntity(key)
    where T : struct
{
    /// <inheritdoc />
    public override int Precedence => (int)EvalPrecedence.Variable;

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

        Expression right = Expression.Property(mathExpression.ParameterExpression, Key);
        right = right.Type != typeof(T) ? Expression.Convert(right, typeof(T)) : right;
        right = right.Type != typeof(TResult) ? Expression.Convert(right, typeof(TResult)) : right;
        right = mathExpression.BuildExponentiation<TResult>(ref i, separator, closingSymbol, right);
        return left.IsZero() ? right : Expression.Multiply(left, right).Reduce();
    }
}
