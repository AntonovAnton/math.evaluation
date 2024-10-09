using System;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MathEvaluation.Context;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// The .NET Standart 2.1 programming math context 
/// supports all constants and functions provided by <see cref="System.Math" /> class.
/// Provides evaluating C# math expressions.
/// </summary>
/// <seealso cref="MathEvaluation.Context.MathContext" />
public class DecimalDotNetStandartMathContext : DotNetStandartMathContext
{
    /// <summary>Initializes a new instance of the <see cref="DecimalDotNetStandartMathContext" /> class.</summary>
    public DecimalDotNetStandartMathContext()
        : base()
    {
        BindConstant(1m, "true");
        BindConstant(0m, "false");

        static decimal postfixIncrementFn(decimal left) => left++;
        BindOperandOperator(postfixIncrementFn, "++ ", true);
        BindOperandOperator(postfixIncrementFn, "++\t", true);
        BindOperandOperator(postfixIncrementFn, "++\r", true);
        BindOperandOperator(postfixIncrementFn, "++\n", true);

        static decimal postfixDecrementFn(decimal left) => left--;
        BindOperandOperator(postfixDecrementFn, "-- ", true);
        BindOperandOperator(postfixDecrementFn, "--\t", true);
        BindOperandOperator(postfixDecrementFn, "--\r", true);
        BindOperandOperator(postfixDecrementFn, "--\n", true);

        static decimal emptyFn(decimal value) => value;
        BindOperandOperator(emptyFn, 'f', true);
        BindOperandOperator(emptyFn, 'd', true);
        BindOperandOperator(emptyFn, 'm', true);
        BindOperandOperator(emptyFn, 'l', true);
        BindOperandOperator(emptyFn, 'u', true);
        BindOperandOperator(emptyFn, "ul", true);
        BindOperandOperator(emptyFn, "lu", true);
        BindOperandOperator(emptyFn, 'F', true);
        BindOperandOperator(emptyFn, 'D', true);
        BindOperandOperator(emptyFn, 'M', true);
        BindOperandOperator(emptyFn, 'L', true);
        BindOperandOperator(emptyFn, "Lu", true);
        BindOperandOperator(emptyFn, "LU", true);
        BindOperandOperator(emptyFn, 'U', true);
        BindOperandOperator(emptyFn, "Ul", true);
        BindOperandOperator(emptyFn, "UL", true);

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

        BindConstant(decimal.One);
        BindConstant(decimal.MinusOne);
        BindConstant(decimal.Zero);
        BindConstant(decimal.MaxValue);
        BindConstant(decimal.MinValue);

        BindConstant((decimal)byte.MaxValue, "byte.MaxValue");
        BindConstant((decimal)byte.MinValue, "byte.MinValue");
        BindConstant((decimal)sbyte.MaxValue, "sbyte.MaxValue");
        BindConstant((decimal)sbyte.MinValue, "sbyte.MinValue");
        BindConstant((decimal)short.MaxValue, "short.MaxValue");
        BindConstant((decimal)short.MinValue, "short.MinValue");
        BindConstant((decimal)ushort.MaxValue, "ushort.MaxValue");
        BindConstant((decimal)ushort.MinValue, "ushort.MinValue");
        BindConstant((decimal)int.MaxValue, "int.MaxValue");
        BindConstant((decimal)int.MinValue, "int.MinValue");
        BindConstant((decimal)uint.MaxValue, "uint.MaxValue");
        BindConstant((decimal)uint.MinValue, "uint.MinValue");
        BindConstant((decimal)long.MaxValue, "long.MaxValue");
        BindConstant((decimal)long.MinValue, "long.MinValue");
        BindConstant((decimal)ulong.MaxValue, "ulong.MaxValue");
        BindConstant((decimal)ulong.MinValue, "ulong.MinValue");

        BindConstant((decimal)byte.MaxValue, "Byte.MaxValue");
        BindConstant((decimal)byte.MinValue, "Byte.MinValue");
        BindConstant((decimal)sbyte.MaxValue, "SByte.MaxValue");
        BindConstant((decimal)sbyte.MinValue, "SByte.MinValue");
        BindConstant((decimal)short.MaxValue, "Int16.MaxValue");
        BindConstant((decimal)short.MinValue, "Int16.MinValue");
        BindConstant((decimal)ushort.MaxValue, "UInt16.MaxValue");
        BindConstant((decimal)ushort.MinValue, "UInt16.MinValue");
        BindConstant((decimal)int.MaxValue, "Int32.MaxValue");
        BindConstant((decimal)int.MinValue, "Int32.MinValue");
        BindConstant((decimal)uint.MaxValue, "UInt32.MaxValue");
        BindConstant((decimal)uint.MinValue, "UInt32.MinValue");
        BindConstant((decimal)long.MaxValue, "Int64.MaxValue");
        BindConstant((decimal)long.MinValue, "Int64.MinValue");
        BindConstant((decimal)ulong.MaxValue, "UInt64.MaxValue");
        BindConstant((decimal)ulong.MinValue, "UInt64.MinValue");
    }
}

