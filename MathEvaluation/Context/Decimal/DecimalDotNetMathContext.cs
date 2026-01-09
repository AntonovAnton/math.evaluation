#if NET8_0_OR_GREATER

using System;

#pragma warning disable IDE0130 // Namespace does not match folder structure
// ReSharper disable once CheckNamespace
namespace MathEvaluation.Context;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <inheritdoc />
public class DecimalDotNetMathContext : DotNetMathContext
{
    /// <summary>Initializes a new instance of the <see cref="DecimalDotNetMathContext" /> class.</summary>
    public DecimalDotNetMathContext()
        : base()
    {
        BindConstant(1m, "true");
        BindConstant(0m, "false");

        BindConstant(0m, "default");
        BindConstant(0m, "default(T)");

        static decimal postfixIncrementFn(decimal left) => left;

        BindOperandOperator(postfixIncrementFn, "++ ", true);
        BindOperandOperator(postfixIncrementFn, "++\t", true);
        BindOperandOperator(postfixIncrementFn, "++\r", true);
        BindOperandOperator(postfixIncrementFn, "++\n", true);

        static decimal postfixDecrementFn(decimal left) => left;

        BindOperandOperator(postfixDecrementFn, "-- ", true);
        BindOperandOperator(postfixDecrementFn, "--\t", true);
        BindOperandOperator(postfixDecrementFn, "--\r", true);
        BindOperandOperator(postfixDecrementFn, "--\n", true);

        static decimal absFn(decimal v) => Math.Abs(v);

        BindFunction(absFn, "Math.Abs");

        static decimal bigMul(decimal a, decimal b) => Math.BigMul((int)a, (int)b);

        BindFunction(bigMul, "Math.BigMul");

        static decimal ceilingFn(decimal value) => Math.Ceiling(value);

        BindFunction(ceilingFn, "Math.Ceiling");

        static decimal clampFn(decimal value, decimal min, decimal max) => Math.Clamp(value, min, max);

        BindFunction(clampFn, "Math.Clamp");

        static decimal floorFn(decimal value) => Math.Floor(value);

        BindFunction(floorFn, "Math.Floor");

        static decimal maxFn(decimal val1, decimal val2) => Math.Max(val1, val2);

        BindFunction(maxFn, "Math.Max");

        static decimal minFn(decimal val1, decimal val2) => Math.Min(val1, val2);

        BindFunction(minFn, "Math.Min");

        static decimal roundFn(decimal[] args) => args.Length == 1 ? Math.Round(args[0]) : Math.Round(args[0], (int)args[1]);

        BindFunction(roundFn, "Math.Round");

        static decimal signFn(decimal value) => Math.Sign(value);

        BindFunction(signFn, "Math.Sign");

        static decimal truncateFn(decimal value) => Math.Truncate(value);

        BindFunction(truncateFn, "Math.Truncate");
    }
}

#endif