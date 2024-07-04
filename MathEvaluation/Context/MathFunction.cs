using System;

namespace MathEvaluation.Context;

internal class MathFunction<T> : MathEntity
    where T : struct
{
    public Func<T[], T> Fn { get; }

    public char OpenningSymbol { get; }

    public char Separator { get; }

    public char ClosingSymbol { get; }

    public MathFunction(string? key, Func<T[], T> fn, char openningSymbol, char separator, char closingSymbol)
        : base(key)
    {
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
        Separator = separator;
        OpenningSymbol = openningSymbol;
        ClosingSymbol = closingSymbol;
    }
}
