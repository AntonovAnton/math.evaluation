using MathEvaluation.Extensions;
using System;
using System.Linq.Expressions;
using System.Numerics;

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
    public override double Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, double value)
    {
        var tokenPosition = i;
        i += Key.Length;
        var result = ConvertToDouble(Value);

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
        var result = ConvertToDecimal(Value);

        mathExpression.OnEvaluating(tokenPosition, i, result);

        result = mathExpression.EvaluateExponentiationDecimal(tokenPosition, ref i, separator, closingSymbol, result);
        value = value == default ? result : value * result;

        if (value != result)
            mathExpression.OnEvaluating(start, i, value);

        return value;
    }

    /// <inheritdoc/>
    public override Complex Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, Complex value)
    {
        var tokenPosition = i;
        i += Key.Length;
        var result = Value is Complex v ? v : ConvertToDouble(Value);

        mathExpression.OnEvaluating(tokenPosition, i, result);

        result = mathExpression.EvaluateExponentiationComplex(tokenPosition, ref i, separator, closingSymbol, result);
        value = value == default ? result : value * result;

        if (value != result && !double.IsNaN(value.Real))
            mathExpression.OnEvaluating(start, i, value);

        return value;
    }

    /// <inheritdoc/>
    public override Expression Build<TResult>(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, Expression left)
    {
        var tokenPosition = i;
        i += Key.Length;

        Expression right = Expression.Property(mathExpression.ParameterExpression, Key);
        right = right.Type != typeof(T) ? Expression.Convert(right, typeof(T)) : right;
        mathExpression.OnEvaluating(tokenPosition, i, right);

        right = right.Type != typeof(TResult) ? Expression.Convert(right, typeof(TResult)) : right;
        right = mathExpression.BuildExponentiation<TResult>(tokenPosition, ref i, separator, closingSymbol, right);
        var expression = left.IsZero() ? right : Expression.Multiply(left, right).Reduce();

        if (expression != right)
            mathExpression.OnEvaluating(start, i, expression);

        return expression;
    }
}
