using System;
using System.Diagnostics.CodeAnalysis;

namespace MathEvaluation;

public partial class MathExpression
{
    /// <summary>Compiles the <see cref="MathString">math expression string</see>.</summary>
    /// <returns>A delegate that represents the compiled expression.</returns>
    /// <exception cref="MathExpressionException" />
    [RequiresUnreferencedCode("Compiling expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Compiling expressions may require dynamic code generation")]
    public Func<double> Compile()
        => Compile<double>();

    /// <inheritdoc cref="Compile()" />
    /// <param name="parameters">The parameters of the <see cref="MathString">math expression string</see>.</param>
    /// <typeparam name="T">The type of the parameters.</typeparam>
    /// <exception cref="ArgumentNullException">parameters</exception>
    /// <exception cref="NotSupportedException">parameters</exception>
    [RequiresUnreferencedCode("Compiling expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Compiling expressions may require dynamic code generation")]
    public Func<T, double> Compile<T>(T parameters)
        => Compile<T, double>(parameters);
}