using MathEvaluation.Extensions;
using System;
using System.Linq.Expressions;

namespace MathEvaluation.Entities;

/// <summary>
/// The function with one parameter, so opening and closing symbols are optional.
/// </summary>
/// <typeparam name="T"></typeparam>
public class MathUnaryFunction<T> : MathEntity
    where T : struct, IConvertible
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
    public override double Evaluate(MathExpression mathExpression, ref int i, char? separator, char? closingSymbol, double value)
    {
        if (typeof(T) == typeof(decimal))
            return (double)Evaluate(mathExpression, ref i, separator, closingSymbol, (decimal)value);

        var start = i;
        i += Key.Length;
        if (OpeningSymbol.HasValue)
            mathExpression.MathString.ThrowExceptionIfNotOpened(OpeningSymbol.Value, start, ref i);

        var arg = ClosingSymbol.HasValue
            ? mathExpression.Evaluate(ref i, null, ClosingSymbol)
            : mathExpression.EvaluateOperand(ref i, separator, closingSymbol);

        if (ClosingSymbol.HasValue)
            mathExpression.MathString.ThrowExceptionIfNotClosed(ClosingSymbol.Value, start, ref i);

        var result = Convert.ToDouble(Fn(arg is T a ? a : (T)Convert.ChangeType(arg, typeof(T))));
        result = mathExpression.EvaluateExponentiation(ref i, separator, closingSymbol, result);
        return value == default ? result : value * result;
    }

    /// <inheritdoc/>
    public override decimal Evaluate(MathExpression mathExpression, ref int i, char? separator, char? closingSymbol, decimal value)
    {
        if (typeof(T) == typeof(double))
            return (decimal)Evaluate(mathExpression, ref i, separator, closingSymbol, (double)value);

        var start = i;
        i += Key.Length;
        if (OpeningSymbol.HasValue)
            mathExpression.MathString.ThrowExceptionIfNotOpened(OpeningSymbol.Value, start, ref i);

        var arg = ClosingSymbol.HasValue
            ? mathExpression.EvaluateDecimal(ref i, null, ClosingSymbol)
            : mathExpression.EvaluateOperandDecimal(ref i, separator, closingSymbol);

        if (ClosingSymbol.HasValue)
            mathExpression.MathString.ThrowExceptionIfNotClosed(ClosingSymbol.Value, start, ref i);

        var result = Convert.ToDecimal(Fn(arg is T a ? a : (T)Convert.ChangeType(arg, typeof(T))));
        result = mathExpression.EvaluateExponentiationDecimal(ref i, separator, closingSymbol, result);
        return value == default ? result : value * result;
    }

    /// <inheritdoc/>
    public override Expression Build<TResult>(MathExpression mathExpression, ref int i, char? separator, char? closingSymbol, Expression left)
    {
        var start = i;
        i += Key.Length;
        if (OpeningSymbol.HasValue)
            mathExpression.MathString.ThrowExceptionIfNotOpened(OpeningSymbol.Value, start, ref i);

        var arg = ClosingSymbol.HasValue
            ? mathExpression.Build<T>(ref i, null, ClosingSymbol)
            : mathExpression.BuildOperand<T>(ref i, separator, closingSymbol);

        if (ClosingSymbol.HasValue)
            mathExpression.MathString.ThrowExceptionIfNotClosed(ClosingSymbol.Value, start, ref i);

        Expression right = Expression.Invoke(Expression.Constant(Fn), arg);
        right = right.Type != typeof(TResult) ? Expression.Convert(right, typeof(TResult)) : right;
        right = mathExpression.BuildExponentiation<TResult>(ref i, separator, closingSymbol, right);
        return left.IsZero() ? right : Expression.Multiply(left, right).Reduce();
    }
}
