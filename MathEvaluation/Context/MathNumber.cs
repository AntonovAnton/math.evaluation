namespace MathEvaluation.Context;

internal class MathNumber(string? name, double value)
    : MathOperand<double>(name)
{
    public double Value { get; } = value;
}
