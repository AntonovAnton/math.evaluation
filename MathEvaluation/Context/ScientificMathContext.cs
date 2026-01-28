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
        : base()
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

        BindOperator<double>(floorDivisionFn, "//");

        BindOperator('×', OperatorType.Multiply);
        BindOperator('·', OperatorType.Multiply);

        BindOperator('^', OperatorType.Power);

        static double powerFn(double left, double right) => Math.Pow(left, right);
        BindFunction<double>(powerFn, "pow");
        BindFunction<double>(powerFn, "Pow");
        BindFunction<double>(powerFn, "POW");

        static double absFn(double v) => Math.Abs(v);

        BindFunction<double>(absFn, '|', '|');
        BindFunction<double>(absFn, "abs");
        BindFunction<double>(absFn, "Abs");
        BindFunction<double>(absFn, "ABS");

        static double posFn(double v) => Math.Max(v, 0d);

        BindFunction<double>(posFn, "pos");
        BindFunction<double>(posFn, "Pos");
        BindFunction<double>(posFn, "POS");

        static double ceilingFn(double v) => Math.Ceiling(v);

        BindFunction<double>(ceilingFn, '⌈', '⌉');
        BindFunction<double>(ceilingFn, "ceil");
        BindFunction<double>(ceilingFn, "Ceil");
        BindFunction<double>(ceilingFn, "CEIL");

        static double floorFn(double v) => Math.Floor(v);

        BindFunction<double>(floorFn, '⌊', '⌋');
        BindFunction<double>(floorFn, "floor");
        BindFunction<double>(floorFn, "Floor");
        BindFunction<double>(floorFn, "FLOOR");

        static double sqrtFn(double v) => Math.Sqrt(v);

        BindFunction<double>(sqrtFn, '\u221a'); //square root symbol
        BindFunction<double>(sqrtFn, "sqrt");
        BindFunction<double>(sqrtFn, "Sqrt");
        BindFunction<double>(sqrtFn, "SQRT");

        static double cubeFn(double v) => Math.Pow(v, 1 / 3d);

        BindFunction<double>(cubeFn, '\u221b'); //cube root symbol
        BindFunction<double>(cubeFn, "cbrt");
        BindFunction<double>(cubeFn, "Cbrt");
        BindFunction<double>(cubeFn, "CBRT");

        static double fRootFn(double v) => Math.Pow(v, 0.25d);

        BindFunction<double>(fRootFn, '\u221c'); //fourth root symbol

        static double logFn(double v) => Math.Log(v);

        BindFunction<double>(logFn, "ln");
        BindFunction<double>(logFn, "Ln");
        BindFunction<double>(logFn, "LN");

        static double log10Fn(double v) => Math.Log10(v);

        BindFunction<double>(log10Fn, "log");
        BindFunction<double>(log10Fn, "Log");
        BindFunction<double>(log10Fn, "LOG");

        static double factorialFn(double v) => Factorial(v);

        BindOperandOperator<double>(factorialFn, '!', true);

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

        BindOperator<double>(equalToFn, '↔', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator<double>(equalToFn, '⇔', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator<double>(equalToFn, '≡', (int)EvalPrecedence.Equivalence);

        static double notEqualToFn(double left, double right) => left != right ? 1.0 : default;

        BindOperator<double>(notEqualToFn, '↮', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator<double>(notEqualToFn, '⇎', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator<double>(notEqualToFn, '≢', (int)EvalPrecedence.Equivalence);

        BindOperator('>', OperatorType.GreaterThan);
        BindOperator('<', OperatorType.LessThan);

        BindOperator('≥', OperatorType.GreaterThanOrEqual);
        BindOperator('⪰', OperatorType.GreaterThanOrEqual);

        BindOperator('≤', OperatorType.LessThanOrEqual);
        BindOperator('⪯', OperatorType.LessThanOrEqual);

        static double implicationFn(double left, double right) => left == default || right != default ? 1.0 : default;

        BindOperator<double>(implicationFn, '→', (int)EvalPrecedence.LogicalImplication);
        BindOperator<double>(implicationFn, '⇒', (int)EvalPrecedence.LogicalImplication);

        static double reverseImplicationFn(double left, double right) => left != default || right == default ? 1.0 : default;

        BindOperator<double>(reverseImplicationFn, '←', (int)EvalPrecedence.LogicalImplication);
        BindOperator<double>(reverseImplicationFn, '⟸', (int)EvalPrecedence.LogicalImplication);

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

        static double radiansFn(double v) => MathTrig.DegreesToRadians(v);

        BindOperandOperator<double>(radiansFn, '\u00b0', true); //degree symbol
        BindFunction<double>(radiansFn, "rad");
        BindFunction<double>(radiansFn, "Rad");
        BindFunction<double>(radiansFn, "RAD");

        static double degreesFn(double v) => MathTrig.RadiansToDegrees(v);
        BindFunction<double>(degreesFn, "deg");
        BindFunction<double>(degreesFn, "Deg");
        BindFunction<double>(degreesFn, "DEG");

        static double sinFn(double v) => MathTrig.Sin(v);

        BindFunction<double>(sinFn, "sin");
        BindFunction<double>(sinFn, "Sin");
        BindFunction<double>(sinFn, "SIN");

        static double cosFn(double v) => MathTrig.Cos(v);

        BindFunction<double>(cosFn, "cos");
        BindFunction<double>(cosFn, "Cos");
        BindFunction<double>(cosFn, "COS");

        static double tanFn(double v) => MathTrig.Tan(v);

        BindFunction<double>(tanFn, "tan");
        BindFunction<double>(tanFn, "Tan");
        BindFunction<double>(tanFn, "TAN");

        static double secFn(double v) => MathTrig.Sec(v);

        BindFunction<double>(secFn, "sec");
        BindFunction<double>(secFn, "Sec");
        BindFunction<double>(secFn, "SEC");

        static double cscFn(double v) => MathTrig.Csc(v);

        BindFunction<double>(cscFn, "csc");
        BindFunction<double>(cscFn, "Csc");
        BindFunction<double>(cscFn, "CSC");

        static double cotFn(double v) => MathTrig.Cot(v);

        BindFunction<double>(cotFn, "cot");
        BindFunction<double>(cotFn, "Cot");
        BindFunction<double>(cotFn, "COT");

        static double sinhFn(double v) => MathTrig.Sinh(v);

        BindFunction<double>(sinhFn, "sinh");
        BindFunction<double>(sinhFn, "Sinh");
        BindFunction<double>(sinhFn, "SINH");

        static double coshFn(double v) => MathTrig.Cosh(v);

        BindFunction<double>(coshFn, "cosh");
        BindFunction<double>(coshFn, "Cosh");
        BindFunction<double>(coshFn, "COSH");

        static double tanhFn(double v) => MathTrig.Tanh(v);

        BindFunction<double>(tanhFn, "tanh");
        BindFunction<double>(tanhFn, "Tanh");
        BindFunction<double>(tanhFn, "TANH");

        static double sechFn(double v) => MathTrig.Sech(v);

        BindFunction<double>(sechFn, "sech");
        BindFunction<double>(sechFn, "Sech");
        BindFunction<double>(sechFn, "SECH");

        static double cschFn(double v) => MathTrig.Csch(v);

        BindFunction<double>(cschFn, "csch");
        BindFunction<double>(cschFn, "Csch");
        BindFunction<double>(cschFn, "CSCH");

        static double cothFn(double v) => MathTrig.Coth(v);

        BindFunction<double>(cothFn, "coth");
        BindFunction<double>(cothFn, "Coth");
        BindFunction<double>(cothFn, "COTH");

        static double asinFn(double v) => MathTrig.Asin(v);

        BindFunction<double>(asinFn, "sin^-1");
        BindFunction<double>(asinFn, "Sin^-1");
        BindFunction<double>(asinFn, "SIN^-1");

        static double acosFn(double v) => MathTrig.Acos(v);

        BindFunction<double>(acosFn, "cos^-1");
        BindFunction<double>(acosFn, "Cos^-1");
        BindFunction<double>(acosFn, "COS^-1");

        static double atanFn(double v) => MathTrig.Atan(v);

        BindFunction<double>(atanFn, "tan^-1");
        BindFunction<double>(atanFn, "Tan^-1");
        BindFunction<double>(atanFn, "TAN^-1");

        static double asecFn(double v) => MathTrig.Asec(v);

        BindFunction<double>(asecFn, "sec^-1");
        BindFunction<double>(asecFn, "Sec^-1");
        BindFunction<double>(asecFn, "SEC^-1");

        static double acscFn(double v) => MathTrig.Acsc(v);

        BindFunction<double>(acscFn, "csc^-1");
        BindFunction<double>(acscFn, "Csc^-1");
        BindFunction<double>(acscFn, "CSC^-1");

        static double acotFn(double v) => MathTrig.Acot(v);

        BindFunction<double>(acotFn, "cot^-1");
        BindFunction<double>(acotFn, "Cot^-1");
        BindFunction<double>(acotFn, "COT^-1");

        static double asinhFn(double v) => MathTrig.Asinh(v);

        BindFunction<double>(asinhFn, "sinh^-1");
        BindFunction<double>(asinhFn, "Sinh^-1");
        BindFunction<double>(asinhFn, "SINH^-1");

        static double acoshFn(double v) => MathTrig.Acosh(v);

        BindFunction<double>(acoshFn, "cosh^-1");
        BindFunction<double>(acoshFn, "Cosh^-1");
        BindFunction<double>(acoshFn, "COSH^-1");

        static double atanhFn(double v) => MathTrig.Atanh(v);

        BindFunction<double>(atanhFn, "tanh^-1");
        BindFunction<double>(atanhFn, "Tanh^-1");
        BindFunction<double>(atanhFn, "TANH^-1");

        static double asechFn(double v) => MathTrig.Asech(v);

        BindFunction<double>(asechFn, "sech^-1");
        BindFunction<double>(asechFn, "Sech^-1");
        BindFunction<double>(asechFn, "SECH^-1");

        static double acschFn(double v) => MathTrig.Acsch(v);

        BindFunction<double>(acschFn, "csch^-1");
        BindFunction<double>(acschFn, "Csch^-1");
        BindFunction<double>(acschFn, "CSCH^-1");

        static double acothFn(double v) => MathTrig.Acoth(v);

        BindFunction<double>(acothFn, "coth^-1");
        BindFunction<double>(acothFn, "Coth^-1");
        BindFunction<double>(acothFn, "COTH^-1");

        BindFunction<double>(asinFn, "arcsin");
        BindFunction<double>(asinFn, "Arcsin");
        BindFunction<double>(asinFn, "ARCSIN");
        BindFunction<double>(acosFn, "arccos");
        BindFunction<double>(acosFn, "Arccos");
        BindFunction<double>(acosFn, "ARCCOS");
        BindFunction<double>(atanFn, "arctan");
        BindFunction<double>(atanFn, "Arctan");
        BindFunction<double>(atanFn, "ARCTAN");
        BindFunction<double>(asecFn, "arcsec");
        BindFunction<double>(asecFn, "Arcsec");
        BindFunction<double>(asecFn, "ARCSEC");
        BindFunction<double>(acscFn, "arccsc");
        BindFunction<double>(acscFn, "Arccsc");
        BindFunction<double>(acscFn, "ARCCSC");
        BindFunction<double>(acotFn, "arccot");
        BindFunction<double>(acotFn, "Arccot");
        BindFunction<double>(acotFn, "ARCCOT");

        BindFunction<double>(asinFn, "asin");
        BindFunction<double>(asinFn, "Asin");
        BindFunction<double>(asinFn, "ASIN");
        BindFunction<double>(acosFn, "acos");
        BindFunction<double>(acosFn, "Acos");
        BindFunction<double>(acosFn, "ACOS");
        BindFunction<double>(atanFn, "atan");
        BindFunction<double>(atanFn, "Atan");
        BindFunction<double>(atanFn, "ATAN");
        BindFunction<double>(asecFn, "asec");
        BindFunction<double>(asecFn, "Asec");
        BindFunction<double>(asecFn, "ASEC");
        BindFunction<double>(acscFn, "acsc");
        BindFunction<double>(acscFn, "Acsc");
        BindFunction<double>(acscFn, "ACSC");
        BindFunction<double>(acotFn, "acot");
        BindFunction<double>(acotFn, "Acot");
        BindFunction<double>(acotFn, "ACOT");

        BindFunction<double>(asinhFn, "arsinh");
        BindFunction<double>(asinhFn, "Arsinh");
        BindFunction<double>(asinhFn, "ARSINH");
        BindFunction<double>(acoshFn, "arcosh");
        BindFunction<double>(acoshFn, "Arcosh");
        BindFunction<double>(acoshFn, "ARCOSH");
        BindFunction<double>(atanhFn, "artanh");
        BindFunction<double>(atanhFn, "Artanh");
        BindFunction<double>(atanhFn, "ARTANH");
        BindFunction<double>(asechFn, "arsech");
        BindFunction<double>(asechFn, "Arsech");
        BindFunction<double>(asechFn, "ARSECH");
        BindFunction<double>(acschFn, "arcsch");
        BindFunction<double>(acschFn, "Arcsch");
        BindFunction<double>(acschFn, "ARCSCH");
        BindFunction<double>(acothFn, "arcoth");
        BindFunction<double>(acothFn, "Arcoth");
        BindFunction<double>(acothFn, "ARCOTH");

        BindFunction<double>(asinhFn, "asinh");
        BindFunction<double>(asinhFn, "Asinh");
        BindFunction<double>(asinhFn, "ASINH");
        BindFunction<double>(acoshFn, "acosh");
        BindFunction<double>(acoshFn, "Acosh");
        BindFunction<double>(acoshFn, "ACOSH");
        BindFunction<double>(atanhFn, "atanh");
        BindFunction<double>(atanhFn, "Atanh");
        BindFunction<double>(atanhFn, "ATANH");
        BindFunction<double>(asechFn, "asech");
        BindFunction<double>(asechFn, "Asech");
        BindFunction<double>(asechFn, "ASECH");
        BindFunction<double>(acschFn, "acsch");
        BindFunction<double>(acschFn, "Acsch");
        BindFunction<double>(acschFn, "ACSCH");
        BindFunction<double>(acothFn, "acoth");
        BindFunction<double>(acothFn, "Acoth");
        BindFunction<double>(acothFn, "ACOTH");

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