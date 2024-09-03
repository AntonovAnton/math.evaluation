using System;
using MathEvaluation.Entities;

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
        BindConstant(1m, "true");
        BindConstant(0m, "false");

        static decimal modFn(decimal left, decimal right) => left % right;
        BindOperator(modFn, '%');

        static decimal equalToFn(decimal left, decimal right) => left == right ? 1.0m : default;
        BindOperator(equalToFn, "==", (int)EvalPrecedence.Equality);

        static decimal notEqualToFn(decimal left, decimal right) => left != right ? 1.0m : default;
        BindOperator(notEqualToFn, "!=", (int)EvalPrecedence.Equality);

        static decimal greaterThanFn(decimal left, decimal right) => left > right ? 1.0m : default;
        BindOperator(greaterThanFn, '>', (int)EvalPrecedence.RelationalOperator);

        static decimal lessThanFn(decimal left, decimal right) => left < right ? 1.0m : default;
        BindOperator(lessThanFn, '<', (int)EvalPrecedence.RelationalOperator);

        static decimal greaterThanOrEqualToFn(decimal left, decimal right) => left >= right ? 1.0m : default;
        BindOperator(greaterThanOrEqualToFn, ">=", (int)EvalPrecedence.RelationalOperator);

        static decimal lessThanOrEqualToFn(decimal left, decimal right) => left <= right ? 1.0m : default;
        BindOperator(lessThanOrEqualToFn, "<=", (int)EvalPrecedence.RelationalOperator);

        static decimal andFn(decimal left, decimal right) => left != default && right != default ? 1.0m : default;
        BindOperator(andFn, "&&", (int)EvalPrecedence.LogicalConditionalAnd);

        static decimal orFn(decimal left, decimal right) => left != default || right != default ? 1.0m : default;
        BindOperator(orFn, "||", (int)EvalPrecedence.LogicalConditionalOr);

        static decimal logicalNegationFn(decimal right) => right == default ? 1.0m : default;
        BindOperandOperator(logicalNegationFn, '!');

        static decimal logicalAndFn(decimal left, decimal right) => (long)left & (long)right;
        BindOperator(logicalAndFn, '&', (int)EvalPrecedence.LogicalAnd);

        static decimal logicalOrFn(decimal left, decimal right) => (long)left | (long)right;
        BindOperator(logicalOrFn, '|', (int)EvalPrecedence.LogicalOr);

        static decimal logicalExclusiveOrFn(decimal left, decimal right) => (long)left ^ (long)right;
        BindOperator(logicalExclusiveOrFn, '^', (int)EvalPrecedence.LogicalXor);

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

        BindOperandOperator((decimal value) => value, 'f', true);
        BindOperandOperator((decimal value) => value, 'd', true);
        BindOperandOperator((decimal value) => value, 'm', true);
        BindOperandOperator((decimal value) => value, 'l', true);
        BindOperandOperator((decimal value) => value, 'u', true);
        BindOperandOperator((decimal value) => value, "ul", true);
        BindOperandOperator((decimal value) => value, "lu", true);
        BindOperandOperator((decimal value) => value, 'F', true);
        BindOperandOperator((decimal value) => value, 'D', true);
        BindOperandOperator((decimal value) => value, 'M', true);
        BindOperandOperator((decimal value) => value, 'L', true);
        BindOperandOperator((decimal value) => value, "Lu", true);
        BindOperandOperator((decimal value) => value, "LU", true);
        BindOperandOperator((decimal value) => value, 'U', true);
        BindOperandOperator((decimal value) => value, "Ul", true);
        BindOperandOperator((decimal value) => value, "UL", true);

        BindConstant(Math.PI);
        BindConstant(Math.E);

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
        BindFunction((decimal[] args) => args.Length == 1 ? Math.Round(args[0]) : Math.Round(args[0], (int)args[1]), "Math.Round");
        BindFunction((decimal value) => Math.Sign(value), "Math.Sign");
        BindFunction(Math.Sin);
        BindFunction(Math.Sinh);
        BindFunction(Math.Sqrt);
        BindFunction(Math.Tan);
        BindFunction(Math.Tanh);
        BindFunction((decimal value) => Math.Truncate(value), "Math.Truncate");

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

