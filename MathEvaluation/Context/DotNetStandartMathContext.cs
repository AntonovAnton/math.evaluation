using System;
using MathEvaluation.Entities;

namespace MathEvaluation.Context;

/// <summary>
/// The .NET Standart 2.1 programming math context 
/// supports all constants and functions provided by <see cref="System.Math" /> class.
/// Provides evaluating C# math expressions.
/// </summary>
/// <seealso cref="MathEvaluation.Context.MathContext" />
public class DotNetStandartMathContext : MathContext
{
    /// <summary>Initializes a new instance of the <see cref="DotNetStandartMathContext" /> class.</summary>
    public DotNetStandartMathContext()
        : base()
    {
        BindConstant(1d, "true");
        BindConstant(0d, "false");

        static double modFn(double left, double right) => left % right;
        BindOperator(modFn, '%', (int)EvalPrecedence.Basic);

        static double equalToFn(double left, double right) => left == right ? 1.0 : default;
        BindOperator(equalToFn, "==", (int)EvalPrecedence.Equality);

        static double notEqualToFn(double left, double right) => left != right ? 1.0 : default;
        BindOperator(notEqualToFn, "!=", (int)EvalPrecedence.Equality);

        static double greaterThanFn(double left, double right) => left > right ? 1.0 : default;
        BindOperator(greaterThanFn, '>', (int)EvalPrecedence.RelationalOperator);

        static double lessThanFn(double left, double right) => left < right ? 1.0 : default;
        BindOperator(lessThanFn, '<', (int)EvalPrecedence.RelationalOperator);

        static double greaterThanOrEqualToFn(double left, double right) => left >= right ? 1.0 : default;
        BindOperator(greaterThanOrEqualToFn, ">=", (int)EvalPrecedence.RelationalOperator);

        static double lessThanOrEqualToFn(double left, double right) => left <= right ? 1.0 : default;
        BindOperator(lessThanOrEqualToFn, "<=", (int)EvalPrecedence.RelationalOperator);

        static double andFn(double left, double right) => left != default && right != default ? 1.0 : default;
        BindOperator(andFn, "&&", (int)EvalPrecedence.LogicalConditionalAnd);

        static double orFn(double left, double right) => left != default || right != default ? 1.0 : default;
        BindOperator(orFn, "||", (int)EvalPrecedence.LogicalConditionalOr);

        static double logicalNegationFn(double right) => right == default ? 1.0 : default;
        BindOperandOperator(logicalNegationFn, '!');

        static double logicalAndFn(double left, double right) => (long)left & (long)right;
        BindOperator(logicalAndFn, '&', (int)EvalPrecedence.LogicalAnd);

        static double logicalOrFn(double left, double right) => (long)left | (long)right;
        BindOperator(logicalOrFn, '|', (int)EvalPrecedence.LogicalOr);

        static double logicalExclusiveOrFn(double left, double right) => (long)left ^ (long)right;
        BindOperator(logicalExclusiveOrFn, '^', (int)EvalPrecedence.LogicalXor);

        static double bitwiseComplementFn(double right) => ~(long)right;
        BindOperandOperator(bitwiseComplementFn, '~');

        static double postfixIncrementFn(double left) => left++;
        BindOperandOperator(postfixIncrementFn, "++ ", true);
        BindOperandOperator(postfixIncrementFn, "++\t", true);
        BindOperandOperator(postfixIncrementFn, "++\r", true);
        BindOperandOperator(postfixIncrementFn, "++\n", true);

        static double postfixDecrementFn(double left) => left--;
        BindOperandOperator(postfixDecrementFn, "-- ", true);
        BindOperandOperator(postfixDecrementFn, "--\t", true);
        BindOperandOperator(postfixDecrementFn, "--\r", true);
        BindOperandOperator(postfixDecrementFn, "--\n", true);

        BindOperandOperator((double value) => value, 'f', true);
        BindOperandOperator((double value) => value, 'd', true);
        BindOperandOperator((double value) => value, 'm', true);
        BindOperandOperator((double value) => value, 'l', true);
        BindOperandOperator((double value) => value, 'u', true);
        BindOperandOperator((double value) => value, "ul", true);
        BindOperandOperator((double value) => value, "lu", true);
        BindOperandOperator((double value) => value, 'F', true);
        BindOperandOperator((double value) => value, 'D', true);
        BindOperandOperator((double value) => value, 'M', true);
        BindOperandOperator((double value) => value, 'L', true);
        BindOperandOperator((double value) => value, "Lu", true);
        BindOperandOperator((double value) => value, "LU", true);
        BindOperandOperator((double value) => value, 'U', true);
        BindOperandOperator((double value) => value, "Ul", true);
        BindOperandOperator((double value) => value, "UL", true);

        BindConstant(Math.PI);
        BindConstant(Math.E);

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
        BindFunction((double[] args) => args.Length == 1 ? Math.Round(args[0]) : Math.Round(args[0], (int)args[1]), "Math.Round");
        BindFunction((double value) => Math.Sign(value), "Math.Sign");
        BindFunction(Math.Sin);
        BindFunction(Math.Sinh);
        BindFunction(Math.Sqrt);
        BindFunction(Math.Tan);
        BindFunction(Math.Tanh);
        BindFunction((double value) => Math.Truncate(value), "Math.Truncate");

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
        BindConstant((double)decimal.One, "decimal.One");
        BindConstant((double)decimal.MinusOne, "decimal.MinusOne");
        BindConstant((double)decimal.Zero, "decimal.Zero");
        BindConstant((double)decimal.MaxValue, "decimal.MaxValue");
        BindConstant((double)decimal.MinValue, "decimal.MinValue");

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

        BindConstant((double)byte.MaxValue, "byte.MaxValue");
        BindConstant((double)byte.MinValue, "byte.MinValue");
        BindConstant((double)sbyte.MaxValue, "sbyte.MaxValue");
        BindConstant((double)sbyte.MinValue, "sbyte.MinValue");
        BindConstant((double)short.MaxValue, "short.MaxValue");
        BindConstant((double)short.MinValue, "short.MinValue");
        BindConstant((double)ushort.MaxValue, "ushort.MaxValue");
        BindConstant((double)ushort.MinValue, "ushort.MinValue");
        BindConstant((double)int.MaxValue, "int.MaxValue");
        BindConstant((double)int.MinValue, "int.MinValue");
        BindConstant((double)uint.MaxValue, "uint.MaxValue");
        BindConstant((double)uint.MinValue, "uint.MinValue");
        BindConstant((double)long.MaxValue, "long.MaxValue");
        BindConstant((double)long.MinValue, "long.MinValue");
        BindConstant((double)ulong.MaxValue, "ulong.MaxValue");
        BindConstant((double)ulong.MinValue, "ulong.MinValue");

        BindConstant((double)Byte.MaxValue, "Byte.MaxValue");
        BindConstant((double)Byte.MinValue, "Byte.MinValue");
        BindConstant((double)SByte.MaxValue, "SByte.MaxValue");
        BindConstant((double)SByte.MinValue, "SByte.MinValue");
        BindConstant((double)Int16.MaxValue, "Int16.MaxValue");
        BindConstant((double)Int16.MinValue, "Int16.MinValue");
        BindConstant((double)UInt16.MaxValue, "UInt16.MaxValue");
        BindConstant((double)UInt16.MinValue, "UInt16.MinValue");
        BindConstant((double)Int32.MaxValue, "Int32.MaxValue");
        BindConstant((double)Int32.MinValue, "Int32.MinValue");
        BindConstant((double)UInt32.MaxValue, "UInt32.MaxValue");
        BindConstant((double)UInt32.MinValue, "UInt32.MinValue");
        BindConstant((double)Int64.MaxValue, "Int64.MaxValue");
        BindConstant((double)Int64.MinValue, "Int64.MinValue");
        BindConstant((double)UInt64.MaxValue, "UInt64.MaxValue");
        BindConstant((double)UInt64.MinValue, "UInt64.MinValue");
    }
}

