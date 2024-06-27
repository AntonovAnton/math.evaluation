using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MathEvaluation.Context;

public class MathContext : IMathContext
{
    private MathContextTrie _mathContextTrie = new();

    public void Bind(object args)
    {
        if (args == null)
            throw new ArgumentNullException(nameof(args));

        foreach (var propertyInfo in args
                     .GetType()
                     .GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (!propertyInfo.CanRead || !IsNumericType(propertyInfo.PropertyType))
                continue;

            var name = propertyInfo.Name;
            var value = Convert.ToDouble(propertyInfo.GetValue(args, null));
            _mathContextTrie.AddMathOperand(new MathNumber(name, value));
        }
    }

    public void BindVariable(double value, [CallerArgumentExpression(nameof(value))] string? name = null)
        => _mathContextTrie.AddMathOperand(new MathNumber(name, value));

    public void BindFunction(Func<double, double> value, [CallerArgumentExpression(nameof(value))] string? name = null)
        => _mathContextTrie.AddMathOperand(new MathFunction(name, value));

    protected void BindConstant(double value, [CallerArgumentExpression(nameof(value))] string? name = null)
        => BindVariable(value, name);

    protected void BindFunction(Func<double, double> value, char openingSymbol, char closureSymbol)
        => _mathContextTrie.AddMathOperand(new MathFunction(openingSymbol.ToString(), value, closureSymbol));

    protected void BindConverter(Func<double, double> value, [CallerArgumentExpression(nameof(value))] string? name = null)
        => _mathContextTrie.AddMathOperand(new MathOperandConverter(name, value));

    protected virtual bool TryEvaluate(ReadOnlySpan<char> expression, IFormatProvider provider, ref int i, char? separator,
        bool isEvaluatedFirst, ref double value)
    {
        var operand = _mathContextTrie.FindMathOperand(expression[i..]);
        if (operand == null)
        {
            return false;
        }

        if (operand is MathNumber mathNumber)
        {
            i += operand.Name.Length;
            value = (value == 0 ? 1 : value) *
                MathEvaluator.EvaluateExponentiation(expression, this, provider, ref i, separator, mathNumber.Value);
            return true;
        }

        if (operand is MathOperandConverter mathOperandConvertor)
        {
            i += operand.Name.Length;
            var fn = mathOperandConvertor.Fn;
            var result = fn(value);
            value = MathEvaluator.EvaluateExponentiation(expression, this, provider, ref i, separator, result);
            return true;
        }

        if (operand is MathFunction mathFunction)
        {
            i += operand.Name.Length;
            var fn = mathFunction.Fn;
            var result = mathFunction.Separator.HasValue
                ? fn(MathEvaluator.EvaluateLowestBasic(expression, this, provider, ref i, mathFunction.Separator))
                : fn(MathEvaluator.EvaluateBasic(expression, this, provider, ref i, separator, true));
            value = (value == 0 ? 1 : value) *
                MathEvaluator.EvaluateExponentiation(expression, this, provider, ref i, separator, result);
            return true;
        }

        return false;
    }

    private static bool IsNumericType(Type type) => Type.GetTypeCode(type) switch
    {
        TypeCode.SByte or TypeCode.Byte or TypeCode.Int16 or TypeCode.UInt16 or TypeCode.Int32 or TypeCode.UInt32
            or TypeCode.Int64 or TypeCode.UInt64 or TypeCode.Single or TypeCode.Double or TypeCode.Decimal => true,
        _ => false
    };

    #region explicit IMathContext

    bool IMathContext.TryEvaluate(ReadOnlySpan<char> expression, IFormatProvider provider, ref int i, char? separator,
        bool isEvaluatedFirst, ref double value)
    {
        return TryEvaluate(expression, provider, ref i, separator, isEvaluatedFirst, ref value);
    }

    #endregion
}
