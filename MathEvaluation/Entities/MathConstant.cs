using System.Linq.Expressions;
using System.Numerics;

namespace MathEvaluation.Entities;

/// <summary>
///     The math constant.
/// </summary>
/// <typeparam name="T"></typeparam>
internal class MathConstant<T>(string? key, T value) : MathEntity(key)
#if NET8_0_OR_GREATER
    where T : struct, INumberBase<T>
#else
    where T : struct
#endif
{
    /// <inheritdoc />
    public override int Precedence => (int)EvalPrecedence.Constant;

    /// <summary>Gets the value.</summary>
    /// <value>The value.</value>
    public T Value { get; } = value;

    /// <inheritdoc />
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

    /// <inheritdoc />
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

#if NET8_0_OR_GREATER

    /// <inheritdoc />
    public override TResult Evaluate<TResult>(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, TResult value)
    {
        var tokenPosition = i;
        i += Key.Length;

        var result = ConvertNumber<T, TResult>(Value);
        mathExpression.OnEvaluating(tokenPosition, i, result);

        result = mathExpression.EvaluateExponentiation(tokenPosition, ref i, separator, closingSymbol, result);
        value = value == default ? result : value * result;

        if (value != result && !(value is Complex c && (double.IsNaN(c.Real) || double.IsNaN(c.Imaginary))))
            mathExpression.OnEvaluating(start, i, value);

        return value;
    }

#endif

    /// <inheritdoc />
    public override Complex Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, Complex value)
    {
        var tokenPosition = i;
        i += Key.Length;

        var result = Value is Complex v ? v : ConvertToDouble(Value);
        mathExpression.OnEvaluating(tokenPosition, i, result);

        result = mathExpression.EvaluateExponentiationComplex(tokenPosition, ref i, separator, closingSymbol, result);
        value = value == default ? result : value * result;

        if (value != result && !double.IsNaN(value.Real) && !double.IsNaN(value.Imaginary))
            mathExpression.OnEvaluating(start, i, value);

        return value;
    }

    /// <inheritdoc />
    public override Expression Build<TResult>(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, Expression left)
    {
        var tokenPosition = i;
        i += Key.Length;

        Expression right = Expression.Constant(Value);
        right = BuildConvert<TResult>(right);
        mathExpression.OnEvaluating(tokenPosition, i, right);

        right = mathExpression.BuildExponentiation<TResult>(tokenPosition, ref i, separator, closingSymbol, right);
        var expression = MathExpression.BuildMultiplyIfLeftNotDefault<TResult>(left, right);

        if (expression != right)
            mathExpression.OnEvaluating(start, i, expression);

        return expression;
    }
}