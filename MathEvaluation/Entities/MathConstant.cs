using System.Linq.Expressions;
using System.Numerics;

namespace MathEvaluation.Entities;

/// <summary>
/// The math constant.
/// </summary>
/// <typeparam name="T"></typeparam>
public class MathConstant<T>(string? key, T value) : MathEntity(key)
    where T : struct
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

        mathExpression.OnEvaluating(tokenPosition, i, Value);

        var result = ConvertToDouble(Value);
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
        mathExpression.OnEvaluating(tokenPosition, i, Value);

        var result = ConvertToDecimal(Value);
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
        mathExpression.OnEvaluating(tokenPosition, i, Value);

        var result = Value is Complex v ? v : ConvertToDouble(Value);
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
        Expression right = Expression.Constant(Value);
        mathExpression.OnEvaluating(tokenPosition, i, right);

        right = BuildConvert<TResult>(right);
        right = mathExpression.BuildExponentiation<TResult>(tokenPosition, ref i, separator, closingSymbol, right);
        var expression = MathExpression.BuildMultipyIfLeftNotDefault<TResult>(left, right);

        if (expression != right)
            mathExpression.OnEvaluating(start, i, expression);
        return expression;
    }
}
