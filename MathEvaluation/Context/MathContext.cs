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
            if (!propertyInfo.CanRead)
                continue;

            var key = propertyInfo.Name;
            var value = propertyInfo.GetValue(args, null);
            if (IsNumericType(propertyInfo.PropertyType))
            {
                BindVariable(Convert.ToDouble(value), key);
            }
            else if (value is Func<double> fn1)
                BindVariable(fn1, key);
            else if (value is Func<double, double> fn2)
                BindFunction(fn2, key);
            else if (value is Func<double, double, double> fn3)
                BindFunction(fn3, key);
            else if (value is Func<double, double, double, double> fn4)
                BindFunction(fn4, key);
            else if (value is Func<double, double, double, double, double> fn5)
                BindFunction(fn5, key);
            else if (value is Func<double, double, double, double, double, double> fn6)
                BindFunction(fn6, key);
            else if (value is Func<double[], double> fns)
                BindFunction(fns, key);
            else
            {
                if (propertyInfo.PropertyType.FullName.StartsWith("System.Func"))
                    throw new NotSupportedException($"{propertyInfo.PropertyType} isn't supported, you can use Func<T[], T> istead.");

                throw new NotSupportedException($"{propertyInfo.PropertyType} isn't supported.");
            }
        }
    }

    public void BindVariable(double value, char key)
        => _mathContextTrie.AddMathEntity(new MathVariable<double>(key.ToString(), value));

    public void BindVariable(double value, [CallerArgumentExpression(nameof(value))] string? key = null)
        => _mathContextTrie.AddMathEntity(new MathVariable<double>(key, value));

    public void BindVariable(Func<double> getValue, char key)
        => _mathContextTrie.AddMathEntity(new MathVariableFunction<double>(key.ToString(), getValue));

    public void BindVariable(Func<double> getValue, [CallerArgumentExpression(nameof(getValue))] string? key = null)
        => _mathContextTrie.AddMathEntity(new MathVariableFunction<double>(key, getValue));

    public void BindFunction(Func<double, double> fn, char key)
        => _mathContextTrie.AddMathEntity(new BasicMathFunction<double>(key.ToString(), fn));

    public void BindFunction(Func<double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null)
        => _mathContextTrie.AddMathEntity(new BasicMathFunction<double>(key, fn));

    public void BindFunction(Func<double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
        => BindFunction((double[] args) => fn(args[0], args[1]), key, openingSymbol, separator, closingSymbol);

    public void BindFunction(Func<double, double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
        => BindFunction((double[] args) => fn(args[0], args[1], args[3]), key, openingSymbol, separator, closingSymbol);

    public void BindFunction(Func<double, double, double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
        => BindFunction((double[] args) => fn(args[0], args[1], args[3], args[4]), key, openingSymbol, separator, closingSymbol);

    public void BindFunction(Func<double, double, double, double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
        => BindFunction((double[] args) => fn(args[0], args[1], args[3], args[4], args[5]), key, openingSymbol, separator, closingSymbol);

    public void BindFunction(Func<double[], double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null,
        char openingSymbol = IMathContext.DefaultOpeningSymbol, char separator = IMathContext.DefaultParamsSeparator,
        char closingSymbol = IMathContext.DefaultClosingSymbol)
        => _mathContextTrie.AddMathEntity(new MathFunction<double>(key, fn, openingSymbol, separator, closingSymbol));

    protected void BindFunction(Func<double, double> fn, char openingSymbol, char closingSymbol)
        => _mathContextTrie.AddMathEntity(new BasicMathFunction<double>(openingSymbol.ToString(), fn, closingSymbol));

    protected void BindConverter(Func<double, double> fn, char key)
        => _mathContextTrie.AddMathEntity(new MathOperandConverter<double>(key.ToString(), fn));

    protected void BindConverter(Func<double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null)
        => _mathContextTrie.AddMathEntity(new MathOperandConverter<double>(key, fn));

    protected void BindOperator(Func<double, double, double> fn, char key)
        => _mathContextTrie.AddMathEntity(new MathOperator<double>(key.ToString(), fn));

    protected void BindOperator(Func<double, double, double> fn, [CallerArgumentExpression(nameof(fn))] string? key = null)
        => _mathContextTrie.AddMathEntity(new MathOperator<double>(key, fn));

    private static bool IsNumericType(Type type) => Type.GetTypeCode(type) switch
    {
        TypeCode.SByte or TypeCode.Byte or TypeCode.Int16 or TypeCode.UInt16 or TypeCode.Int32 or TypeCode.UInt32
            or TypeCode.Int64 or TypeCode.UInt64 or TypeCode.Single or TypeCode.Double or TypeCode.Decimal => true,
        _ => false
    };

    #region explicit IMathContext

    IMathEntity? IMathContext.FindMathEntity(ReadOnlySpan<char> expression)
        => _mathContextTrie.FindMathEntity(expression);

    #endregion
}
