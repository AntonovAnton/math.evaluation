using System;
using System.Linq.Expressions;

namespace MathEvaluation.Entities;

/// <summary>
/// The math operator processes the left and right expressions.
/// </summary>
/// <typeparam name="T"></typeparam>
public class MathOperator<T> : MathEntity
    where T : struct, IConvertible
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
        value = Convert.ToDouble(Fn(
            value is T v ? v : (T)Convert.ChangeType(value, typeof(T)),
            right is T r ? r : (T)Convert.ChangeType(right, typeof(T))));

        mathExpression.OnEvaluating(start, i, value);
        return value;
    }

    /// <inheritdoc/>
    public override decimal Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, decimal value)
    {
        if (typeof(T) == typeof(double))
            return (decimal)Evaluate(mathExpression, start, ref i, separator, closingSymbol, (double)value);

        i += Key.Length;
        var right = mathExpression.EvaluateDecimal(ref i, separator, closingSymbol, Precedence);
        value = Convert.ToDecimal(Fn(
            value is T v ? v : (T)Convert.ChangeType(value, typeof(T)),
            right is T r ? r : (T)Convert.ChangeType(right, typeof(T))));

        mathExpression.OnEvaluating(start, i, value);
        return value;
    }

    /// <inheritdoc/>
    public override Expression Build<TResult>(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, Expression left)
    {
        i += Key.Length;
        left = left.Type != typeof(T) ? Expression.Convert(left, typeof(T)) : left;
        var right = mathExpression.Build<T>(ref i, separator, closingSymbol, Precedence);

        Expression result = Expression.Invoke(Expression.Constant(Fn), left, right);
        result = result.Type != typeof(TResult) ? Expression.Convert(result, typeof(TResult)) : result;

        mathExpression.OnEvaluating(start, i, result);
        return result;
    }
}
