using System;

namespace MathEvaluation.Context;

internal class MathOperandConverter(string? name, Func<double, double> fn)
    : MathFunction(name, fn);
