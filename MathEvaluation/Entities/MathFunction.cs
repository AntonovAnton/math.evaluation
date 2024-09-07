using System;
using System.Linq.Expressions;

namespace MathEvaluation.Entities;

/// <summary>
/// The function with multiple parameters, so opening, separator, and closing symbol are required.
/// </summary>
/// <typeparam name="T"></typeparam>
public class MathFunction<T> : MathEntity
    where T : struct
{
    /// <summary>Gets the function.</summary>
    /// <value>The function.</value>
    public Func<T[], T> Fn { get; }

    /// <summary>Gets the opening symbol.</summary>
    /// <value>The opening symbol.</value>
    public char OpeningSymbol { get; }

    /// <summary>Gets the parameters separator.</summary>
    /// <value>The parameters separator.</value>
    public char Separator { get; }

    /// <summary>Gets the closing symbol.</summary>
    /// <value>The closing symbol.</value>
    public char ClosingSymbol { get; }

    /// <inheritdoc />
    public override int Precedence => (int)EvalPrecedence.Function;

    /// <summary>Initializes a new instance of the <see cref="MathFunction{T}" /> class.</summary>
    /// <param name="key">The key.</param>
    /// <param name="fn">The function.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="separator">The parameters separator.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <exception cref="ArgumentNullException"/>
    public MathFunction(string? key, Func<T[], T> fn, char openingSymbol, char separator, char closingSymbol)
        : base(key)
    {
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
        Separator = separator;
        OpeningSymbol = openingSymbol;
        ClosingSymbol = closingSymbol;
    }

    /// <inheritdoc/>
    public override Expression BuildExpression()
    {
        return Expression.Constant(Fn);
    }
}
