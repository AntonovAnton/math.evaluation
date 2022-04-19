using Microsoft.CodeAnalysis.CSharp.Scripting;

try
{
    const string expression = "(2 / (2 + 3.33) * 4) - -6";

    var result = await CalcUsingRoslyn(expression);
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