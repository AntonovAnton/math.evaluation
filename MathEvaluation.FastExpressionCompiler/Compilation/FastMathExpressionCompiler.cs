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
}
