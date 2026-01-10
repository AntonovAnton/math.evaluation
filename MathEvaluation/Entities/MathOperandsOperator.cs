using System;
using System.Linq.Expressions;
using System.Numerics;

namespace MathEvaluation.Entities;

/// <summary>
///     The math operator processes the left and right math operands.
/// </summary>
/// <typeparam name="T"></typeparam>
internal class MathOperandsOperator<T> : MathEntity
    where T : struct, INumberBase<T>
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
    public override TResult Evaluate<TResult>(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, TResult value)
    {
        i += Key.Length;
        var right = mathExpression.EvaluateOperand<T>(ref i, separator, closingSymbol);
        right = mathExpression.EvaluateExponentiation(start, ref i, separator, closingSymbol, right);
        var result = Fn(ConvertNumber<TResult, T>(value), right);

        mathExpression.OnEvaluating(start, i, result);

        return ConvertNumber<T, TResult>(result);
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