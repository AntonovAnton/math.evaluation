using MathEvaluation.Extensions;
using System;
using System.Linq.Expressions;
using System.Numerics;

namespace MathEvaluation.Entities;

/// <summary>
/// Base class for a math entity.
/// </summary>
public abstract class MathEntity : IMathEntity
{
    /// <summary>
    /// The not complex error message.
    /// </summary>
    protected static string NotComplexErrorMessage = "Evaluation the imaginary component of a complex number only supports functions designed for complex numbers. Please use a context configured for complex number operations.";

    /// <inheritdoc/>
    public string Key { get; }

    /// <inheritdoc/>
    public abstract int Precedence { get; }

    /// <summary>Initializes a new instance of the <see cref="MathEntity" /> class.</summary>
    /// <param name="key">The key.</param>
    /// <exception cref="ArgumentNullException"/>
    protected MathEntity(string? key)
    {
        Key = key ?? throw new ArgumentNullException(nameof(key));
    }

    /// <inheritdoc/>
    public abstract double Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, double value);

    /// <inheritdoc/>
    public abstract decimal Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, decimal value);

    /// <inheritdoc/>
    public abstract Complex Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, Complex value);

    /// <inheritdoc/>
    public abstract Expression Build<TResult>(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, Expression left)
        where TResult : struct;

    /// <inheritdoc cref="Convert.ChangeType(object, Type)"/>
    protected static object ChangeType<T>(T value, Type conversionType)
        where T : struct
    {
        if (conversionType == typeof(Complex) && typeof(T).IsConvertibleToDouble())
            return new Complex(Convert.ToDouble(value), 0d);

        if (value is Complex c && conversionType.IsConvertibleToDouble())
        {
            if (c.Imaginary != default)
                throw new NotSupportedException(NotComplexErrorMessage);

            return c.Real;
        }

        return Convert.ChangeType(value, conversionType);
    }

    /// <inheritdoc cref="Convert.ToDouble(object)"/>
    protected static double ConvertToDouble<T>(T value)
    {
        if (value is double d)
            return d;

        if (value is decimal dec)
            return (double)dec;

        if (value is Complex c)
        {
            if (c.Imaginary != default)
                throw new NotSupportedException(NotComplexErrorMessage);

            return c.Real;
        }

        return Convert.ToDouble(value);
    }

    /// <inheritdoc cref="Convert.ToDecimal(object)"/>
    protected static decimal ConvertToDecimal<T>(T value)
    {
        if (value is decimal d)
            return d;

        if (value is double dob)
            return (decimal)dob;

        if (value is Complex c)
        {
            if (c.Imaginary != default)
                throw new NotSupportedException(NotComplexErrorMessage);

            return (decimal)c.Real;
        }

        return Convert.ToDecimal(value);
    }

    /// <summary> Converts to string. </summary>
    /// <returns>
    /// A <see cref="System.String" /> that represents this instance.
    /// </returns>
    public override string ToString()
        => $"{nameof(Key)}: \"{Key}\", {nameof(Precedence)}: {Precedence}";
}
