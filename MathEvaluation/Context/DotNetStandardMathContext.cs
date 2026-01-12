using System;
using System.Numerics;
using MathEvaluation.Entities;
// ReSharper disable BuiltInTypeReferenceStyleForMemberAccess

namespace MathEvaluation.Context;

/// <summary>
///     The .NET Standard 2.1 programming math context
///     supports all constants and functions provided by <see cref="System.Math" /> and <see cref="System.Numerics.Complex" /> classes.
///     Provides evaluating C# math expressions.
/// </summary>
/// <seealso cref="MathEvaluation.Context.MathContext" />
public class DotNetStandardMathContext : MathContext
{
    /// <summary>Initializes a new instance of the <see cref="DotNetStandardMathContext" /> class.</summary>
    public DotNetStandardMathContext()
        : base()
    {
        BindConstant(1d, "true");
        BindConstant(0d, "false");

        BindConstant(0d, "default");
        BindConstant(0d, "default(T)");
        BindConstant(0d, "default(bool)");
        BindConstant(0d, "default(double)");
        BindConstant(0d, "default(float)");
        BindConstant(0m, "default(decimal)");
        BindConstant(0d, "default(int)");
        BindConstant(0d, "default(uint)");
        BindConstant(0d, "default(long)");
        BindConstant(0d, "default(ulong)");
        BindConstant(0d, "default(byte)");
        BindConstant(0d, "default(sbyte)");
        BindConstant(0d, "default(short)");
        BindConstant(0d, "default(ushort)");
        BindConstant(0d, "default(char)");
        BindConstant(0d, "default(Boolean)");
        BindConstant(0d, "default(Double)");
        BindConstant(0d, "default(Single)");
        BindConstant(0m, "default(Decimal)");
        BindConstant(0d, "default(Int32)");
        BindConstant(0d, "default(UInt32)");
        BindConstant(0d, "default(Int64)");
        BindConstant(0d, "default(UInt64)");
        BindConstant(0d, "default(Byte)");
        BindConstant(0d, "default(SByte)");
        BindConstant(0d, "default(Int16)");
        BindConstant(0d, "default(UInt16)");
        BindConstant(0d, "default(Char)");

        BindOperator('%', OperatorType.Modulo);

        BindOperator("==", OperatorType.Equal);
        BindOperator("!=", OperatorType.NotEqual);

        BindOperator('>', OperatorType.GreaterThan);
        BindOperator('<', OperatorType.LessThan);
        BindOperator(">=", OperatorType.GreaterThanOrEqual);
        BindOperator("<=", OperatorType.LessThanOrEqual);

        BindOperator("&&", OperatorType.LogicalConditionalAnd);
        BindOperator("||", OperatorType.LogicalConditionalOr);
        BindOperator('!', OperatorType.LogicalNegation);

        BindOperator('&', OperatorType.BitwiseAnd);
        BindOperator('|', OperatorType.BitwiseOr);
        BindOperator('^', OperatorType.BitwiseXor);
        BindOperator('~', OperatorType.BitwiseNegation);

        static double postfixIncrementFn(double left) => left;

        BindOperandOperator<double>(postfixIncrementFn, "++ ", true);
        BindOperandOperator<double>(postfixIncrementFn, "++\t", true);
        BindOperandOperator<double>(postfixIncrementFn, "++\r", true);
        BindOperandOperator<double>(postfixIncrementFn, "++\n", true);

        static double postfixDecrementFn(double left) => left;

        BindOperandOperator<double>(postfixDecrementFn, "-- ", true);
        BindOperandOperator<double>(postfixDecrementFn, "--\t", true);
        BindOperandOperator<double>(postfixDecrementFn, "--\r", true);
        BindOperandOperator<double>(postfixDecrementFn, "--\n", true);

        BindConstant(1d, 'f');
        BindConstant(1d, 'd');
        BindConstant(1m, 'm');
        BindConstant(1d, 'l');
        BindConstant(1d, 'u');
        BindConstant(1d, "ul");
        BindConstant(1d, "lu");
        BindConstant(1d, 'F');
        BindConstant(1d, 'D');
        BindConstant(1m, 'M');
        BindConstant(1d, 'L');
        BindConstant(1d, "Lu");
        BindConstant(1d, "LU");
        BindConstant(1d, 'U');
        BindConstant(1d, "Ul");
        BindConstant(1d, "UL");
        BindConstant(1d, "double");
        BindConstant(1d, "float");
        BindConstant(1m, "decimal");
        BindConstant(1d, "short");
        BindConstant(1d, "ushort");
        BindConstant(1d, "int");
        BindConstant(1d, "uint");
        BindConstant(1d, "long");
        BindConstant(1d, "ulong");
        BindConstant(1d, "byte");
        BindConstant(1d, "sbyte");
        BindConstant(1d, "char");
        BindConstant(1d, "Double");
        BindConstant(1d, "Single");
        BindConstant(1m, "Decimal");
        BindConstant(1d, "Int16");
        BindConstant(1d, "UInt16");
        BindConstant(1d, "Int32");
        BindConstant(1d, "UInt32");
        BindConstant(1d, "Int64");
        BindConstant(1d, "UInt64");
        BindConstant(1d, "Byte");
        BindConstant(1d, "SByte");
        BindConstant(1d, "Char");

        BindConstant(Math.PI);
        BindConstant(Math.E);

        static double absFn(double v) => Math.Abs(v);

        BindFunction<double>(absFn, "Math.Abs");

        static double acosFn(double v) => Math.Acos(v);

        BindFunction<double>(acosFn, "Math.Acos");

        static double acoshFn(double v) => Math.Acosh(v);

        BindFunction<double>(acoshFn, "Math.Acosh");

        static double asinFn(double v) => Math.Asin(v);

        BindFunction<double>(asinFn, "Math.Asin");

        static double asinhFn(double v) => Math.Asinh(v);

        BindFunction<double>(asinhFn, "Math.Asinh");

        static double atanFn(double v) => Math.Atan(v);

        BindFunction<double>(atanFn, "Math.Atan");

        static double atan2Fn(double y, double x) => Math.Atan2(y, x);

        BindFunction<double>(atan2Fn, "Math.Atan2");

        static double atanhFn(double v) => Math.Atanh(v);

        BindFunction<double>(atanhFn, "Math.Atanh");

        static double bigMul(double a, double b) => Math.BigMul((int)a, (int)b);

        BindFunction<double>(bigMul, "Math.BigMul");

        static double cbrtFn(double value) => Math.Cbrt(value);

        BindFunction<double>(cbrtFn, "Math.Cbrt");

        static double ceilingFn(double value) => Math.Ceiling(value);

        BindFunction<double>(ceilingFn, "Math.Ceiling");

        static double clampFn(double value, double min, double max) => Math.Clamp(value, min, max);

        BindFunction<double>(clampFn, "Math.Clamp");

        static double cosFn(double value) => Math.Cos(value);

        BindFunction<double>(cosFn, "Math.Cos");

        static double coshFn(double value) => Math.Cosh(value);

        BindFunction<double>(coshFn, "Math.Cosh");

        static double expFn(double value) => Math.Exp(value);

        BindFunction<double>(expFn, "Math.Exp");

        static double floorFn(double value) => Math.Floor(value);

        BindFunction<double>(floorFn, "Math.Floor");

        static double remainderFn(double x, double y) => Math.IEEERemainder(x, y);

        BindFunction<double>(remainderFn, "Math.IEEERemainder");

        static double logFn(double[] args) => args.Length == 1 ? Math.Log(args[0]) : Math.Log(args[0], args[1]);

        BindFunction<double>(logFn, "Math.Log");

        static double log10Fn(double value) => Math.Log10(value);

        BindFunction<double>(log10Fn, "Math.Log10");

        static double maxFn(double val1, double val2) => Math.Max(val1, val2);

        BindFunction<double>(maxFn, "Math.Max");

        static double minFn(double val1, double val2) => Math.Min(val1, val2);

        BindFunction<double>(minFn, "Math.Min");

        static double powFn(double x, double y) => Math.Pow(x, y);

        BindFunction<double>(powFn, "Math.Pow");

        static double roundFn(double[] args) => args.Length == 1 ? Math.Round(args[0]) : Math.Round(args[0], (int)args[1]);

        BindFunction<double>(roundFn, "Math.Round");

        static double signFn(double value) => Math.Sign(value);

        BindFunction<double>(signFn, "Math.Sign");

        static double sinFn(double value) => Math.Sin(value);

        BindFunction<double>(sinFn, "Math.Sin");

        static double sinhFn(double value) => Math.Sinh(value);

        BindFunction<double>(sinhFn, "Math.Sinh");

        static double sqrtFn(double value) => Math.Sqrt(value);

        BindFunction<double>(sqrtFn, "Math.Sqrt");

        static double tanFn(double value) => Math.Tan(value);

        BindFunction<double>(tanFn, "Math.Tan");

        static double tanhFn(double value) => Math.Tanh(value);

        BindFunction<double>(tanhFn, "Math.Tanh");

        static double truncateFn(double value) => Math.Truncate(value);

        BindFunction<double>(truncateFn, "Math.Truncate");

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

        BindConstant((double)byte.MaxValue, "Byte.MaxValue");
        BindConstant((double)byte.MinValue, "Byte.MinValue");
        BindConstant((double)sbyte.MaxValue, "SByte.MaxValue");
        BindConstant((double)sbyte.MinValue, "SByte.MinValue");
        BindConstant((double)short.MaxValue, "Int16.MaxValue");
        BindConstant((double)short.MinValue, "Int16.MinValue");
        BindConstant((double)ushort.MaxValue, "UInt16.MaxValue");
        BindConstant((double)ushort.MinValue, "UInt16.MinValue");
        BindConstant((double)int.MaxValue, "Int32.MaxValue");
        BindConstant((double)int.MinValue, "Int32.MinValue");
        BindConstant((double)uint.MaxValue, "UInt32.MaxValue");
        BindConstant((double)uint.MinValue, "UInt32.MinValue");
        BindConstant((double)long.MaxValue, "Int64.MaxValue");
        BindConstant((double)long.MinValue, "Int64.MinValue");
        BindConstant((double)ulong.MaxValue, "UInt64.MaxValue");
        BindConstant((double)ulong.MinValue, "UInt64.MinValue");

        #region

        BindConstant(Complex.One, "Complex");

        BindConstant(Complex.Zero, "default(Complex)");
        BindConstant(Complex.Zero);
        BindConstant(Complex.One);
        BindConstant(Complex.ImaginaryOne);

        static Complex newComplexFn(Complex arg1, Complex arg2) => new(arg1.Real, arg2.Real);

        BindFunction<Complex>(newComplexFn, "new Complex");

        static Complex absComplexFn(Complex v) => Complex.Abs(v);

        BindFunction<Complex>(absComplexFn, "Complex.Abs");

        static Complex acosComplexFn(Complex v) => Complex.Acos(v);

        BindFunction<Complex>(acosComplexFn, "Complex.Acos");

        static Complex asinComplexFn(Complex v) => Complex.Asin(v);

        BindFunction<Complex>(asinComplexFn, "Complex.Asin");

        static Complex atanComplexFn(Complex v) => Complex.Atan(v);

        BindFunction<Complex>(atanComplexFn, "Complex.Atan");

        static Complex cosComplexFn(Complex value) => Complex.Cos(value);

        BindFunction<Complex>(cosComplexFn, "Complex.Cos");

        static Complex coshComplexFn(Complex value) => Complex.Cosh(value);

        BindFunction<Complex>(coshComplexFn, "Complex.Cosh");

        static Complex expComplexFn(Complex value) => Complex.Exp(value);

        BindFunction<Complex>(expComplexFn, "Complex.Exp");

        static Complex logComplexFn(Complex[] args) => args.Length == 1 ? Complex.Log(args[0]) : Complex.Log(args[0], args[1].Real);

        BindFunction<Complex>(logComplexFn, "Complex.Log");

        static Complex log10ComplexFn(Complex value) => Complex.Log10(value);

        BindFunction<Complex>(log10ComplexFn, "Complex.Log10");

        static Complex powComplexFn(Complex x, Complex y) => Complex.Pow(x, y);

        BindFunction<Complex>(powComplexFn, "Complex.Pow");

        static Complex sinComplexFn(Complex value) => Complex.Sin(value);

        BindFunction<Complex>(sinComplexFn, "Complex.Sin");

        static Complex sinhComplexFn(Complex value) => Complex.Sinh(value);

        BindFunction<Complex>(sinhComplexFn, "Complex.Sinh");

        static Complex sqrtComplexFn(Complex value) => Complex.Sqrt(value);

        BindFunction<Complex>(sqrtComplexFn, "Complex.Sqrt");

        static Complex tanComplexFn(Complex value) => Complex.Tan(value);

        BindFunction<Complex>(tanComplexFn, "Complex.Tan");

        static Complex tanhComplexFn(Complex value) => Complex.Tanh(value);

        BindFunction<Complex>(tanhComplexFn, "Complex.Tanh");

        static Complex addComplexFn(Complex left, Complex right) => Complex.Add(left, right);

        BindFunction<Complex>(addComplexFn, "Complex.Add");

        static Complex conjugateComplexFn(Complex value) => Complex.Conjugate(value);

        BindFunction<Complex>(conjugateComplexFn, "Complex.Conjugate");

        static Complex divideComplexFn(Complex left, Complex right) => Complex.Divide(left, right);

        BindFunction<Complex>(divideComplexFn, "Complex.Divide");

        static Complex fpcComplexFn(Complex magnitude, Complex phase) => Complex.FromPolarCoordinates(magnitude.Real, phase.Real);

        BindFunction<Complex>(fpcComplexFn, "Complex.FromPolarCoordinates");

        static Complex multiplyComplexFn(Complex left, Complex right) => Complex.Multiply(left, right);

        BindFunction<Complex>(multiplyComplexFn, "Complex.Multiply");

        static Complex negateComplexFn(Complex value) => Complex.Negate(value);

        BindFunction<Complex>(negateComplexFn, "Complex.Negate");

        static Complex reciprocalComplexFn(Complex value) => Complex.Reciprocal(value);

        BindFunction<Complex>(reciprocalComplexFn, "Complex.Reciprocal");

        static Complex subtractComplexFn(Complex left, Complex right) => Complex.Subtract(left, right);

        BindFunction<Complex>(subtractComplexFn, "Complex.Subtract");

        #endregion
    }
}
