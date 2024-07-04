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
        BindVariable(1d, 'F');
        BindVariable(1d, 'D');
        BindVariable(1d, 'M');
        BindVariable(1d, 'L');
        BindVariable(Math.PI);
        BindVariable(Math.E);

        BindFunction(Math.Abs);
        BindFunction(Math.Acos);
        BindFunction(Math.Acosh);
        BindFunction(Math.Asin);
        BindFunction(Math.Asinh);
        BindFunction(Math.Atan);
        BindFunction(Math.Atan2);
        BindFunction(Math.Atanh);
        BindFunction(args => Math.BigMul((int)args[0], (int)args[1]), "Math.BigMul");
        BindFunction(Math.Cbrt);
        BindFunction(Math.Ceiling);
        BindFunction(Math.Clamp);
        BindFunction(Math.Cos);
        BindFunction(Math.Cosh);
        BindFunction(Math.Exp);
        BindFunction(Math.Floor);
        BindFunction(Math.IEEERemainder);
        BindFunction(args => args.Length == 1 ? Math.Log(args[0]) : Math.Log(args[0], args[1]), "Math.Log");
        BindFunction(Math.Log10);
        BindFunction(Math.Max);
        BindFunction(Math.Min);
        BindFunction(Math.Pow);
        BindFunction(Math.Round);
        BindFunction(value => Math.Sign(value), "Math.Sign");
        BindFunction(Math.Sin);
        BindFunction(Math.Sinh);
        BindFunction(Math.Sqrt);
        BindFunction(Math.Tan);
        BindFunction(Math.Tanh);
        BindFunction(Math.Truncate);

        BindVariable(byte.MaxValue);
        BindVariable(byte.MinValue);
        BindVariable(sbyte.MaxValue);
        BindVariable(sbyte.MinValue);
        BindVariable(short.MaxValue);
        BindVariable(short.MinValue);
        BindVariable(ushort.MaxValue);
        BindVariable(ushort.MinValue);
        BindVariable(int.MaxValue);
        BindVariable(int.MinValue);
        BindVariable(uint.MaxValue);
        BindVariable(uint.MinValue);
        BindVariable(long.MaxValue);
        BindVariable(long.MinValue);
        BindVariable(ulong.MaxValue);
        BindVariable(ulong.MinValue);
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

        BindVariable(Byte.MaxValue);
        BindVariable(Byte.MinValue);
        BindVariable(SByte.MaxValue);
        BindVariable(SByte.MinValue);
        BindVariable(Int16.MaxValue);
        BindVariable(Int16.MinValue);
        BindVariable(UInt16.MaxValue);
        BindVariable(UInt16.MinValue);
        BindVariable(Int32.MaxValue);
        BindVariable(Int32.MinValue);
        BindVariable(UInt32.MaxValue);
        BindVariable(UInt32.MinValue);
        BindVariable(Int64.MaxValue);
        BindVariable(Int64.MinValue);
        BindVariable(UInt64.MaxValue);
        BindVariable(UInt64.MinValue);
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
        BindVariable((double)decimal.One, "Decimal.One");
        BindVariable((double)decimal.MinusOne, "Decimal.MinusOne");
        BindVariable((double)decimal.Zero, "Decimal.Zero");
        BindVariable((double)decimal.MaxValue, "Decimal.MaxValue");
        BindVariable((double)decimal.MinValue, "Decimal.MinValue");
    }
}

