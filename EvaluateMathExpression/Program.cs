using System.Globalization;
using Microsoft.CodeAnalysis.CSharp.Scripting;

try
{
    var expression = "-(2 / (2 + 3.33) * 4) - -6";
    var result = Calc(expression);
    Console.WriteLine($"Using recursive: {expression} = {result}");

    expression = "4 * 0.1 - 2";
    result = Calc(expression);
    Console.WriteLine($"Using recursive: {expression} = {result}");

    expression = "6 + -( -4)";
    result = Calc(expression);
    Console.WriteLine($"Using recursive: {expression} = {result}");

    expression = "6 + -(4)";
    result = Calc(expression);
    Console.WriteLine($"Using recursive: {expression} = {result}");

    expression = "(2.323 * 323 - 1 / (2 + 3.33) * 4) - -6";
    result = Calc(expression);
    Console.WriteLine($"Using recursive: {expression} = {result}");

    expression = "(2.323 * 323 - 1 / -(2 + 3.33) * 4) - -6";
    result = Calc(expression);
    Console.WriteLine($"Using recursive: {expression} = {result}");

    //the following expressions also work :)
    expression = "1 - - 1";
    result = Calc(expression);
    Console.WriteLine($"Using recursive: {expression} = {result}");

    expression = "6 + - (4)";
    result = Calc(expression);
    Console.WriteLine($"Using recursive: {expression} = {result}");

    expression = "6 + -(- 4)";
    result = Calc(expression);
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

double Calc(string expression)
{
    var i = 0;
    var result = Evaluate(expression, ref i);
    return result;
}

double Evaluate(string expression, ref int i)
{
    var result = 0d;
    while (expression.Length > i)
    {
        i++;
        switch (expression[i - 1])
        {
            case '(':
                result = Evaluate(expression, ref i);
                break;
            case ')':
                return result;
            case > '0' and < '9' or '.':
                result = GetNumber(expression, ref i);
                break;
            case '*':
                result *= Calculate(result, expression, ref i);
                break;
            case '/':
                result /= Calculate(result, expression, ref i);
                break;
            case '+':
                result += Evaluate(expression, ref i);
                break;
            case '-':
                result -= Evaluate(expression, ref i);
                break;
        }
    }

    return result;
}

double Calculate(double result, string expression, ref int i)
{
    while (expression.Length > i)
    {
        i++;
        switch (expression[i - 1])
        {
            case '(':
                return Evaluate(expression, ref i);
            case > '0' and < '9' or '.':
                return GetNumber(expression, ref i);
            case '*':
                return result * Evaluate(expression, ref i);
            case '/':
                return result / Evaluate(expression, ref i);
            case '+':
                return result + Evaluate(expression, ref i);
            case '-':
                return result - Evaluate(expression, ref i);
        }
    }

    return result;
}

double GetNumber(string expression, ref int i)
{
    var startIndex = i - 1;
    while (expression.Length > i)
    {
        if (expression[i] is > '0' and < '9' or '.')
        {
            i++;
        }
        else
        {
            break;
        }
    }

    var number = Convert.ToDouble(expression[startIndex..i], CultureInfo.InvariantCulture);
    return number;
}