using System;

namespace MathEvaluation.Context;

internal class MathFunction(string? name, Func<double, double> fn, char? separator = null)
    : MathOperand<Func<double, double>>(name)
{
    public Func<double, double> Fn { get; } = fn;

    public char? Separator { get; } = separator;
}