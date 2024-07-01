using System;

namespace MathEvaluation.Context;

internal class MathOperandConverter(string? key, Func<double, double> fn)
    : MathFunction(key, fn);
