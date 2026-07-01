using System.Linq.Expressions;
using System.Numerics;

namespace MathEvaluation.Entities;

/// <summary>
///     The math constant.
/// </summary>
/// <typeparam name="T"></typeparam>
internal class MathConstant<T> : MathEntity
    where T : struct, INumberBase<T>
{
    private readonly T _constantValue;

    /// <summary>
    ///     The math constant.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public MathConstant(string? key, T constantValue) : base(key)
    {
        _constantValue = constantValue;
    }

    /// <inheritdoc />
    public override int Precedence => (int)EvalPrecedence.Constant;

    /// <inheritdoc />
    public override TResult Evaluate<TResult>(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, TResult value)
    {
        var tokenPosition = i;
        i += Key.Length;

        var result = ConvertNumber<T, TResult>(_constantValue);
        mathExpression.OnEvaluating(tokenPosition, i, result);

        result = mathExpression.EvaluateExponentiation(tokenPosition, ref i, separator, closingSymbol, result);
        value = value == default ? result : value * result;

        if (value != result)
            mathExpression.OnEvaluating(start, i, value, skipNaN: true);

        return value;
    }

    /// <inheritdoc />
    public override Expression Build<TResult>(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, Expression left)
    {
        var tokenPosition = i;
        i += Key.Length;

        Expression right = Expression.Constant(_constantValue, typeof(T));
        right = BuildConvert<TResult>(right);
        mathExpression.OnEvaluating(tokenPosition, i, right);

        right = mathExpression.BuildExponentiation<TResult>(tokenPosition, ref i, separator, closingSymbol, right);
        var expression = MathExpression.BuildMultiplyIfLeftNotDefault<TResult>(left, right);

        if (expression != right)
            mathExpression.OnEvaluating(start, i, expression);

        return expression;
    }
}