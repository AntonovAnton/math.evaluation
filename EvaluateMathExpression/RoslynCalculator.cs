using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace EvaluateMathExpression;

internal sealed class RoslynCalculator
{
    async Task<double> Calculate(string expression)
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
}