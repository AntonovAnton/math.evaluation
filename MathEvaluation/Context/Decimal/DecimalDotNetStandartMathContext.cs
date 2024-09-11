using System;
using System.Linq.Expressions;
using MathEvaluation.Entities;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MathEvaluation.Context;
#pragma warning restore IDE0130 // Namespace does not match folder structure

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
        BindConstant(1d, "true");
        BindConstant(0d, "false");

        static decimal modFn(decimal left, decimal right) => left % right;
        BindOperator(modFn, '%', (int)EvalPrecedence.Basic, ExpressionType.Modulo);

        static decimal equalToFn(decimal left, decimal right) => left == right ? 1.0m : default;
        BindOperator(equalToFn, "==", (int)EvalPrecedence.Equality, ExpressionType.Equal);

        static decimal notEqualToFn(decimal left, decimal right) => left != right ? 1.0m : default;
        BindOperator(notEqualToFn, "!=", (int)EvalPrecedence.Equality, ExpressionType.NotEqual);

        static decimal greaterThanFn(decimal left, decimal right) => left > right ? 1.0m : default;
        BindOperator(greaterThanFn, '>', (int)EvalPrecedence.RelationalOperator, ExpressionType.GreaterThan);

        static decimal lessThanFn(decimal left, decimal right) => left < right ? 1.0m : default;
        BindOperator(lessThanFn, '<', (int)EvalPrecedence.RelationalOperator, ExpressionType.LessThan);

        static decimal greaterThanOrEqualToFn(decimal left, decimal right) => left >= right ? 1.0m : default;
        BindOperator(greaterThanOrEqualToFn, ">=", (int)EvalPrecedence.RelationalOperator, ExpressionType.GreaterThanOrEqual);

        static decimal lessThanOrEqualToFn(decimal left, decimal right) => left <= right ? 1.0m : default;
        BindOperator(lessThanOrEqualToFn, "<=", (int)EvalPrecedence.RelationalOperator, ExpressionType.LessThanOrEqual);

        static decimal andFn(decimal left, decimal right) => left != default && right != default ? 1.0m : default;
        BindOperator(andFn, "&&", (int)EvalPrecedence.LogicalConditionalAnd, ExpressionType.AndAlso);

        static decimal orFn(decimal left, decimal right) => left != default || right != default ? 1.0m : default;
        BindOperator(orFn, "||", (int)EvalPrecedence.LogicalConditionalOr, ExpressionType.OrElse);

        static decimal logicalNegationFn(decimal right) => right == default ? 1.0m : default;
        BindOperandOperator(logicalNegationFn, '!');

        static decimal logicalAndFn(decimal left, decimal right) => (long)left & (long)right;
        BindOperator(logicalAndFn, '&', (int)EvalPrecedence.LogicalAnd, ExpressionType.And);

        static decimal logicalOrFn(decimal left, decimal right) => (long)left | (long)right;
        BindOperator(logicalOrFn, '|', (int)EvalPrecedence.LogicalOr, ExpressionType.Or);

        static decimal logicalExclusiveOrFn(decimal left, decimal right) => (long)left ^ (long)right;
        BindOperator(logicalExclusiveOrFn, '^', (int)EvalPrecedence.LogicalXor, ExpressionType.ExclusiveOr);

        static decimal bitwiseComplementFn(decimal right) => ~(long)right;
        BindOperandOperator(bitwiseComplementFn, '~');

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

        BindConstant(Math.PI);
        BindConstant(Math.E);

        static decimal absFn(decimal v) => Math.Abs(v);
        BindFunction(absFn, "Math.Abs");

        static double acosFn(double v) => Math.Acos(v);
        BindFunction(acosFn, "Math.Acos");

        static double acoshFn(double v) => Math.Acosh(v);
        BindFunction(acoshFn, "Math.Acosh");

        static double asinFn(double v) => Math.Asin(v);
        BindFunction(asinFn, "Math.Asin");

        static double asinhFn(double v) => Math.Asinh(v);
        BindFunction(asinhFn, "Math.Asinh");

        static double atanFn(double v) => Math.Atan(v);
        BindFunction(atanFn, "Math.Atan");

        static double atan2Fn(double y, double x) => Math.Atan2(y, x);
        BindFunction(atan2Fn, "Math.Atan2");

        static double atanhFn(double v) => Math.Atanh(v);
        BindFunction(atanhFn, "Math.Atanh");

        static decimal bigMul(decimal a, decimal b) => Math.BigMul((int)a, (int)b);
        BindFunction(bigMul, "Math.BigMul");

        static double cbrtFn(double value) => Math.Cbrt(value);
        BindFunction(cbrtFn, "Math.Cbrt");

        static decimal ceilingFn(decimal value) => Math.Ceiling(value);
        BindFunction(ceilingFn, "Math.Ceiling");

        static decimal clampFn(decimal value, decimal min, decimal max) => Math.Clamp(value, min, max);
        BindFunction(clampFn, "Math.Clamp");

        static double cosFn(double value) => Math.Cos(value);
        BindFunction(cosFn, "Math.Cos");

        static double coshFn(double value) => Math.Cosh(value);
        BindFunction(coshFn, "Math.Cosh");

        static double expFn(double value) => Math.Exp(value);
        BindFunction(expFn, "Math.Exp");

        static decimal floorFn(decimal value) => Math.Floor(value);
        BindFunction(floorFn, "Math.Floor");

        static double remainderFn(double x, double y) => Math.IEEERemainder(x, y);
        BindFunction(remainderFn, "Math.IEEERemainder");

        static double logFn(double[] args) => args.Length == 1 ? Math.Log(args[0]) : Math.Log(args[0], args[1]);
        BindFunction(logFn, "Math.Log");

        static double log10Fn(double value) => Math.Log10(value);
        BindFunction(log10Fn, "Math.Log10");

        static decimal maxFn(decimal val1, decimal val2) => Math.Max(val1, val2);
        BindFunction(maxFn, "Math.Max");

        static decimal minFn(decimal val1, decimal val2) => Math.Min(val1, val2);
        BindFunction(minFn, "Math.Min");

        static double powFn(double x, double y) => Math.Pow(x, y);
        BindFunction(powFn, "Math.Pow");

        static decimal roundFn(decimal[] args) => args.Length == 1 ? Math.Round(args[0]) : Math.Round(args[0], (int)args[1]);
        BindFunction(roundFn, "Math.Round");

        static decimal signFn(decimal value) => Math.Sign(value);
        BindFunction(signFn, "Math.Sign");

        static double sinFn(double value) => Math.Sin(value);
        BindFunction(sinFn, "Math.Sin");

        static double sinhFn(double value) => Math.Sinh(value);
        BindFunction(sinhFn, "Math.Sinh");

        static double sqrtFn(double value) => Math.Sqrt(value);
        BindFunction(sqrtFn, "Math.Sqrt");

        static double tanFn(double value) => Math.Tan(value);
        BindFunction(tanFn, "Math.Tan");

        static double tanhFn(double value) => Math.Tanh(value);
        BindFunction(tanhFn, "Math.Tanh");

        static decimal truncateFn(decimal value) => Math.Truncate(value);
        BindFunction(truncateFn, "Math.Truncate");

        //double.ToString() represents double.PositiveInfinity from time to time as "Infinity" or '∞'
        BindConstant(double.PositiveInfinity, "Infinity");
        BindConstant(double.PositiveInfinity, '\u221e'); //infinity symbol

        BindConstant(float.NaN);
        BindConstant(float.Epsilon);
        BindConstant(float.PositiveInfinity);
        BindConstant(float.NegativeInfinity);
        BindConstant(float.MaxValue);
        BindConstant(float.MinValue);
        BindConstant(double.NaN);
        BindConstant(double.Epsilon);
        BindConstant(double.PositiveInfinity);
        BindConstant(double.NegativeInfinity);
        BindConstant(double.MaxValue);
        BindConstant(double.MinValue);
        BindConstant(decimal.One);
        BindConstant(decimal.MinusOne);
        BindConstant(decimal.Zero);
        BindConstant(decimal.MaxValue);
        BindConstant(decimal.MinValue);

        BindConstant(Single.NaN);
        BindConstant(Single.Epsilon);
        BindConstant(Single.PositiveInfinity);
        BindConstant(Single.NegativeInfinity);
        BindConstant(Single.MaxValue);
        BindConstant(Single.MinValue);
        BindConstant(Double.NaN);
        BindConstant(Double.Epsilon);
        BindConstant(Double.PositiveInfinity);
        BindConstant(Double.NegativeInfinity);
        BindConstant(Double.MaxValue);
        BindConstant(Double.MinValue);

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

