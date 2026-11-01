namespace MathEvaluation.Compilation;

/// <summary>
///     Continues the compiled math expression after a task, since expression trees can't contain 'await'.
/// </summary>
internal static class AsyncContinuation
{
    /// <summary>
    ///     Invokes the continuation with the result of the task after the task is completed,
    ///     unless the cancellation token is cancelled, so no async function is invoked after the cancellation.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="continuation">The continuation.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <typeparam name="T">The type of the result of the task.</typeparam>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <returns>The result of the continuation.</returns>
    /// <exception cref="OperationCanceledException" />
    public static ValueTask<TResult> Then<T, TResult>(ValueTask<T> task, Func<T, ValueTask<TResult>> continuation, CancellationToken cancellationToken)
    {
        if (!task.IsCompletedSuccessfully)
            return Await(task, continuation, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();
        return continuation(task.Result);
    }

    /// <summary>
    ///     Throws the <see cref="OperationCanceledException" /> if the cancellation token is cancelled.
    ///     The compiled expression calls this static method instead of the instance method of the token,
    ///     since FastExpressionCompiler emits invalid code for an instance method call on a captured struct parameter.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="OperationCanceledException" />
    public static void ThrowIfCancellationRequested(CancellationToken cancellationToken)
        => cancellationToken.ThrowIfCancellationRequested();

    /// <summary>Converts the result of the task to a number (1 for true, 0 for false), since a math expression calculates numbers.</summary>
    /// <param name="task">The task.</param>
    /// <returns>The task of the number.</returns>
    public static ValueTask<double> ToDouble(ValueTask<bool> task)
        => task.IsCompletedSuccessfully ? new ValueTask<double>(Convert.ToDouble(task.Result)) : AwaitToDouble(task);

    private static async ValueTask<double> AwaitToDouble(ValueTask<bool> task)
        => Convert.ToDouble(await task.ConfigureAwait(false));

    private static async ValueTask<TResult> Await<T, TResult>(ValueTask<T> task, Func<T, ValueTask<TResult>> continuation,
        CancellationToken cancellationToken)
    {
        var result = await task.ConfigureAwait(false);
        cancellationToken.ThrowIfCancellationRequested();
        return await continuation(result).ConfigureAwait(false);
    }
}
