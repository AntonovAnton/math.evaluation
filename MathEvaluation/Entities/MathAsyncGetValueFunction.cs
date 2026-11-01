namespace MathEvaluation.Entities;

/// <summary>
///     The async getting value function, the parentheses are optional.
///     It is supported only by compiling the math expression into an async delegate.
/// </summary>
/// <typeparam name="T"></typeparam>
internal class MathAsyncGetValueFunction<T> : MathAsyncEntity<T>
    where T : struct, INumberBase<T>
{
    /// <summary>Gets the getting value function.</summary>
    /// <value>The getting value function.</value>
    private Func<CancellationToken, ValueTask<T>> Fn { get; }

    /// <summary>Initializes a new instance of the <see cref="MathAsyncGetValueFunction{T}" /> class.</summary>
    /// <param name="key">The key.</param>
    /// <param name="fn">The getting value function.</param>
    /// <exception cref="ArgumentNullException" />
    public MathAsyncGetValueFunction(string? key, Func<CancellationToken, ValueTask<T>> fn)
        : base(key)
    {
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
    }

    /// <inheritdoc />
    [RequiresUnreferencedCode("Building expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Building expressions in some cases requires dynamic code generation")]
    public override Expression Build<TResult>(MathExpression mathExpression, int start, ref int i, int depth, char? separator, char? closingSymbol,
        Expression left)
    {
        var tokenPosition = i;
        i += Key.Length;
        mathExpression.MathString.SkipParenthesis(ref i);

        var task = Expression.Invoke(Expression.Constant(Fn), GetCancellationToken(mathExpression));
        return BuildAwait<TResult>(mathExpression, start, ref i, depth, separator, closingSymbol, left, tokenPosition, task);
    }
}