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
    public abstract TResult Evaluate<TResult>(MathExpression mathExpression, int start, ref int i, int depth, char? separator, char? closingSymbol,
        TResult value)
        where TResult : struct, INumberBase<TResult>;

    /// <inheritdoc />
    [RequiresUnreferencedCode("Building expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Building expressions in some cases requires dynamic code generation")]
    public abstract Expression Build<TResult>(MathExpression mathExpression, int start, ref int i, int depth, char? separator, char? closingSymbol,
        Expression left)
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

#if NET11_0_OR_GREATER
        if (value is not null && TryCreateNumber(value, conversionType, out var number))
            return number;
#endif

        if (conversionType == typeof(Complex))
            return new Complex(Convert.ToDouble(value), 0d);

        var result = value switch
        {
            Complex c when c.Imaginary != 0.0 => throw new InvalidCastException(
                $"Cannot convert the Complex number to a {conversionType.Name}, value = {value}."),
            Complex c => conversionType == typeof(double) ? c.Real : Convert.ChangeType(c.Real, conversionType),
            IConvertible ic => Convert.ChangeType(ic, conversionType),
            _ => Convert.ChangeType(value?.ToString(), conversionType)
        };

        return result ?? throw new InvalidCastException($"Conversion returned null for value = {value}.");
    }

    /// <summary>Converts the constant value to a number of type <typeparamref name="TResult" />.</summary>
    protected static TResult ConvertConstant<TResult>(object? value)
        where TResult : INumberBase<TResult>
        => value switch
        {
            TResult n => n,
#if NET11_0_OR_GREATER
            not null when IsGenericComplex(typeof(TResult)) => CreateNumber<TResult>(value),
#endif
            _ => (TResult)ChangeType(value, typeof(TResult))
        };

    /// <inheritdoc cref="Convert.ToDouble(object)" />
    protected static double ConvertToDouble<T>(T value)
        => value switch
        {
            double d => d,
            decimal dec => (double)dec,
            Complex c when c.Imaginary != 0.0 => throw new InvalidCastException($"Cannot convert the Complex number to a Double, value = {value}."),
            Complex c => c.Real,
#if NET11_0_OR_GREATER
            Decimal32 or Decimal64 or Decimal128 or BFloat16 or Half => CreateNumber<double>(value),
#endif
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
    [RequiresUnreferencedCode("Using Expression.Property requires unreferenced code as properties may be trimmed")]
    [RequiresDynamicCode("Building conversion expressions may require dynamic code generation")]
    protected static Expression BuildConvert<TResult>(Expression expression)
        where TResult : INumberBase<TResult>
    {
        if (expression.Type == typeof(TResult))
            return expression;

#if NET11_0_OR_GREATER
        if (IsGenericComplex(typeof(TResult)))
        {
            //convert to Complex<T>
            if (expression is ConstantExpression gc)
                return Expression.Constant(ConvertConstant<TResult>(gc.Value), typeof(TResult));

            var componentType = typeof(TResult).GetGenericArguments()[0];
            return expression.Type == typeof(bool)
                ? Expression.Condition(expression, Expression.Constant(TResult.One, typeof(TResult)), Expression.Constant(TResult.Zero, typeof(TResult)))
                : Expression.New(typeof(TResult).GetConstructor([componentType, componentType])!,
                    expression.Type == componentType ? expression : Expression.Convert(expression, componentType),
                    Expression.Default(componentType));
        }

        if (IsGenericComplex(expression.Type))
        {
            //if Imaginary is zero use Real (T), otherwise throw exception
            var componentType = expression.Type.GetGenericArguments()[0];
            var real = Expression.Property(expression, "Real");
            var imaginary = Expression.Property(expression, "Imaginary");

            var constructorInfo = typeof(InvalidCastException).GetConstructor([typeof(string)]);
            var message = $"Cannot convert a Complex<{componentType.Name}> number to a {componentType.Name}.";
            var exceptionExpr = Expression.Throw(Expression.New(constructorInfo!, Expression.Constant(message)), componentType);

            expression = Expression.Condition(Expression.Equal(imaginary, Expression.Default(componentType)), real, exceptionExpr);
            if (expression.Type == typeof(TResult))
                return expression;
        }
#endif

        if (expression is ConstantExpression c)
            return Expression.Constant(ChangeType(c.Value, typeof(TResult)), typeof(TResult));

        if (typeof(TResult) == typeof(Complex))
        {
            //convert to Complex
            var real = BuildConvert<double>(expression);
            var imaginary = Expression.Constant(0.0, typeof(double));
            return Expression.New(typeof(Complex).GetConstructor([typeof(double), typeof(double)])!, real, imaginary);
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

        if (expression.NodeType == ExpressionType.Convert && ((UnaryExpression)expression).Operand.Type == typeof(TResult))
            return ((UnaryExpression)expression).Operand;

        if (expression.Type == typeof(bool))
            return Expression.Condition(expression, Expression.Constant(TResult.One, typeof(TResult)), Expression.Constant(TResult.Zero, typeof(TResult)));

        return Expression.Convert(expression, typeof(TResult)).Reduce();
    }

#if NET11_0_OR_GREATER
    /// <summary>Determines whether the type is a generic <see cref="Complex{T}" /> type.</summary>
    protected static bool IsGenericComplex(Type type)
        => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Complex<>);

    /// <summary>Creates the <typeparamref name="TTarget" /> number from the boxed number.</summary>
    /// <exception cref="InvalidCastException">The value isn't a number.</exception>
    private static TTarget CreateNumber<TTarget>(object value)
        where TTarget : INumberBase<TTarget>
        => value switch
        {
            TTarget t => t,
            double v => TTarget.CreateChecked(v),
            decimal v => TTarget.CreateChecked(v),
            float v => TTarget.CreateChecked(v),
            int v => TTarget.CreateChecked(v),
            long v => TTarget.CreateChecked(v),
            Half v => TTarget.CreateChecked(v),
            BFloat16 v => TTarget.CreateChecked(v),
            Decimal32 v => TTarget.CreateChecked(v),
            Decimal64 v => TTarget.CreateChecked(v),
            Decimal128 v => TTarget.CreateChecked(v),
            uint v => TTarget.CreateChecked(v),
            ulong v => TTarget.CreateChecked(v),
            short v => TTarget.CreateChecked(v),
            ushort v => TTarget.CreateChecked(v),
            byte v => TTarget.CreateChecked(v),
            sbyte v => TTarget.CreateChecked(v),
            char v => TTarget.CreateChecked(v),
            nint v => TTarget.CreateChecked(v),
            nuint v => TTarget.CreateChecked(v),
            Int128 v => TTarget.CreateChecked(v),
            UInt128 v => TTarget.CreateChecked(v),
            BigInteger v => TTarget.CreateChecked(v),
            Complex v => TTarget.CreateChecked(v),
            bool v => v ? TTarget.One : TTarget.Zero,
            IConvertible ic => TTarget.CreateChecked(ic.ToDouble(CultureInfo.InvariantCulture)),
            _ => throw new InvalidCastException($"Cannot convert the {value.GetType().Name} value to a {typeof(TTarget).Name}, value = {value}.")
        };

    /// <summary>Tries to convert the boxed number to the .NET 11+ numeric type (and back), which doesn't implement <see cref="IConvertible" />.</summary>
    /// <returns><c>true</c> if the conversion type is a .NET 11+ numeric type; otherwise, <c>false</c>.</returns>
    private static bool TryCreateNumber(object value, Type conversionType, [NotNullWhen(true)] out object? result)
    {
        result = null;
        if (conversionType == typeof(Decimal64))
            result = CreateNumber<Decimal64>(value);
        else if (conversionType == typeof(Decimal128))
            result = CreateNumber<Decimal128>(value);
        else if (conversionType == typeof(Decimal32))
            result = CreateNumber<Decimal32>(value);
        else if (conversionType == typeof(BFloat16))
            result = CreateNumber<BFloat16>(value);
        else if (conversionType == typeof(Half))
            result = CreateNumber<Half>(value);
        else if (value is Decimal32 or Decimal64 or Decimal128 or BFloat16 or Half)
        {
            // the source doesn't implement IConvertible
            if (conversionType == typeof(double))
                result = CreateNumber<double>(value);
            else if (conversionType == typeof(decimal))
                result = CreateNumber<decimal>(value);
            else
                result = Convert.ChangeType(CreateNumber<double>(value), conversionType, CultureInfo.InvariantCulture);
        }

        return result != null;
    }
#endif

    #endregion
}