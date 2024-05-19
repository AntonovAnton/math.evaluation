using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Math.Evaluation;

public sealed class MathEvaluator
{
    private static readonly Regex NumberInParenthesesRegex =
        new(@"\((?<number>\s*[-]*\s*\d*[.|,| |٬|٫|’|⹁| ]*\d+\s*)\)", RegexOptions.Compiled);

    /// <summary>
    /// If you subtract a negative number, the two negatives should combine to make a positive
    /// </summary>
    private static readonly Regex
        TwoNegativesRegex = new(@"[-]+\s*[-]+\s*(?<number>\d*[.|,| |٬|٫|’|⹁| ]*\d+)", RegexOptions.Compiled);

    private static readonly Regex TwoDivisionsRegex =
        new(@"(?<division>((\s*\(.*\)\s*)|(\d*[.|,| |٬|٫|’|⹁| ]*\d+\s*))\/+((\s*[-]*\s*\d*[.|,| |٬|٫|’|⹁| ]*\d+)|(\s*\(.*\))))\s*\/",
            RegexOptions.Compiled);

    private static readonly Regex DivisionBeforeMultiplicationRegex =
        new(@"(?<division>((\s*\(.*\)\s*)|(\d*[.|,| |٬|٫|’|⹁| ]*\d+\s*))\/+((\s*[-]*\s*\d*[.|,| |٬|٫|’|⹁| ]*\d+)|(\s*\(.*\))))\s*\*",
            RegexOptions.Compiled);

    public double Evaluate(string expression, CultureInfo? cultureInfo = null)
    {
        try
        {
            cultureInfo ??= CultureInfo.InvariantCulture;

            expression = expression.Replace(cultureInfo.NumberFormat.CurrencySymbol, string.Empty);
            expression = NumberInParenthesesRegex.Replace(expression, "${number}");
            expression = TwoNegativesRegex.Replace(expression, "+ ${number}");
            Match match;
            do
            {
                match = TwoDivisionsRegex.Match(expression);
                if (match.Success)
                {
                    expression = TwoDivisionsRegex.Replace(expression, "(${division}) /");
                }
            } while (match.Success);

            expression = DivisionBeforeMultiplicationRegex.Replace(expression, "(${division}) *");

            var i = 0;
            var value = Calculate(expression.AsSpan(), cultureInfo, ref i);
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
                    [nameof(cultureInfo)] = cultureInfo
                }
            };
        }
    }

    private double Calculate(ReadOnlySpan<char> expression, IFormatProvider formatProvider, ref int i, double value = 0d)
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
                default:
                    i++;
                    break;
            }
        }

        return value;
    }

    private double Evaluate(ReadOnlySpan<char> expression, IFormatProvider formatProvider, ref int i, double value = 0d)
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
            {
                i++;
            }
            else
            {
                break;
            }
        }

        var value = double.Parse(expression[start..i], NumberStyles.Number, formatProvider);
        return value;
    }
}