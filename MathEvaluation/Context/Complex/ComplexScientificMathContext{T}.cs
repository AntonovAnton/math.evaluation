#if NET11_0_OR_GREATER
using MathTrigonometric;

#pragma warning disable IDE0130 // Namespace does not match folder structure
// ReSharper disable once CheckNamespace
namespace MathEvaluation.Context;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
///     The scientific math context for generic <see cref="Complex{T}">complex</see> numbers (.NET 11+), such as
///     <c>Complex&lt;double&gt;</c>, <c>Complex&lt;float&gt;</c>, <c>Complex&lt;Half&gt;</c>, <c>Complex&lt;BFloat16&gt;</c>,
///     or the IEEE 754 decimal <c>Complex&lt;Decimal32&gt;</c>, <c>Complex&lt;Decimal64&gt;</c>, <c>Complex&lt;Decimal128&gt;</c>.
///     It supports all trigonometric functions, logarithms, other scientific math functions, and constants,
///     and it calculates them in the precision of the <typeparamref name="T" /> components instead of <see cref="double" />.
///     For a complete list of features and supported functions, please refer to the documentation at
///     <see href="https://github.com/AntonovAnton/math.evaluation" />.
/// </summary>
/// <typeparam name="T">The type of the real and imaginary components.</typeparam>
/// <seealso cref="MathEvaluation.Context.ComplexScientificMathContext" />
public class ComplexScientificMathContext<T> : ScientificMathContext
    where T : struct, IFloatingPointIeee754<T>, IMinMaxValue<T>
{
    /// <summary>Initializes a new instance of the <see cref="ComplexScientificMathContext{T}" /> class.</summary>
    public ComplexScientificMathContext()
    {
        #region constants

        // the constants are bound in the precision of T, the base context binds them as double
        var pi = new Complex<T>(T.Pi, T.Zero);
        BindConstant(pi, 'π');
        BindConstant(pi, "pi");
        BindConstant(pi, "Pi");
        BindConstant(pi, "PI");
        BindConstant(new Complex<T>(T.E, T.Zero), 'e');
        BindConstant(new Complex<T>(T.Tau, T.Zero), 'τ');
        BindConstant(new Complex<T>(T.PositiveInfinity, T.Zero), '∞'); //infinity symbol

        #endregion

        static Complex<T> powerFn(Complex<T> v, Complex<T> power) => Complex<T>.Pow(v, power);
        BindFunction<Complex<T>>(powerFn, "pow");
        BindFunction<Complex<T>>(powerFn, "Pow");
        BindFunction<Complex<T>>(powerFn, "POW");

        #region operators

        // the C# compatible operators of the base context aren't defined for Complex<T>, so they are bound in the precision of T
        BindOperandsOperator<Complex<T>>(powerFn, '^', (int)EvalPrecedence.Exponentiation);

        static Complex<T> modFn(Complex<T> left, Complex<T> right) => new(GetReal(left) % GetReal(right), T.Zero);

        BindOperator<Complex<T>>(modFn, "mod");
        BindOperator<Complex<T>>(modFn, "Mod");
        BindOperator<Complex<T>>(modFn, "MOD");
        BindOperator<Complex<T>>(modFn, "modulo");
        BindOperator<Complex<T>>(modFn, "Modulo");
        BindOperator<Complex<T>>(modFn, "MODULO");

        static Complex<T> greaterThanFn(Complex<T> left, Complex<T> right) => GetReal(left) > GetReal(right) ? Complex<T>.One : default;

        BindOperator<Complex<T>>(greaterThanFn, '>', (int)EvalPrecedence.RelationalOperator);

        static Complex<T> lessThanFn(Complex<T> left, Complex<T> right) => GetReal(left) < GetReal(right) ? Complex<T>.One : default;

        BindOperator<Complex<T>>(lessThanFn, '<', (int)EvalPrecedence.RelationalOperator);

        static Complex<T> greaterThanOrEqualFn(Complex<T> left, Complex<T> right) => GetReal(left) >= GetReal(right) ? Complex<T>.One : default;

        BindOperator<Complex<T>>(greaterThanOrEqualFn, '≥', (int)EvalPrecedence.RelationalOperator);
        BindOperator<Complex<T>>(greaterThanOrEqualFn, '⪰', (int)EvalPrecedence.RelationalOperator);

        static Complex<T> lessThanOrEqualFn(Complex<T> left, Complex<T> right) => GetReal(left) <= GetReal(right) ? Complex<T>.One : default;

        BindOperator<Complex<T>>(lessThanOrEqualFn, '≤', (int)EvalPrecedence.RelationalOperator);
        BindOperator<Complex<T>>(lessThanOrEqualFn, '⪯', (int)EvalPrecedence.RelationalOperator);

        #endregion

        static Complex<T> absFn(Complex<T> v) => new(Complex<T>.Abs(v), T.Zero);

        BindFunction<Complex<T>>(absFn, '|', '|');
        BindFunction<Complex<T>>(absFn, "abs");
        BindFunction<Complex<T>>(absFn, "Abs");
        BindFunction<Complex<T>>(absFn, "ABS");

        static Complex<T> sqrtFn(Complex<T> v) => Complex<T>.Sqrt(v);

        BindFunction<Complex<T>>(sqrtFn, '\u221a'); //square root symbol
        BindFunction<Complex<T>>(sqrtFn, "sqrt");
        BindFunction<Complex<T>>(sqrtFn, "Sqrt");
        BindFunction<Complex<T>>(sqrtFn, "SQRT");

        static Complex<T> cubeFn(Complex<T> v) => Complex<T>.Pow(v, T.One / T.CreateTruncating(3));

        BindFunction<Complex<T>>(cubeFn, '\u221b'); //cube root symbol
        BindFunction<Complex<T>>(cubeFn, "cbrt");
        BindFunction<Complex<T>>(cubeFn, "Cbrt");
        BindFunction<Complex<T>>(cubeFn, "CBRT");

        static Complex<T> fRootFn(Complex<T> v) => Complex<T>.Pow(v, T.One / T.CreateTruncating(4));

        BindFunction<Complex<T>>(fRootFn, '\u221c'); //fourth root symbol

        static Complex<T> logFn(Complex<T> v) => Complex<T>.Log(v);

        BindFunction<Complex<T>>(logFn, "ln");
        BindFunction<Complex<T>>(logFn, "Ln");
        BindFunction<Complex<T>>(logFn, "LN");

        static Complex<T> log10Fn(Complex<T> v) => Complex<T>.Log10(v);

        BindFunction<Complex<T>>(log10Fn, "log");
        BindFunction<Complex<T>>(log10Fn, "Log");
        BindFunction<Complex<T>>(log10Fn, "LOG");

        static Complex<T> expFn(Complex<T> v) => Complex<T>.Exp(v);

        BindFunction<Complex<T>>(expFn, "exp");
        BindFunction<Complex<T>>(expFn, "Exp");
        BindFunction<Complex<T>>(expFn, "EXP");

        #region boolean logic

        static Complex<T> equalToFn(Complex<T> left, Complex<T> right) => left == right ? Complex<T>.One : default;

        BindOperator<Complex<T>>(equalToFn, '↔', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator<Complex<T>>(equalToFn, '⇔', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator<Complex<T>>(equalToFn, '≡', (int)EvalPrecedence.Equivalence);

        static Complex<T> notEqualToFn(Complex<T> left, Complex<T> right) => left != right ? Complex<T>.One : default;

        BindOperator<Complex<T>>(notEqualToFn, '↮', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator<Complex<T>>(notEqualToFn, '⇎', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator<Complex<T>>(notEqualToFn, '≢', (int)EvalPrecedence.Equivalence);

        static Complex<T> implicationFn(Complex<T> left, Complex<T> right) => left == default || right != default ? Complex<T>.One : default;

        BindOperator<Complex<T>>(implicationFn, '→', (int)EvalPrecedence.LogicalImplication);
        BindOperator<Complex<T>>(implicationFn, '⇒', (int)EvalPrecedence.LogicalImplication);

        static Complex<T> reverseImplicationFn(Complex<T> left, Complex<T> right) => left != default || right == default ? Complex<T>.One : default;

        BindOperator<Complex<T>>(reverseImplicationFn, '←', (int)EvalPrecedence.LogicalImplication);
        BindOperator<Complex<T>>(reverseImplicationFn, '⟸', (int)EvalPrecedence.LogicalImplication);

        #endregion

        #region trigonometric functions

        static Complex<T> sinFn(Complex<T> v) => MathTrig.Sin(v);

        BindFunction<Complex<T>>(sinFn, "sin");
        BindFunction<Complex<T>>(sinFn, "Sin");
        BindFunction<Complex<T>>(sinFn, "SIN");

        static Complex<T> cosFn(Complex<T> v) => MathTrig.Cos(v);

        BindFunction<Complex<T>>(cosFn, "cos");
        BindFunction<Complex<T>>(cosFn, "Cos");
        BindFunction<Complex<T>>(cosFn, "COS");

        static Complex<T> tanFn(Complex<T> v) => MathTrig.Tan(v);

        BindFunction<Complex<T>>(tanFn, "tan");
        BindFunction<Complex<T>>(tanFn, "Tan");
        BindFunction<Complex<T>>(tanFn, "TAN");

        static Complex<T> secFn(Complex<T> v) => MathTrig.Sec(v);

        BindFunction<Complex<T>>(secFn, "sec");
        BindFunction<Complex<T>>(secFn, "Sec");
        BindFunction<Complex<T>>(secFn, "SEC");

        static Complex<T> cscFn(Complex<T> v) => MathTrig.Csc(v);

        BindFunction<Complex<T>>(cscFn, "csc");
        BindFunction<Complex<T>>(cscFn, "Csc");
        BindFunction<Complex<T>>(cscFn, "CSC");

        static Complex<T> cotFn(Complex<T> v) => MathTrig.Cot(v);

        BindFunction<Complex<T>>(cotFn, "cot");
        BindFunction<Complex<T>>(cotFn, "Cot");
        BindFunction<Complex<T>>(cotFn, "COT");

        static Complex<T> sinhFn(Complex<T> v) => MathTrig.Sinh(v);

        BindFunction<Complex<T>>(sinhFn, "sinh");
        BindFunction<Complex<T>>(sinhFn, "Sinh");
        BindFunction<Complex<T>>(sinhFn, "SINH");

        static Complex<T> coshFn(Complex<T> v) => MathTrig.Cosh(v);

        BindFunction<Complex<T>>(coshFn, "cosh");
        BindFunction<Complex<T>>(coshFn, "Cosh");
        BindFunction<Complex<T>>(coshFn, "COSH");

        static Complex<T> tanhFn(Complex<T> v) => MathTrig.Tanh(v);

        BindFunction<Complex<T>>(tanhFn, "tanh");
        BindFunction<Complex<T>>(tanhFn, "Tanh");
        BindFunction<Complex<T>>(tanhFn, "TANH");

        static Complex<T> sechFn(Complex<T> v) => MathTrig.Sech(v);

        BindFunction<Complex<T>>(sechFn, "sech");
        BindFunction<Complex<T>>(sechFn, "Sech");
        BindFunction<Complex<T>>(sechFn, "SECH");

        static Complex<T> cschFn(Complex<T> v) => MathTrig.Csch(v);

        BindFunction<Complex<T>>(cschFn, "csch");
        BindFunction<Complex<T>>(cschFn, "Csch");
        BindFunction<Complex<T>>(cschFn, "CSCH");

        static Complex<T> cothFn(Complex<T> v) => MathTrig.Coth(v);

        BindFunction<Complex<T>>(cothFn, "coth");
        BindFunction<Complex<T>>(cothFn, "Coth");
        BindFunction<Complex<T>>(cothFn, "COTH");

        static Complex<T> asinFn(Complex<T> v) => MathTrig.Asin(v);

        BindFunction<Complex<T>>(asinFn, "sin^-1");
        BindFunction<Complex<T>>(asinFn, "Sin^-1");
        BindFunction<Complex<T>>(asinFn, "SIN^-1");

        static Complex<T> acosFn(Complex<T> v) => MathTrig.Acos(v);

        BindFunction<Complex<T>>(acosFn, "cos^-1");
        BindFunction<Complex<T>>(acosFn, "Cos^-1");
        BindFunction<Complex<T>>(acosFn, "COS^-1");

        static Complex<T> atanFn(Complex<T> v) => MathTrig.Atan(v);

        BindFunction<Complex<T>>(atanFn, "tan^-1");
        BindFunction<Complex<T>>(atanFn, "Tan^-1");
        BindFunction<Complex<T>>(atanFn, "TAN^-1");

        static Complex<T> asecFn(Complex<T> v) => MathTrig.Asec(v);

        BindFunction<Complex<T>>(asecFn, "sec^-1");
        BindFunction<Complex<T>>(asecFn, "Sec^-1");
        BindFunction<Complex<T>>(asecFn, "SEC^-1");

        static Complex<T> acscFn(Complex<T> v) => MathTrig.Acsc(v);

        BindFunction<Complex<T>>(acscFn, "csc^-1");
        BindFunction<Complex<T>>(acscFn, "Csc^-1");
        BindFunction<Complex<T>>(acscFn, "CSC^-1");

        static Complex<T> acotFn(Complex<T> v) => MathTrig.Acot(v);

        BindFunction<Complex<T>>(acotFn, "cot^-1");
        BindFunction<Complex<T>>(acotFn, "Cot^-1");
        BindFunction<Complex<T>>(acotFn, "COT^-1");

        static Complex<T> asinhFn(Complex<T> v) => MathTrig.Asinh(v);

        BindFunction<Complex<T>>(asinhFn, "sinh^-1");
        BindFunction<Complex<T>>(asinhFn, "Sinh^-1");
        BindFunction<Complex<T>>(asinhFn, "SINH^-1");

        static Complex<T> acoshFn(Complex<T> v) => MathTrig.Acosh(v);

        BindFunction<Complex<T>>(acoshFn, "cosh^-1");
        BindFunction<Complex<T>>(acoshFn, "Cosh^-1");
        BindFunction<Complex<T>>(acoshFn, "COSH^-1");

        static Complex<T> atanhFn(Complex<T> v) => MathTrig.Atanh(v);

        BindFunction<Complex<T>>(atanhFn, "tanh^-1");
        BindFunction<Complex<T>>(atanhFn, "Tanh^-1");
        BindFunction<Complex<T>>(atanhFn, "TANH^-1");

        static Complex<T> asechFn(Complex<T> v) => MathTrig.Asech(v);

        BindFunction<Complex<T>>(asechFn, "sech^-1");
        BindFunction<Complex<T>>(asechFn, "Sech^-1");
        BindFunction<Complex<T>>(asechFn, "SECH^-1");

        static Complex<T> acschFn(Complex<T> v) => MathTrig.Acsch(v);

        BindFunction<Complex<T>>(acschFn, "csch^-1");
        BindFunction<Complex<T>>(acschFn, "Csch^-1");
        BindFunction<Complex<T>>(acschFn, "CSCH^-1");

        static Complex<T> acothFn(Complex<T> v) => MathTrig.Acoth(v);

        BindFunction<Complex<T>>(acothFn, "coth^-1");
        BindFunction<Complex<T>>(acothFn, "Coth^-1");
        BindFunction<Complex<T>>(acothFn, "COTH^-1");

        BindFunction<Complex<T>>(asinFn, "arcsin");
        BindFunction<Complex<T>>(asinFn, "Arcsin");
        BindFunction<Complex<T>>(asinFn, "ARCSIN");
        BindFunction<Complex<T>>(acosFn, "arccos");
        BindFunction<Complex<T>>(acosFn, "Arccos");
        BindFunction<Complex<T>>(acosFn, "ARCCOS");
        BindFunction<Complex<T>>(atanFn, "arctan");
        BindFunction<Complex<T>>(atanFn, "Arctan");
        BindFunction<Complex<T>>(atanFn, "ARCTAN");
        BindFunction<Complex<T>>(asecFn, "arcsec");
        BindFunction<Complex<T>>(asecFn, "Arcsec");
        BindFunction<Complex<T>>(asecFn, "ARCSEC");
        BindFunction<Complex<T>>(acscFn, "arccsc");
        BindFunction<Complex<T>>(acscFn, "Arccsc");
        BindFunction<Complex<T>>(acscFn, "ARCCSC");
        BindFunction<Complex<T>>(acotFn, "arccot");
        BindFunction<Complex<T>>(acotFn, "Arccot");
        BindFunction<Complex<T>>(acotFn, "ARCCOT");

        BindFunction<Complex<T>>(asinFn, "asin");
        BindFunction<Complex<T>>(asinFn, "Asin");
        BindFunction<Complex<T>>(asinFn, "ASIN");
        BindFunction<Complex<T>>(acosFn, "acos");
        BindFunction<Complex<T>>(acosFn, "Acos");
        BindFunction<Complex<T>>(acosFn, "ACOS");
        BindFunction<Complex<T>>(atanFn, "atan");
        BindFunction<Complex<T>>(atanFn, "Atan");
        BindFunction<Complex<T>>(atanFn, "ATAN");
        BindFunction<Complex<T>>(asecFn, "asec");
        BindFunction<Complex<T>>(asecFn, "Asec");
        BindFunction<Complex<T>>(asecFn, "ASEC");
        BindFunction<Complex<T>>(acscFn, "acsc");
        BindFunction<Complex<T>>(acscFn, "Acsc");
        BindFunction<Complex<T>>(acscFn, "ACSC");
        BindFunction<Complex<T>>(acotFn, "acot");
        BindFunction<Complex<T>>(acotFn, "Acot");
        BindFunction<Complex<T>>(acotFn, "ACOT");

        BindFunction<Complex<T>>(asinhFn, "arsinh");
        BindFunction<Complex<T>>(asinhFn, "Arsinh");
        BindFunction<Complex<T>>(asinhFn, "ARSINH");
        BindFunction<Complex<T>>(acoshFn, "arcosh");
        BindFunction<Complex<T>>(acoshFn, "Arcosh");
        BindFunction<Complex<T>>(acoshFn, "ARCOSH");
        BindFunction<Complex<T>>(atanhFn, "artanh");
        BindFunction<Complex<T>>(atanhFn, "Artanh");
        BindFunction<Complex<T>>(atanhFn, "ARTANH");
        BindFunction<Complex<T>>(asechFn, "arsech");
        BindFunction<Complex<T>>(asechFn, "Arsech");
        BindFunction<Complex<T>>(asechFn, "ARSECH");
        BindFunction<Complex<T>>(acschFn, "arcsch");
        BindFunction<Complex<T>>(acschFn, "Arcsch");
        BindFunction<Complex<T>>(acschFn, "ARCSCH");
        BindFunction<Complex<T>>(acothFn, "arcoth");
        BindFunction<Complex<T>>(acothFn, "Arcoth");
        BindFunction<Complex<T>>(acothFn, "ARCOTH");


        BindFunction<Complex<T>>(asinhFn, "asinh");
        BindFunction<Complex<T>>(asinhFn, "Asinh");
        BindFunction<Complex<T>>(asinhFn, "ASINH");
        BindFunction<Complex<T>>(acoshFn, "acosh");
        BindFunction<Complex<T>>(acoshFn, "Acosh");
        BindFunction<Complex<T>>(acoshFn, "ACOSH");
        BindFunction<Complex<T>>(atanhFn, "atanh");
        BindFunction<Complex<T>>(atanhFn, "Atanh");
        BindFunction<Complex<T>>(atanhFn, "ATANH");
        BindFunction<Complex<T>>(asechFn, "asech");
        BindFunction<Complex<T>>(asechFn, "Asech");
        BindFunction<Complex<T>>(asechFn, "ASECH");
        BindFunction<Complex<T>>(acschFn, "acsch");
        BindFunction<Complex<T>>(acschFn, "Acsch");
        BindFunction<Complex<T>>(acschFn, "ACSCH");
        BindFunction<Complex<T>>(acothFn, "acoth");
        BindFunction<Complex<T>>(acothFn, "Acoth");
        BindFunction<Complex<T>>(acothFn, "ACOTH");

        #endregion
    }

    /// <summary>Gets the real part, the imaginary part has to be zero.</summary>
    /// <exception cref="InvalidCastException">The imaginary part isn't zero.</exception>
    private static T GetReal(Complex<T> value)
    {
        if (value.Imaginary != T.Zero)
            throw new InvalidCastException($"Cannot convert the Complex<{typeof(T).Name}> number to a {typeof(T).Name}, value = {value}.");

        return value.Real;
    }
}
#endif
