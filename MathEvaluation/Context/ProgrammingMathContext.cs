namespace MathEvaluation.Context;

/// <summary>
///     The base programming math context supports floor division '//', uses '**' notation for exponentiation, and '%' for
///     modulo operation. The constants, the floor division, the modulo, and the iif function are calculated in the precision of <see cref="double" />.
///     For a complete list of features and supported functions, please refer to the documentation at
///     <see href="https://github.com/AntonovAnton/math.evaluation" />.
/// </summary>
/// <seealso cref="MathEvaluation.Context.MathContext" />
public class ProgrammingMathContext : ProgrammingMathContext<double>
{
    /// <summary>Initializes a new instance of the <see cref="ProgrammingMathContext" /> class.</summary>
    public ProgrammingMathContext()
    {
    }
}

/// <summary>
///     The base programming math context for the <typeparamref name="T" /> floating-point numbers, such as <see cref="double" />,
///     <see cref="decimal" />, or the .NET 11 IEEE 754 decimal types. It supports floor division '//', uses '**' notation for exponentiation,
///     and '%' for modulo operation. The constants, the floor division, the modulo, and the iif function are calculated
///     in the precision of <typeparamref name="T" />.
///     For a complete list of features and supported functions, please refer to the documentation at
///     <see href="https://github.com/AntonovAnton/math.evaluation" />.
/// </summary>
/// <typeparam name="T">The type of the numbers.</typeparam>
/// <seealso cref="MathEvaluation.Context.MathContext" />
public class ProgrammingMathContext<T> : MathContext
    where T : struct, IFloatingPoint<T>
{
    /// <summary>Initializes a new instance of the <see cref="ProgrammingMathContext{T}" /> class.</summary>
    public ProgrammingMathContext()
    {
        BindConstant(T.One, "true");
        BindConstant(T.One, "True");
        BindConstant(T.One, "TRUE");

        BindConstant(T.Zero, "false");
        BindConstant(T.Zero, "False");
        BindConstant(T.Zero, "FALSE");

        BindOperator('%', OperatorType.Modulo);

        if (typeof(T) != typeof(double))
        {
            // the C# compatible operator calculates the modulo of not integer numbers in double
            static T modFn(T left, T right) => left % right;

            BindOperator<T>(modFn, '%');
        }

        static T floorDivisionFn(T left, T right) => T.Floor(left / right);

        BindOperator<T>(floorDivisionFn, "//");

        BindOperator("**", OperatorType.Power);

        BindOperator('=', OperatorType.Equal);
        BindOperator("<>", OperatorType.NotEqual);

        BindOperator('>', OperatorType.GreaterThan);
        BindOperator('<', OperatorType.LessThan);
        BindOperator(">=", OperatorType.GreaterThanOrEqual);
        BindOperator("<=", OperatorType.LessThanOrEqual);

        BindOperator("and", OperatorType.LogicalAnd);
        BindOperator("And", OperatorType.LogicalAnd);
        BindOperator("AND", OperatorType.LogicalAnd);

        BindOperator("or", OperatorType.LogicalOr);
        BindOperator("Or", OperatorType.LogicalOr);
        BindOperator("OR", OperatorType.LogicalOr);

        BindOperator("xor", OperatorType.LogicalXor);
        BindOperator("Xor", OperatorType.LogicalXor);
        BindOperator("XOR", OperatorType.LogicalXor);

        BindOperator("not", OperatorType.LogicalNot);
        BindOperator("Not", OperatorType.LogicalNot);
        BindOperator("NOT", OperatorType.LogicalNot);

        static T iifFn(T[] args) => args[0] != T.Zero
            ? args.Length > 1 ? args[1] : T.One
            : args.Length > 2
                ? args[2]
                : args.Length > 3
                    ? throw new ArgumentOutOfRangeException(nameof(args), "Count of args > 3")
                    : T.Zero;

        BindFunction<T>(iifFn, "iif");
        BindFunction<T>(iifFn, "Iif");
        BindFunction<T>(iifFn, "IIF");
    }
}
