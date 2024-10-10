using System;
using System.Linq.Expressions;
using System.Numerics;

namespace MathEvaluation.Entities;

/// <summary>
/// Base class for a math entity.
/// </summary>
public abstract class MathEntity : IMathEntity
{
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

    /// <summary> Converts to string. </summary>
    /// <returns>
    /// A <see cref="System.String" /> that represents this instance.
    /// </returns>
    public override string ToString()
        => $"{nameof(Key)}: \"{Key}\", {nameof(Precedence)}: {Precedence}";

    #region protected static Convert Methods

    /// <inheritdoc cref="Convert.ChangeType(object, Type)"/>
    protected static object ChangeType<T>(T value, Type conversionType)
    {
        if (conversionType == typeof(Complex))
            return new Complex(Convert.ToDouble(value), 0d);

        if (value is Complex c)
        {
            if (c.Imaginary != default)
                throw new InvalidCastException($"Cannot convert the Complex number to a {conversionType.Name}, value = {value}.");

            return conversionType == typeof(double)
                ? c.Real
                : Convert.ChangeType(c.Real, conversionType);
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
                throw new InvalidCastException($"Cannot convert the Complex number to a Double, value = {value}.");

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
                throw new InvalidCastException($"Cannot convert the Complex number to a Decimal, value = {value}.");

            return (decimal)c.Real;
        }

        return Convert.ToDecimal(value);
    }

    /// <summary>
    /// Builds the convertion operation.
    /// </summary>
    /// <typeparam name="TResult">A <see cref="System.Type"/> to set the System.Linq.Expressions.Expression.Type property equal to.</typeparam>
    /// <param name="expression">The expression tree.</param>
    /// <returns></returns>
    protected static Expression BuildConvert<TResult>(Expression expression)
    {
        if (expression.Type == typeof(TResult))
            return expression;

        if (expression is ConstantExpression c)
            return Expression.Constant(ChangeType(c.Value, typeof(TResult)));

        if (typeof(TResult) == typeof(Complex))
        {
            //convert to Complex
            var real = BuildConvert<double>(expression);
            var imaginary = Expression.Constant(0.0);
            return Expression.New(typeof(Complex).GetConstructor([typeof(double), typeof(double)]), real, imaginary);
        }

        if (typeof(TResult) == typeof(bool))
        {
            //if it is default then false overwise true
            return Expression.NotEqual(expression, Expression.Default(expression.Type)).Reduce();
        }

        if (expression.Type == typeof(Complex))
        {
            //if Imaginary is default use Real, overwise throw exception
            var real = Expression.Property(expression, nameof(Complex.Real));
            var imaginary = Expression.Property(expression, nameof(Complex.Imaginary));

            var constructorInfo = typeof(InvalidCastException).GetConstructor([typeof(string)]);
            const string message = "Cannot convert a Complex number to a Double.";
            var exceptionExpr = Expression.Throw(Expression.New(constructorInfo, Expression.Constant(message)), typeof(double));

            expression = Expression.Condition(Expression.Equal(imaginary, Expression.Default(typeof(double))), real, exceptionExpr);
        }

        if (expression.Type == typeof(TResult))
            return expression;

        if (typeof(TResult) == typeof(decimal) && expression.Type == typeof(bool))
            return Expression.Condition(expression, Expression.Constant(1.0m), Expression.Constant(0.0m));

        return Expression.Convert(expression, typeof(TResult)).Reduce();
    }

    #endregion
}
