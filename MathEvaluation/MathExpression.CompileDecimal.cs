using System;

namespace MathEvaluation;

public partial class MathExpression
{
    /// <inheritdoc cref="Compile{TResult}()"/>
    public Func<decimal> CompileDecimal()
        => Compile<decimal>();

    /// <inheritdoc cref="Compile{T, TResult}(T)"/>
    public Func<T, decimal> CompileDecimal<T>(T parameters)
        => Compile<T, decimal>(parameters);
}