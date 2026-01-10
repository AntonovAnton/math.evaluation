using MathEvaluation.Entities;
using MathTrigonometric;
using System.Numerics;

#pragma warning disable IDE0130 // Namespace does not match folder structure
// ReSharper disable once CheckNamespace
namespace MathEvaluation.Context;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
///     The scientific math context for <see cref="Complex">complex</see> numbers supports all trigonometric functions,
///     logarithms, other scientific math functions, and constants.
///     For a complete list of features and supported functions, please refer to the documentation at
///     <see href="https://github.com/AntonovAnton/math.evaluation" />.
/// </summary>
/// <seealso cref="MathEvaluation.Context.MathContext" />
public class ComplexScientificMathContext : ScientificMathContext
{
    /// <summary>Initializes a new instance of the <see cref="ScientificMathContext" /> class.</summary>
    public ComplexScientificMathContext()
    {
        static Complex powerFn(Complex v, Complex power) => Complex.Pow(v, power);
        BindFunction<Complex>(powerFn, "pow");
        BindFunction<Complex>(powerFn, "Pow");
        BindFunction<Complex>(powerFn, "POW");

        static Complex absFn(Complex v) => Complex.Abs(v);

        BindFunction<Complex>(absFn, '|', '|');
        BindFunction<Complex>(absFn, "abs");
        BindFunction<Complex>(absFn, "Abs");
        BindFunction<Complex>(absFn, "ABS");

        static Complex sqrtFn(Complex v) => Complex.Sqrt(v);

        BindFunction<Complex>(sqrtFn, '\u221a'); //square root symbol
        BindFunction<Complex>(sqrtFn, "sqrt");
        BindFunction<Complex>(sqrtFn, "Sqrt");
        BindFunction<Complex>(sqrtFn, "SQRT");

        static Complex cubeFn(Complex v) => Complex.Pow(v, 1 / 3d);

        BindFunction<Complex>(cubeFn, '\u221b'); //cube root symbol
        BindFunction<Complex>(cubeFn, "cbrt");
        BindFunction<Complex>(cubeFn, "Cbrt");
        BindFunction<Complex>(cubeFn, "CBRT");

        static Complex fRootFn(Complex v) => Complex.Pow(v, 0.25d);

        BindFunction<Complex>(fRootFn, '\u221c'); //fourth root symbol

        static Complex logFn(Complex v) => Complex.Log(v);

        BindFunction<Complex>(logFn, "ln");
        BindFunction<Complex>(logFn, "Ln");
        BindFunction<Complex>(logFn, "LN");

        static Complex log10Fn(Complex v) => Complex.Log10(v);

        BindFunction<Complex>(log10Fn, "log");
        BindFunction<Complex>(log10Fn, "Log");
        BindFunction<Complex>(log10Fn, "LOG");

        #region boolean logic

        static Complex equalToFn(Complex left, Complex right) => left == right ? Complex.One : default;

        BindOperator<Complex>(equalToFn, '↔', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator<Complex>(equalToFn, '⇔', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator<Complex>(equalToFn, '≡', (int)EvalPrecedence.Equivalence);

        static Complex notEqualToFn(Complex left, Complex right) => left != right ? Complex.One : default;

        BindOperator<Complex>(notEqualToFn, '↮', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator<Complex>(notEqualToFn, '⇎', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator<Complex>(notEqualToFn, '≢', (int)EvalPrecedence.Equivalence);

        static Complex implicationFn(Complex left, Complex right) => left == default || right != default ? Complex.One : default;

        BindOperator<Complex>(implicationFn, '→', (int)EvalPrecedence.LogicalImplication);
        BindOperator<Complex>(implicationFn, '⇒', (int)EvalPrecedence.LogicalImplication);

        static Complex reverseImplicationFn(Complex left, Complex right) => left != default || right == default ? Complex.One : default;

        BindOperator<Complex>(reverseImplicationFn, '←', (int)EvalPrecedence.LogicalImplication);
        BindOperator<Complex>(reverseImplicationFn, '⟸', (int)EvalPrecedence.LogicalImplication);

        #endregion

        #region trigonometric functions

        static Complex sinFn(Complex v) => MathTrig.Sin(v);

        BindFunction<Complex>(sinFn, "sin");
        BindFunction<Complex>(sinFn, "Sin");
        BindFunction<Complex>(sinFn, "SIN");

        static Complex cosFn(Complex v) => MathTrig.Cos(v);

        BindFunction<Complex>(cosFn, "cos");
        BindFunction<Complex>(cosFn, "Cos");
        BindFunction<Complex>(cosFn, "COS");

        static Complex tanFn(Complex v) => MathTrig.Tan(v);

        BindFunction<Complex>(tanFn, "tan");
        BindFunction<Complex>(tanFn, "Tan");
        BindFunction<Complex>(tanFn, "TAN");

        static Complex secFn(Complex v) => MathTrig.Sec(v);

        BindFunction<Complex>(secFn, "sec");
        BindFunction<Complex>(secFn, "Sec");
        BindFunction<Complex>(secFn, "SEC");

        static Complex cscFn(Complex v) => MathTrig.Csc(v);

        BindFunction<Complex>(cscFn, "csc");
        BindFunction<Complex>(cscFn, "Csc");
        BindFunction<Complex>(cscFn, "CSC");

        static Complex cotFn(Complex v) => MathTrig.Cot(v);

        BindFunction<Complex>(cotFn, "cot");
        BindFunction<Complex>(cotFn, "Cot");
        BindFunction<Complex>(cotFn, "COT");

        static Complex sinhFn(Complex v) => MathTrig.Sinh(v);

        BindFunction<Complex>(sinhFn, "sinh");
        BindFunction<Complex>(sinhFn, "Sinh");
        BindFunction<Complex>(sinhFn, "SINH");

        static Complex coshFn(Complex v) => MathTrig.Cosh(v);

        BindFunction<Complex>(coshFn, "cosh");
        BindFunction<Complex>(coshFn, "Cosh");
        BindFunction<Complex>(coshFn, "COSH");

        static Complex tanhFn(Complex v) => MathTrig.Tanh(v);

        BindFunction<Complex>(tanhFn, "tanh");
        BindFunction<Complex>(tanhFn, "Tanh");
        BindFunction<Complex>(tanhFn, "TANH");

        static Complex sechFn(Complex v) => MathTrig.Sech(v);

        BindFunction<Complex>(sechFn, "sech");
        BindFunction<Complex>(sechFn, "Sech");
        BindFunction<Complex>(sechFn, "SECH");

        static Complex cschFn(Complex v) => MathTrig.Csch(v);

        BindFunction<Complex>(cschFn, "csch");
        BindFunction<Complex>(cschFn, "Csch");
        BindFunction<Complex>(cschFn, "CSCH");

        static Complex cothFn(Complex v) => MathTrig.Coth(v);

        BindFunction<Complex>(cothFn, "coth");
        BindFunction<Complex>(cothFn, "Coth");
        BindFunction<Complex>(cothFn, "COTH");

        static Complex asinFn(Complex v) => MathTrig.Asin(v);

        BindFunction<Complex>(asinFn, "sin^-1");
        BindFunction<Complex>(asinFn, "Sin^-1");
        BindFunction<Complex>(asinFn, "SIN^-1");

        static Complex acosFn(Complex v) => MathTrig.Acos(v);

        BindFunction<Complex>(acosFn, "cos^-1");
        BindFunction<Complex>(acosFn, "Cos^-1");
        BindFunction<Complex>(acosFn, "COS^-1");

        static Complex atanFn(Complex v) => MathTrig.Atan(v);

        BindFunction<Complex>(atanFn, "tan^-1");
        BindFunction<Complex>(atanFn, "Tan^-1");
        BindFunction<Complex>(atanFn, "TAN^-1");

        static Complex asecFn(Complex v) => MathTrig.Asec(v);

        BindFunction<Complex>(asecFn, "sec^-1");
        BindFunction<Complex>(asecFn, "Sec^-1");
        BindFunction<Complex>(asecFn, "SEC^-1");

        static Complex acscFn(Complex v) => MathTrig.Acsc(v);

        BindFunction<Complex>(acscFn, "csc^-1");
        BindFunction<Complex>(acscFn, "Csc^-1");
        BindFunction<Complex>(acscFn, "CSC^-1");

        static Complex acotFn(Complex v) => MathTrig.Acot(v);

        BindFunction<Complex>(acotFn, "cot^-1");
        BindFunction<Complex>(acotFn, "Cot^-1");
        BindFunction<Complex>(acotFn, "COT^-1");

        static Complex asinhFn(Complex v) => MathTrig.Asinh(v);

        BindFunction<Complex>(asinhFn, "sinh^-1");
        BindFunction<Complex>(asinhFn, "Sinh^-1");
        BindFunction<Complex>(asinhFn, "SINH^-1");

        static Complex acoshFn(Complex v) => MathTrig.Acosh(v);

        BindFunction<Complex>(acoshFn, "cosh^-1");
        BindFunction<Complex>(acoshFn, "Cosh^-1");
        BindFunction<Complex>(acoshFn, "COSH^-1");

        static Complex atanhFn(Complex v) => MathTrig.Atanh(v);

        BindFunction<Complex>(atanhFn, "tanh^-1");
        BindFunction<Complex>(atanhFn, "Tanh^-1");
        BindFunction<Complex>(atanhFn, "TANH^-1");

        static Complex asechFn(Complex v) => MathTrig.Asech(v);

        BindFunction<Complex>(asechFn, "sech^-1");
        BindFunction<Complex>(asechFn, "Sech^-1");
        BindFunction<Complex>(asechFn, "SECH^-1");

        static Complex acschFn(Complex v) => MathTrig.Acsch(v);

        BindFunction<Complex>(acschFn, "csch^-1");
        BindFunction<Complex>(acschFn, "Csch^-1");
        BindFunction<Complex>(acschFn, "CSCH^-1");

        static Complex acothFn(Complex v) => MathTrig.Acoth(v);

        BindFunction<Complex>(acothFn, "coth^-1");
        BindFunction<Complex>(acothFn, "Coth^-1");
        BindFunction<Complex>(acothFn, "COTH^-1");

        BindFunction<Complex>(asinFn, "arcsin");
        BindFunction<Complex>(asinFn, "Arcsin");
        BindFunction<Complex>(asinFn, "ARCSIN");
        BindFunction<Complex>(acosFn, "arccos");
        BindFunction<Complex>(acosFn, "Arccos");
        BindFunction<Complex>(acosFn, "ARCCOS");
        BindFunction<Complex>(atanFn, "arctan");
        BindFunction<Complex>(atanFn, "Arctan");
        BindFunction<Complex>(atanFn, "ARCTAN");
        BindFunction<Complex>(asecFn, "arcsec");
        BindFunction<Complex>(asecFn, "Arcsec");
        BindFunction<Complex>(asecFn, "ARCSEC");
        BindFunction<Complex>(acscFn, "arccsc");
        BindFunction<Complex>(acscFn, "Arccsc");
        BindFunction<Complex>(acscFn, "ARCCSC");
        BindFunction<Complex>(acotFn, "arccot");
        BindFunction<Complex>(acotFn, "Arccot");
        BindFunction<Complex>(acotFn, "ARCCOT");

        BindFunction<Complex>(asinFn, "asin");
        BindFunction<Complex>(asinFn, "Asin");
        BindFunction<Complex>(asinFn, "ASIN");
        BindFunction<Complex>(acosFn, "acos");
        BindFunction<Complex>(acosFn, "Acos");
        BindFunction<Complex>(acosFn, "ACOS");
        BindFunction<Complex>(atanFn, "atan");
        BindFunction<Complex>(atanFn, "Atan");
        BindFunction<Complex>(atanFn, "ATAN");
        BindFunction<Complex>(asecFn, "asec");
        BindFunction<Complex>(asecFn, "Asec");
        BindFunction<Complex>(asecFn, "ASEC");
        BindFunction<Complex>(acscFn, "acsc");
        BindFunction<Complex>(acscFn, "Acsc");
        BindFunction<Complex>(acscFn, "ACSC");
        BindFunction<Complex>(acotFn, "acot");
        BindFunction<Complex>(acotFn, "Acot");
        BindFunction<Complex>(acotFn, "ACOT");

        BindFunction<Complex>(asinhFn, "arsinh");
        BindFunction<Complex>(asinhFn, "Arsinh");
        BindFunction<Complex>(asinhFn, "ARSINH");
        BindFunction<Complex>(acoshFn, "arcosh");
        BindFunction<Complex>(acoshFn, "Arcosh");
        BindFunction<Complex>(acoshFn, "ARCOSH");
        BindFunction<Complex>(atanhFn, "artanh");
        BindFunction<Complex>(atanhFn, "Artanh");
        BindFunction<Complex>(atanhFn, "ARTANH");
        BindFunction<Complex>(asechFn, "arsech");
        BindFunction<Complex>(asechFn, "Arsech");
        BindFunction<Complex>(asechFn, "ARSECH");
        BindFunction<Complex>(acschFn, "arcsch");
        BindFunction<Complex>(acschFn, "Arcsch");
        BindFunction<Complex>(acschFn, "ARCSCH");
        BindFunction<Complex>(acothFn, "arcoth");
        BindFunction<Complex>(acothFn, "Arcoth");
        BindFunction<Complex>(acothFn, "ARCOTH");


        BindFunction<Complex>(asinhFn, "asinh");
        BindFunction<Complex>(asinhFn, "Asinh");
        BindFunction<Complex>(asinhFn, "ASINH");
        BindFunction<Complex>(acoshFn, "acosh");
        BindFunction<Complex>(acoshFn, "Acosh");
        BindFunction<Complex>(acoshFn, "ACOSH");
        BindFunction<Complex>(atanhFn, "atanh");
        BindFunction<Complex>(atanhFn, "Atanh");
        BindFunction<Complex>(atanhFn, "ATANH");
        BindFunction<Complex>(asechFn, "asech");
        BindFunction<Complex>(asechFn, "Asech");
        BindFunction<Complex>(asechFn, "ASECH");
        BindFunction<Complex>(acschFn, "acsch");
        BindFunction<Complex>(acschFn, "Acsch");
        BindFunction<Complex>(acschFn, "ACSCH");
        BindFunction<Complex>(acothFn, "acoth");
        BindFunction<Complex>(acothFn, "Acoth");
        BindFunction<Complex>(acothFn, "ACOTH");

        #endregion
    }
}