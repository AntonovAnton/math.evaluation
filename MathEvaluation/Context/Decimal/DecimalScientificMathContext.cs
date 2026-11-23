#pragma warning disable IDE0130 // Namespace does not match folder structure
// ReSharper disable once CheckNamespace
namespace MathEvaluation.Context;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
///     The scientific math context for <see cref="decimal" /> numbers supports all trigonometric functions, logarithms,
///     other scientific math functions, and constants. The constants (π, e, τ), the rounding functions, the factorial, the modulo,
///     the angle conversion, and the boolean logic are calculated in decimal, the other functions are the double functions,
///     since decimal doesn't define them.
///     For a complete list of features and supported functions, please refer to the documentation at
///     <see href="https://github.com/AntonovAnton/math.evaluation" />.
/// </summary>
/// <seealso cref="MathEvaluation.Context.MathContext" />
public class DecimalScientificMathContext : ScientificMathContext
{
    private const decimal Pi = 3.1415926535897932384626433833m;
    private const decimal E = 2.7182818284590452353602874714m;
    private const decimal Tau = 6.2831853071795864769252867666m;

    /// <summary>Initializes a new instance of the <see cref="DecimalScientificMathContext" /> class.</summary>
    public DecimalScientificMathContext()
    {
        // the constants are bound in the precision of decimal, the base context binds them as double
        BindConstant(Pi, 'π');
        BindConstant(Pi, "pi");
        BindConstant(Pi, "Pi");
        BindConstant(Pi, "PI");
        BindConstant(E, 'e');
        BindConstant(Tau, 'τ');

        // the modulo of the base context is calculated in double for not integer numbers
        static decimal modFn(decimal left, decimal right) => left % right;

        BindOperator<decimal>(modFn, "mod");
        BindOperator<decimal>(modFn, "Mod");
        BindOperator<decimal>(modFn, "MOD");
        BindOperator<decimal>(modFn, "modulo");
        BindOperator<decimal>(modFn, "Modulo");
        BindOperator<decimal>(modFn, "MODULO");

        static decimal radiansFn(decimal v) => v * Pi / 180m;

        BindOperandOperator<decimal>(radiansFn, '°', true); //degree symbol
        BindFunction<decimal>(radiansFn, "rad");
        BindFunction<decimal>(radiansFn, "Rad");
        BindFunction<decimal>(radiansFn, "RAD");

        static decimal degreesFn(decimal v) => v * 180m / Pi;

        BindFunction<decimal>(degreesFn, "deg");
        BindFunction<decimal>(degreesFn, "Deg");
        BindFunction<decimal>(degreesFn, "DEG");

        static decimal floorDivisionFn(decimal left, decimal right) => Math.Floor(left / right);

        BindOperator<decimal>(floorDivisionFn, "//");

        static decimal absFn(decimal v) => Math.Abs(v);

        BindFunction<decimal>(absFn, '|', '|');
        BindFunction<decimal>(absFn, "abs");
        BindFunction<decimal>(absFn, "Abs");
        BindFunction<decimal>(absFn, "ABS");

        static decimal posFn(decimal v) => Math.Max(v, 0m);

        BindFunction<decimal>(posFn, "pos");
        BindFunction<decimal>(posFn, "Pos");
        BindFunction<decimal>(posFn, "POS");

        static decimal ceilingFn(decimal v) => Math.Ceiling(v);

        BindFunction<decimal>(ceilingFn, '⌈', '⌉');
        BindFunction<decimal>(ceilingFn, "ceil");
        BindFunction<decimal>(ceilingFn, "Ceil");
        BindFunction<decimal>(ceilingFn, "CEIL");

        static decimal floorFn(decimal v) => Math.Floor(v);

        BindFunction<decimal>(floorFn, '⌊', '⌋');
        BindFunction<decimal>(floorFn, "floor");
        BindFunction<decimal>(floorFn, "Floor");
        BindFunction<decimal>(floorFn, "FLOOR");

        static decimal factorialFn(decimal v) => Factorial(v);

        BindOperandOperator<decimal>(factorialFn, '!', true);

        #region boolean logic

        BindConstant(1m, "true");
        BindConstant(1m, "True");
        BindConstant(1m, "TRUE");
        BindConstant(1m, 'T');
        BindConstant(1m, '⊤');

        BindConstant(0m, "false");
        BindConstant(0m, "False");
        BindConstant(0m, "FALSE");
        BindConstant(0m, 'F');
        BindConstant(0m, '⊥');

        static decimal equalToFn(decimal left, decimal right) => left == right ? 1.0m : default;

        BindOperator<decimal>(equalToFn, '↔', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator<decimal>(equalToFn, '⇔', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator<decimal>(equalToFn, '≡', (int)EvalPrecedence.Equivalence);

        static decimal notEqualToFn(decimal left, decimal right) => left != right ? 1.0m : default;

        BindOperator<decimal>(notEqualToFn, '↮', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator<decimal>(notEqualToFn, '⇎', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator<decimal>(notEqualToFn, '≢', (int)EvalPrecedence.Equivalence);

        static decimal implicationFn(decimal left, decimal right) => left == default || right != default ? 1.0m : default;

        BindOperator<decimal>(implicationFn, '→', (int)EvalPrecedence.LogicalImplication);
        BindOperator<decimal>(implicationFn, '⇒', (int)EvalPrecedence.LogicalImplication);

        static decimal reverseImplicationFn(decimal left, decimal right) => left != default || right == default ? 1.0m : default;

        BindOperator<decimal>(reverseImplicationFn, '←', (int)EvalPrecedence.LogicalImplication);
        BindOperator<decimal>(reverseImplicationFn, '⟸', (int)EvalPrecedence.LogicalImplication);

        #endregion
    }

    private static decimal Factorial(decimal n)
    {
        if (n < 0.0m)
            throw new ArgumentException($"Negative number {n} isn't allowed by the factorial function.");

        if (n % 1.0m > 0m)
            throw new ArgumentException($"Not integer number {n} isn't supported by the factorial function.");

        var result = 1m;
        for (var i = n; i > 1m; i--)
            result *= i;

        return result;
    }
}