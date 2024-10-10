using System;
using System.Numerics;

namespace MathEvaluation;

public partial class MathExpression
{
    /// <inheritdoc cref="Compile{TResult}()"/>
    public Func<Complex> CompileComplex()
        => Compile<Complex>();

    /// <inheritdoc cref="Compile{T, TResult}(T)"/>
    public Func<T, Complex> CompileComplex<T>(T parameters)
        => Compile<T, Complex>(parameters);
}