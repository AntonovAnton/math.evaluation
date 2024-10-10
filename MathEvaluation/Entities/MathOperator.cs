using System;
using System.Linq.Expressions;
using System.Numerics;

namespace MathEvaluation.Entities;

/// <summary>
/// The math operator processes the left and right expressions.
/// </summary>
/// <typeparam name="T"></typeparam>
public class MathOperator<T> : MathEntity
    where T : struct
{
    /// <summary>Gets the function.</summary>
    /// <value>The function.</value>
    public Func<T, T, T> Fn { get; }

    /// <inheritdoc />
    public override int Precedence { get; }

    /// <summary>Initializes a new instance of the <see cref="MathOperator{T}" /> class.</summary>
    /// <param name="key">The key (the operator notation).</param>
    /// <param name="fn">The function.</param>
    /// <param name="precedece">The operator precedence.</param>
    /// <exception cref="ArgumentNullException"/>
    public MathOperator(string? key, Func<T, T, T> fn, int precedece = (int)EvalPrecedence.Basic)
        : base(key)
    {
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
        Precedence = precedece;
    }

    /// <inheritdoc/>
    public override double Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, double value)
    {
        if (typeof(T) == typeof(decimal))
            return (double)Evaluate(mathExpression, start, ref i, separator, closingSymbol, (decimal)value);

        i += Key.Length;
        var right = mathExpression.Evaluate(ref i, separator, closingSymbol, Precedence);
        var result = Fn(
            value is T v1 ? v1 : (T)ChangeType(value, typeof(T)),
            right is T v2 ? v2 : (T)ChangeType(right, typeof(T)));

        mathExpression.OnEvaluating(start, i, result);

        var dResult = ConvertToDouble(result);
        return dResult;
    }

    /// <inheritdoc/>
    public override decimal Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, decimal value)
    {
        if (typeof(T) == typeof(double))
            return (decimal)Evaluate(mathExpression, start, ref i, separator, closingSymbol, (double)value);

        i += Key.Length;
        var right = mathExpression.EvaluateDecimal(ref i, separator, closingSymbol, Precedence);
        var result = Fn(
            value is T v1 ? v1 : (T)ChangeType(value, typeof(T)),
            right is T v2 ? v2 : (T)ChangeType(right, typeof(T)));

        mathExpression.OnEvaluating(start, i, result);

        var dResult = ConvertToDecimal(result);
        return dResult;
    }

    /// <inheritdoc/>
    public override Complex Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, Complex value)
    {
        i += Key.Length;
        var right = mathExpression.EvaluateComplex(ref i, separator, closingSymbol, Precedence);
        var result = Fn(
            value is T v1 ? v1 : (T)ChangeType(value, typeof(T)),
            right is T v2 ? v2 : (T)ChangeType(right, typeof(T)));

        mathExpression.OnEvaluating(start, i, result);

        var dResult = result is Complex r ? r : ConvertToDouble(result);
        return dResult;
    }

    /// <inheritdoc/>
    public override Expression Build<TResult>(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, Expression left)
    {
        i += Key.Length;

        left = BuildConvert<T>(left);
        var right = mathExpression.Build<T>(ref i, separator, closingSymbol, Precedence);
        Expression result = Expression.Invoke(Expression.Constant(Fn), left, right);

        mathExpression.OnEvaluating(start, i, result);

        result = BuildConvert<TResult>(result);
        return result;
    }
}
