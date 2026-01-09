using System;
using System.Linq.Expressions;
using System.Numerics;

namespace MathEvaluation.Entities;

/// <summary>
///     The math operator processes the left and right math operands.
/// </summary>
/// <typeparam name="T"></typeparam>
internal class MathOperandsOperator<T> : MathEntity
#if NET8_0_OR_GREATER
    where T : struct, INumberBase<T>
#else
    where T : struct
#endif
{
    /// <summary>Gets the function.</summary>
    /// <value>The function.</value>
    public Func<T, T, T> Fn { get; }

    /// <inheritdoc />
    public override int Precedence { get; }

    /// <summary>Initializes a new instance of the <see cref="MathOperandsOperator{T}" /> class.</summary>
    /// <param name="key">The key (the operator notation).</param>
    /// <param name="fn">The function.</param>
    /// <param name="precedence">The operator precedence.</param>
    /// <exception cref="ArgumentNullException" />
    public MathOperandsOperator(string? key, Func<T, T, T> fn, int precedence)
        : base(key)
    {
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
        Precedence = precedence;
    }

    /// <inheritdoc />
    public override double Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, double value)
    {
        if (typeof(T) == typeof(decimal))
            return (double)Evaluate(mathExpression, start, ref i, separator, closingSymbol, (decimal)value);

        i += Key.Length;
        var right = mathExpression.EvaluateOperand(ref i, separator, closingSymbol);
        right = mathExpression.EvaluateExponentiation(start, ref i, separator, closingSymbol, right);
        var result = Fn(
            value is T v1 ? v1 : (T)ChangeType(value, typeof(T)),
            right is T v2 ? v2 : (T)ChangeType(right, typeof(T)));

        mathExpression.OnEvaluating(start, i, result);

        return ConvertToDouble(result);
    }

    /// <inheritdoc />
    public override decimal Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, decimal value)
    {
        if (typeof(T) == typeof(double))
            return (decimal)Evaluate(mathExpression, start, ref i, separator, closingSymbol, (double)value);

        i += Key.Length;
        var right = mathExpression.EvaluateOperandDecimal(ref i, separator, closingSymbol);
        right = mathExpression.EvaluateExponentiationDecimal(start, ref i, separator, closingSymbol, right);
        var result = Fn(
            value is T v1 ? v1 : (T)ChangeType(value, typeof(T)),
            right is T v2 ? v2 : (T)ChangeType(right, typeof(T)));

        mathExpression.OnEvaluating(start, i, result);

        return ConvertToDecimal(result);
    }

#if NET8_0_OR_GREATER

    /// <inheritdoc />
    public override TResult Evaluate<TResult>(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, TResult value)
    {
        i += Key.Length;
        var right = mathExpression.EvaluateOperand<T>(ref i, separator, closingSymbol);
        right = mathExpression.EvaluateExponentiation(start, ref i, separator, closingSymbol, right);
        var result = Fn(ConvertNumber<TResult, T>(value), right);

        mathExpression.OnEvaluating(start, i, result);

        return ConvertNumber<T, TResult>(result);
    }

#endif

    /// <inheritdoc />
    public override Complex Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, Complex value)
    {
        i += Key.Length;
        var right = mathExpression.EvaluateOperandComplex(ref i, separator, closingSymbol);
        right = mathExpression.EvaluateExponentiationComplex(start, ref i, separator, closingSymbol, right);
        var result = Fn(
            value is T v1 ? v1 : (T)ChangeType(value, typeof(T)),
            right is T v2 ? v2 : (T)ChangeType(right, typeof(T)));

        mathExpression.OnEvaluating(start, i, result);

        return result is Complex r ? r : ConvertToDouble(result);
    }

    /// <inheritdoc />
    public override Expression Build<TResult>(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, Expression left)
    {
        i += Key.Length;

        left = BuildConvert<T>(left);
        var right = mathExpression.BuildOperand<T>(ref i, separator, closingSymbol);
        right = mathExpression.BuildExponentiation<T>(start, ref i, separator, closingSymbol, right);

        Expression result = Expression.Invoke(Expression.Constant(Fn), left, right);

        mathExpression.OnEvaluating(start, i, result);

        result = BuildConvert<TResult>(result);
        return result;
    }
}