using System;
using System.Numerics;

#if NET8_0_OR_GREATER

namespace MathEvaluation.Context;

/// <summary>
///     The .NET 8+ math context extends <see cref="DotNetStandardMathContext" /> with additional support for
///     <see cref="System.Numerics.BigInteger" />, modern numeric types (Half, Int128, UInt128),
///     and new Math functions available in .NET 8+.
///     Provides full INumber&lt;T&gt; generic math capabilities for evaluating C# math expressions.
/// </summary>
/// <seealso cref="MathEvaluation.Context.DotNetStandardMathContext" />
public class DotNetMathContext : DotNetStandardMathContext
{
    /// <summary>Initializes a new instance of the <see cref="DotNetMathContext" /> class.</summary>
    public DotNetMathContext()
        : base()
    {
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
        BindConstant(default(IntPtr));
        BindConstant(default(UIntPtr));

        BindConstant(default(Half));
        BindConstant(default(Int128));
        BindConstant(default(UInt128));
        BindConstant(default(BigInteger));

        BindConstant(1f, 'f');
        BindConstant(1L, 'l');
        BindConstant(1u, 'u');
        BindConstant(1ul, "ul");
        BindConstant(1LU, "lu");
        BindConstant(1F, 'F');
        BindConstant(1L, 'L');
        BindConstant(1Lu, "Lu");
        BindConstant(1LU, "LU");
        BindConstant(1U, 'U');
        BindConstant(1U, "Ul");
        BindConstant(1U, "UL");
        BindConstant(1F, "float");
        BindConstant((short)1, "short");
        BindConstant((ushort)1, "ushort");
        BindConstant(1, "int");
        BindConstant(1u, "uint");
        BindConstant(1L, "long");
        BindConstant(1ul, "ulong");
        BindConstant((byte)1, "byte");
        BindConstant((sbyte)1, "sbyte");
        BindConstant((char)1, "char");
        BindConstant((float)1, "Single");
        BindConstant((short)1, "Int16");
        BindConstant((ushort)1, "UInt16");
        BindConstant(1, "Int32");
        BindConstant((uint)1, "UInt32");
        BindConstant((long)1, "Int64");
        BindConstant((ulong)1, "UInt64");
        BindConstant((byte)1, "Byte");
        BindConstant((sbyte)1, "SByte");
        BindConstant((char)1, "Char");

        BindConstant((nint)1, "nint");
        BindConstant((nuint)1, "nuint");
        BindConstant((IntPtr)1, "IntPtr");
        BindConstant((UIntPtr)1, "UIntPtr");
        BindConstant(Half.One, "Half");
        BindConstant(Int128.One, "Int128");
        BindConstant(UInt128.One, "UInt128");
        BindConstant(BigInteger.One, "BigInteger");

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

        // Math.Tau constant (new in .NET 5+)
        BindConstant(Math.Tau);

        // .NET 6+ Math functions
        BindFunction(Math.BitDecrement);
        BindFunction(Math.BitIncrement);
        BindFunction(Math.CopySign);
        BindFunction(Math.FusedMultiplyAdd);
        BindFunction(Math.ReciprocalEstimate);
        BindFunction(Math.ReciprocalSqrtEstimate);

        // .NET 8+ Math functions
        BindFunction(Math.Log2);
        BindFunction(static (double v) => Math.ILogB(v), "Math.ILogB");
        BindFunction(static (double x, double n) => Math.ScaleB(x, (int)n), "Math.ScaleB");

        BindFunction(Math.MaxMagnitude);
        BindFunction(Math.MinMagnitude);

        // IntPtr/nint constants
        BindConstant(nint.MaxValue);
        BindConstant(nint.MinValue);
        BindConstant(nuint.MaxValue);
        BindConstant(nuint.MinValue);
        BindConstant(nint.Zero);
        BindConstant(nuint.Zero);
        BindConstant(IntPtr.MaxValue);
        BindConstant(IntPtr.MinValue);
        BindConstant(UIntPtr.MaxValue);
        BindConstant(UIntPtr.MinValue);
        BindConstant(IntPtr.Zero);
        BindConstant(UIntPtr.Zero);

        #region Half Type Support

        // Half constants (new in .NET 5+)
        BindConstant(Half.Epsilon);
        BindConstant(Half.MaxValue);
        BindConstant(Half.MinValue);
        BindConstant(Half.NaN);
        BindConstant(Half.NegativeInfinity);
        BindConstant(Half.NegativeOne);
        BindConstant(Half.NegativeZero);
        BindConstant(Half.PositiveInfinity);
        BindConstant(Half.One);
        BindConstant(Half.Zero);
        BindConstant(Half.Tau);
        BindConstant(Half.E);
        BindConstant(Half.Pi);
        BindConstant(Half.MultiplicativeIdentity);

        // Half functions
        BindFunction<Half>(Half.Abs);
        BindFunction<Half>(Half.Acos);
        BindFunction<Half>(Half.Acosh);
        BindFunction<Half>(Half.Asin);
        BindFunction<Half>(Half.Asinh);
        BindFunction<Half>(Half.Atan);
        BindFunction<Half>(Half.Atanh);
        BindFunction<Half>(Half.Atan2);
        BindFunction<Half>(Half.Cbrt);
        BindFunction<Half>(Half.Ceiling);
        BindFunction<Half>(Half.Cos);
        BindFunction<Half>(Half.Cosh);
        BindFunction<Half>(Half.Exp);
        BindFunction<Half>(Half.Floor);
        BindFunction<Half>(Half.FusedMultiplyAdd);
        BindFunction<Half>(static (Half value) => (Half)Half.ILogB(value), "Half.ILogB");

        static Half halfLogFn(Half[] args) => args.Length == 1 ? Half.Log(args[0]) : Half.Log(args[0], args[1]);
        BindFunction<Half>(halfLogFn, "Half.Log");

        static Half halfRoundFn(Half[] args) => args.Length == 1 ? Half.Round(args[0]) : Half.Round(args[0], (int)args[1]);
        BindFunction<Half>(halfRoundFn, "Half.Round");

        BindFunction<Half>(Half.Log10);
        BindFunction<Half>(Half.Log2);
        BindFunction<Half>(Half.Pow);
        BindFunction<Half>(static (Half x, Half n) => Half.RootN(x, (int)n), "Half.RootN");
        BindFunction<Half>(static (Half x, Half n) => Half.ScaleB(x, (int)n), "Half.ScaleB");
        BindFunction<Half>(static (Half value) => (Half)Half.Sign(value), "Half.Sign");
        BindFunction<Half>(Half.Sin);
        BindFunction<Half>(Half.Sinh);
        BindFunction<Half>(Half.Sqrt);
        BindFunction<Half>(Half.Tan);
        BindFunction<Half>(Half.Tanh);
        BindFunction<Half>(Half.Truncate);
        BindFunction<Half>(Half.CopySign);
        BindFunction<Half>(Half.AcosPi);
        BindFunction<Half>(Half.AsinPi);
        BindFunction<Half>(Half.Atan2Pi);
        BindFunction<Half>(Half.AtanPi);
        BindFunction<Half>(Half.BitDecrement);
        BindFunction<Half>(Half.BitIncrement);
        BindFunction<Half>(Half.Clamp);
#if NET10_0_OR_GREATER
        BindFunction<Half>(Half.ClampNative);
        BindFunction<Half>(Half.MaxNative);
        BindFunction<Half>(Half.MinNative);
#endif
        BindFunction<Half>(Half.CosPi);
        BindFunction<Half>(Half.CreateChecked);
        BindFunction<Half>(Half.CreateSaturating);
        BindFunction<Half>(Half.CreateTruncating);
        BindFunction<Half>(Half.DegreesToRadians);
        BindFunction<Half>(Half.Exp);
        BindFunction<Half>(Half.ExpM1);
        BindFunction<Half>(Half.Exp10);
        BindFunction<Half>(Half.Exp10M1);
        BindFunction<Half>(Half.Exp2);
        BindFunction<Half>(Half.Exp2M1);
        BindFunction<Half>(Half.Hypot);
        BindFunction<Half>(Half.Ieee754Remainder);
        BindFunction<Half>(Half.Lerp);
        BindFunction<Half>(Half.LogP1);
        BindFunction<Half>(Half.Log2P1);
        BindFunction<Half>(Half.Log10P1);
        BindFunction<Half>(Half.Max);
        BindFunction<Half>(Half.MaxMagnitude);
        BindFunction<Half>(Half.MaxMagnitudeNumber);
        BindFunction<Half>(Half.MaxNumber);
        BindFunction<Half>(Half.Min);
        BindFunction<Half>(Half.MinMagnitude);
        BindFunction<Half>(Half.MinMagnitudeNumber);
        BindFunction<Half>(Half.MinNumber);
#if NET9_0_OR_GREATER
        BindFunction<Half>(Half.MultiplyAddEstimate);
#endif
        BindFunction<Half>(Half.RadiansToDegrees);
        BindFunction<Half>(Half.ReciprocalEstimate);
        BindFunction<Half>(Half.ReciprocalSqrtEstimate);
        BindFunction<Half>(Half.SinPi);
        BindFunction<Half>(Half.TanPi);

        #endregion

        #region double Type Support

        // Additional Double constants and functions
        BindConstant(double.E);
        BindConstant(double.Pi);
        BindConstant(double.Tau);
        BindConstant(double.NegativeZero);

        // Double functions
        BindFunction(double.Abs);
        BindFunction(double.Acos);
        BindFunction(double.Acosh);
        BindFunction(double.Asin);
        BindFunction(double.Asinh);
        BindFunction(double.Atan);
        BindFunction(double.Atanh);
        BindFunction(double.Atan2);
        BindFunction(double.Cbrt);
        BindFunction(double.Ceiling);
        BindFunction(double.Cos);
        BindFunction(double.Cosh);
        BindFunction(double.Exp);
        BindFunction(double.Floor);
        BindFunction(double.FusedMultiplyAdd);
        BindFunction((double v) => double.ILogB(v), "double.ILogB");

        static double doubleLogFn(double[] args) => args.Length == 1 ? double.Log(args[0]) : double.Log(args[0], args[1]);
        BindFunction(doubleLogFn, "double.Log");

        static double doubleRoundFn(double[] args) => args.Length == 1 ? double.Round(args[0]) : double.Round(args[0], (int)args[1]);
        BindFunction(doubleRoundFn, "double.Round");

        BindFunction(double.Log10);
        BindFunction(double.Log2);
        BindFunction(double.Pow);
        BindFunction(static (double x, double n) => double.RootN(x, (int)n), "double.RootN");
        BindFunction(static (double x, double n) => double.ScaleB(x, (int)n), "double.ScaleB");
        BindFunction(static (double v) => double.Sign(v), "double.Sign");
        BindFunction(double.Sin);
        BindFunction(double.Sinh);
        BindFunction(double.Sqrt);
        BindFunction(double.Tan);
        BindFunction(double.Tanh);
        BindFunction(double.Truncate);
        BindFunction(double.CopySign);
        BindFunction(double.AcosPi);
        BindFunction(double.AsinPi);
        BindFunction(double.Atan2Pi);
        BindFunction(double.AtanPi);
        BindFunction(double.BitDecrement);
        BindFunction(double.BitIncrement);
        BindFunction(double.Clamp);
#if NET10_0_OR_GREATER
        BindFunction(double.ClampNative);
        BindFunction(double.MaxNative);
        BindFunction(double.MinNative);
#endif
        BindFunction(double.CosPi);
        BindFunction(double.CreateChecked);
        BindFunction(double.CreateSaturating);
        BindFunction(double.CreateTruncating);
        BindFunction(double.DegreesToRadians);
        BindFunction(double.ExpM1);
        BindFunction(double.Exp10);
        BindFunction(double.Exp10M1);
        BindFunction(double.Exp2);
        BindFunction(double.Exp2M1);
        BindFunction(double.Hypot);
        BindFunction(double.Ieee754Remainder);
        BindFunction(double.Lerp);
        BindFunction(double.LogP1);
        BindFunction(double.Log2P1);
        BindFunction(double.Log10P1);
        BindFunction(double.Max);
        BindFunction(double.MaxMagnitude);
        BindFunction(double.MaxMagnitudeNumber);
        BindFunction(double.MaxNumber);
        BindFunction(double.Min);
        BindFunction(double.MinMagnitude);
        BindFunction(double.MinMagnitudeNumber);
        BindFunction(double.MinNumber);
#if NET9_0_OR_GREATER
        BindFunction(double.MultiplyAddEstimate);
#endif
        BindFunction(double.RadiansToDegrees);
        BindFunction(double.ReciprocalEstimate);
        BindFunction(double.ReciprocalSqrtEstimate);
        BindFunction(double.SinPi);
        BindFunction(double.TanPi);

        #endregion

        #region double Type Support (using explicit Double type)

        // Additional Double constants and functions
        BindConstant(Double.E);
        BindConstant(Double.Pi);
        BindConstant(Double.Tau);
        BindConstant(Double.NegativeZero);

        // Double functions
        BindFunction(Double.Abs);
        BindFunction(Double.Acos);
        BindFunction(Double.Acosh);
        BindFunction(Double.Asin);
        BindFunction(Double.Asinh);
        BindFunction(Double.Atan);
        BindFunction(Double.Atanh);
        BindFunction(Double.Atan2);
        BindFunction(Double.Cbrt);
        BindFunction(Double.Ceiling);
        BindFunction(Double.Cos);
        BindFunction(Double.Cosh);
        BindFunction(Double.Exp);
        BindFunction(Double.Floor);
        BindFunction(Double.FusedMultiplyAdd);
        BindFunction((Double v) => Double.ILogB(v), "Double.ILogB");

        static Double DoubleLogFn(Double[] args) => args.Length == 1 ? Double.Log(args[0]) : Double.Log(args[0], args[1]);
        BindFunction(DoubleLogFn, "Double.Log");

        static Double DoubleRoundFn(Double[] args) => args.Length == 1 ? Double.Round(args[0]) : Double.Round(args[0], (int)args[1]);
        BindFunction(DoubleRoundFn, "Double.Round");

        BindFunction(Double.Log10);
        BindFunction(Double.Log2);
        BindFunction(Double.Pow);
        BindFunction(static (Double x, Double n) => Double.RootN(x, (int)n), "Double.RootN");
        BindFunction(static (Double x, Double n) => Double.ScaleB(x, (int)n), "Double.ScaleB");
        BindFunction(static (Double v) => Double.Sign(v), "Double.Sign");
        BindFunction(Double.Sin);
        BindFunction(Double.Sinh);
        BindFunction(Double.Sqrt);
        BindFunction(Double.Tan);
        BindFunction(Double.Tanh);
        BindFunction(Double.Truncate);
        BindFunction(Double.CopySign);
        BindFunction(Double.AcosPi);
        BindFunction(Double.AsinPi);
        BindFunction(Double.Atan2Pi);
        BindFunction(Double.AtanPi);
        BindFunction(Double.BitDecrement);
        BindFunction(Double.BitIncrement);
        BindFunction(Double.Clamp);
#if NET10_0_OR_GREATER
        BindFunction(Double.ClampNative);
        BindFunction(Double.MaxNative);
        BindFunction(Double.MinNative);
#endif
        BindFunction(Double.CosPi);
        BindFunction(Double.CreateChecked);
        BindFunction(Double.CreateSaturating);
        BindFunction(Double.CreateTruncating);
        BindFunction(Double.DegreesToRadians);
        BindFunction(Double.ExpM1);
        BindFunction(Double.Exp10);
        BindFunction(Double.Exp10M1);
        BindFunction(Double.Exp2);
        BindFunction(Double.Exp2M1);
        BindFunction(Double.Hypot);
        BindFunction(Double.Ieee754Remainder);
        BindFunction(Double.Lerp);
        BindFunction(Double.LogP1);
        BindFunction(Double.Log2P1);
        BindFunction(Double.Log10P1);
        BindFunction(Double.Max);
        BindFunction(Double.MaxMagnitude);
        BindFunction(Double.MaxMagnitudeNumber);
        BindFunction(Double.MaxNumber);
        BindFunction(Double.Min);
        BindFunction(Double.MinMagnitude);
        BindFunction(Double.MinMagnitudeNumber);
        BindFunction(Double.MinNumber);
#if NET9_0_OR_GREATER
        BindFunction(Double.MultiplyAddEstimate);
#endif
        BindFunction(Double.RadiansToDegrees);
        BindFunction(Double.ReciprocalEstimate);
        BindFunction(Double.ReciprocalSqrtEstimate);
        BindFunction(Double.SinPi);
        BindFunction(Double.TanPi);

        #endregion

        #region float Type Support

        // Single constants
        BindConstant(float.E);
        BindConstant(float.Pi);
        BindConstant(float.Tau);
        BindConstant(float.NegativeZero);

        BindFunction<float>(float.Abs);
        BindFunction<float>(float.Acos);
        BindFunction<float>(float.Acosh);
        BindFunction<float>(float.Asin);
        BindFunction<float>(float.Asinh);
        BindFunction<float>(float.Atan);
        BindFunction<float>(float.Atanh);
        BindFunction<float>(float.Atan2);
        BindFunction<float>(float.Cbrt);
        BindFunction<float>(float.Ceiling);
        BindFunction<float>(float.Cos);
        BindFunction<float>(float.Cosh);
        BindFunction<float>(float.Exp);
        BindFunction<float>(float.Floor);
        BindFunction<float>(float.FusedMultiplyAdd);
        BindFunction<float>(static (float value) => float.ILogB(value), "Single.ILogB");

        static float floatLogFn(float[] args) => args.Length == 1 ? float.Log(args[0]) : float.Log(args[0], args[1]);
        BindFunction<float>(floatLogFn, "float.Log");

        static float floatRoundFn(float[] args) => args.Length == 1 ? float.Round(args[0]) : float.Round(args[0], (int)args[1]);
        BindFunction<float>(floatRoundFn, "float.Round");

        BindFunction<float>(float.Log10);
        BindFunction<float>(float.Log2);
        BindFunction<float>(float.Pow);
        BindFunction<float>(static (float x, float n) => float.RootN(x, (int)n), "float.RootN");
        BindFunction<float>(static (float x, float n) => float.ScaleB(x, (int)n), "float.ScaleB");
        BindFunction<float>(static (float v) => float.Sign(v), "float.Sign");
        BindFunction<float>(float.Sin);
        BindFunction<float>(float.Sinh);
        BindFunction<float>(float.Sqrt);
        BindFunction<float>(float.Tan);
        BindFunction<float>(float.Tanh);
        BindFunction<float>(float.Truncate);
        BindFunction<float>(float.CopySign);
        BindFunction<float>(float.AcosPi);
        BindFunction<float>(float.AsinPi);
        BindFunction<float>(float.Atan2Pi);
        BindFunction<float>(float.AtanPi);
        BindFunction<float>(float.BitDecrement);
        BindFunction<float>(float.BitIncrement);
        BindFunction<float>(float.Clamp);
#if NET10_0_OR_GREATER
        BindFunction<float>(float.ClampNative);
        BindFunction<float>(float.MaxNative);
        BindFunction<float>(float.MinNative);
#endif
        BindFunction<float>(float.CosPi);
        BindFunction<float>(float.CreateChecked);
        BindFunction<float>(float.CreateSaturating);
        BindFunction<float>(float.CreateTruncating);
        BindFunction<float>(float.DegreesToRadians);
        BindFunction<float>(float.ExpM1);
        BindFunction<float>(float.Exp10);
        BindFunction<float>(float.Exp10M1);
        BindFunction<float>(float.Exp2);
        BindFunction<float>(float.Exp2M1);
        BindFunction<float>(float.Hypot);
        BindFunction<float>(float.Ieee754Remainder);
        BindFunction<float>(float.Lerp);
        BindFunction<float>(float.LogP1);
        BindFunction<float>(float.Log2P1);
        BindFunction<float>(float.Log10P1);
        BindFunction<float>(float.Max);
        BindFunction<float>(float.MaxMagnitude);
        BindFunction<float>(float.MaxMagnitudeNumber);
        BindFunction<float>(float.MaxNumber);
        BindFunction<float>(float.Min);
        BindFunction<float>(float.MinMagnitude);
        BindFunction<float>(float.MinMagnitudeNumber);
        BindFunction<float>(float.MinNumber);
#if NET9_0_OR_GREATER
        BindFunction<float>(float.MultiplyAddEstimate);
#endif
        BindFunction<float>(float.RadiansToDegrees);
        BindFunction<float>(float.ReciprocalEstimate);
        BindFunction<float>(float.ReciprocalSqrtEstimate);
        BindFunction<float>(float.SinPi);
        BindFunction<float>(float.TanPi);

        #endregion

        #region float Type Support (using explicit Single type)

        // Single constants
        BindConstant(Single.E);
        BindConstant(Single.Pi);
        BindConstant(Single.Tau);
        BindConstant(Single.NegativeZero);

        BindFunction<Single>(Single.Abs);
        BindFunction<Single>(Single.Acos);
        BindFunction<Single>(Single.Acosh);
        BindFunction<Single>(Single.Asin);
        BindFunction<Single>(Single.Asinh);
        BindFunction<Single>(Single.Atan);
        BindFunction<Single>(Single.Atanh);
        BindFunction<Single>(Single.Atan2);
        BindFunction<Single>(Single.Cbrt);
        BindFunction<Single>(Single.Ceiling);
        BindFunction<Single>(Single.Cos);
        BindFunction<Single>(Single.Cosh);
        BindFunction<Single>(Single.Exp);
        BindFunction<Single>(Single.Floor);
        BindFunction<Single>(Single.FusedMultiplyAdd);
        BindFunction<Single>(static (Single value) => Single.ILogB(value), "Single.ILogB");

        static Single singleLogFn(Single[] args) => args.Length == 1 ? Single.Log(args[0]) : Single.Log(args[0], args[1]);
        BindFunction<Single>(singleLogFn, "Single.Log");

        static Single singleRoundFn(Single[] args) => args.Length == 1 ? Single.Round(args[0]) : Single.Round(args[0], (int)args[1]);
        BindFunction<Single>(singleRoundFn, "Single.Round");

        BindFunction<Single>(Single.Log10);
        BindFunction<Single>(Single.Log2);
        BindFunction<Single>(Single.Pow);
        BindFunction<Single>(static (Single x, Single n) => Single.RootN(x, (int)n), "Single.RootN");
        BindFunction<Single>(static (Single x, Single n) => Single.ScaleB(x, (int)n), "Single.ScaleB");
        BindFunction<Single>(static (Single v) => Single.Sign(v), "Single.Sign");
        BindFunction<Single>(Single.Sin);
        BindFunction<Single>(Single.Sinh);
        BindFunction<Single>(Single.Sqrt);
        BindFunction<Single>(Single.Tan);
        BindFunction<Single>(Single.Tanh);
        BindFunction<Single>(Single.Truncate);
        BindFunction<Single>(Single.CopySign);
        BindFunction<Single>(Single.AcosPi);
        BindFunction<Single>(Single.AsinPi);
        BindFunction<Single>(Single.Atan2Pi);
        BindFunction<Single>(Single.AtanPi);
        BindFunction<Single>(Single.BitDecrement);
        BindFunction<Single>(Single.BitIncrement);
        BindFunction<Single>(Single.Clamp);
#if NET10_0_OR_GREATER
        BindFunction<Single>(Single.ClampNative);
        BindFunction<Single>(Single.MaxNative);
        BindFunction<Single>(Single.MinNative);
#endif
        BindFunction<Single>(Single.CosPi);
        BindFunction<Single>(Single.CreateChecked);
        BindFunction<Single>(Single.CreateSaturating);
        BindFunction<Single>(Single.CreateTruncating);
        BindFunction<Single>(Single.DegreesToRadians);
        BindFunction<Single>(Single.ExpM1);
        BindFunction<Single>(Single.Exp10);
        BindFunction<Single>(Single.Exp10M1);
        BindFunction<Single>(Single.Exp2);
        BindFunction<Single>(Single.Exp2M1);
        BindFunction<Single>(Single.Hypot);
        BindFunction<Single>(Single.Ieee754Remainder);
        BindFunction<Single>(Single.Lerp);
        BindFunction<Single>(Single.LogP1);
        BindFunction<Single>(Single.Log2P1);
        BindFunction<Single>(Single.Log10P1);
        BindFunction<Single>(Single.Max);
        BindFunction<Single>(Single.MaxMagnitude);
        BindFunction<Single>(Single.MaxMagnitudeNumber);
        BindFunction<Single>(Single.MaxNumber);
        BindFunction<Single>(Single.Min);
        BindFunction<Single>(Single.MinMagnitude);
        BindFunction<Single>(Single.MinMagnitudeNumber);
        BindFunction<Single>(Single.MinNumber);
#if NET9_0_OR_GREATER
        BindFunction<Single>(Single.MultiplyAddEstimate);
#endif
        BindFunction<Single>(Single.RadiansToDegrees);
        BindFunction<Single>(Single.ReciprocalEstimate);
        BindFunction<Single>(Single.ReciprocalSqrtEstimate);
        BindFunction<Single>(Single.SinPi);
        BindFunction<Single>(Single.TanPi);

        #endregion

        #region decimal Type Support

        // Decimal constants
        BindConstant(decimal.One);
        BindConstant(decimal.MinusOne);
        BindConstant(decimal.Zero);
        BindConstant(decimal.MaxValue);
        BindConstant(decimal.MinValue);

        // Decimal functions
        BindFunction<decimal>(decimal.Abs);
        BindFunction<decimal>(decimal.Ceiling);
        BindFunction<decimal>(decimal.Clamp);
        BindFunction<decimal>(decimal.CreateChecked);
        BindFunction<decimal>(decimal.CreateSaturating);
        BindFunction<decimal>(decimal.CreateTruncating);
        BindFunction<decimal>(decimal.Floor);
        BindFunction<decimal>(decimal.Max);
        BindFunction<decimal>(decimal.MaxMagnitude);
        BindFunction<decimal>(decimal.Min);
        BindFunction<decimal>(decimal.MinMagnitude);
        BindFunction<decimal>(static (decimal v) => decimal.Sign(v), "decimal.Sign");
        BindFunction<decimal>(decimal.Truncate);
        BindFunction<decimal>(decimal.CopySign);
        BindFunction<decimal>(decimal.Add);
        BindFunction<decimal>(decimal.Divide);
        BindFunction<decimal>(static (decimal cy) => decimal.FromOACurrency((long)cy), "decimal.FromOACurrency");
        BindFunction<decimal>(decimal.Multiply);
        BindFunction<decimal>(decimal.Negate);
        BindFunction<decimal>(decimal.Remainder);
        BindFunction<decimal>(decimal.Subtract);

        static decimal decimalRoundFn(decimal[] args) => args.Length == 1 ? decimal.Round(args[0]) : decimal.Round(args[0], (int)args[1]);
        BindFunction<decimal>(decimalRoundFn, "decimal.Round");

        #endregion

        #region decimal Type Support (using explicit Decimal type)

        // Decimal constants
        BindConstant(Decimal.One);
        BindConstant(Decimal.MinusOne);
        BindConstant(Decimal.Zero);
        BindConstant(Decimal.MaxValue);
        BindConstant(Decimal.MinValue);

        // Decimal functions
        BindFunction<Decimal>(Decimal.Abs);
        BindFunction<Decimal>(Decimal.Ceiling);
        BindFunction<Decimal>(Decimal.Clamp);
        BindFunction<Decimal>(Decimal.CreateChecked);
        BindFunction<Decimal>(Decimal.CreateSaturating);
        BindFunction<Decimal>(Decimal.CreateTruncating);
        BindFunction<Decimal>(Decimal.Floor);
        BindFunction<Decimal>(Decimal.Max);
        BindFunction<Decimal>(Decimal.MaxMagnitude);
        BindFunction<Decimal>(Decimal.Min);
        BindFunction<Decimal>(Decimal.MinMagnitude);
        BindFunction<Decimal>(static (Decimal v) => Decimal.Sign(v), "Decimal.Sign");
        BindFunction<Decimal>(Decimal.Truncate);
        BindFunction<Decimal>(Decimal.CopySign);
        BindFunction<Decimal>(Decimal.Add);
        BindFunction<Decimal>(Decimal.Divide);
        BindFunction<Decimal>(static (Decimal cy) => Decimal.FromOACurrency((long)cy), "Decimal.FromOACurrency");
        BindFunction<Decimal>(Decimal.Multiply);
        BindFunction<Decimal>(Decimal.Negate);
        BindFunction<Decimal>(Decimal.Remainder);
        BindFunction<Decimal>(Decimal.Subtract);

        static Decimal DecimalRoundFn(Decimal[] args) => args.Length == 1 ? Decimal.Round(args[0]) : Decimal.Round(args[0], (int)args[1]);
        BindFunction<Decimal>(DecimalRoundFn, "Decimal.Round");

        #endregion

        #region BigInteger Type Support

        // BigInteger constants and functions
        BindConstant(BigInteger.Zero);
        BindConstant(BigInteger.One);
        BindConstant(BigInteger.MinusOne);

        static BigInteger newComplexFn(BigInteger arg) => new((double)arg);
        BindFunction<BigInteger>(newComplexFn, "new BigInteger");

        BindFunction<BigInteger>(BigInteger.Abs);
        BindFunction<BigInteger>(BigInteger.Add);
        BindFunction<BigInteger>(BigInteger.Subtract);
        BindFunction<BigInteger>(BigInteger.Multiply);
        BindFunction<BigInteger>(BigInteger.Divide);
        BindFunction<BigInteger>(BigInteger.Remainder);
        BindFunction<BigInteger>(static (BigInteger a, BigInteger b) => BigInteger.Pow(a, (int)b), "BigInteger.Pow");
        BindFunction<BigInteger>(BigInteger.ModPow);
        BindFunction<BigInteger>(BigInteger.GreatestCommonDivisor);
        BindFunction<BigInteger>(BigInteger.Max);
        BindFunction<BigInteger>(BigInteger.Min);
        BindFunction<BigInteger>(BigInteger.Negate);
        BindFunction<BigInteger>(BigInteger.Clamp);
        BindFunction<BigInteger>(BigInteger.CopySign);
        BindFunction<BigInteger>(BigInteger.CreateChecked);
        BindFunction<BigInteger>(BigInteger.CreateSaturating);
        BindFunction<BigInteger>(BigInteger.CreateTruncating);
        BindFunction<BigInteger>(BigInteger.LeadingZeroCount);

        static double biLogFn(double[] args) => args.Length == 1 ? BigInteger.Log((BigInteger)args[0]) : BigInteger.Log((BigInteger)args[0], args[1]);
        BindFunction(biLogFn, "BigInteger.Log");

        BindFunction(static (double v) => BigInteger.Log10((BigInteger)v), "BigInteger.Log10");
        BindFunction<BigInteger>(BigInteger.Log2);
        BindFunction<BigInteger>(BigInteger.MaxMagnitude);
        BindFunction<BigInteger>(BigInteger.MinMagnitude);
        BindFunction<BigInteger>(BigInteger.PopCount);
        BindFunction<BigInteger>(BigInteger.Remainder);
        BindFunction<BigInteger>(static (BigInteger value, BigInteger shift) => BigInteger.RotateLeft(value, (int)shift), "BigInteger.RotateLeft");
        BindFunction<BigInteger>(static (BigInteger value, BigInteger shift) => BigInteger.RotateRight(value, (int)shift), "BigInteger.RotateRight");
        BindFunction<BigInteger>(BigInteger.TrailingZeroCount);

        #endregion

        #region Int128/UInt128 Type Support

        // Int128/UInt128 constants (new in .NET 7+)
        BindConstant(Int128.Zero);
        BindConstant(Int128.One);
        BindConstant(Int128.NegativeOne);
        BindConstant(Int128.MaxValue);
        BindConstant(Int128.MinValue);

        BindConstant(UInt128.Zero);
        BindConstant(UInt128.One);
        BindConstant(UInt128.MaxValue);
        BindConstant(UInt128.MinValue);

        static Int128 newInt128Fn(Int128 upper, Int128 lower) => new((ulong)upper, (ulong)lower);
        BindFunction<Int128>(newInt128Fn, "new Int128");

        static UInt128 newUInt128Fn(UInt128 upper, UInt128 lower) => new((ulong)upper, (ulong)lower);
        BindFunction<UInt128>(newUInt128Fn, "new UInt128");

        BindFunction<Int128>(Int128.Abs);
        BindFunction<Int128>(Int128.Clamp);
        BindFunction<Int128>(Int128.CopySign);
        BindFunction<Int128>(Int128.CreateChecked);
        BindFunction<Int128>(Int128.CreateSaturating);
        BindFunction<Int128>(Int128.CreateTruncating);
        BindFunction<Int128>(Int128.LeadingZeroCount);
        BindFunction<Int128>(Int128.Log2);
        BindFunction<Int128>(Int128.Max);
        BindFunction<Int128>(Int128.MaxMagnitude);
        BindFunction<Int128>(Int128.Min);
        BindFunction<Int128>(Int128.MinMagnitude);
        BindFunction<Int128>(Int128.PopCount);
        BindFunction<Int128>(static (Int128 value, Int128 shift) => Int128.RotateLeft(value, (int)shift), "Int128.RotateLeft");
        BindFunction<Int128>(static (Int128 value, Int128 shift) => Int128.RotateRight(value, (int)shift), "Int128.RotateRight");
        BindFunction<Int128>(static (Int128 value) => Int128.Sign(value), "Int128.Sign");
        BindFunction<Int128>(Int128.TrailingZeroCount);

        BindFunction<UInt128>(UInt128.Clamp);
        BindFunction<UInt128>(UInt128.CreateChecked);
        BindFunction<UInt128>(UInt128.CreateSaturating);
        BindFunction<UInt128>(UInt128.CreateTruncating);
        BindFunction<UInt128>(UInt128.LeadingZeroCount);
        BindFunction<UInt128>(UInt128.Log2);
        BindFunction<UInt128>(UInt128.Max);
        BindFunction<UInt128>(UInt128.Min);
        BindFunction<UInt128>(UInt128.PopCount);
        BindFunction<UInt128>(static (UInt128 value, UInt128 shift) => UInt128.RotateLeft(value, (int)shift), "UInt128.RotateLeft");
        BindFunction<UInt128>(static (UInt128 value, UInt128 shift) => UInt128.RotateRight(value, (int)shift), "UInt128.RotateRight");
        BindFunction<UInt128>(UInt128.TrailingZeroCount);

        #endregion

        #region byte Type Support

        BindFunction<byte>(byte.Clamp);
        BindFunction<byte>(byte.CreateChecked);
        BindFunction<byte>(byte.CreateSaturating);
        BindFunction<byte>(byte.CreateTruncating);
        BindFunction<byte>(byte.LeadingZeroCount);
        BindFunction<byte>(byte.Log2);
        BindFunction<byte>(byte.Max);
        BindFunction<byte>(byte.Min);
        BindFunction<byte>(byte.PopCount);
        BindFunction<byte>(static (byte value, byte shift) => byte.RotateLeft(value, (int)shift), "byte.RotateLeft");
        BindFunction<byte>(static (byte value, byte shift) => byte.RotateRight(value, (int)shift), "byte.RotateRight");
        BindFunction<byte>(static (byte value) => (byte)byte.Sign(value), "byte.Sign");
        BindFunction<byte>(byte.TrailingZeroCount);

        #endregion

        #region byte Type Support (using explicit Byte type)

        BindFunction<Byte>(Byte.Clamp);
        BindFunction<Byte>(Byte.CreateChecked);
        BindFunction<Byte>(Byte.CreateSaturating);
        BindFunction<Byte>(Byte.CreateTruncating);
        BindFunction<Byte>(Byte.LeadingZeroCount);
        BindFunction<Byte>(Byte.Log2);
        BindFunction<Byte>(Byte.Max);
        BindFunction<Byte>(Byte.Min);
        BindFunction<Byte>(Byte.PopCount);
        BindFunction<Byte>(static (Byte value, Byte shift) => Byte.RotateLeft(value, (int)shift), "Byte.RotateLeft");
        BindFunction<Byte>(static (Byte value, Byte shift) => Byte.RotateRight(value, (int)shift), "Byte.RotateRight");
        BindFunction<Byte>(static (Byte value) => (Byte)Byte.Sign(value), "Byte.Sign");
        BindFunction<Byte>(Byte.TrailingZeroCount);

        #endregion

        #region sbyte Type Support

        BindFunction<sbyte>(sbyte.Abs);
        BindFunction<sbyte>(sbyte.Clamp);
        BindFunction<sbyte>(sbyte.CopySign);
        BindFunction<sbyte>(sbyte.CreateChecked);
        BindFunction<sbyte>(sbyte.CreateSaturating);
        BindFunction<sbyte>(sbyte.CreateTruncating);
        BindFunction<sbyte>(sbyte.LeadingZeroCount);
        BindFunction<sbyte>(sbyte.Log2);
        BindFunction<sbyte>(sbyte.Max);
        BindFunction<sbyte>(sbyte.MaxMagnitude);
        BindFunction<sbyte>(sbyte.Min);
        BindFunction<sbyte>(sbyte.MinMagnitude);
        BindFunction<sbyte>(sbyte.PopCount);
        BindFunction<sbyte>(static (sbyte value, sbyte shift) => sbyte.RotateLeft(value, (int)shift), "sbyte.RotateLeft");
        BindFunction<sbyte>(static (sbyte value, sbyte shift) => sbyte.RotateRight(value, (int)shift), "sbyte.RotateRight");
        BindFunction<sbyte>(static (sbyte value) => (sbyte)sbyte.Sign(value), "sbyte.Sign");
        BindFunction<sbyte>(sbyte.TrailingZeroCount);

        #endregion

        #region sbyte Type Support (using explicit SByte type)

        BindFunction<SByte>(SByte.Abs);
        BindFunction<SByte>(SByte.Clamp);
        BindFunction<SByte>(SByte.CopySign);
        BindFunction<SByte>(SByte.CreateChecked);
        BindFunction<SByte>(SByte.CreateSaturating);
        BindFunction<SByte>(SByte.CreateTruncating);
        BindFunction<SByte>(SByte.LeadingZeroCount);
        BindFunction<SByte>(SByte.Log2);
        BindFunction<SByte>(SByte.Max);
        BindFunction<SByte>(SByte.MaxMagnitude);
        BindFunction<SByte>(SByte.Min);
        BindFunction<SByte>(SByte.MinMagnitude);
        BindFunction<SByte>(SByte.PopCount);
        BindFunction<SByte>(static (SByte value, SByte shift) => SByte.RotateLeft(value, (int)shift), "SByte.RotateLeft");
        BindFunction<SByte>(static (SByte value, SByte shift) => SByte.RotateRight(value, (int)shift), "SByte.RotateRight");
        BindFunction<SByte>(static (SByte value) => (SByte)SByte.Sign(value), "SByte.Sign");
        BindFunction<SByte>(SByte.TrailingZeroCount);

        #endregion

        #region short Type Support

        BindFunction<short>(short.Abs);
        BindFunction<short>(short.Clamp);
        BindFunction<short>(short.CopySign);
        BindFunction<short>(short.CreateChecked);
        BindFunction<short>(short.CreateSaturating);
        BindFunction<short>(short.CreateTruncating);
        BindFunction<short>(short.LeadingZeroCount);
        BindFunction<short>(short.Log2);
        BindFunction<short>(short.Max);
        BindFunction<short>(short.MaxMagnitude);
        BindFunction<short>(short.Min);
        BindFunction<short>(short.MinMagnitude);
        BindFunction<short>(short.PopCount);
        BindFunction<short>(static (short value, short shift) => short.RotateLeft(value, (int)shift), "short.RotateLeft");
        BindFunction<short>(static (short value, short shift) => short.RotateRight(value, (int)shift), "short.RotateRight");
        BindFunction<short>(static (short value) => (short)short.Sign(value), "short.Sign");
        BindFunction<short>(short.TrailingZeroCount);

        #endregion

        #region short Type Support (using explicit Int16 type)

        BindFunction<Int16>(Int16.Abs);
        BindFunction<Int16>(Int16.Clamp);
        BindFunction<Int16>(Int16.CopySign);
        BindFunction<Int16>(Int16.CreateChecked);
        BindFunction<Int16>(Int16.CreateSaturating);
        BindFunction<Int16>(Int16.CreateTruncating);
        BindFunction<Int16>(Int16.LeadingZeroCount);
        BindFunction<Int16>(Int16.Log2);
        BindFunction<Int16>(Int16.Max);
        BindFunction<Int16>(Int16.MaxMagnitude);
        BindFunction<Int16>(Int16.Min);
        BindFunction<Int16>(Int16.MinMagnitude);
        BindFunction<Int16>(Int16.PopCount);
        BindFunction<Int16>(static (Int16 value, Int16 shift) => Int16.RotateLeft(value, (int)shift), "Int16.RotateLeft");
        BindFunction<Int16>(static (Int16 value, Int16 shift) => Int16.RotateRight(value, (int)shift), "Int16.RotateRight");
        BindFunction<Int16>(static (Int16 value) => (Int16)Int16.Sign(value), "Int16.Sign");
        BindFunction<Int16>(Int16.TrailingZeroCount);

        #endregion

        #region ushort Type Support

        BindFunction<ushort>(ushort.Clamp);
        BindFunction<ushort>(ushort.CreateChecked);
        BindFunction<ushort>(ushort.CreateSaturating);
        BindFunction<ushort>(ushort.CreateTruncating);
        BindFunction<ushort>(ushort.LeadingZeroCount);
        BindFunction<ushort>(ushort.Log2);
        BindFunction<ushort>(ushort.Max);
        BindFunction<ushort>(ushort.Min);
        BindFunction<ushort>(ushort.PopCount);
        BindFunction<ushort>(static (ushort value, ushort shift) => ushort.RotateLeft(value, (int)shift), "ushort.RotateLeft");
        BindFunction<ushort>(static (ushort value, ushort shift) => ushort.RotateRight(value, (int)shift), "ushort.RotateRight");
        BindFunction<ushort>(ushort.TrailingZeroCount);

        #endregion

        #region ushort Type Support (using explicit UInt16 type)

        BindFunction<UInt16>(UInt16.Clamp);
        BindFunction<UInt16>(UInt16.CreateChecked);
        BindFunction<UInt16>(UInt16.CreateSaturating);
        BindFunction<UInt16>(UInt16.CreateTruncating);
        BindFunction<UInt16>(UInt16.LeadingZeroCount);
        BindFunction<UInt16>(UInt16.Log2);
        BindFunction<UInt16>(UInt16.Max);
        BindFunction<UInt16>(UInt16.Min);
        BindFunction<UInt16>(UInt16.PopCount);
        BindFunction<UInt16>(static (UInt16 value, UInt16 shift) => UInt16.RotateLeft(value, (int)shift), "UInt16.RotateLeft");
        BindFunction<UInt16>(static (UInt16 value, UInt16 shift) => UInt16.RotateRight(value, (int)shift), "UInt16.RotateRight");
        BindFunction<UInt16>(UInt16.TrailingZeroCount);

        #endregion

        #region int Type Support

        BindFunction<int>(int.Abs);
        BindFunction<int>(int.Clamp);
        BindFunction<int>(int.CopySign);
        BindFunction<int>(int.CreateChecked);
        BindFunction<int>(int.CreateSaturating);
        BindFunction<int>(int.CreateTruncating);
        BindFunction<int>(int.LeadingZeroCount);
        BindFunction<int>(int.Log2);
        BindFunction<int>(int.Max);
        BindFunction<int>(int.MaxMagnitude);
        BindFunction<int>(int.Min);
        BindFunction<int>(int.MinMagnitude);
        BindFunction<int>(int.PopCount);
        BindFunction<int>(int.RotateLeft);
        BindFunction<int>(int.RotateRight);
        BindFunction<int>(int.Sign);
        BindFunction<int>(int.TrailingZeroCount);

        #endregion

        #region int Type Support (using explicit Int32 type)

        BindFunction<Int32>(Int32.Abs);
        BindFunction<Int32>(Int32.Clamp);
        BindFunction<Int32>(Int32.CopySign);
        BindFunction<Int32>(Int32.CreateChecked);
        BindFunction<Int32>(Int32.CreateSaturating);
        BindFunction<Int32>(Int32.CreateTruncating);
        BindFunction<Int32>(Int32.LeadingZeroCount);
        BindFunction<Int32>(Int32.Log2);
        BindFunction<Int32>(Int32.Max);
        BindFunction<Int32>(Int32.MaxMagnitude);
        BindFunction<Int32>(Int32.Min);
        BindFunction<Int32>(Int32.MinMagnitude);
        BindFunction<Int32>(Int32.PopCount);
        BindFunction<Int32>(Int32.RotateLeft);
        BindFunction<Int32>(Int32.RotateRight);
        BindFunction<Int32>(Int32.Sign);
        BindFunction<Int32>(Int32.TrailingZeroCount);

        #endregion

        #region uint Type Support

        BindFunction<uint>(uint.Clamp);
        BindFunction<uint>(uint.CreateChecked);
        BindFunction<uint>(uint.CreateSaturating);
        BindFunction<uint>(uint.CreateTruncating);
        BindFunction<uint>(uint.LeadingZeroCount);
        BindFunction<uint>(uint.Log2);
        BindFunction<uint>(uint.Max);
        BindFunction<uint>(uint.Min);
        BindFunction<uint>(uint.PopCount);
        BindFunction<uint>(static (uint value, uint shift) => uint.RotateLeft(value, (int)shift), "uint.RotateLeft");
        BindFunction<uint>(static (uint value, uint shift) => uint.RotateRight(value, (int)shift), "uint.RotateRight");
        BindFunction<uint>(uint.TrailingZeroCount);

        #endregion

        #region uint Type Support (using explicit UInt32 type)

        BindFunction<UInt32>(UInt32.Clamp);
        BindFunction<UInt32>(UInt32.CreateChecked);
        BindFunction<UInt32>(UInt32.CreateSaturating);
        BindFunction<UInt32>(UInt32.CreateTruncating);
        BindFunction<UInt32>(UInt32.LeadingZeroCount);
        BindFunction<UInt32>(UInt32.Log2);
        BindFunction<UInt32>(UInt32.Max);
        BindFunction<UInt32>(UInt32.Min);
        BindFunction<UInt32>(UInt32.PopCount);
        BindFunction<UInt32>(static (UInt32 value, UInt32 shift) => UInt32.RotateLeft(value, (int)shift), "UInt32.RotateLeft");
        BindFunction<UInt32>(static (UInt32 value, UInt32 shift) => UInt32.RotateRight(value, (int)shift), "UInt32.RotateRight");
        BindFunction<UInt32>(UInt32.TrailingZeroCount);

        #endregion

        #region long Type Support

        BindFunction<long>(long.Abs);
        BindFunction<long>(long.Clamp);
        BindFunction<long>(long.CopySign);
        BindFunction<long>(long.CreateChecked);
        BindFunction<long>(long.CreateSaturating);
        BindFunction<long>(long.CreateTruncating);
        BindFunction<long>(long.LeadingZeroCount);
        BindFunction<long>(long.Log2);
        BindFunction<long>(long.Max);
        BindFunction<long>(long.MaxMagnitude);
        BindFunction<long>(long.Min);
        BindFunction<long>(long.MinMagnitude);
        BindFunction<long>(long.PopCount);
        BindFunction<long>(static (long value, long shift) => long.RotateLeft(value, (int)shift), "long.RotateLeft");
        BindFunction<long>(static (long value, long shift) => long.RotateRight(value, (int)shift), "long.RotateRight");
        BindFunction<long>(static (long value) => long.Sign(value), "long.Sign");
        BindFunction<long>(long.TrailingZeroCount);

        #endregion

        #region long Type Support (using explicit Int64 type)

        BindFunction<Int64>(Int64.Abs);
        BindFunction<Int64>(Int64.Clamp);
        BindFunction<Int64>(Int64.CopySign);
        BindFunction<Int64>(Int64.CreateChecked);
        BindFunction<Int64>(Int64.CreateSaturating);
        BindFunction<Int64>(Int64.CreateTruncating);
        BindFunction<Int64>(Int64.LeadingZeroCount);
        BindFunction<Int64>(Int64.Log2);
        BindFunction<Int64>(Int64.Max);
        BindFunction<Int64>(Int64.MaxMagnitude);
        BindFunction<Int64>(Int64.Min);
        BindFunction<Int64>(Int64.MinMagnitude);
        BindFunction<Int64>(Int64.PopCount);
        BindFunction<Int64>(static (Int64 value, Int64 shift) => Int64.RotateLeft(value, (int)shift), "Int64.RotateLeft");
        BindFunction<Int64>(static (Int64 value, Int64 shift) => Int64.RotateRight(value, (int)shift), "Int64.RotateRight");
        BindFunction<Int64>(static (Int64 value) => Int64.Sign(value), "Int64.Sign");
        BindFunction<Int64>(Int64.TrailingZeroCount);

        #endregion

        #region ulong Type Support

        BindFunction<ulong>(ulong.Clamp);
        BindFunction<ulong>(ulong.CreateChecked);
        BindFunction<ulong>(ulong.CreateSaturating);
        BindFunction<ulong>(ulong.CreateTruncating);
        BindFunction<ulong>(ulong.LeadingZeroCount);
        BindFunction<ulong>(ulong.Log2);
        BindFunction<ulong>(ulong.Max);
        BindFunction<ulong>(ulong.Min);
        BindFunction<ulong>(ulong.PopCount);
        BindFunction<ulong>(static (ulong value, ulong shift) => ulong.RotateLeft(value, (int)shift), "ulong.RotateLeft");
        BindFunction<ulong>(static (ulong value, ulong shift) => ulong.RotateRight(value, (int)shift), "ulong.RotateRight");
        BindFunction<ulong>(static (ulong value) => (ulong)ulong.Sign(value), "ulong.Sign");
        BindFunction<ulong>(ulong.TrailingZeroCount);

        #endregion

        #region ulong Type Support (using explicit UInt64 type)

        BindFunction<UInt64>(UInt64.Clamp);
        BindFunction<UInt64>(UInt64.CreateChecked);
        BindFunction<UInt64>(UInt64.CreateSaturating);
        BindFunction<UInt64>(UInt64.CreateTruncating);
        BindFunction<UInt64>(UInt64.LeadingZeroCount);
        BindFunction<UInt64>(UInt64.Log2);
        BindFunction<UInt64>(UInt64.Max);
        BindFunction<UInt64>(UInt64.Min);
        BindFunction<UInt64>(UInt64.PopCount);
        BindFunction<UInt64>(static (UInt64 value, UInt64 shift) => UInt64.RotateLeft(value, (int)shift), "UInt64.RotateLeft");
        BindFunction<UInt64>(static (UInt64 value, UInt64 shift) => UInt64.RotateRight(value, (int)shift), "UInt64.RotateRight");
        BindFunction<UInt64>(static (UInt64 value) => (UInt64)UInt64.Sign(value), "UInt64.Sign");
        BindFunction<UInt64>(UInt64.TrailingZeroCount);

        #endregion

        #region nint Type Support

        BindFunction<nint>(nint.Abs);
        BindFunction<nint>(nint.Clamp);
        BindFunction<nint>(nint.CopySign);
        BindFunction<nint>(nint.CreateChecked);
        BindFunction<nint>(nint.CreateSaturating);
        BindFunction<nint>(nint.CreateTruncating);
        BindFunction<nint>(nint.LeadingZeroCount);
        BindFunction<nint>(nint.Log2);
        BindFunction<nint>(nint.Max);
        BindFunction<nint>(nint.MaxMagnitude);
        BindFunction<nint>(nint.Min);
        BindFunction<nint>(nint.MinMagnitude);
        BindFunction<nint>(nint.PopCount);
        BindFunction<nint>(static (nint value, nint shift) => nint.RotateLeft(value, (int)shift), "nint.RotateLeft");
        BindFunction<nint>(static (nint value, nint shift) => nint.RotateRight(value, (int)shift), "nint.RotateRight");
        BindFunction<nint>(static (nint value) => nint.Sign(value), "nint.Sign");
        BindFunction<nint>(nint.TrailingZeroCount);

        #endregion

        #region nint Type Support (using explicit IntPtr type)

        BindFunction<IntPtr>(IntPtr.Abs);
        BindFunction<IntPtr>(IntPtr.Clamp);
        BindFunction<IntPtr>(IntPtr.CopySign);
        BindFunction<IntPtr>(IntPtr.CreateChecked);
        BindFunction<IntPtr>(IntPtr.CreateSaturating);
        BindFunction<IntPtr>(IntPtr.CreateTruncating);
        BindFunction<IntPtr>(IntPtr.LeadingZeroCount);
        BindFunction<IntPtr>(IntPtr.Log2);
        BindFunction<IntPtr>(IntPtr.Max);
        BindFunction<IntPtr>(IntPtr.MaxMagnitude);
        BindFunction<IntPtr>(IntPtr.Min);
        BindFunction<IntPtr>(IntPtr.MinMagnitude);
        BindFunction<IntPtr>(IntPtr.PopCount);
        BindFunction<IntPtr>(static (IntPtr value, IntPtr shift) => IntPtr.RotateLeft(value, (int)shift), "IntPtr.RotateLeft");
        BindFunction<IntPtr>(static (IntPtr value, IntPtr shift) => IntPtr.RotateRight(value, (int)shift), "IntPtr.RotateRight");
        BindFunction<IntPtr>(static (IntPtr value) => IntPtr.Sign(value), "IntPtr.Sign");
        BindFunction<IntPtr>(IntPtr.TrailingZeroCount);

        #endregion

        #region nuint Type Support

        static nuint newNuintFn(nuint arg) => new(arg);
        BindFunction<nuint>(newNuintFn, "new nuint");

        BindFunction<nuint>(nuint.Clamp);
        BindFunction<nuint>(nuint.CreateChecked);
        BindFunction<nuint>(nuint.CreateSaturating);
        BindFunction<nuint>(nuint.CreateTruncating);
        BindFunction<nuint>(nuint.LeadingZeroCount);
        BindFunction<nuint>(nuint.Log2);
        BindFunction<nuint>(nuint.Max);
        BindFunction<nuint>(nuint.Min);
        BindFunction<nuint>(nuint.PopCount);
        BindFunction<nuint>(static (nuint value, nuint shift) => nuint.RotateLeft(value, (int)shift), "nuint.RotateLeft");
        BindFunction<nuint>(static (nuint value, nuint shift) => nuint.RotateRight(value, (int)shift), "nuint.RotateRight");
        BindFunction<nuint>(static (nuint value) => (nuint)nuint.Sign(value), "nuint.Sign");
        BindFunction<nuint>(nuint.TrailingZeroCount);

        #endregion

        #region nuint Type Support (using explicit UIntPtr type)

        static UIntPtr newUIntPtrFn(UIntPtr arg) => new(arg);
        BindFunction<nuint>(newUIntPtrFn, "new UIntPtr");

        BindFunction<UIntPtr>(UIntPtr.Clamp);
        BindFunction<UIntPtr>(UIntPtr.CreateChecked);
        BindFunction<UIntPtr>(UIntPtr.CreateSaturating);
        BindFunction<UIntPtr>(UIntPtr.CreateTruncating);
        BindFunction<UIntPtr>(UIntPtr.LeadingZeroCount);
        BindFunction<UIntPtr>(UIntPtr.Log2);
        BindFunction<UIntPtr>(UIntPtr.Max);
        BindFunction<UIntPtr>(UIntPtr.Min);
        BindFunction<UIntPtr>(UIntPtr.PopCount);
        BindFunction<UIntPtr>(static (UIntPtr value, UIntPtr shift) => UIntPtr.RotateLeft(value, (int)shift), "UIntPtr.RotateLeft");
        BindFunction<UIntPtr>(static (UIntPtr value, UIntPtr shift) => UIntPtr.RotateRight(value, (int)shift), "UIntPtr.RotateRight");
        BindFunction<UIntPtr>(static (UIntPtr value) => (UIntPtr)UIntPtr.Sign(value), "UIntPtr.Sign");
        BindFunction<UIntPtr>(UIntPtr.TrailingZeroCount);

        #endregion

        #region Complex Type Support

        // Complex constants
        BindConstant(Complex.NaN);
        BindConstant(Complex.Infinity);

        // Complex functions
        BindFunction(Complex.CreateChecked);
        BindFunction(Complex.CreateSaturating);
        BindFunction(Complex.CreateTruncating);
        BindFunction(Complex.MaxMagnitude);
        BindFunction(Complex.MinMagnitude);

        #endregion
    }
}

#endif
