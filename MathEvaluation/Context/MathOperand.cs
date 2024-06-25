namespace MathEvaluation.Context;

internal class MathOperand(string name, double value)
{
    public string Name { get; } = name;

    public double Value { get; } = value;
}
