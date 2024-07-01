using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MathEvaluation.Context;

public class MathContext : IMathContext
{
    private readonly MathContextTrie _mathContextTrie = new();

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

            var key = propertyInfo.Name;
            var value = Convert.ToDouble(propertyInfo.GetValue(args, null));
            _mathContextTrie.AddMathEntity(new MathVariable(key, value));
        }
    }

    public void BindVariable(double value, [CallerArgumentExpression(nameof(value))] string? key = null)
        => _mathContextTrie.AddMathEntity(new MathVariable(key, value));

    public void BindFunction(Func<double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null)
        => _mathContextTrie.AddMathEntity(new MathFunction(key, fn));

    protected void BindConstant(double value, [CallerArgumentExpression(nameof(value))] string? key = null)
        => _mathContextTrie.AddMathEntity(new MathConstant(key, value));

    protected void BindFunction(Func<double, double> fn, char openingSymbol, char closureSymbol)
        => _mathContextTrie.AddMathEntity(new MathFunction(openingSymbol.ToString(), fn, closureSymbol));

    protected void BindConverter(Func<double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null)
        => _mathContextTrie.AddMathEntity(new MathOperandConverter(key, fn));

    protected void BindOperator(Func<double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null)
        => _mathContextTrie.AddMathEntity(new MathOperator(key, fn));

    private static bool IsNumericType(Type type) => Type.GetTypeCode(type) switch
    {
        TypeCode.SByte or TypeCode.Byte or TypeCode.Int16 or TypeCode.UInt16 or TypeCode.Int32 or TypeCode.UInt32
            or TypeCode.Int64 or TypeCode.UInt64 or TypeCode.Single or TypeCode.Double or TypeCode.Decimal => true,
        _ => false
    };

    #region explicit IMathContext

    IMathEntity? IMathContext.FindMathEntity(ReadOnlySpan<char> expression)
    {
        return _mathContextTrie.FindMathEntity(expression);
    }

    #endregion
}
