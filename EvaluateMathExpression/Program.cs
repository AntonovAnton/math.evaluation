using EvaluateMathExpression;

try
{
    var calculator = new ReflectionCalculator();

    var expression = string.Empty;
    var result = 0d;

    expression = "2 + (5 - 1)";
    result = calculator.Calculate(expression);
    Console.WriteLine($"Using recursive: {expression} = {result}");

    expression = "4 * 0.1 - 2";
    result = calculator.Calculate(expression);
    Console.WriteLine($"Using recursive: {expression} = {result}");

    expression = "6 + -( -4)";
    result = calculator.Calculate(expression);
    Console.WriteLine($"Using recursive: {expression} = {result}");

    expression = "6 + -(4)";
    result = calculator.Calculate(expression);
    Console.WriteLine($"Using recursive: {expression} = {result}");

    expression = "(2.323 * 323 - 1 / (2 + 3.33) * 4) - 6";
    result = calculator.Calculate(expression);
    Console.WriteLine($"Using recursive: {expression} = {result}");

    expression = "2 - (5 * 10 / 2) - 1";
    result = calculator.Calculate(expression);
    Console.WriteLine($"Using recursive: {expression} = {result}");

    expression = "1 - -1";
    result = calculator.Calculate(expression);
    Console.WriteLine($"Using recursive: {expression} = {result}");

    expression = "6 + - (4)";
    result = calculator.Calculate(expression);
    Console.WriteLine($"Using recursive: {expression} = {result}");

    expression = "6 + -(- 4)";
    result = calculator.Calculate(expression);
    Console.WriteLine($"Using recursive: {expression} = {result}");

    return 0;
}
catch (Exception exception)
{
    Console.WriteLine(exception.ToString());
    return 1;
}