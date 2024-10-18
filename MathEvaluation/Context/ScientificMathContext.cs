using MathEvaluation.Entities;
using MathTrigonometric;
using System;

namespace MathEvaluation.Context;

/// <summary>
///     The base scientific math context supports all trigonometric functions, logarithms, other scientific math functions,
///     and constants.
///     For a complete list of features and supported functions, please refer to the documentation at
///     <see href="https://github.com/AntonovAnton/math.evaluation" />.
/// </summary>
/// <seealso cref="MathEvaluation.Context.MathContext" />
public class ScientificMathContext : MathContext
{
    /// <summary>Initializes a new instance of the <see cref="ScientificMathContext" /> class.</summary>
    public ScientificMathContext()
    {
        BindConstant(Math.PI, 'π');
        BindConstant(Math.PI, "pi");
        BindConstant(Math.PI, "Pi");
        BindConstant(Math.PI, "PI");
        BindConstant(Math.E, 'e');
        BindConstant(Math.PI * 2, 'τ');
        BindConstant(double.PositiveInfinity, '\u221e'); //infinity symbol

        BindOperator("mod", OperatorType.Modulo);
        BindOperator("Mod", OperatorType.Modulo);
        BindOperator("MOD", OperatorType.Modulo);
        BindOperator("modulo", OperatorType.Modulo);
        BindOperator("Modulo", OperatorType.Modulo);
        BindOperator("MODULO", OperatorType.Modulo);

        BindOperator('÷', OperatorType.Divide);

        static double floorDivisionFn(double left, double right) => Math.Floor(left / right);

        BindOperator(floorDivisionFn, "//");

        BindOperator('×', OperatorType.Multiply);
        BindOperator('·', OperatorType.Multiply);

        BindOperator('^', OperatorType.Power);

        static double absFn(double v) => Math.Abs(v);

        BindFunction(absFn, '|', '|');
        BindFunction(absFn, "abs");
        BindFunction(absFn, "Abs");
        BindFunction(absFn, "ABS");

        static double ceilingFn(double v) => Math.Ceiling(v);

        BindFunction(ceilingFn, '⌈', '⌉');

        static double floorFn(double v) => Math.Floor(v);

        BindFunction(floorFn, '⌊', '⌋');

        static double sqrtFn(double v) => Math.Sqrt(v);

        BindFunction(sqrtFn, '\u221a'); //square root symbol

        static double cubeFn(double v) => Math.Pow(v, 1 / 3d);

        BindFunction(cubeFn, '\u221b'); //cube root symbol

        static double fRootFn(double v) => Math.Pow(v, 0.25d);

        BindFunction(fRootFn, '\u221c'); //fourth root symbol

        static double logFn(double v) => Math.Log(v);

        BindFunction(logFn, "ln");
        BindFunction(logFn, "Ln");
        BindFunction(logFn, "LN");

        static double log10Fn(double v) => Math.Log10(v);

        BindFunction(log10Fn, "log");
        BindFunction(log10Fn, "Log");
        BindFunction(log10Fn, "LOG");

        static double factorialFn(double v) => Factorial(v);

        BindOperandOperator(factorialFn, '!', true);

        #region boolean logic

        BindConstant(1d, "true");
        BindConstant(1d, "True");
        BindConstant(1d, "TRUE");
        BindConstant(1d, 'T');
        BindConstant(1d, '⊤');

        BindConstant(0d, "false");
        BindConstant(0d, "False");
        BindConstant(0d, "FALSE");
        BindConstant(0d, 'F');
        BindConstant(0d, '⊥');

        BindOperator('=', OperatorType.Equal);
        BindOperator('≠', OperatorType.NotEqual);

        static double equalToFn(double left, double right) => left == right ? 1.0 : default;

        BindOperator(equalToFn, '↔', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator(equalToFn, '⇔', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator(equalToFn, '≡', (int)EvalPrecedence.Equivalence);

        static double notEqualToFn(double left, double right) => left != right ? 1.0 : default;

        BindOperator(notEqualToFn, '↮', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator(notEqualToFn, '⇎', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator(notEqualToFn, '≢', (int)EvalPrecedence.Equivalence);

        BindOperator('>', OperatorType.GreaterThan);
        BindOperator('<', OperatorType.LessThan);

        BindOperator('≥', OperatorType.GreaterThanOrEqual);
        BindOperator('⪰', OperatorType.GreaterThanOrEqual);

        BindOperator('≤', OperatorType.LessThanOrEqual);
        BindOperator('⪯', OperatorType.LessThanOrEqual);

        static double implicationFn(double left, double right) => left == default || right != default ? 1.0 : default;

        BindOperator(implicationFn, '→', (int)EvalPrecedence.LogicalImplication);
        BindOperator(implicationFn, '⇒', (int)EvalPrecedence.LogicalImplication);

        static double reverseImplicationFn(double left, double right) => left != default || right == default ? 1.0 : default;

        BindOperator(reverseImplicationFn, '←', (int)EvalPrecedence.LogicalImplication);
        BindOperator(reverseImplicationFn, '⟸', (int)EvalPrecedence.LogicalImplication);

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

        static double degFn(double v) => MathTrig.DegreesToRadians(v);

        BindOperandOperator(degFn, '\u00b0', true); //degree symbol

        static double sinFn(double v) => MathTrig.Sin(v);

        BindFunction(sinFn, "sin");
        BindFunction(sinFn, "Sin");
        BindFunction(sinFn, "SIN");

        static double cosFn(double v) => MathTrig.Cos(v);

        BindFunction(cosFn, "cos");
        BindFunction(cosFn, "Cos");
        BindFunction(cosFn, "COS");

        static double tanFn(double v) => MathTrig.Tan(v);

        BindFunction(tanFn, "tan");
        BindFunction(tanFn, "Tan");
        BindFunction(tanFn, "TAN");

        static double secFn(double v) => MathTrig.Sec(v);

        BindFunction(secFn, "sec");
        BindFunction(secFn, "Sec");
        BindFunction(secFn, "SEC");

        static double cscFn(double v) => MathTrig.Csc(v);

        BindFunction(cscFn, "csc");
        BindFunction(cscFn, "Csc");
        BindFunction(cscFn, "CSC");

        static double cotFn(double v) => MathTrig.Cot(v);

        BindFunction(cotFn, "cot");
        BindFunction(cotFn, "Cot");
        BindFunction(cotFn, "COT");

        static double sinhFn(double v) => MathTrig.Sinh(v);

        BindFunction(sinhFn, "sinh");
        BindFunction(sinhFn, "Sinh");
        BindFunction(sinhFn, "SINH");

        static double coshFn(double v) => MathTrig.Cosh(v);

        BindFunction(coshFn, "cosh");
        BindFunction(coshFn, "Cosh");
        BindFunction(coshFn, "COSH");

        static double tanhFn(double v) => MathTrig.Tanh(v);

        BindFunction(tanhFn, "tanh");
        BindFunction(tanhFn, "Tanh");
        BindFunction(tanhFn, "TANH");

        static double sechFn(double v) => MathTrig.Sech(v);

        BindFunction(sechFn, "sech");
        BindFunction(sechFn, "Sech");
        BindFunction(sechFn, "SECH");

        static double cschFn(double v) => MathTrig.Csch(v);

        BindFunction(cschFn, "csch");
        BindFunction(cschFn, "Csch");
        BindFunction(cschFn, "CSCH");

        static double cothFn(double v) => MathTrig.Coth(v);

        BindFunction(cothFn, "coth");
        BindFunction(cothFn, "Coth");
        BindFunction(cothFn, "COTH");

        static double asinFn(double v) => MathTrig.Asin(v);

        BindFunction(asinFn, "sin^-1");
        BindFunction(asinFn, "Sin^-1");
        BindFunction(asinFn, "SIN^-1");

        static double acosFn(double v) => MathTrig.Acos(v);

        BindFunction(acosFn, "cos^-1");
        BindFunction(acosFn, "Cos^-1");
        BindFunction(acosFn, "COS^-1");

        static double atanFn(double v) => MathTrig.Atan(v);

        BindFunction(atanFn, "tan^-1");
        BindFunction(atanFn, "Tan^-1");
        BindFunction(atanFn, "TAN^-1");

        static double asecFn(double v) => MathTrig.Asec(v);

        BindFunction(asecFn, "sec^-1");
        BindFunction(asecFn, "Sec^-1");
        BindFunction(asecFn, "SEC^-1");

        static double acscFn(double v) => MathTrig.Acsc(v);

        BindFunction(acscFn, "csc^-1");
        BindFunction(acscFn, "Csc^-1");
        BindFunction(acscFn, "CSC^-1");

        static double acotFn(double v) => MathTrig.Acot(v);

        BindFunction(acotFn, "cot^-1");
        BindFunction(acotFn, "Cot^-1");
        BindFunction(acotFn, "COT^-1");

        static double asinhFn(double v) => MathTrig.Asinh(v);

        BindFunction(asinhFn, "sinh^-1");
        BindFunction(asinhFn, "Sinh^-1");
        BindFunction(asinhFn, "SINH^-1");

        static double acoshFn(double v) => MathTrig.Acosh(v);

        BindFunction(acoshFn, "cosh^-1");
        BindFunction(acoshFn, "Cosh^-1");
        BindFunction(acoshFn, "COSH^-1");

        static double atanhFn(double v) => MathTrig.Atanh(v);

        BindFunction(atanhFn, "tanh^-1");
        BindFunction(atanhFn, "Tanh^-1");
        BindFunction(atanhFn, "TANH^-1");

        static double asechFn(double v) => MathTrig.Asech(v);

        BindFunction(asechFn, "sech^-1");
        BindFunction(asechFn, "Sech^-1");
        BindFunction(asechFn, "SECH^-1");

        static double acschFn(double v) => MathTrig.Acsch(v);

        BindFunction(acschFn, "csch^-1");
        BindFunction(acschFn, "Csch^-1");
        BindFunction(acschFn, "CSCH^-1");

        static double acothFn(double v) => MathTrig.Acoth(v);

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

    private static long Factorial(double n)
    {
        if (n < 0.0)
            throw new ArgumentException($"Negative number {n} isn't allowed by the factorial function.");

        if (n % 1.0 > double.Epsilon)
            throw new ArgumentException($"Not integer number {n} isn't supported by the factorial function.");

        var i = (long)n;
        var result = 1L;
        while (i > 0)
        {
            result *= i;
            i--;
        }

        return result;
    }
}