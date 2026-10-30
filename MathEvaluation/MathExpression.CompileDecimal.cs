using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MathEvaluation;

public partial class MathExpression
{
    /// <inheritdoc cref="Compile()" />
    [RequiresUnreferencedCode("Compiling expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Compiling expressions may require dynamic code generation")]
    public Func<decimal> CompileDecimal()
        => Compile<decimal>();

    /// <inheritdoc cref="Compile{T}(T)" />
    [RequiresUnreferencedCode("Compiling expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Compiling expressions may require dynamic code generation")]
    public Func<T, decimal> CompileDecimal<T>(T parameters)
        => Compile<T, decimal>(parameters);

    /// <inheritdoc cref="CompileAsyncFunc{TResult}()" />
    [RequiresUnreferencedCode("Compiling expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Compiling expressions may require dynamic code generation")]
    public Func<ValueTask<decimal>> CompileDecimalAsyncFunc()
        => CompileAsyncFunc<decimal>();

    /// <inheritdoc cref="CompileAsyncFunc{T, TResult}(T)" />
    [RequiresUnreferencedCode("Compiling expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Compiling expressions may require dynamic code generation")]
    public Func<T, ValueTask<decimal>> CompileDecimalAsyncFunc<T>(T parameters)
        => CompileAsyncFunc<T, decimal>(parameters);
}