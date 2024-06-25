﻿using System;
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
            _mathContextTrie.AddMathOperand(new MathOperand(name, value));
        }
    }

    public void BindVariable(double value, [CallerArgumentExpression(nameof(value))] string? name = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name));

        _mathContextTrie.AddMathOperand(new MathOperand(name, value));
    }


    private static bool IsNumericType(Type type)
    {
        return Type.GetTypeCode(type) switch
        {
            TypeCode.SByte or TypeCode.Byte or TypeCode.Int16 or TypeCode.UInt16 or TypeCode.Int32 or TypeCode.UInt32
                or TypeCode.Int64 or TypeCode.UInt64 or TypeCode.Single or TypeCode.Double or TypeCode.Decimal => true,
            _ => false
        };
    }

    #region explicit IMathContext implementation

    bool IMathContext.TryEvaluateMathOperand(ReadOnlySpan<char> expression, IFormatProvider provider, ref int i, char? separator, bool isAbs, out double value)
    {
        value = 0;

        var operand = _mathContextTrie.FindMathOperand(expression[i..]);
        if (operand == null)
        {
            return false;
        }

        i += operand.Name.Length;
        value = operand.Value;
        return true;
    }

    #endregion
}
