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
    where T : struct
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
    public override double Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, double value)
    {
        if (typeof(T) == typeof(decimal))
            return (double)Evaluate(mathExpression, start, ref i, separator, closingSymbol, (decimal)value);

        var tokenPosition = i;
        i += Key.Length;
        if (OpeningSymbol.HasValue)
            mathExpression.MathString.ThrowExceptionIfNotOpened(OpeningSymbol.Value, tokenPosition, ref i);

        var arg = ClosingSymbol.HasValue
            ? mathExpression.Evaluate(ref i, null, ClosingSymbol)
            : mathExpression.EvaluateOperand(ref i, separator, closingSymbol);

        if (ClosingSymbol.HasValue)
            mathExpression.MathString.ThrowExceptionIfNotClosed(ClosingSymbol.Value, tokenPosition, ref i);

        var fnResult = Fn(arg is T a ? a : (T)ChangeType(arg, typeof(T)));
        mathExpression.OnEvaluating(tokenPosition, i, fnResult);

        var result = ConvertToDouble(fnResult);
        result = mathExpression.EvaluateExponentiation(tokenPosition, ref i, separator, closingSymbol, result);
        value = value == default ? result : value * result;

        if (value != result && !double.IsNaN(value))
            mathExpression.OnEvaluating(start, i, value);

        return value;
    }

    /// <inheritdoc/>
    public override decimal Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, decimal value)
    {
        if (typeof(T) == typeof(double))
            return (decimal)Evaluate(mathExpression, start, ref i, separator, closingSymbol, (double)value);

        var tokenPosition = i;
        i += Key.Length;
        if (OpeningSymbol.HasValue)
            mathExpression.MathString.ThrowExceptionIfNotOpened(OpeningSymbol.Value, tokenPosition, ref i);

        var arg = ClosingSymbol.HasValue
            ? mathExpression.EvaluateDecimal(ref i, null, ClosingSymbol)
            : mathExpression.EvaluateOperandDecimal(ref i, separator, closingSymbol);

        if (ClosingSymbol.HasValue)
            mathExpression.MathString.ThrowExceptionIfNotClosed(ClosingSymbol.Value, tokenPosition, ref i);

        var fnResult = Fn(arg is T a ? a : (T)ChangeType(arg, typeof(T)));
        mathExpression.OnEvaluating(tokenPosition, i, fnResult);

        var result = ConvertToDecimal(fnResult);
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
        if (OpeningSymbol.HasValue)
            mathExpression.MathString.ThrowExceptionIfNotOpened(OpeningSymbol.Value, tokenPosition, ref i);

        var arg = ClosingSymbol.HasValue
            ? mathExpression.EvaluateComplex(ref i, null, ClosingSymbol)
            : mathExpression.EvaluateOperandComplex(ref i, separator, closingSymbol);

        if (ClosingSymbol.HasValue)
            mathExpression.MathString.ThrowExceptionIfNotClosed(ClosingSymbol.Value, tokenPosition, ref i);

        var fnResult = Fn(arg is T a ? a : (T)ChangeType(arg, typeof(T)));
        mathExpression.OnEvaluating(tokenPosition, i, fnResult);

        var result = fnResult is Complex r ? r : ConvertToDouble(fnResult);
        result = mathExpression.EvaluateExponentiationComplex(tokenPosition, ref i, separator, closingSymbol, result);
        value = value == default ? result : value * result;

        if (value != result && !double.IsNaN(value.Real) && !double.IsNaN(value.Imaginary))
            mathExpression.OnEvaluating(start, i, value);

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
