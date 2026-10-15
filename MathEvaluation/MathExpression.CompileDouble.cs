using System;
using System.Diagnostics.CodeAnalysis;

namespace MathEvaluation;

public partial class MathExpression
{
    /// <inheritdoc cref="Compile{TResult}()" />
    [RequiresUnreferencedCode("Compiling expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Compiling expressions may require dynamic code generation")]
    public Func<double> Compile()
        => Compile<double>();

    /// <inheritdoc cref="Compile{T, TResult}(T)" />
    [RequiresUnreferencedCode("Compiling expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Compiling expressions may require dynamic code generation")]
    public Func<T, double> Compile<T>(T parameters)
        => Compile<T, double>(parameters);
}