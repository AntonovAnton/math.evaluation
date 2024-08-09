using System;

namespace MathEvaluation.Context;

/// <summary>
/// The .NET Standart 2.1 programming math context 
/// supports all constants and functions provided by <see cref="System.Math" /> class.
/// Provides evaluating C# math expressions.
/// </summary>
/// <seealso cref="MathEvaluation.Context.MathContext" />
/// <seealso cref="MathEvaluation.Context.IProgrammingMathContext" />
public class DotNetStandartMathContext : MathContext, IProgrammingMathContext
{
    /// <summary>Initializes a new instance of the <see cref="DotNetStandartMathContext" /> class.</summary>
    public DotNetStandartMathContext()
        : base()
    {
        BindVariable(1d, "true");
        BindVariable(0d, "false");

        static double modFn(double leftOperand, double rigntOperand) => leftOperand % rigntOperand;
        BindOperator(modFn, '%', (int)EvalPrecedence.Basic);

        static double equalToFn(double leftOperand, double rigntOperand) => leftOperand == rigntOperand ? 1.0 : default;
        BindOperator(equalToFn, "==", (int)EvalPrecedence.Equality);

        static double notEqualToFn(double leftOperand, double rigntOperand) => leftOperand != rigntOperand ? 1.0 : default;
        BindOperator(notEqualToFn, "!=", (int)EvalPrecedence.Equality);

        static double greaterThanFn(double leftOperand, double rigntOperand) => leftOperand > rigntOperand ? 1.0 : default;
        BindOperator(greaterThanFn, '>', (int)EvalPrecedence.Comparison);

        static double lessThanFn(double leftOperand, double rigntOperand) => leftOperand < rigntOperand ? 1.0 : default;
        BindOperator(lessThanFn, '<', (int)EvalPrecedence.Comparison);

        static double greaterThanOrEqualToFn(double leftOperand, double rigntOperand) => leftOperand >= rigntOperand ? 1.0 : default;
        BindOperator(greaterThanOrEqualToFn, ">=", (int)EvalPrecedence.Comparison);

        static double lessThanOrEqualToFn(double leftOperand, double rigntOperand) => leftOperand <= rigntOperand ? 1.0 : default;
        BindOperator(lessThanOrEqualToFn, "<=", (int)EvalPrecedence.Comparison);

        static double andFn(double leftOperand, double rigntOperand) => leftOperand != default && rigntOperand != default ? 1.0 : default;
        BindOperator(andFn, "&&", (int)EvalPrecedence.LogicalConditionalAnd);

        static double orFn(double leftOperand, double rigntOperand) => leftOperand != default || rigntOperand != default ? 1.0 : default;
        BindOperator(orFn, "||", (int)EvalPrecedence.LogicalConditionalOr);

        static double logicalNegationFn(double rigntOperand) => rigntOperand == default ? 1.0 : default;
        BindConverter(logicalNegationFn, '!');

        static double logicalAndFn(double leftOperand, double rigntOperand) => (long)leftOperand & (long)rigntOperand;
        BindOperator(logicalAndFn, '&', (int)EvalPrecedence.LogicalAnd);

        static double logicalOrFn(double leftOperand, double rigntOperand) => (long)leftOperand | (long)rigntOperand;
        BindOperator(logicalOrFn, '|', (int)EvalPrecedence.LogicalOr);

        static double logicalExclusiveOrFn(double leftOperand, double rigntOperand) => (long)leftOperand ^ (long)rigntOperand;
        BindOperator(logicalExclusiveOrFn, '^', (int)EvalPrecedence.LogicalXor);

        static double bitwiseComplementFn(double rigntOperand) => ~(long)rigntOperand;
        BindConverter(bitwiseComplementFn, '~');

        BindVariable(1d, 'f');
        BindVariable(1d, 'd');
        BindVariable(1d, 'm');
        BindVariable(1d, 'l');
        BindVariable(1d, 'u');
        BindVariable(1d, "ul");
        BindVariable(1d, "lu");
        BindVariable(1d, 'F');
        BindVariable(1d, 'D');
        BindVariable(1d, 'M');
        BindVariable(1d, 'L');
        BindVariable(1d, "Lu");
        BindVariable(1d, "LU");
        BindVariable(1d, 'U');
        BindVariable(1d, "Ul");
        BindVariable(1d, "UL");
        BindVariable(Math.PI);
        BindVariable(Math.E);

        BindFunction((double value) => Math.Abs(value), "Math.Abs");
        BindFunction(Math.Acos);
        BindFunction(Math.Acosh);
        BindFunction(Math.Asin);
        BindFunction(Math.Asinh);
        BindFunction(Math.Atan);
        BindFunction(Math.Atan2);
        BindFunction(Math.Atanh);
        BindFunction((double a, double b) => Math.BigMul((int)a, (int)b), "Math.BigMul");
        BindFunction(Math.Cbrt);
        BindFunction((double value) => Math.Ceiling(value), "Math.Ceiling");
        BindFunction((double value, double min, double max) => Math.Clamp(value, min, max), "Math.Clamp");
        BindFunction(Math.Cos);
        BindFunction(Math.Cosh);
        BindFunction(Math.Exp);
        BindFunction((double value) => Math.Floor(value), "Math.Floor");
        BindFunction(Math.IEEERemainder);
        BindFunction(args => args.Length == 1 ? Math.Log(args[0]) : Math.Log(args[0], args[1]), "Math.Log");
        BindFunction(Math.Log10);
        BindFunction((double val1, double val2) => Math.Max(val1, val2), "Math.Max");
        BindFunction((double val1, double val2) => Math.Min(val1, val2), "Math.Min");
        BindFunction(Math.Pow);
        BindFunction((double value) => Math.Round(value), "Math.Round");
        BindFunction((double value) => Math.Sign(value), "Math.Sign");
        BindFunction(Math.Sin);
        BindFunction(Math.Sinh);
        BindFunction(Math.Sqrt);
        BindFunction(Math.Tan);
        BindFunction(Math.Tanh);
        BindFunction((double value) => Math.Truncate(value), "Math.Truncate");

        BindVariable((double)byte.MaxValue, "byte.MaxValue");
        BindVariable((double)byte.MinValue, "byte.MinValue");
        BindVariable((double)sbyte.MaxValue, "sbyte.MaxValue");
        BindVariable((double)sbyte.MinValue, "sbyte.MinValue");
        BindVariable((double)short.MaxValue, "short.MaxValue");
        BindVariable((double)short.MinValue, "short.MinValue");
        BindVariable((double)ushort.MaxValue, "ushort.MaxValue");
        BindVariable((double)ushort.MinValue, "ushort.MinValue");
        BindVariable((double)int.MaxValue, "int.MaxValue");
        BindVariable((double)int.MinValue, "int.MinValue");
        BindVariable((double)uint.MaxValue, "uint.MaxValue");
        BindVariable((double)uint.MinValue, "uint.MinValue");
        BindVariable((double)long.MaxValue, "long.MaxValue");
        BindVariable((double)long.MinValue, "long.MinValue");
        BindVariable((double)ulong.MaxValue, "ulong.MaxValue");
        BindVariable((double)ulong.MinValue, "ulong.MinValue");
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
        BindVariable((double)decimal.One, "decimal.One");
        BindVariable((double)decimal.MinusOne, "decimal.MinusOne");
        BindVariable((double)decimal.Zero, "decimal.Zero");
        BindVariable((double)decimal.MaxValue, "decimal.MaxValue");
        BindVariable((double)decimal.MinValue, "decimal.MinValue");

        BindVariable((double)Byte.MaxValue, "Byte.MaxValue");
        BindVariable((double)Byte.MinValue, "Byte.MinValue");
        BindVariable((double)SByte.MaxValue, "SByte.MaxValue");
        BindVariable((double)SByte.MinValue, "SByte.MinValue");
        BindVariable((double)Int16.MaxValue, "Int16.MaxValue");
        BindVariable((double)Int16.MinValue, "Int16.MinValue");
        BindVariable((double)UInt16.MaxValue, "UInt16.MaxValue");
        BindVariable((double)UInt16.MinValue, "UInt16.MinValue");
        BindVariable((double)Int32.MaxValue, "Int32.MaxValue");
        BindVariable((double)Int32.MinValue, "Int32.MinValue");
        BindVariable((double)UInt32.MaxValue, "UInt32.MaxValue");
        BindVariable((double)UInt32.MinValue, "UInt32.MinValue");
        BindVariable((double)Int64.MaxValue, "Int64.MaxValue");
        BindVariable((double)Int64.MinValue, "Int64.MinValue");
        BindVariable((double)UInt64.MaxValue, "UInt64.MaxValue");
        BindVariable((double)UInt64.MinValue, "UInt64.MinValue");
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

