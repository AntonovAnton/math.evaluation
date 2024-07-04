using System;

namespace MathEvaluation.Context;

internal class BasicMathFunction<T> : MathEntity
    where T : struct
{
    public Func<T, T> Fn { get; }

    public char? ClosingSymbol { get; }

    public BasicMathFunction(string? key, Func<T, T> fn, char? closingSymbol = null)
        : base(key)
    {
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
        ClosingSymbol = closingSymbol;
    }
}
