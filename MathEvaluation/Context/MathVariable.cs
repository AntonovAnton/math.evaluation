namespace MathEvaluation.Context;

internal class MathVariable(string? key, double value)
    : MathOperand(key)
{
    public double Value { get; } = value;
}
