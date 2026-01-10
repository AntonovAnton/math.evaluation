using MathEvaluation.Extensions;
using System;
using System.Linq.Expressions;
using System.Numerics;

namespace MathEvaluation.Entities;

/// <summary>
/// The function with one parameter, so opening and closing symbols are optional.
/// </summary>
/// <typeparam name="T"></typeparam>
internal class MathUnaryFunction<T> : MathEntity
    where T : struct, INumberBase<T>
{
    /// <summary>Gets the function.</summary>
    /// <value>The function.</value>
    public Func<T, T> Fn { get; }

    /// <summary>Gets the opening symbol.</summary>
    /// <value>The opening symbol.</value>
    public char? OpeningSymbol { get; }

    /// <summary>Gets the closing symbol.</summary>
    /// <value>The closing symbol.</value>
    public char? ClosingSymbol { get; }

    /// <inheritdoc />
    public override int Precedence => (int)EvalPrecedence.Function;

    /// <summary>Initializes a new instance of the <see cref="MathUnaryFunction{T}" /> class.</summary>
    /// <param name="key">The key.</param>
    /// <param name="fn">The function.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <exception cref="ArgumentNullException"/>
    public MathUnaryFunction(string? key, Func<T, T> fn, char? openingSymbol = null, char? closingSymbol = null)
        : base(key)
    {
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
        ClosingSymbol = closingSymbol;
        OpeningSymbol = openingSymbol;
    }

    /// <inheritdoc/>
    public override TResult Evaluate<TResult>(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, TResult value)
    {
        var tokenPosition = i;
        i += Key.Length;
        if (OpeningSymbol.HasValue)
            mathExpression.MathString.ThrowExceptionIfNotOpened(OpeningSymbol.Value, tokenPosition, ref i);

        var arg = ClosingSymbol.HasValue
            ? mathExpression.Evaluate<T>(ref i, null, ClosingSymbol)
            : mathExpression.EvaluateOperand<T>(ref i, separator, closingSymbol);

        if (ClosingSymbol.HasValue)
            mathExpression.MathString.ThrowExceptionIfNotClosed(ClosingSymbol.Value, tokenPosition, ref i);

        var fnResult = Fn(arg);
        mathExpression.OnEvaluating(tokenPosition, i, fnResult);

        var result = ConvertNumber<T, TResult>(fnResult);
        result = mathExpression.EvaluateExponentiation(tokenPosition, ref i, separator, closingSymbol, result);
        value = value == default ? result : value * result;

        if (value != result)
            mathExpression.OnEvaluating(start, i, value, skipNaN: true);

        return value;
    }

    /// <inheritdoc/>
    public override Expression Build<TResult>(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, Expression left)
    {
        var tokenPosition = i;
        i += Key.Length;
        if (OpeningSymbol.HasValue)
            mathExpression.MathString.ThrowExceptionIfNotOpened(OpeningSymbol.Value, tokenPosition, ref i);

        var arg = ClosingSymbol.HasValue
            ? mathExpression.Build<T>(ref i, null, ClosingSymbol)
            : mathExpression.BuildOperand<T>(ref i, separator, closingSymbol);

        if (ClosingSymbol.HasValue)
            mathExpression.MathString.ThrowExceptionIfNotClosed(ClosingSymbol.Value, tokenPosition, ref i);

        Expression right = Expression.Invoke(Expression.Constant(Fn), arg);
        mathExpression.OnEvaluating(tokenPosition, i, right);

        right = BuildConvert<TResult>(right);
        right = mathExpression.BuildExponentiation<TResult>(tokenPosition, ref i, separator, closingSymbol, right);
        var expression = MathExpression.BuildMultiplyIfLeftNotDefault<TResult>(left, right);

        if (expression != right)
            mathExpression.OnEvaluating(start, i, expression);

        return expression;
    }
}
