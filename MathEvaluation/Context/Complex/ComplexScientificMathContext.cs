using MathEvaluation.Entities;
using MathTrigonometric;
using System.Numerics;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MathEvaluation.Context;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// The scientific math context for <see cref="Complex">complex</see> numbers supports all trigonometric functions, logarithms, other scientific math functions, and constants. 
/// For a complete list of features and supported functions, please refer to the documentation at <see href="https://github.com/AntonovAnton/math.evaluation"/>.
/// </summary>
/// <seealso cref="MathEvaluation.Context.MathContext" />
public class ComplexScientificMathContext : ScientificMathContext
{
    /// <summary>Initializes a new instance of the <see cref="ScientificMathContext" /> class.</summary>
    public ComplexScientificMathContext()
        : base()
    {
        static Complex absFn(Complex v) => Complex.Abs(v);
        BindFunction(absFn, '|', '|');
        BindFunction(absFn, "abs");
        BindFunction(absFn, "Abs");
        BindFunction(absFn, "ABS");

        static Complex sqrtFn(Complex v) => Complex.Sqrt(v);
        BindFunction(sqrtFn, '\u221a'); //square root symbol

        static Complex cubeFn(Complex v) => Complex.Pow(v, 1/3d);
        BindFunction(cubeFn, '\u221b'); //cube root symbol

        static Complex fRootFn(Complex v) => Complex.Pow(v, 0.25d);
        BindFunction(fRootFn, '\u221c'); //fourth root symbol

        static Complex logFn(Complex v) => Complex.Log(v);
        BindFunction(logFn, "ln");
        BindFunction(logFn, "Ln");
        BindFunction(logFn, "LN");

        static Complex log10Fn(Complex v) => Complex.Log10(v);
        BindFunction(log10Fn, "log");
        BindFunction(log10Fn, "Log");
        BindFunction(log10Fn, "LOG");

        #region boolean logic

        static Complex equalToFn(Complex left, Complex right) => left == right ? Complex.One : default;
        BindOperator(equalToFn, '↔', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator(equalToFn, '⇔', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator(equalToFn, '≡', (int)EvalPrecedence.Equivalence);

        static Complex notEqualToFn(Complex left, Complex right) => left != right ? Complex.One : default;
        BindOperator(notEqualToFn, '↮', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator(notEqualToFn, '⇎', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator(notEqualToFn, '≢', (int)EvalPrecedence.Equivalence);

        static Complex implicationFn(Complex left, Complex right) => left == default || right != default ? Complex.One : default;
        BindOperator(implicationFn, '→', (int)EvalPrecedence.LogicalImplication);
        BindOperator(implicationFn, '⇒', (int)EvalPrecedence.LogicalImplication);

        static Complex reverseImplicationFn(Complex left, Complex right) => left != default || right == default ? Complex.One : default;
        BindOperator(reverseImplicationFn, '←', (int)EvalPrecedence.LogicalImplication);
        BindOperator(reverseImplicationFn, '⟸', (int)EvalPrecedence.LogicalImplication);

        #endregion

        #region trigonometric functions

        static Complex sinFn(Complex v) => MathTrig.Sin(v);
        BindFunction(sinFn, "sin");
        BindFunction(sinFn, "Sin");
        BindFunction(sinFn, "SIN");

        static Complex cosFn(Complex v) => MathTrig.Cos(v);
        BindFunction(cosFn, "cos");
        BindFunction(cosFn, "Cos");
        BindFunction(cosFn, "COS");

        static Complex tanFn(Complex v) => MathTrig.Tan(v);
        BindFunction(tanFn, "tan");
        BindFunction(tanFn, "Tan");
        BindFunction(tanFn, "TAN");

        static Complex secFn(Complex v) => MathTrig.Sec(v);
        BindFunction(secFn, "sec");
        BindFunction(secFn, "Sec");
        BindFunction(secFn, "SEC");

        static Complex cscFn(Complex v) => MathTrig.Csc(v);
        BindFunction(cscFn, "csc");
        BindFunction(cscFn, "Csc");
        BindFunction(cscFn, "CSC");

        static Complex cotFn(Complex v) => MathTrig.Cot(v);
        BindFunction(cotFn, "cot");
        BindFunction(cotFn, "Cot");
        BindFunction(cotFn, "COT");

        static Complex sinhFn(Complex v) => MathTrig.Sinh(v);
        BindFunction(sinhFn, "sinh");
        BindFunction(sinhFn, "Sinh");
        BindFunction(sinhFn, "SINH");

        static Complex coshFn(Complex v) => MathTrig.Cosh(v);
        BindFunction(coshFn, "cosh");
        BindFunction(coshFn, "Cosh");
        BindFunction(coshFn, "COSH");

        static Complex tanhFn(Complex v) => MathTrig.Tanh(v);
        BindFunction(tanhFn, "tanh");
        BindFunction(tanhFn, "Tanh");
        BindFunction(tanhFn, "TANH");

        static Complex sechFn(Complex v) => MathTrig.Sech(v);
        BindFunction(sechFn, "sech");
        BindFunction(sechFn, "Sech");
        BindFunction(sechFn, "SECH");

        static Complex cschFn(Complex v) => MathTrig.Csch(v);
        BindFunction(cschFn, "csch");
        BindFunction(cschFn, "Csch");
        BindFunction(cschFn, "CSCH");

        static Complex cothFn(Complex v) => MathTrig.Coth(v);
        BindFunction(cothFn, "coth");
        BindFunction(cothFn, "Coth");
        BindFunction(cothFn, "COTH");

        static Complex asinFn(Complex v) => MathTrig.Asin(v);
        BindFunction(asinFn, "sin^-1");
        BindFunction(asinFn, "Sin^-1");
        BindFunction(asinFn, "SIN^-1");

        static Complex acosFn(Complex v) => MathTrig.Acos(v);
        BindFunction(acosFn, "cos^-1");
        BindFunction(acosFn, "Cos^-1");
        BindFunction(acosFn, "COS^-1");

        static Complex atanFn(Complex v) => MathTrig.Atan(v);
        BindFunction(atanFn, "tan^-1");
        BindFunction(atanFn, "Tan^-1");
        BindFunction(atanFn, "TAN^-1");

        static Complex asecFn(Complex v) => MathTrig.Asec(v);
        BindFunction(asecFn, "sec^-1");
        BindFunction(asecFn, "Sec^-1");
        BindFunction(asecFn, "SEC^-1");

        static Complex acscFn(Complex v) => MathTrig.Acsc(v);
        BindFunction(acscFn, "csc^-1");
        BindFunction(acscFn, "Csc^-1");
        BindFunction(acscFn, "CSC^-1");

        static Complex acotFn(Complex v) => MathTrig.Acot(v);
        BindFunction(acotFn, "cot^-1");
        BindFunction(acotFn, "Cot^-1");
        BindFunction(acotFn, "COT^-1");

        static Complex asinhFn(Complex v) => MathTrig.Asinh(v);
        BindFunction(asinhFn, "sinh^-1");
        BindFunction(asinhFn, "Sinh^-1");
        BindFunction(asinhFn, "SINH^-1");

        static Complex acoshFn(Complex v) => MathTrig.Acosh(v);
        BindFunction(acoshFn, "cosh^-1");
        BindFunction(acoshFn, "Cosh^-1");
        BindFunction(acoshFn, "COSH^-1");

        static Complex atanhFn(Complex v) => MathTrig.Atanh(v);
        BindFunction(atanhFn, "tanh^-1");
        BindFunction(atanhFn, "Tanh^-1");
        BindFunction(atanhFn, "TANH^-1");

        static Complex asechFn(Complex v) => MathTrig.Asech(v);
        BindFunction(asechFn, "sech^-1");
        BindFunction(asechFn, "Sech^-1");
        BindFunction(asechFn, "SECH^-1");

        static Complex acschFn(Complex v) => MathTrig.Acsch(v);
        BindFunction(acschFn, "csch^-1");
        BindFunction(acschFn, "Csch^-1");
        BindFunction(acschFn, "CSCH^-1");

        static Complex acothFn(Complex v) => MathTrig.Acoth(v);
        BindFunction(acothFn, "coth^-1");
        BindFunction(acothFn, "Coth^-1");
        BindFunction(acothFn, "COTH^-1");

        BindFunction(asinFn, "arcsin");
        BindFunction(asinFn, "Arcsin");
        BindFunction(asinFn, "ARCSIN");
        BindFunction(acosFn, "arccos");
        BindFunction(acosFn, "Arccos");
        BindFunction(acosFn, "ARCCOS");
        BindFunction(atanFn, "arctan");
        BindFunction(atanFn, "Arctan");
        BindFunction(atanFn, "ARCTAN");
        BindFunction(asecFn, "arcsec");
        BindFunction(asecFn, "Arcsec");
        BindFunction(asecFn, "ARCSEC");
        BindFunction(acscFn, "arccsc");
        BindFunction(acscFn, "Arccsc");
        BindFunction(acscFn, "ARCCSC");
        BindFunction(acotFn, "arccot");
        BindFunction(acotFn, "Arccot");
        BindFunction(acotFn, "ARCCOT");

        BindFunction(asinhFn, "arsinh");
        BindFunction(asinhFn, "Arsinh");
        BindFunction(asinhFn, "ARSINH");
        BindFunction(acoshFn, "arcosh");
        BindFunction(acoshFn, "Arcosh");
        BindFunction(acoshFn, "ARCOSH");
        BindFunction(atanhFn, "artanh");
        BindFunction(atanhFn, "Artanh");
        BindFunction(atanhFn, "ARTANH");
        BindFunction(asechFn, "arsech");
        BindFunction(asechFn, "Arsech");
        BindFunction(asechFn, "ARSECH");
        BindFunction(acschFn, "arcsch");
        BindFunction(acschFn, "Arcsch");
        BindFunction(acschFn, "ARCSCH");
        BindFunction(acothFn, "arcoth");
        BindFunction(acothFn, "Arcoth");
        BindFunction(acothFn, "ARCOTH");

        #endregion
    }
}
