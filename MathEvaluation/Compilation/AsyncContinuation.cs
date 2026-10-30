using System;
using System.Threading.Tasks;

namespace MathEvaluation.Compilation;

/// <summary>
///     Continues the compiled math expression after a task, since expression trees can't contain 'await'.
/// </summary>
internal static class AsyncContinuation
{
    /// <summary>Invokes the continuation with the result of the task after the task is completed.</summary>
    /// <param name="task">The task.</param>
    /// <param name="continuation">The continuation.</param>
    /// <typeparam name="T">The type of the result of the task.</typeparam>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <returns>The result of the continuation.</returns>
    public static ValueTask<TResult> Then<T, TResult>(ValueTask<T> task, Func<T, ValueTask<TResult>> continuation)
        => task.IsCompletedSuccessfully ? continuation(task.Result) : Await(task, continuation);

    private static async ValueTask<TResult> Await<T, TResult>(ValueTask<T> task, Func<T, ValueTask<TResult>> continuation)
        => await continuation(await task.ConfigureAwait(false)).ConfigureAwait(false);
}
