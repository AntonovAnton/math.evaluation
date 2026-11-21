using MathTrigonometric;

namespace MathEvaluation.Context;

/// <summary>
///     The base scientific math context supports all trigonometric functions, logarithms, other scientific math functions,
///     and constants, abd it calculates them in the precision of <see cref="double" />.
///     For a complete list of features and supported functions, please refer to the documentation at
///     <see href="https://github.com/AntonovAnton/math.evaluation" />.
/// </summary>
/// <seealso cref="MathEvaluation.Context.MathContext" />
public class ScientificMathContext : ScientificMathContext<double>
{
    /// <summary>Initializes a new instance of the <see cref="ScientificMathContext" /> class.</summary>
    public ScientificMathContext()
    {
    }
}

/// <summary>
///     The base scientific math context for the IEEE 754 floating-point numbers, such as <see cref="double" />, <see cref="float" />,
///     <see cref="Half" />, or the .NET 11 types BFloat16, Decimal32, Decimal64, and Decimal128.
///     It supports all trigonometric functions, logarithms, other scientific math functions, and constants,
///     and it calculates them in the precision of <typeparamref name="T" />.
///     For a complete list of features and supported functions, please refer to the documentation at
///     <see href="https://github.com/AntonovAnton/math.evaluation" />.
/// </summary>
/// <typeparam name="T">The type of the numbers.</typeparam>
/// <seealso cref="MathEvaluation.Context.MathContext" />
public class ScientificMathContext<T> : MathContext
    where T : struct, IFloatingPointIeee754<T>, IMinMaxValue<T>
{
    /// <summary>Initializes a new instance of the <see cref="ScientificMathContext{T}" /> class.</summary>
    public ScientificMathContext()
    {
        BindConstant(T.Pi, 'π');
        BindConstant(T.Pi, "pi");
        BindConstant(T.Pi, "Pi");
        BindConstant(T.Pi, "PI");
        BindConstant(T.E, 'e');
        BindConstant(T.Tau, 'τ');
        BindConstant(T.PositiveInfinity, '\u221e'); //infinity symbol


        BindOperator('÷', OperatorType.Divide);

        static T floorDivisionFn(T left, T right) => T.Floor(left / right);

        BindOperator<T>(floorDivisionFn, "//");

        BindOperator('×', OperatorType.Multiply);
        BindOperator('·', OperatorType.Multiply);

        BindOperator('^', OperatorType.Power);

        BindOperator("mod", OperatorType.Modulo);
        BindOperator("Mod", OperatorType.Modulo);
        BindOperator("MOD", OperatorType.Modulo);
        BindOperator("modulo", OperatorType.Modulo);
        BindOperator("Modulo", OperatorType.Modulo);
        BindOperator("MODULO", OperatorType.Modulo);

        static T powerFn(T left, T right) => T.Pow(left, right);

        BindFunction<T>(powerFn, "pow");
        BindFunction<T>(powerFn, "Pow");
        BindFunction<T>(powerFn, "POW");

        if (typeof(T) != typeof(double))
        {
            // the C# compatible operators calculate the power and the modulo of not integer numbers in double
            BindOperandsOperator<T>(powerFn, '^', (int)EvalPrecedence.Exponentiation);

            static T modFn(T left, T right) => left % right;

            BindOperator<T>(modFn, "mod");
            BindOperator<T>(modFn, "Mod");
            BindOperator<T>(modFn, "MOD");
            BindOperator<T>(modFn, "modulo");
            BindOperator<T>(modFn, "Modulo");
            BindOperator<T>(modFn, "MODULO");
        }

        static T absFn(T v) => T.Abs(v);

        BindFunction<T>(absFn, '|', '|');
        BindFunction<T>(absFn, "abs");
        BindFunction<T>(absFn, "Abs");
        BindFunction<T>(absFn, "ABS");

        static T posFn(T v) => T.Max(v, T.Zero);

        BindFunction<T>(posFn, "pos");
        BindFunction<T>(posFn, "Pos");
        BindFunction<T>(posFn, "POS");

        static T ceilingFn(T v) => T.Ceiling(v);

        BindFunction<T>(ceilingFn, '⌈', '⌉');
        BindFunction<T>(ceilingFn, "ceil");
        BindFunction<T>(ceilingFn, "Ceil");
        BindFunction<T>(ceilingFn, "CEIL");

        static T floorFn(T v) => T.Floor(v);

        BindFunction<T>(floorFn, '⌊', '⌋');
        BindFunction<T>(floorFn, "floor");
        BindFunction<T>(floorFn, "Floor");
        BindFunction<T>(floorFn, "FLOOR");

        static T sqrtFn(T v) => T.Sqrt(v);

        BindFunction<T>(sqrtFn, '\u221a'); //square root symbol
        BindFunction<T>(sqrtFn, "sqrt");
        BindFunction<T>(sqrtFn, "Sqrt");
        BindFunction<T>(sqrtFn, "SQRT");

        static T cubeFn(T v) => T.Pow(v, T.One / T.CreateTruncating(3));

        BindFunction<T>(cubeFn, '\u221b'); //cube root symbol
        BindFunction<T>(cubeFn, "cbrt");
        BindFunction<T>(cubeFn, "Cbrt");
        BindFunction<T>(cubeFn, "CBRT");

        static T fRootFn(T v) => T.Pow(v, T.One / T.CreateTruncating(4));

        BindFunction<T>(fRootFn, '\u221c'); //fourth root symbol

        static T logFn(T v) => T.Log(v);

        BindFunction<T>(logFn, "ln");
        BindFunction<T>(logFn, "Ln");
        BindFunction<T>(logFn, "LN");

        static T log10Fn(T v) => T.Log10(v);

        BindFunction<T>(log10Fn, "log");
        BindFunction<T>(log10Fn, "Log");
        BindFunction<T>(log10Fn, "LOG");

        static T expFn(T v) => T.Exp(v);

        BindFunction<T>(expFn, "exp");
        BindFunction<T>(expFn, "Exp");
        BindFunction<T>(expFn, "EXP");

        static T factorialFn(T v) => Factorial(v);

        BindOperandOperator<T>(factorialFn, '!', true);

        #region boolean logic

        BindConstant(T.One, "true");
        BindConstant(T.One, "True");
        BindConstant(T.One, "TRUE");
        BindConstant(T.One, 'T');
        BindConstant(T.One, '⊤');

        BindConstant(T.Zero, "false");
        BindConstant(T.Zero, "False");
        BindConstant(T.Zero, "FALSE");
        BindConstant(T.Zero, 'F');
        BindConstant(T.Zero, '⊥');

        BindOperator('=', OperatorType.Equal);
        BindOperator('≠', OperatorType.NotEqual);

        static T equalToFn(T left, T right) => left == right ? T.One : T.Zero;

        BindOperator<T>(equalToFn, '↔', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator<T>(equalToFn, '⇔', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator<T>(equalToFn, '≡', (int)EvalPrecedence.Equivalence);

        static T notEqualToFn(T left, T right) => left != right ? T.One : T.Zero;

        BindOperator<T>(notEqualToFn, '↮', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator<T>(notEqualToFn, '⇎', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator<T>(notEqualToFn, '≢', (int)EvalPrecedence.Equivalence);

        BindOperator('>', OperatorType.GreaterThan);
        BindOperator('<', OperatorType.LessThan);

        BindOperator('≥', OperatorType.GreaterThanOrEqual);
        BindOperator('⪰', OperatorType.GreaterThanOrEqual);

        BindOperator('≤', OperatorType.LessThanOrEqual);
        BindOperator('⪯', OperatorType.LessThanOrEqual);

        static T implicationFn(T left, T right) => left == T.Zero || right != T.Zero ? T.One : T.Zero;

        BindOperator<T>(implicationFn, '→', (int)EvalPrecedence.LogicalImplication);
        BindOperator<T>(implicationFn, '⇒', (int)EvalPrecedence.LogicalImplication);

        static T reverseImplicationFn(T left, T right) => left != T.Zero || right == T.Zero ? T.One : T.Zero;

        BindOperator<T>(reverseImplicationFn, '←', (int)EvalPrecedence.LogicalImplication);
        BindOperator<T>(reverseImplicationFn, '⟸', (int)EvalPrecedence.LogicalImplication);

        BindOperator('∧', OperatorType.LogicalAnd);
        BindOperator("and", OperatorType.LogicalAnd);
        BindOperator("And", OperatorType.LogicalAnd);
        BindOperator("AND", OperatorType.LogicalAnd);

        BindOperator('∨', OperatorType.LogicalOr);
        BindOperator("or", OperatorType.LogicalOr);
        BindOperator("Or", OperatorType.LogicalOr);
        BindOperator("OR", OperatorType.LogicalOr);

        BindOperator('⊕', OperatorType.LogicalXor);
        BindOperator("xor", OperatorType.LogicalXor);
        BindOperator("Xor", OperatorType.LogicalXor);
        BindOperator("XOR", OperatorType.LogicalXor);

        BindOperator('¬', OperatorType.LogicalNegation);
        BindOperator("not", OperatorType.LogicalNot);
        BindOperator("Not", OperatorType.LogicalNot);
        BindOperator("NOT", OperatorType.LogicalNot);

        #endregion

        #region trigonometric functions

        static T radiansFn(T v) => MathTrig.DegreesToRadians(v);

        BindOperandOperator<T>(radiansFn, '\u00b0', true); //degree symbol
        BindFunction<T>(radiansFn, "rad");
        BindFunction<T>(radiansFn, "Rad");
        BindFunction<T>(radiansFn, "RAD");

        static T degreesFn(T v) => MathTrig.RadiansToDegrees(v);

        BindFunction<T>(degreesFn, "deg");
        BindFunction<T>(degreesFn, "Deg");
        BindFunction<T>(degreesFn, "DEG");

        static T sinFn(T v) => MathTrig.Sin(v);

        BindFunction<T>(sinFn, "sin");
        BindFunction<T>(sinFn, "Sin");
        BindFunction<T>(sinFn, "SIN");

        static T cosFn(T v) => MathTrig.Cos(v);

        BindFunction<T>(cosFn, "cos");
        BindFunction<T>(cosFn, "Cos");
        BindFunction<T>(cosFn, "COS");

        static T tanFn(T v) => MathTrig.Tan(v);

        BindFunction<T>(tanFn, "tan");
        BindFunction<T>(tanFn, "Tan");
        BindFunction<T>(tanFn, "TAN");

        static T secFn(T v) => MathTrig.Sec(v);

        BindFunction<T>(secFn, "sec");
        BindFunction<T>(secFn, "Sec");
        BindFunction<T>(secFn, "SEC");

        static T cscFn(T v) => MathTrig.Csc(v);

        BindFunction<T>(cscFn, "csc");
        BindFunction<T>(cscFn, "Csc");
        BindFunction<T>(cscFn, "CSC");

        static T cotFn(T v) => MathTrig.Cot(v);

        BindFunction<T>(cotFn, "cot");
        BindFunction<T>(cotFn, "Cot");
        BindFunction<T>(cotFn, "COT");

        static T sinhFn(T v) => MathTrig.Sinh(v);

        BindFunction<T>(sinhFn, "sinh");
        BindFunction<T>(sinhFn, "Sinh");
        BindFunction<T>(sinhFn, "SINH");

        static T coshFn(T v) => MathTrig.Cosh(v);

        BindFunction<T>(coshFn, "cosh");
        BindFunction<T>(coshFn, "Cosh");
        BindFunction<T>(coshFn, "COSH");

        static T tanhFn(T v) => MathTrig.Tanh(v);

        BindFunction<T>(tanhFn, "tanh");
        BindFunction<T>(tanhFn, "Tanh");
        BindFunction<T>(tanhFn, "TANH");

        static T sechFn(T v) => MathTrig.Sech(v);

        BindFunction<T>(sechFn, "sech");
        BindFunction<T>(sechFn, "Sech");
        BindFunction<T>(sechFn, "SECH");

        static T cschFn(T v) => MathTrig.Csch(v);

        BindFunction<T>(cschFn, "csch");
        BindFunction<T>(cschFn, "Csch");
        BindFunction<T>(cschFn, "CSCH");

        static T cothFn(T v) => MathTrig.Coth(v);

        BindFunction<T>(cothFn, "coth");
        BindFunction<T>(cothFn, "Coth");
        BindFunction<T>(cothFn, "COTH");

        static T asinFn(T v) => MathTrig.Asin(v);

        BindFunction<T>(asinFn, "sin^-1");
        BindFunction<T>(asinFn, "Sin^-1");
        BindFunction<T>(asinFn, "SIN^-1");

        static T acosFn(T v) => MathTrig.Acos(v);

        BindFunction<T>(acosFn, "cos^-1");
        BindFunction<T>(acosFn, "Cos^-1");
        BindFunction<T>(acosFn, "COS^-1");

        static T atanFn(T v) => MathTrig.Atan(v);

        BindFunction<T>(atanFn, "tan^-1");
        BindFunction<T>(atanFn, "Tan^-1");
        BindFunction<T>(atanFn, "TAN^-1");

        static T asecFn(T v) => MathTrig.Asec(v);

        BindFunction<T>(asecFn, "sec^-1");
        BindFunction<T>(asecFn, "Sec^-1");
        BindFunction<T>(asecFn, "SEC^-1");

        static T acscFn(T v) => MathTrig.Acsc(v);

        BindFunction<T>(acscFn, "csc^-1");
        BindFunction<T>(acscFn, "Csc^-1");
        BindFunction<T>(acscFn, "CSC^-1");

        static T acotFn(T v) => MathTrig.Acot(v);

        BindFunction<T>(acotFn, "cot^-1");
        BindFunction<T>(acotFn, "Cot^-1");
        BindFunction<T>(acotFn, "COT^-1");

        static T asinhFn(T v) => MathTrig.Asinh(v);

        BindFunction<T>(asinhFn, "sinh^-1");
        BindFunction<T>(asinhFn, "Sinh^-1");
        BindFunction<T>(asinhFn, "SINH^-1");

        static T acoshFn(T v) => MathTrig.Acosh(v);

        BindFunction<T>(acoshFn, "cosh^-1");
        BindFunction<T>(acoshFn, "Cosh^-1");
        BindFunction<T>(acoshFn, "COSH^-1");

        static T atanhFn(T v) => MathTrig.Atanh(v);

        BindFunction<T>(atanhFn, "tanh^-1");
        BindFunction<T>(atanhFn, "Tanh^-1");
        BindFunction<T>(atanhFn, "TANH^-1");

        static T asechFn(T v) => MathTrig.Asech(v);

        BindFunction<T>(asechFn, "sech^-1");
        BindFunction<T>(asechFn, "Sech^-1");
        BindFunction<T>(asechFn, "SECH^-1");

        static T acschFn(T v) => MathTrig.Acsch(v);

        BindFunction<T>(acschFn, "csch^-1");
        BindFunction<T>(acschFn, "Csch^-1");
        BindFunction<T>(acschFn, "CSCH^-1");

        static T acothFn(T v) => MathTrig.Acoth(v);

        BindFunction<T>(acothFn, "coth^-1");
        BindFunction<T>(acothFn, "Coth^-1");
        BindFunction<T>(acothFn, "COTH^-1");

        BindFunction<T>(asinFn, "arcsin");
        BindFunction<T>(asinFn, "Arcsin");
        BindFunction<T>(asinFn, "ARCSIN");
        BindFunction<T>(acosFn, "arccos");
        BindFunction<T>(acosFn, "Arccos");
        BindFunction<T>(acosFn, "ARCCOS");
        BindFunction<T>(atanFn, "arctan");
        BindFunction<T>(atanFn, "Arctan");
        BindFunction<T>(atanFn, "ARCTAN");
        BindFunction<T>(asecFn, "arcsec");
        BindFunction<T>(asecFn, "Arcsec");
        BindFunction<T>(asecFn, "ARCSEC");
        BindFunction<T>(acscFn, "arccsc");
        BindFunction<T>(acscFn, "Arccsc");
        BindFunction<T>(acscFn, "ARCCSC");
        BindFunction<T>(acotFn, "arccot");
        BindFunction<T>(acotFn, "Arccot");
        BindFunction<T>(acotFn, "ARCCOT");

        BindFunction<T>(asinFn, "asin");
        BindFunction<T>(asinFn, "Asin");
        BindFunction<T>(asinFn, "ASIN");
        BindFunction<T>(acosFn, "acos");
        BindFunction<T>(acosFn, "Acos");
        BindFunction<T>(acosFn, "ACOS");
        BindFunction<T>(atanFn, "atan");
        BindFunction<T>(atanFn, "Atan");
        BindFunction<T>(atanFn, "ATAN");
        BindFunction<T>(asecFn, "asec");
        BindFunction<T>(asecFn, "Asec");
        BindFunction<T>(asecFn, "ASEC");
        BindFunction<T>(acscFn, "acsc");
        BindFunction<T>(acscFn, "Acsc");
        BindFunction<T>(acscFn, "ACSC");
        BindFunction<T>(acotFn, "acot");
        BindFunction<T>(acotFn, "Acot");
        BindFunction<T>(acotFn, "ACOT");

        BindFunction<T>(asinhFn, "arsinh");
        BindFunction<T>(asinhFn, "Arsinh");
        BindFunction<T>(asinhFn, "ARSINH");
        BindFunction<T>(acoshFn, "arcosh");
        BindFunction<T>(acoshFn, "Arcosh");
        BindFunction<T>(acoshFn, "ARCOSH");
        BindFunction<T>(atanhFn, "artanh");
        BindFunction<T>(atanhFn, "Artanh");
        BindFunction<T>(atanhFn, "ARTANH");
        BindFunction<T>(asechFn, "arsech");
        BindFunction<T>(asechFn, "Arsech");
        BindFunction<T>(asechFn, "ARSECH");
        BindFunction<T>(acschFn, "arcsch");
        BindFunction<T>(acschFn, "Arcsch");
        BindFunction<T>(acschFn, "ARCSCH");
        BindFunction<T>(acothFn, "arcoth");
        BindFunction<T>(acothFn, "Arcoth");
        BindFunction<T>(acothFn, "ARCOTH");

        BindFunction<T>(asinhFn, "asinh");
        BindFunction<T>(asinhFn, "Asinh");
        BindFunction<T>(asinhFn, "ASINH");
        BindFunction<T>(acoshFn, "acosh");
        BindFunction<T>(acoshFn, "Acosh");
        BindFunction<T>(acoshFn, "ACOSH");
        BindFunction<T>(atanhFn, "atanh");
        BindFunction<T>(atanhFn, "Atanh");
        BindFunction<T>(atanhFn, "ATANH");
        BindFunction<T>(asechFn, "asech");
        BindFunction<T>(asechFn, "Asech");
        BindFunction<T>(asechFn, "ASECH");
        BindFunction<T>(acschFn, "acsch");
        BindFunction<T>(acschFn, "Acsch");
        BindFunction<T>(acschFn, "ACSCH");
        BindFunction<T>(acothFn, "acoth");
        BindFunction<T>(acothFn, "Acoth");
        BindFunction<T>(acothFn, "ACOTH");

        #endregion
    }

    private static T Factorial(T n)
    {
        if (T.IsNegative(n))
            throw new ArgumentException($"Negative number {n} isn't allowed by the factorial function.");

        if (!T.IsInteger(n))
            throw new ArgumentException($"Not integer number {n} isn't supported by the factorial function.");

        var result = T.One;
        for (var i = n; i > T.One; i -= T.One)
            result *= i;

        return result;
    }
}
