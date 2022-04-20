using System.Globalization;

namespace EvaluateMathExpression;

internal sealed class ReflectionCalculator
{
    public double Calculate(string expression)
    {
        try
        {
            var i = 0;
            var value = Calculate(expression, ref i);
            return value;
        }
        catch (Exception exception)
        {
            Console.WriteLine($"An exception has occurred while evaluating mathematical expression: {expression}");
            Console.WriteLine(exception.ToString());
            return double.NaN;
        }
    }

    private double Evaluate(string expression, ref int i, double value = 0d)
    {
        while (expression.Length > i)
        {
            switch (expression[i])
            {
                case '(':
                    i++;
                    value = Evaluate(expression, ref i);
                    break;
                case ')':
                    i++;
                    return value;
                case >= '0' and <= '9' or '.':
                    value = GetNumber(expression, ref i);
                    break;
                case '*':
                    i++;
                    value *= Evaluate(expression, ref i);
                    return value;
                case '/':
                    i++;
                    value /= Evaluate(expression, ref i);
                    return value;
                case '+':
                    i++;
                    value += Calculate(expression, ref i);
                    return value;
                case '-':
                    i++;
                    value -= Calculate(expression, ref i);
                    return value;
                default:
                    i++;
                    break;
            }
        }

        return value;
    }

    private double Calculate(string expression, ref int i, double value = 0d)
    {
        while (expression.Length > i)
        {
            switch (expression[i])
            {
                case '(':
                    i++;
                    value = Evaluate(expression, ref i);
                    break;
                case >= '0' and <= '9' or '.':
                    value = GetNumber(expression, ref i);
                    break;
                case '+':
                    i++;
                    value += Evaluate(expression, ref i);
                    return value;
                case '-':
                    i++;
                    value -= Evaluate(expression, ref i);
                    return value;
                default:
                    i++;
                    break;
            }
        }

        return value;
    }

    private static double GetNumber(string expression, ref int i)
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

        var number = Convert.ToDouble(expression[start..i], CultureInfo.InvariantCulture);
        return number;
    }
}