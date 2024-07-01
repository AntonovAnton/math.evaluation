using System;
using System.Runtime.CompilerServices;

namespace MathEvaluation.Context;

public interface IMathContext
{
    void Bind(object args);

    void BindVariable(double value, [CallerArgumentExpression(nameof(value))] string? key = null);

    void BindFunction(Func<double, double> value, [CallerArgumentExpression(nameof(value))] string? key = null);

    internal IMathEntity? FindMathEntity(ReadOnlySpan<char> expression);
}
