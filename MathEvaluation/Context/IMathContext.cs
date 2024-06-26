using System;
using System.Runtime.CompilerServices;

namespace MathEvaluation.Context;

public interface IMathContext
{
    void Bind(object args);

    void BindVariable(double value, [CallerArgumentExpression(nameof(value))] string? name = null);

    internal bool TryEvaluate(ReadOnlySpan<char> expression, IFormatProvider provider, ref int i,
        char? separator, bool isAbs, bool isEvaluatedFirst, ref double value);
}
