using System;
using MathEvaluation.Entities;

#pragma warning disable IDE0130 // Namespace does not match folder structure
// ReSharper disable once CheckNamespace
namespace MathEvaluation.Context;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
///     The base scientific math context supports all trigonometric functions, logarithms, other scientific math functions,
///     and constants.
///     For a complete list of features and supported functions, please refer to the documentation at
///     <see href="https://github.com/AntonovAnton/math.evaluation" />.
/// </summary>
/// <seealso cref="MathEvaluation.Context.MathContext" />
public class DecimalScientificMathContext : ScientificMathContext
{
    /// <summary>Initializes a new instance of the <see cref="DecimalScientificMathContext" /> class.</summary>
    public DecimalScientificMathContext()
        : base()
    {
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

    private static long Factorial(decimal n)
    {
        if (n < 0.0m)
            throw new ArgumentException($"Negative number {n} isn't allowed by the factorial function.");

        if (n % 1.0m > 0m)
            throw new ArgumentException($"Not integer number {n} isn't supported by the factorial function.");

        var i = (long)n;
        var result = 1L;
        while (i > 0)
        {
            result *= i;
            i--;
        }

        return result;
    }
}