using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace MathEvaluation;

public partial class MathExpression
{
    /// <inheritdoc cref="Compile()" />
    [RequiresUnreferencedCode("Compiling expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Compiling expressions may require dynamic code generation")]
    public Func<Complex> CompileComplex()
        => Compile<Complex>();

    /// <inheritdoc cref="Compile{T}(T)" />
    [RequiresUnreferencedCode("Compiling expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Compiling expressions may require dynamic code generation")]
    public Func<T, Complex> CompileComplex<T>(T parameters)
        => Compile<T, Complex>(parameters);
}