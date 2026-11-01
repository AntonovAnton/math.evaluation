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

    /// <inheritdoc cref="CompileAsyncFunc{TResult}()" />
    [RequiresUnreferencedCode("Compiling expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Compiling expressions may require dynamic code generation")]
    public Func<CancellationToken, ValueTask<Complex>> CompileComplexAsyncFunc()
        => CompileAsyncFunc<Complex>();

    /// <inheritdoc cref="CompileAsyncFunc{T, TResult}(T)" />
    [RequiresUnreferencedCode("Compiling expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Compiling expressions may require dynamic code generation")]
    public Func<T, CancellationToken, ValueTask<Complex>> CompileComplexAsyncFunc<T>(T parameters)
        => CompileAsyncFunc<T, Complex>(parameters);
}