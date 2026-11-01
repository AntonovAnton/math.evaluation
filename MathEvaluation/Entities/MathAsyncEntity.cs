namespace MathEvaluation.Entities;

/// <summary>
///     Base class for a math async function. It is supported only by compiling the math expression into an async delegate.
/// </summary>
/// <typeparam name="T"></typeparam>
internal abstract class MathAsyncEntity<T> : MathEntity
    where T : struct, INumberBase<T>
{
    /// <inheritdoc />
    public override int Precedence => (int)EvalPrecedence.Function;

    /// <summary>Initializes a new instance of the <see cref="MathAsyncEntity{T}" /> class.</summary>
    /// <param name="key">The key.</param>
    /// <exception cref="ArgumentNullException" />
    protected MathAsyncEntity(string? key)
        : base(key)
    {
    }

    /// <inheritdoc />
    public override TResult Evaluate<TResult>(MathExpression mathExpression, int start, ref int i, int depth, char? separator, char? closingSymbol,
        TResult value)
        => throw new NotSupportedException($"The async function '{Key}' can't be evaluated synchronously, use the CompileAsyncFunc method instead.");

    /// <summary>Gets the cancellation token of the compiled async delegate, it is passed to the async function as the last argument.</summary>
    /// <param name="mathExpression">The math expression.</param>
    /// <returns>
    ///     The cancellation token parameter, or <see cref="CancellationToken.None" /> if the math expression isn't compiled
    ///     into an async delegate (the compilation fails then anyway).
    /// </returns>
    protected static Expression GetCancellationToken(MathExpression mathExpression)
        => mathExpression.CancellationTokenExpression ?? (Expression)Expression.Constant(CancellationToken.None);

    /// <summary>
    ///     Adds to await of the task before the math expression (like: 'await f(a, b)') and builds the expression that uses its result.
    /// </summary>
    /// <param name="mathExpression">The math expression.</param>
    /// <param name="start">The start position of the expression in the math expression string.</param>
    /// <param name="i">The current char index, it is just after the function call.</param>
    /// <param name="depth">The recursion depth.</param>
    /// <param name="separator">The parameters' separator.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <param name="left">The left expression.</param>
    /// <param name="tokenPosition">The position of the function call in the math expression string.</param>
    /// <param name="task">The expression that returns a <see cref="System.Threading.Tasks.ValueTask{TResult}" />.</param>
    [RequiresUnreferencedCode("Building expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Building expressions in some cases requires dynamic code generation")]
    protected Expression BuildAwait<TResult>(MathExpression mathExpression, int start, ref int i, int depth, char? separator, char? closingSymbol,
        Expression left, int tokenPosition, Expression task)
        where TResult : struct, INumberBase<TResult>
    {
        mathExpression.Statements ??= new ExpressionStatements();
        Expression right = mathExpression.Statements.AddAwait(mathExpression.MathString[tokenPosition..i], task);
        mathExpression.RaiseEvaluatingStep(tokenPosition, i, right);

        right = BuildConvert<TResult>(right);
        right = mathExpression.BuildExponentiation<TResult>(tokenPosition, ref i, depth, separator, closingSymbol, right);
        var expression = MathExpression.BuildMultiplyIfLeftNotDefault<TResult>(left, right);

        if (expression != right)
            mathExpression.RaiseEvaluatingStep(start, i, expression);

        return expression;
    }
}