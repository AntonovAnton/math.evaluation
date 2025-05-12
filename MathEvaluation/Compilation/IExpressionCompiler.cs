using System;
using System.Linq.Expressions;

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
}
