namespace MathEvaluation.Context;

internal class MathConstant(string? key, double value)
    : MathOperand(key)
{
    public double Value { get; } = value;
}