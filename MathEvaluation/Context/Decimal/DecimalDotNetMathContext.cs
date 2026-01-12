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

        BindOperandOperator<decimal>(postfixIncrementFn, "++ ", true);
        BindOperandOperator<decimal>(postfixIncrementFn, "++\t", true);
        BindOperandOperator<decimal>(postfixIncrementFn, "++\r", true);
        BindOperandOperator<decimal>(postfixIncrementFn, "++\n", true);

        static decimal postfixDecrementFn(decimal left) => left;

        BindOperandOperator<decimal>(postfixDecrementFn, "-- ", true);
        BindOperandOperator<decimal>(postfixDecrementFn, "--\t", true);
        BindOperandOperator<decimal>(postfixDecrementFn, "--\r", true);
        BindOperandOperator<decimal>(postfixDecrementFn, "--\n", true);

        static decimal absFn(decimal v) => Math.Abs(v);

        BindFunction<decimal>(absFn, "Math.Abs");

        static decimal bigMul(decimal a, decimal b) => Math.BigMul((int)a, (int)b);

        BindFunction<decimal>(bigMul, "Math.BigMul");

        static decimal ceilingFn(decimal value) => Math.Ceiling(value);

        BindFunction<decimal>(ceilingFn, "Math.Ceiling");

        static decimal clampFn(decimal value, decimal min, decimal max) => Math.Clamp(value, min, max);

        BindFunction<decimal>(clampFn, "Math.Clamp");

        static decimal floorFn(decimal value) => Math.Floor(value);

        BindFunction<decimal>(floorFn, "Math.Floor");

        static decimal maxFn(decimal val1, decimal val2) => Math.Max(val1, val2);

        BindFunction<decimal>(maxFn, "Math.Max");

        static decimal minFn(decimal val1, decimal val2) => Math.Min(val1, val2);

        BindFunction<decimal>(minFn, "Math.Min");

        static decimal roundFn(decimal[] args) => args.Length == 1 ? Math.Round(args[0]) : Math.Round(args[0], (int)args[1]);

        BindFunction<decimal>(roundFn, "Math.Round");

        static decimal signFn(decimal value) => Math.Sign(value);

        BindFunction<decimal>(signFn, "Math.Sign");

        static decimal truncateFn(decimal value) => Math.Truncate(value);

        BindFunction<decimal>(truncateFn, "Math.Truncate");
    }
}