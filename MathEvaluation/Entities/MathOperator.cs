using System;
using System.Linq.Expressions;
using System.Numerics;

namespace MathEvaluation.Entities;

/// <summary>
///     The math operator processes the left and right expressions.
/// </summary>
/// <typeparam name="T"></typeparam>
internal class MathOperator<T> : MathEntity
    where T : struct, INumberBase<T>
{
    /// <summary>Gets the function.</summary>
    /// <value>The function.</value>
    public Func<T, T, T> Fn { get; }

    /// <inheritdoc />
    public override int Precedence { get; }

    /// <summary>Initializes a new instance of the <see cref="MathOperator{T}" /> class.</summary>
    /// <param name="key">The key (the operator notation).</param>
    /// <param name="fn">The function.</param>
    /// <param name="precedence">The operator precedence.</param>
    /// <exception cref="ArgumentNullException" />
    public MathOperator(string? key, Func<T, T, T> fn, int precedence = (int)EvalPrecedence.Basic)
        : base(key)
    {
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
        Precedence = precedence;
    }

    /// <inheritdoc />
    public override TResult Evaluate<TResult>(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, TResult value)
    {
        i += Key.Length;
        var left = ConvertNumber<TResult, T>(value);
        var right = mathExpression.Evaluate<T>(ref i, separator, closingSymbol, Precedence);
        var result = Fn(left, right);

        mathExpression.OnEvaluating(start, i, result);

        return ConvertNumber<T, TResult>(result);
    }

    /// <inheritdoc />
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