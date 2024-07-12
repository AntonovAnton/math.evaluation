using System;

namespace MathEvaluation.Context;

public class DotNetStandartMathContext : ProgrammingMathContext
{
    public DotNetStandartMathContext()
        : base()
    {
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
        BindFunction((double[] args) => Math.BigMul((int)args[0], (int)args[1]), "Math.BigMul");
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

