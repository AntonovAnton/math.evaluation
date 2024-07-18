using System;

namespace MathEvaluation.Context;

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

    /// <summary>Gets the openning symbol.</summary>
    /// <value>The openning symbol.</value>
    public char OpenningSymbol { get; }

    /// <summary>Gets the parameters separator.</summary>
    /// <value>The parameters separator.</value>
    public char Separator { get; }

    /// <summary>Gets the closing symbol.</summary>
    /// <value>The closing symbol.</value>
    public char ClosingSymbol { get; }

    /// <summary>Initializes a new instance of the <see cref="MathFunction{T}" /> class.</summary>
    /// <param name="key">The key.</param>
    /// <param name="fn">The function.</param>
    /// <param name="openningSymbol">The openning symbol.</param>
    /// <param name="separator">The parameters separator.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <exception cref="System.ArgumentNullException">fn</exception>
    public MathFunction(string? key, Func<T[], T> fn, char openningSymbol, char separator, char closingSymbol)
        : base(key)
    {
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
        Separator = separator;
        OpenningSymbol = openningSymbol;
        ClosingSymbol = closingSymbol;
    }
}
