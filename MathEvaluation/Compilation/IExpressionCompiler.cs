namespace MathEvaluation.Compilation;

/// <summary>
/// Interface for compiling expressions.
/// </summary>
public interface IExpressionCompiler
{
    /// <summary>
    /// Compiles the specified <paramref name="expression"/> into a delegate.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value of the delegate.</typeparam>
    /// <param name="expression">The LINQ <see cref="Expression{TDelegate}"/> tree representing the parsed mathematical expression.</param>
    /// <returns>A delegate that represents the compiled expression.</returns>
    Func<TResult> Compile<TResult>(Expression<Func<TResult>> expression)
        where TResult : struct;

    /// <summary>
    /// Compiles the specified <paramref name="expression"/> with a parameter into a delegate.
    /// </summary>
    /// <typeparam name="T">The type of the parameter.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the delegate.</typeparam>
    /// <param name="expression">The LINQ <see cref="Expression{TDelegate}"/> tree representing the parsed mathematical expression.</param>
    /// <returns>A delegate that represents the compiled expression.</returns>
    Func<T, TResult> Compile<T, TResult>(Expression<Func<T, TResult>> expression)
        where TResult : struct;

    /// <summary>
    /// Compiles the specified <paramref name="expression"/> with two parameters into a delegate,
    /// it is used to compile a math expression with async functions and parameters (the parameters and the cancellation token).
    /// The default implementation uses the <see cref="LambdaExpression.Compile()" /> method.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the delegate.</typeparam>
    /// <param name="expression">The LINQ <see cref="Expression{TDelegate}"/> tree representing the parsed mathematical expression.</param>
    /// <returns>A delegate that represents the compiled expression.</returns>
    Func<T1, T2, TResult> Compile<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> expression)
        where TResult : struct
        => expression.Compile();
}