using System;
using System.Linq.Expressions;
using MathEvaluation.Extensions;

namespace MathEvaluation.Entities;

/// <summary>
/// The getting value function.
/// </summary>
/// <typeparam name="T"></typeparam>
public class MathGetValueFunction<T> : MathEntity
    where T : struct, IConvertible
{
    /// <summary>Gets the getting value function.</summary>
    /// <value>The getting value function.</value>
    public Func<T> Fn { get; }

    /// <inheritdoc />
    public override int Precedence => (int)EvalPrecedence.Function;

    /// <summary>Initializes a new instance of the <see cref="MathGetValueFunction{T}" /> class.</summary>
    /// <param name="key">The key.</param>
    /// <param name="fn">The getting value function.</param>
    /// <exception cref="ArgumentNullException"/>
    public MathGetValueFunction(string? key, Func<T> fn)
        : base(key)
    {
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
    }

    /// <inheritdoc/>
    public override double Evaluate(MathExpression mathExpression, ref int i, char? separator, char? closingSymbol, double value)
    {
        i += Key.Length;
        mathExpression.MathString.SkipParenthesis(ref i);

        var result = Convert.ToDouble(Fn());
        result = mathExpression.EvaluateExponentiation(ref i, separator, closingSymbol, result);
        return value == default ? result : value * result;
    }

    /// <inheritdoc/>
    public override decimal Evaluate(MathExpression mathExpression, ref int i, char? separator, char? closingSymbol, decimal value)
    {
        i += Key.Length;
        mathExpression.MathString.SkipParenthesis(ref i);

        var result = Convert.ToDecimal(Fn());
        result = mathExpression.EvaluateExponentiationDecimal(ref i, separator, closingSymbol, result);
        return value == default ? result : value * result;
    }

    /// <inheritdoc/>
    public override Expression Build<TResult>(MathExpression mathExpression, ref int i, char? separator, char? closingSymbol, Expression left)
    {
        i += Key.Length;
        mathExpression.MathString.SkipParenthesis(ref i);

        Expression right = Expression.Invoke(Expression.Constant(Fn));
        right = right.Type != typeof(TResult) ? Expression.Convert(right, typeof(TResult)) : right;
        right = mathExpression.BuildExponentiation<TResult>(ref i, separator, closingSymbol, right);
        return left.IsZero() ? right : Expression.Multiply(left, right).Reduce();
    }
}
