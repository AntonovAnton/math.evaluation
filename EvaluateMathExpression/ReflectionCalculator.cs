using System.Globalization;

namespace EvaluateMathExpression;

internal sealed class ReflectionCalculator
{
    public double Calculate(string expression)
    {
        try
        {
            var i = 0;
            var value = Evaluate(expression.AsSpan(), ref i);
            return value;
        }
        catch (Exception exception)
        {
            Console.WriteLine($"An exception has occurred while evaluating mathematical expression: {expression}");
            Console.WriteLine(exception.ToString());
            return double.NaN;
        }
    }

    private double Evaluate(ReadOnlySpan<char> expression, ref int i, double value = 0d)
    {
        while (expression.Length > i)
        {
            switch (expression[i])
            {
                case '(':
                    i++;
                    var parenthesesI = 0;
                    value = Evaluate(expression[i..], ref parenthesesI, value);
                    i += parenthesesI;
                    break;
                case ')':
                    i++;
                    return value;
                case >= '0' and <= '9' or '.':
                    value = GetNumber(expression, ref i);
                    break;
                case '*':
                    i++;
                    value *= Calculate(expression, ref i);
                    break;
                case '/':
                    i++;
                    value /= Calculate(expression, ref i);
                    break;
                case '-':
                    i++;
                    value -= Calculate(expression, ref i);
                    break;
                case '+':
                    i++;
                    value += Calculate(expression, ref i);
                    break;
                default:
                    i++;
                    break;
            }
        }

        return value;
    }

    private double Calculate(ReadOnlySpan<char> expression, ref int i, double value = 0d)
    {
        while (expression.Length > i)
        {
            switch (expression[i])
            {
                case '(':
                    i++;
                    var parenthesesI = 0;
                    value = Evaluate(expression[i..], ref parenthesesI, value);
                    i += parenthesesI;
                    break;
                case ')':
                    return value;
                case >= '0' and <= '9' or '.':
                    value = GetNumber(expression, ref i);
                    break;
                case '*':
                    i++;
                    value *= Calculate(expression, ref i);
                    return value;
                case '/':
                    i++;
                    value /= Calculate(expression, ref i);
                    return value;
                case '+':
                    return value;
                case '-':
                    return value;
                default:
                    i++;
                    break;
            }
        }

        return value;
    }

    private static double GetNumber(ReadOnlySpan<char> expression, ref int i)
    {
        var start = i;
        i++;
        while (expression.Length > i)
        {
            if (expression[i] is >= '0' and <= '9' or '.')
            {
                i++;
            }
            else
            {
                break;
            }
        }

        var str = expression[start..i].ToString();
        var value = Convert.ToDouble(str, CultureInfo.InvariantCulture);
        return value;
    }
}