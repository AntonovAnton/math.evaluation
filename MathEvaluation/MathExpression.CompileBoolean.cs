using System;

namespace MathEvaluation;

public partial class MathExpression
{
    /// <inheritdoc cref="Compile()"/>
    public Func<bool> CompileBoolean()
    {
        var fn = Compile();
        return () => fn() != default;
    }

    /// <inheritdoc cref="Compile{T}(T)"/>
    public Func<T, bool> CompileBoolean<T>(T parameters)
    {
        var fn = Compile(parameters);
        return (T parameters) => fn(parameters) != default;
    }
}
