﻿using System;

namespace MathEvaluation.Context;

internal class MathOperandConverter<T> : MathEntity
    where T : struct
{
    public Func<T, T> Fn { get; }

    public MathOperandConverter(string? key, Func<T, T> fn)
        : base(key)
    {
        Fn = fn ?? throw new ArgumentNullException(nameof(fn));
    }
}