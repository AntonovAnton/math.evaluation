using EvaluateMathExpression;
using System.Globalization;

Console.OutputEncoding = System.Text.Encoding.UTF8;

try
{
    var calculator = new RecursionCalculator();

    var expression = string.Empty;
    var result = 0d;

    expression = "2 / 5 / 2 * 5";
    result = calculator.Eval(expression);
    Console.WriteLine($"Using recursive: {expression} = {result}");

    expression = "2 + (5 - 1)";
    result = calculator.Eval(expression);
    Console.WriteLine($"Using recursive: {expression} = {result}");

    expression = "4 * 0.1 - 2";
    result = calculator.Eval(expression);
    Console.WriteLine($"Using recursive: {expression} = {result}");

    expression = "6 + -( -4)";
    result = calculator.Eval(expression);
    Console.WriteLine($"Using recursive: {expression} = {result}");

    expression = "6 + - ( 4)";
    result = calculator.Eval(expression);
    Console.WriteLine($"Using recursive: {expression} = {result}");

    expression = "(2.323 * 323 - 1 / (2 + 3.33) * 4) - 6";
    result = calculator.Eval(expression);
    Console.WriteLine($"Using recursive: {expression} = {result}");

    expression = "(22,888.323 * 323 - 1 / (2 + 22,888.323) * 4) - 6";
    result = calculator.Eval(expression);
    Console.WriteLine($"Using recursive: {expression} = {result}");

    expression = "22\u00a0888,32 \u00a4 * 323 - 1 / (2 + 22\u00a0888,32 \u00a4) * 4 - 6";
    result = calculator.Eval(expression, new CultureInfo("nn"));
    Console.WriteLine($"Using recursive: {expression} = {result}");

    var cultureInfos = CultureInfo.GetCultures(CultureTypes.AllCultures);
    var hashSet = new HashSet<string>();
    foreach (var cultureInfo in cultureInfos)
    {
        var s =
            $"22{cultureInfo.NumberFormat.NumberGroupSeparator}888{cultureInfo.NumberFormat.NumberDecimalSeparator}32 {cultureInfo.NumberFormat.CurrencySymbol}";
        bool isDifferent = false;
        if (!hashSet.Contains(
                $"22{cultureInfo.NumberFormat.NumberGroupSeparator}888{cultureInfo.NumberFormat.NumberDecimalSeparator}32"))
        {
            isDifferent = true;
            hashSet.Add(
                $"22{cultureInfo.NumberFormat.NumberGroupSeparator}888{cultureInfo.NumberFormat.NumberDecimalSeparator}32");
        }

        expression = $"{s} * 30 / 323{cultureInfo.NumberFormat.NumberDecimalSeparator}34 / {cultureInfo.NumberFormat.NumberDecimalSeparator}5 - - 1 / (2 + {s}) * 4 - 6";
        result = calculator.Eval(expression, cultureInfo);
        //if (isDifferent)
            Console.WriteLine($"[InlineData(\"{expression}\", \"{cultureInfo.Name}\", 4241.2297164052907d)]");
        //Console.WriteLine($"Using recursive, culture info {cultureInfo.Name}, NumberGroupSeparator {cultureInfo.NumberFormat.NumberGroupSeparator} NumberDecimalSeparator {cultureInfo.NumberFormat.NumberDecimalSeparator} NumberNegativePattern {cultureInfo.NumberFormat.NumberNegativePattern}: {expression} = {result}");
    }

    expression = "2 - (5 * 10 / 2) - 1";
    result = calculator.Eval(expression);
    Console.WriteLine($"Using recursive: {expression} = {result}");

    expression = "1 - -1";
    result = calculator.Eval(expression);
    Console.WriteLine($"Using recursive: {expression} = {result}");

    return 0;
}
catch (Exception exception)
{
    Console.WriteLine(exception.ToString());
    return 1;
}