namespace MathEvaluation.Entities;

/// <summary>
///     The async function with one parameter, so opening and closing symbols are optional.
///     It is supported only by compiling the math expression into an async delegate.
/// </summary>
/// <typeparam name="T"></typeparam>
internal class MathAsyncUnaryFunction<T> : MathAsyncEntity<T>
    where T : struct, INumberBase<T>
{
    /// <summary>Gets the function.</summary>
    /// <value>The function.</value>
    private Func<T, CancellationToken, ValueTask<T>> Fn { get; }

    /// <summary>Gets the opening symbol.</summary>
    /// <value>The opening symbol.</value>
    private char? OpeningSymbol { get; }

    /// <summary>Gets the closing symbol.</summary>
    /// <value>The closing symbol.</value>
    private char? ClosingSymbol { get; }

    /// <summary>Initializes a new instance of the <see cref="MathAsyncUnaryFunction{T}" /> class.</summary>
    /// <param name="key">The key.</param>
    /// <param name="fn">The function.</param>
    /// <param name="openingSymbol">The opening symbol.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <exception cref="ArgumentNullException" />
    public MathAsyncUnaryFunction(string? key, Func<T, CancellationToken, ValueTask<T>> fn, char? openingSymbol = null, char? closingSymbol = null)
        : base(key)
    {
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
        ClosingSymbol = closingSymbol;
        OpeningSymbol = openingSymbol;
    }

    /// <inheritdoc />
    [RequiresUnreferencedCode("Building expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Building expressions in some cases requires dynamic code generation")]
    public override Expression Build<TResult>(MathExpression mathExpression, int start, ref int i, int depth, char? separator, char? closingSymbol,
        Expression left)
    {
        var tokenPosition = i;
        i += Key.Length;
        if (OpeningSymbol.HasValue)
            mathExpression.MathString.ThrowExceptionIfNotOpened(OpeningSymbol.Value, tokenPosition, ref i);

        var arg = ClosingSymbol.HasValue
            ? mathExpression.Build<T>(ref i, depth, null, ClosingSymbol)
            : mathExpression.BuildOperand<T>(ref i, depth, separator, closingSymbol);

        if (ClosingSymbol.HasValue)
            mathExpression.MathString.ThrowExceptionIfNotClosed(ClosingSymbol.Value, tokenPosition, ref i);

        var task = Expression.Invoke(Expression.Constant(Fn), arg, GetCancellationToken(mathExpression));
        return BuildAwait<TResult>(mathExpression, start, ref i, depth, separator, closingSymbol, left, tokenPosition, task);
    }
}