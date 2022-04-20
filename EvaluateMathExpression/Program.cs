using System.Globalization;
using Microsoft.CodeAnalysis.CSharp.Scripting;

try
{
    //const string expression = "(2 / (2 + 3.33) * 4) - -6";

    const string expression = "4 * 0.1 - 2";

    var result = Calc2(expression);
    Console.WriteLine($"Using recursive: {expression} = {result}");


    result = await CalcUsingRoslyn(expression);
    Console.WriteLine($"Using Roslyn: {expression} = {result}");


    return 0;
}
catch (Exception exception)
{
    Console.WriteLine(exception.ToString());
    return 1;
}


async Task<double> CalcUsingRoslyn(string expression)
{
    try
    {
        var result = await CSharpScript.EvaluateAsync<double>(expression);
        return result;
    }
    catch (Exception exception)
    {
        Console.WriteLine($"An exception has occurred while evaluating mathematical expression: {expression}");
        Console.WriteLine(exception.ToString());
        return double.NaN;
    }
}

double Calc2(string expression)
{
    var i = 0;
    var result = Calc(expression, ref i);
    return result;
}


double Calc(string expression, ref int i)
{
    var result = 0d;
    while (expression.Length > i)
    {
        switch (expression[i])
        {
            case '(':
                i++;
                result = Calc(expression, ref i);
                break;
            case > '0' and < '9' or '.':
                return GetNumber(expression, ref i);
            case '+':
                i++;
                result += Calc(expression, ref i);
                break;
            case '-':
                i++;
                result -= Calc(expression, ref i);
                break;
            case '*':
                i++;
                result *= Calc(expression, ref i);
                break;
            case '/':
                i++;
                result /= Calc(expression, ref i);
                break;
            default:
                i++;
                break;
        }
    }

    return result;
}

double GetNumber(string expression, ref int i)
{
    var chars = new List<char> { expression[i] };
    i++;
    while (expression.Length > i)
    {
        if (expression[i] is > '0' and < '9' or '.')
        {
            chars.Add(expression[i]);
        }
        else
        {
            break;
        }

        i++;
    }

    return Convert.ToDouble(string.Concat(chars), CultureInfo.InvariantCulture);
}