using FastExpressionCompiler;
using System;
using System.Linq.Expressions;

namespace MathEvaluation.Compilation;

/// <summary>
/// A compiler that uses <see href="https://github.com/dadhi/FastExpressionCompiler">FastExpressionCompiler</see> to compile <see cref="Expression">Expression</see> into delegates.
/// </summary>
public sealed class FastMathExpressionCompiler : IExpressionCompiler
{
    /// <inheritdoc />
    public Func<TResult> Compile<TResult>(Expression<Func<TResult>> expression) where TResult : struct
    {
        return expression.CompileFast();
    }

    /// <inheritdoc />
    public Func<T, TResult> Compile<T, TResult>(Expression<Func<T, TResult>> expression) where TResult : struct
    {
        return expression.CompileFast();
    }

    /// <summary>
    /// Compiles the specified <paramref name="expression"/> with two parameters into a delegate,
    /// it is used to compile a math expression with async functions and parameters (the parameters and the cancellation token).
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the delegate.</typeparam>
    /// <param name="expression">The LINQ <see cref="Expression{TDelegate}"/> tree representing the parsed mathematical expression.</param>
    /// <returns>A delegate that represents the compiled expression.</returns>
    public Func<T1, T2, TResult> Compile<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> expression) where TResult : struct
    {
        return expression.CompileFast();
    }
}