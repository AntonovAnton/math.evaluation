using MathEvaluation.Extensions;
using System;
using System.Linq.Expressions;

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
    public override double Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, double value)
    {
        var tokenPosition = i;
        i += Key.Length;
        var result = Value is double v ? v : Convert.ToDouble(Value);

        mathExpression.OnEvaluating(tokenPosition, i, result);

        result = mathExpression.EvaluateExponentiation(tokenPosition, ref i, separator, closingSymbol, result);
        value = value == default ? result : value * result;

        if (value != result && !double.IsNaN(value))
            mathExpression.OnEvaluating(start, i, value);
        return value;
    }

    /// <inheritdoc/>
    public override decimal Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, decimal value)
    {
        var tokenPosition = i;
        i += Key.Length;
        var result = Value is decimal v ? v : Convert.ToDecimal(Value);
        mathExpression.OnEvaluating(tokenPosition, i, result);

        result = mathExpression.EvaluateExponentiationDecimal(tokenPosition, ref i, separator, closingSymbol, result);
        value = value == default ? result : value * result;

        if (value != result)
            mathExpression.OnEvaluating(start, i, value);
        return value;
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
