using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Math.Evaluation;

public class MathEvaluator
{
    /// <summary>
    /// If you subtract a negative number, the two negatives should combine to make a positive
    /// </summary>
    private static readonly Regex
        TwoNegativesRegex = new(@"[-]+\s*[-]+", RegexOptions.Compiled);

    private static readonly Regex DivisionBeforeEqualPrecedenceOperatorRegex =
        new (@"(?<division>((\s*\(.*\)\s*)|[^\/()*%+-]*)\/((\s*\(.*\)\s*)|[^\/()*%+-]*))[\/*%]",
            RegexOptions.Compiled);

    public double Evaluate(string expression, IFormatProvider? formatProvider = null)
    {
        try
        {
            formatProvider ??= CultureInfo.InvariantCulture;
            var numberFormatInfo = NumberFormatInfo.GetInstance(formatProvider);

            expression = expression.Replace(numberFormatInfo.CurrencySymbol, string.Empty);
            expression = TwoNegativesRegex.Replace(expression, "+");
            Match match;
            do
            {
                match = DivisionBeforeEqualPrecedenceOperatorRegex.Match(expression);
                var matchGroup = match.Groups["division"];
                if (!matchGroup.Success || matchGroup.Value.Sum(c => c is '(' or ')' ? 1 : 0) % 2 != 0)
                {
                    break;
                }

                expression = expression.Replace(matchGroup.Value, $"({matchGroup.Value})");
            } while (match.Success);

            var i = 0;
            var value = Calculate(expression.AsSpan(), formatProvider, ref i);
            return value;
        }
        catch (Exception innerException)
        {
            const string message = "An exception has occurred while evaluating mathematical expression";
            throw new ApplicationException(message, innerException)
            {
                Data =
                {
                    [nameof(expression)] = expression,
                    [nameof(formatProvider)] = formatProvider
                }
            };
        }
    }

    private static double Calculate(ReadOnlySpan<char> expression, IFormatProvider formatProvider, ref int i, double value = default)
    {
        while (expression.Length > i)
        {
            switch (expression[i])
            {
                case '(':
                    i++;
                    var parenthesesI = 0;
                    value = (value == 0 ? 1 : value) * Calculate(expression[i..], formatProvider, ref parenthesesI, value);
                    i += parenthesesI;
                    break;
                case ')':
                    i++;
                    return value;
                case >= '0' and <= '9' or '.' or ',' or '٫' or '’' or '٬' or '⹁':
                    value = GetNumber(expression, formatProvider, ref i);
                    break;
                case '*':
                    i++;
                    value *= Evaluate(expression, formatProvider, ref i);
                    break;
                case '/':
                    i++;
                    value /= Evaluate(expression, formatProvider, ref i);
                    break;
                case '-':
                    i++;
                    value -= Evaluate(expression, formatProvider, ref i);
                    break;
                case '+':
                    i++;
                    value += Evaluate(expression, formatProvider, ref i);
                    break;
                case 'π':
                    i++;
                    value = (value == 0 ? 1 : value) * System.Math.PI;
                    break;
                default:
                    i++;
                    break;
            }
        }

        return value;
    }

    private static double Evaluate(ReadOnlySpan<char> expression, IFormatProvider formatProvider, ref int i, double value = default)
    {
        while (expression.Length > i)
        {
            switch (expression[i])
            {
                case '(':
                    i++;
                    var parenthesesI = 0;
                    value = Calculate(expression[i..], formatProvider, ref parenthesesI, value);
                    i += parenthesesI;
                    break;
                case ')':
                    return value;
                case >= '0' and <= '9' or '.' or ',' or '٫' or '’' or '٬' or '⹁':
                    value = GetNumber(expression, formatProvider, ref i);
                    break;
                case '*':
                    i++;
                    value *= Evaluate(expression, formatProvider, ref i);
                    break;
                case '/':
                    i++;
                    value /= Evaluate(expression, formatProvider, ref i);
                    break;
                case '-' or '+':
                    return value;
                case 'π':
                    i++;
                    return System.Math.PI;
                default:
                    i++;
                    break;
            }
        }

        return value;
    }

    private static double GetNumber(ReadOnlySpan<char> expression, IFormatProvider formatProvider, ref int i)
    {
        var start = i;
        i++;
        while (expression.Length > i)
        {
            if (expression[i] is >= '0' and <= '9' or '.' or ',' or '\u202f' or '\u00a0' or '٫' or '’' or '٬' or '⹁')
                i++;
            else
                break;
        }

        var value = double.Parse(expression[start..i], NumberStyles.Number, formatProvider);
        return value;
    }
}