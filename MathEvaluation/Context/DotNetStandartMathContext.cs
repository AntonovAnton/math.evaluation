﻿using MathEvaluation.Entities;
using System;
using System.Numerics;

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

        BindConstant(1d, 'f');
        BindConstant(1d, 'd');
        BindConstant(1d, 'm');
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
        BindConstant(1d, "short");
        BindConstant(1d, "ushort");
        BindConstant(1d, "long");
        BindConstant(1d, "ulong");
        BindConstant(1d, "int");
        BindConstant(1d, "uint");
        BindConstant(1d, "byte");
        BindConstant(1d, "sbyte");
        BindConstant(1m, "decimal");

        BindConstant(Math.PI);
        BindConstant(Math.E);

        static double absFn(double v) => Math.Abs(v);
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

        static double bigMul(double a, double b) => Math.BigMul((int)a, (int)b);
        BindFunction(bigMul, "Math.BigMul");

        static double cbrtFn(double value) => Math.Cbrt(value);
        BindFunction(cbrtFn, "Math.Cbrt");

        static double ceilingFn(double value) => Math.Ceiling(value);
        BindFunction(ceilingFn, "Math.Ceiling");

        static double clampFn(double value, double min, double max) => Math.Clamp(value, min, max);
        BindFunction(clampFn, "Math.Clamp");

        static double cosFn(double value) => Math.Cos(value);
        BindFunction(cosFn, "Math.Cos");

        static double coshFn(double value) => Math.Cosh(value);
        BindFunction(coshFn, "Math.Cosh");

        static double expFn(double value) => Math.Exp(value);
        BindFunction(expFn, "Math.Exp");

        static double floorFn(double value) => Math.Floor(value);
        BindFunction(floorFn, "Math.Floor");

        static double remainderFn(double x, double y) => Math.IEEERemainder(x, y);
        BindFunction(remainderFn, "Math.IEEERemainder");

        static double logFn(double[] args) => args.Length == 1 ? Math.Log(args[0]) : Math.Log(args[0], args[1]);
        BindFunction(logFn, "Math.Log");

        static double log10Fn(double value) => Math.Log10(value);
        BindFunction(log10Fn, "Math.Log10");

        static double maxFn(double val1, double val2) => Math.Max(val1, val2);
        BindFunction(maxFn, "Math.Max");

        static double minFn(double val1, double val2) => Math.Min(val1, val2);
        BindFunction(minFn, "Math.Min");

        static double powFn(double x, double y) => Math.Pow(x, y);
        BindFunction(powFn, "Math.Pow");

        static double roundFn(double[] args) => args.Length == 1 ? Math.Round(args[0]) : Math.Round(args[0], (int)args[1]);
        BindFunction(roundFn, "Math.Round");

        static double signFn(double value) => Math.Sign(value);
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

        static double truncateFn(double value) => Math.Truncate(value);
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

        #region

        BindConstant(Complex.One, "Complex");

        BindConstant(Complex.Zero);
        BindConstant(Complex.One);
        BindConstant(Complex.ImaginaryOne);

        static Complex newComplexFn(Complex arg1, Complex arg2) => new Complex(arg1.Real, arg2.Real);
        BindFunction(newComplexFn, "new Complex");

        static Complex absComplexFn(Complex v) => Complex.Abs(v);
        BindFunction(absComplexFn, "Complex.Abs");

        static Complex acosComplexFn(Complex v) => Complex.Acos(v);
        BindFunction(acosComplexFn, "Complex.Acos");

        static Complex asinComplexFn(Complex v) => Complex.Asin(v);
        BindFunction(asinComplexFn, "Complex.Asin");

        static Complex atanComplexFn(Complex v) => Complex.Atan(v);
        BindFunction(atanComplexFn, "Complex.Atan");

        static Complex cosComplexFn(Complex value) => Complex.Cos(value);
        BindFunction(cosComplexFn, "Complex.Cos");

        static Complex coshComplexFn(Complex value) => Complex.Cosh(value);
        BindFunction(coshComplexFn, "Complex.Cosh");

        static Complex expComplexFn(Complex value) => Complex.Exp(value);
        BindFunction(expComplexFn, "Complex.Exp");

        static Complex logComplexFn(Complex[] args) => args.Length == 1 ? Complex.Log(args[0]) : Complex.Log(args[0], args[1].Real);
        BindFunction(logComplexFn, "Complex.Log");

        static Complex log10ComplexFn(Complex value) => Complex.Log10(value);
        BindFunction(log10ComplexFn, "Complex.Log10");

        static Complex powComplexFn(Complex x, Complex y) => Complex.Pow(x, y);
        BindFunction(powComplexFn, "Complex.Pow");

        static Complex sinComplexFn(Complex value) => Complex.Sin(value);
        BindFunction(sinComplexFn, "Complex.Sin");

        static Complex sinhComplexFn(Complex value) => Complex.Sinh(value);
        BindFunction(sinhComplexFn, "Complex.Sinh");

        static Complex sqrtComplexFn(Complex value) => Complex.Sqrt(value);
        BindFunction(sqrtComplexFn, "Complex.Sqrt");

        static Complex tanComplexFn(Complex value) => Complex.Tan(value);
        BindFunction(tanComplexFn, "Complex.Tan");

        static Complex tanhComplexFn(Complex value) => Complex.Tanh(value);
        BindFunction(tanhComplexFn, "Complex.Tanh");

        static Complex addComplexFn(Complex left, Complex right) => Complex.Add(left, right);
        BindFunction(addComplexFn, "Complex.Add");

        static Complex conjugateComplexFn(Complex value) => Complex.Conjugate(value);
        BindFunction(conjugateComplexFn, "Complex.Conjugate");

        static Complex divideComplexFn(Complex left, Complex right) => Complex.Divide(left, right);
        BindFunction(divideComplexFn, "Complex.Divide");

        static Complex fpcComplexFn(Complex magnitude, Complex phase) => Complex.FromPolarCoordinates(magnitude.Real, phase.Real);
        BindFunction(fpcComplexFn, "Complex.FromPolarCoordinates");

        static Complex multiplyComplexFn(Complex left, Complex right) => Complex.Multiply(left, right);
        BindFunction(multiplyComplexFn, "Complex.Multiply");

        static Complex negateComplexFn(Complex value) => Complex.Negate(value);
        BindFunction(negateComplexFn, "Complex.Negate");

        static Complex reciprocalComplexFn(Complex value) => Complex.Reciprocal(value);
        BindFunction(reciprocalComplexFn, "Complex.Reciprocal");

        static Complex subtractComplexFn(Complex left, Complex right) => Complex.Subtract(left, right);
        BindFunction(subtractComplexFn, "Complex.Subtract");

        #endregion
    }
}

