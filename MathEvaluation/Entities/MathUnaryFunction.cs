using System;
using System.Linq.Expressions;

namespace MathEvaluation.Entities;

/// <summary>
/// The function with one parameter, so opening and closing symbols are optional.
/// </summary>
/// <typeparam name="T"></typeparam>
public class MathUnaryFunction<T> : MathEntity
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
    public override Expression BuildExpression()
    {
        return Expression.Constant(Fn);
    }

    /// <inheritdoc cref="BuildExpression()"/>
    public Expression BuildExpression(Expression operand)
    {
        return Expression.Invoke(BuildExpression(), operand);
    }
}
