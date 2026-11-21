// ReSharper disable BuiltInTypeReferenceStyleForMemberAccess
// ReSharper disable PreferConcreteValueOverDefault

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
    {
        BindConstant(1d, "true");
        BindConstant(0d, "false");

        BindConstant(0d, "default");
        BindConstant(0d, "default(T)");
        BindConstant(0d, "default(bool)");
        BindConstant(default(double));
        BindConstant(0m, "default(decimal)");
        BindConstant(default(float));
        BindConstant(default(int));
        BindConstant(default(uint));
        BindConstant(default(long));
        BindConstant(default(ulong));
        BindConstant(default(byte));
        BindConstant(default(sbyte));
        BindConstant(default(short));
        BindConstant(default(ushort));
        BindConstant(default(char));
        BindConstant(default(nint));
        BindConstant(default(nuint));
        BindConstant(0d, "default(Boolean)");
        BindConstant(default(Double));
        BindConstant(default(Decimal));
        BindConstant(default(Single));
        BindConstant(default(Int32));
        BindConstant(default(UInt32));
        BindConstant(default(Int64));
        BindConstant(default(UInt64));
        BindConstant(default(Byte));
        BindConstant(default(SByte));
        BindConstant(default(Int16));
        BindConstant(default(UInt16));
        BindConstant(default(Char));

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

        BindConstant(1f, 'f');
        BindConstant(1d, 'd');
        BindConstant(1m, 'm');
        BindConstant(1L, 'l');
        BindConstant(1u, 'u');
        BindConstant(1ul, "ul");
        BindConstant(1LU, "lu");
        BindConstant(1F, 'F');
        BindConstant(1D, 'D');
        BindConstant(1M, 'M');
        BindConstant(1L, 'L');
        BindConstant(1Lu, "Lu");
        BindConstant(1LU, "LU");
        BindConstant(1U, 'U');
        BindConstant(1UL, "Ul");
        BindConstant(1UL, "UL");
        BindConstant(1d, "double");
        BindConstant(1f, "float");
        BindConstant(1m, "decimal");
        BindConstant((short)1, "short");
        BindConstant((ushort)1, "ushort");
        BindConstant(1, "int");
        BindConstant(1u, "uint");
        BindConstant(1L, "long");
        BindConstant(1ul, "ulong");
        BindConstant((byte)1, "byte");
        BindConstant((sbyte)1, "sbyte");
        BindConstant((char)1, "char");
        BindConstant(1d, "Double");
        BindConstant(1f, "Single");
        BindConstant(1m, "Decimal");
        BindConstant((short)1, "Int16");
        BindConstant((ushort)1, "UInt16");
        BindConstant(1, "Int32");
        BindConstant(1u, "UInt32");
        BindConstant(1L, "Int64");
        BindConstant(1ul, "UInt64");
        BindConstant((byte)1, "Byte");
        BindConstant((sbyte)1, "SByte");
        BindConstant((char)1, "Char");

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

        BindConstant(byte.MaxValue);
        BindConstant(byte.MinValue);
        BindConstant(sbyte.MaxValue);
        BindConstant(sbyte.MinValue);
        BindConstant(short.MaxValue);
        BindConstant(short.MinValue);
        BindConstant(ushort.MaxValue);
        BindConstant(ushort.MinValue);
        BindConstant(int.MaxValue);
        BindConstant(int.MinValue);
        BindConstant(uint.MaxValue);
        BindConstant(uint.MinValue);
        BindConstant(long.MaxValue);
        BindConstant(long.MinValue);
        BindConstant(ulong.MaxValue);
        BindConstant(ulong.MinValue);

        BindConstant(Byte.MaxValue);
        BindConstant(Byte.MinValue);
        BindConstant(SByte.MaxValue);
        BindConstant(SByte.MinValue);
        BindConstant(Int16.MaxValue);
        BindConstant(Int16.MinValue);
        BindConstant(UInt16.MaxValue);
        BindConstant(UInt16.MinValue);
        BindConstant(Int32.MaxValue);
        BindConstant(Int32.MinValue);
        BindConstant(UInt32.MaxValue);
        BindConstant(UInt32.MinValue);
        BindConstant(Int64.MaxValue);
        BindConstant(Int64.MinValue);
        BindConstant(UInt64.MaxValue);
        BindConstant(UInt64.MinValue);

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