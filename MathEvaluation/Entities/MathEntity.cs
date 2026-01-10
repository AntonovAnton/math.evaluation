using System;
using System.Linq.Expressions;
using System.Numerics;

namespace MathEvaluation.Entities;

/// <summary>
///     Base class for a math entity.
/// </summary>
internal abstract class MathEntity : IMathEntity
{
    /// <inheritdoc />
    public string Key { get; }

    /// <inheritdoc />
    public abstract int Precedence { get; }

    /// <summary>Initializes a new instance of the <see cref="MathEntity" /> class.</summary>
    /// <param name="key">The key.</param>
    /// <exception cref="ArgumentNullException" />
    protected MathEntity(string? key)
    {
        Key = key ?? throw new ArgumentNullException(nameof(key));
    }

    /// <inheritdoc />
    public abstract TResult Evaluate<TResult>(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, TResult value)
        where TResult : struct, INumberBase<TResult>;

    /// <inheritdoc />
    public abstract Expression Build<TResult>(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, Expression left)
        where TResult : struct, INumberBase<TResult>;

    /// <summary> Converts to string. </summary>
    /// <returns>
    ///     A <see cref="System.String" /> that represents this instance.
    /// </returns>
    public override string ToString()
        => $"{nameof(Key)}: \"{Key}\", {nameof(Precedence)}: {Precedence}";

    #region protected static Convert Methods

    /// <inheritdoc cref="Convert.ChangeType(object, Type)" />
    protected static object ChangeType<T>(T value, Type conversionType)
    {
        if (conversionType == typeof(T) && value is not null)
            return value;

        if (conversionType == typeof(Complex))
            return new Complex(Convert.ToDouble(value), 0d);

        var result = value switch
        {
            Complex c when c.Imaginary != default => throw new InvalidCastException(
                $"Cannot convert the Complex number to a {conversionType.Name}, value = {value}."),
            Complex c => conversionType == typeof(double) ? c.Real : Convert.ChangeType(c.Real, conversionType),
            IConvertible ic => Convert.ChangeType(ic, conversionType),
            _ => Convert.ChangeType(value?.ToString(), conversionType)
        };

        return result ?? throw new InvalidCastException($"Conversion returned null for value = {value}.");
    }

    /// <inheritdoc cref="Convert.ToDouble(object)" />
    protected static double ConvertToDouble<T>(T value)
        => value switch
        {
            double d => d,
            decimal dec => (double)dec,
            Complex c when c.Imaginary != default => throw new InvalidCastException($"Cannot convert the Complex number to a Double, value = {value}."),
            Complex c => c.Real,
            _ => Convert.ToDouble(value)
        };

    /// <summary>
    /// Converts the specified value to a number of type TResult.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    /// <exception cref="InvalidCastException"></exception>
    /// <exception cref="OverflowException"></exception>
    protected static TResult ConvertNumber<T, TResult>(T value)
        where T : INumberBase<T>
        where TResult : INumberBase<TResult>
    {
        return value switch
        {
            TResult n => n,
            _ => TResult.CreateChecked(value)
        };
    }

    /// <summary>
    ///     Builds the conversion operation.
    /// </summary>
    /// <typeparam name="TResult">
    ///     A <see cref="System.Type" /> to set the System.Linq.Expressions.Expression.Type property
    ///     equal to.
    /// </typeparam>
    /// <param name="expression">The expression tree.</param>
    /// <returns></returns>
    protected static Expression BuildConvert<TResult>(Expression expression)
    {
        if (expression.Type == typeof(TResult))
            return expression;

        if (expression is ConstantExpression c)
            return Expression.Constant(ChangeType(c.Value, typeof(TResult)), typeof(TResult));

        if (typeof(TResult) == typeof(Complex))
        {
            //convert to Complex
            var real = BuildConvert<double>(expression);
            var imaginary = Expression.Constant(0.0);
            return Expression.New(typeof(Complex).GetConstructor([typeof(double), typeof(double)])!, real, imaginary);
        }

        if (typeof(TResult) == typeof(bool))
        {
            //if it is default then false otherwise true
            return Expression.NotEqual(expression, Expression.Default(expression.Type)).Reduce();
        }

        if (expression.Type == typeof(Complex))
        {
            //if Imaginary is default use Real, otherwise throw exception
            var real = Expression.Property(expression, nameof(Complex.Real));
            var imaginary = Expression.Property(expression, nameof(Complex.Imaginary));

            var constructorInfo = typeof(InvalidCastException).GetConstructor([typeof(string)]);
            const string message = "Cannot convert a Complex number to a Double.";
            var exceptionExpr = Expression.Throw(Expression.New(constructorInfo!, Expression.Constant(message)), typeof(double));

            expression = Expression.Condition(Expression.Equal(imaginary, Expression.Default(typeof(double))), real, exceptionExpr);
        }

        if (expression.NodeType == ExpressionType.Convert && ((UnaryExpression)expression).Operand?.Type == typeof(TResult))
            return ((UnaryExpression)expression).Operand;

        if (typeof(TResult) == typeof(decimal) && expression.Type == typeof(bool))
            return Expression.Condition(expression, Expression.Constant(1.0m, typeof(decimal)), Expression.Constant(0.0m, typeof(decimal)));

        return Expression.Convert(expression, typeof(TResult)).Reduce();
    }

    #endregion
}