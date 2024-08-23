using System;

namespace MathEvaluation.Context.Decimal;

/// <summary>
/// The .NET Standart 2.1 programming math context 
/// supports all constants and functions provided by <see cref="System.Math" /> class.
/// Provides evaluating C# math expressions.
/// </summary>
/// <seealso cref="MathEvaluation.Context.MathContext" />
public class DecimalDotNetStandartMathContext : MathContext
{
    /// <summary>Initializes a new instance of the <see cref="DecimalDotNetStandartMathContext" /> class.</summary>
    public DecimalDotNetStandartMathContext()
        : base()
    {
        BindVariable(1m, "true");
        BindVariable(0m, "false");

        static decimal modFn(decimal leftOperand, decimal rigntOperand) => leftOperand % rigntOperand;
        BindOperator(modFn, '%');

        static decimal equalToFn(decimal leftOperand, decimal rigntOperand) => leftOperand == rigntOperand ? 1.0m : default;
        BindOperator(equalToFn, "==", (int)EvalPrecedence.Equality);

        static decimal notEqualToFn(decimal leftOperand, decimal rigntOperand) => leftOperand != rigntOperand ? 1.0m : default;
        BindOperator(notEqualToFn, "!=", (int)EvalPrecedence.Equality);

        static decimal greaterThanFn(decimal leftOperand, decimal rigntOperand) => leftOperand > rigntOperand ? 1.0m : default;
        BindOperator(greaterThanFn, '>', (int)EvalPrecedence.Comparison);

        static decimal lessThanFn(decimal leftOperand, decimal rigntOperand) => leftOperand < rigntOperand ? 1.0m : default;
        BindOperator(lessThanFn, '<', (int)EvalPrecedence.Comparison);

        static decimal greaterThanOrEqualToFn(decimal leftOperand, decimal rigntOperand) => leftOperand >= rigntOperand ? 1.0m : default;
        BindOperator(greaterThanOrEqualToFn, ">=", (int)EvalPrecedence.Comparison);

        static decimal lessThanOrEqualToFn(decimal leftOperand, decimal rigntOperand) => leftOperand <= rigntOperand ? 1.0m : default;
        BindOperator(lessThanOrEqualToFn, "<=", (int)EvalPrecedence.Comparison);

        static decimal andFn(decimal leftOperand, decimal rigntOperand) => leftOperand != default && rigntOperand != default ? 1.0m : default;
        BindOperator(andFn, "&&", (int)EvalPrecedence.LogicalConditionalAnd);

        static decimal orFn(decimal leftOperand, decimal rigntOperand) => leftOperand != default || rigntOperand != default ? 1.0m : default;
        BindOperator(orFn, "||", (int)EvalPrecedence.LogicalConditionalOr);

        static decimal logicalNegationFn(decimal rigntOperand) => rigntOperand == default ? 1.0m : default;
        BindConverter(logicalNegationFn, '!');

        static decimal logicalAndFn(decimal leftOperand, decimal rigntOperand) => (long)leftOperand & (long)rigntOperand;
        BindOperator(logicalAndFn, '&', (int)EvalPrecedence.LogicalAnd);

        static decimal logicalOrFn(decimal leftOperand, decimal rigntOperand) => (long)leftOperand | (long)rigntOperand;
        BindOperator(logicalOrFn, '|', (int)EvalPrecedence.LogicalOr);

        static decimal logicalExclusiveOrFn(decimal leftOperand, decimal rigntOperand) => (long)leftOperand ^ (long)rigntOperand;
        BindOperator(logicalExclusiveOrFn, '^', (int)EvalPrecedence.LogicalXor);

        static decimal bitwiseComplementFn(decimal rigntOperand) => ~(long)rigntOperand;
        BindConverter(bitwiseComplementFn, '~');

        BindVariable(1m, 'f');
        BindVariable(1m, 'd');
        BindVariable(1m, 'm');
        BindVariable(1m, 'l');
        BindVariable(1m, 'u');
        BindVariable(1m, "ul");
        BindVariable(1m, "lu");
        BindVariable(1m, 'F');
        BindVariable(1m, 'D');
        BindVariable(1m, 'M');
        BindVariable(1m, 'L');
        BindVariable(1m, "Lu");
        BindVariable(1m, "LU");
        BindVariable(1m, 'U');
        BindVariable(1m, "Ul");
        BindVariable(1m, "UL");
        BindVariable(Math.PI);
        BindVariable(Math.E);

        BindFunction((decimal value) => Math.Abs(value), "Math.Abs");
        BindFunction(Math.Acos);
        BindFunction(Math.Acosh);
        BindFunction(Math.Asin);
        BindFunction(Math.Asinh);
        BindFunction(Math.Atan);
        BindFunction(Math.Atan2);
        BindFunction(Math.Atanh);
        BindFunction((decimal a, decimal b) => Math.BigMul((int)a, (int)b), "Math.BigMul");
        BindFunction(Math.Cbrt);
        BindFunction((decimal value) => Math.Ceiling(value), "Math.Ceiling");
        BindFunction((decimal value, decimal min, decimal max) => Math.Clamp(value, min, max), "Math.Clamp");
        BindFunction(Math.Cos);
        BindFunction(Math.Cosh);
        BindFunction(Math.Exp);
        BindFunction((decimal value) => Math.Floor(value), "Math.Floor");
        BindFunction(Math.IEEERemainder);
        BindFunction(args => args.Length == 1 ? Math.Log(args[0]) : Math.Log(args[0], args[1]), "Math.Log");
        BindFunction(Math.Log10);
        BindFunction((decimal val1, decimal val2) => Math.Max(val1, val2), "Math.Max");
        BindFunction((decimal val1, decimal val2) => Math.Min(val1, val2), "Math.Min");
        BindFunction(Math.Pow);
        BindFunction((decimal value) => Math.Round(value), "Math.Round");
        BindFunction((decimal value) => Math.Sign(value), "Math.Sign");
        BindFunction(Math.Sin);
        BindFunction(Math.Sinh);
        BindFunction(Math.Sqrt);
        BindFunction(Math.Tan);
        BindFunction(Math.Tanh);
        BindFunction((decimal value) => Math.Truncate(value), "Math.Truncate");

        BindVariable((decimal)byte.MaxValue, "byte.MaxValue");
        BindVariable((decimal)byte.MinValue, "byte.MinValue");
        BindVariable((decimal)sbyte.MaxValue, "sbyte.MaxValue");
        BindVariable((decimal)sbyte.MinValue, "sbyte.MinValue");
        BindVariable((decimal)short.MaxValue, "short.MaxValue");
        BindVariable((decimal)short.MinValue, "short.MinValue");
        BindVariable((decimal)ushort.MaxValue, "ushort.MaxValue");
        BindVariable((decimal)ushort.MinValue, "ushort.MinValue");
        BindVariable((decimal)int.MaxValue, "int.MaxValue");
        BindVariable((decimal)int.MinValue, "int.MinValue");
        BindVariable((decimal)uint.MaxValue, "uint.MaxValue");
        BindVariable((decimal)uint.MinValue, "uint.MinValue");
        BindVariable((decimal)long.MaxValue, "long.MaxValue");
        BindVariable((decimal)long.MinValue, "long.MinValue");
        BindVariable((decimal)ulong.MaxValue, "ulong.MaxValue");
        BindVariable((decimal)ulong.MinValue, "ulong.MinValue");
        BindVariable(float.NaN);
        BindVariable(float.Epsilon);
        BindVariable(float.PositiveInfinity);
        BindVariable(float.NegativeInfinity);
        BindVariable(float.MaxValue);
        BindVariable(float.MinValue);
        BindVariable(double.NaN);
        BindVariable(double.Epsilon);
        BindVariable(double.PositiveInfinity);
        BindVariable(double.NegativeInfinity);
        BindVariable(double.MaxValue);
        BindVariable(double.MinValue);
        BindVariable(decimal.One);
        BindVariable(decimal.MinusOne);
        BindVariable(decimal.Zero);
        BindVariable(decimal.MaxValue);
        BindVariable(decimal.MinValue);

        BindVariable((decimal)byte.MaxValue, "Byte.MaxValue");
        BindVariable((decimal)byte.MinValue, "Byte.MinValue");
        BindVariable((decimal)sbyte.MaxValue, "SByte.MaxValue");
        BindVariable((decimal)sbyte.MinValue, "SByte.MinValue");
        BindVariable((decimal)short.MaxValue, "Int16.MaxValue");
        BindVariable((decimal)short.MinValue, "Int16.MinValue");
        BindVariable((decimal)ushort.MaxValue, "UInt16.MaxValue");
        BindVariable((decimal)ushort.MinValue, "UInt16.MinValue");
        BindVariable((decimal)int.MaxValue, "Int32.MaxValue");
        BindVariable((decimal)int.MinValue, "Int32.MinValue");
        BindVariable((decimal)uint.MaxValue, "UInt32.MaxValue");
        BindVariable((decimal)uint.MinValue, "UInt32.MinValue");
        BindVariable((decimal)long.MaxValue, "Int64.MaxValue");
        BindVariable((decimal)long.MinValue, "Int64.MinValue");
        BindVariable((decimal)ulong.MaxValue, "UInt64.MaxValue");
        BindVariable((decimal)ulong.MinValue, "UInt64.MinValue");

        BindVariable(Single.NaN);
        BindVariable(Single.Epsilon);
        BindVariable(Single.PositiveInfinity);
        BindVariable(Single.NegativeInfinity);
        BindVariable(Single.MaxValue);
        BindVariable(Single.MinValue);
        BindVariable(Double.NaN);
        BindVariable(Double.Epsilon);
        BindVariable(Double.PositiveInfinity);
        BindVariable(Double.NegativeInfinity);
        BindVariable(Double.MaxValue);
        BindVariable(Double.MinValue);
    }
}

