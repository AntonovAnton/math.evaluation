using System;
using System.Linq.Expressions;
using MathEvaluation.Entities;
using MathTrigonometric;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MathEvaluation.Context;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// The base scientific math context supports all trigonometric functions, logarithms, other scientific math functions, and constants. 
/// For a complete list of features and supported functions, please refer to the documentation at <see href="https://github.com/AntonovAnton/math.evaluation"/>.
/// </summary>
/// <seealso cref="MathEvaluation.Context.MathContext" />
public class DecimalScientificMathContext : MathContext
{
    /// <summary>Initializes a new instance of the <see cref="DecimalScientificMathContext" /> class.</summary>
    public DecimalScientificMathContext()
    {
        BindConstant(Math.PI, 'π');
        BindConstant(Math.PI, "pi");
        BindConstant(Math.PI, "Pi");
        BindConstant(Math.PI, "PI");
        BindConstant(Math.E, 'e');
        BindConstant(Math.PI * 2, 'τ');
        BindConstant(double.PositiveInfinity, '\u221e'); //infinity symbol

        static decimal modFn(decimal left, decimal right) => left % right;
        BindOperator(modFn, "mod", (int)EvalPrecedence.Basic, ExpressionType.Modulo);
        BindOperator(modFn, "Mod", (int)EvalPrecedence.Basic, ExpressionType.Modulo);
        BindOperator(modFn, "MOD", (int)EvalPrecedence.Basic, ExpressionType.Modulo);
        BindOperator(modFn, "modulo", (int)EvalPrecedence.Basic, ExpressionType.Modulo);
        BindOperator(modFn, "Modulo", (int)EvalPrecedence.Basic, ExpressionType.Modulo);
        BindOperator(modFn, "MODULO", (int)EvalPrecedence.Basic, ExpressionType.Modulo);

        static decimal divisionFn(decimal left, decimal right) => left / right;
        BindOperator(divisionFn, '÷', (int)EvalPrecedence.Basic, ExpressionType.Divide);

        static decimal floorDivisionFn(decimal left, decimal right) => Math.Floor(left / right);
        BindOperator(floorDivisionFn, "//");

        static decimal multiplicationFn(decimal left, decimal right) => left * right;
        BindOperator(multiplicationFn, '×', (int)EvalPrecedence.Basic, ExpressionType.Multiply);
        BindOperator(multiplicationFn, '·', (int)EvalPrecedence.Basic, ExpressionType.Multiply);

        static double powFn(double x, double y) => Math.Pow(x, y);
        BindOperandsOperator(powFn, '^', (int)EvalPrecedence.Exponentiation);

        static decimal absFn(decimal v) => Math.Abs(v);
        BindFunction(absFn, '|', '|');
        BindFunction(absFn, "abs");
        BindFunction(absFn, "Abs");
        BindFunction(absFn, "ABS");

        static decimal ceilingFn(decimal v) => Math.Ceiling(v);
        BindFunction(ceilingFn, '⌈', '⌉');

        static decimal floorFn(decimal v) => Math.Floor(v);
        BindFunction(floorFn, '⌊', '⌋');

        static double sqrtFn(double v) => Math.Sqrt(v);
        BindFunction(sqrtFn, '\u221a'); //square root symbol

        static double cubeFn(double v) => Math.Pow(v, 1/3d);
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

        static decimal factorialFn(decimal v) => Factorial(v);
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

        static decimal equalToFn(decimal left, decimal right) => left == right ? 1.0m : default;
        BindOperator(equalToFn, '=', (int)EvalPrecedence.Equality, ExpressionType.Equal);
        BindOperator(equalToFn, '↔', (int)EvalPrecedence.BiconditionalLogicalEquivalence, ExpressionType.Equal);
        BindOperator(equalToFn, '⇔', (int)EvalPrecedence.BiconditionalLogicalEquivalence, ExpressionType.Equal);
        BindOperator(equalToFn, '≡', (int)EvalPrecedence.Equivalence, ExpressionType.Equal);

        static decimal notEqualToFn(decimal left, decimal right) => left != right ? 1.0m : default;
        BindOperator(notEqualToFn, '≠', (int)EvalPrecedence.Equality, ExpressionType.NotEqual);
        BindOperator(notEqualToFn, '↮', (int)EvalPrecedence.BiconditionalLogicalEquivalence, ExpressionType.NotEqual);
        BindOperator(notEqualToFn, '⇎', (int)EvalPrecedence.BiconditionalLogicalEquivalence, ExpressionType.NotEqual);
        BindOperator(notEqualToFn, '≢', (int)EvalPrecedence.Equivalence, ExpressionType.NotEqual);

        static decimal greaterThanFn(decimal left, decimal right) => left > right ? 1.0m : default;
        BindOperator(greaterThanFn, '>', (int)EvalPrecedence.RelationalOperator, ExpressionType.GreaterThan);

        static decimal lessThanFn(decimal left, decimal right) => left < right ? 1.0m : default;
        BindOperator(lessThanFn, '<', (int)EvalPrecedence.RelationalOperator, ExpressionType.LessThan);

        static decimal greaterThanOrEqualToFn(decimal left, decimal right) => left >= right ? 1.0m : default;
        BindOperator(greaterThanOrEqualToFn, '≥', (int)EvalPrecedence.RelationalOperator, ExpressionType.GreaterThanOrEqual);
        BindOperator(greaterThanOrEqualToFn, '⪰', (int)EvalPrecedence.RelationalOperator, ExpressionType.GreaterThanOrEqual);

        static decimal lessThanOrEqualToFn(decimal left, decimal right) => left <= right ? 1.0m : default;
        BindOperator(lessThanOrEqualToFn, '≤', (int)EvalPrecedence.RelationalOperator, ExpressionType.LessThanOrEqual);
        BindOperator(lessThanOrEqualToFn, '⪯', (int)EvalPrecedence.RelationalOperator, ExpressionType.LessThanOrEqual);

        static decimal implicationFn(decimal left, decimal right) => left == default || right != default ? 1.0m : default;
        BindOperator(implicationFn, '→', (int)EvalPrecedence.LogicalImplication);
        BindOperator(implicationFn, '⇒', (int)EvalPrecedence.LogicalImplication);

        static decimal reverseImplicationFn(decimal left, decimal right) => left != default || right == default ? 1.0m : default;
        BindOperator(reverseImplicationFn, '←', (int)EvalPrecedence.LogicalImplication);
        BindOperator(reverseImplicationFn, '⟸', (int)EvalPrecedence.LogicalImplication);

        static decimal andFn(decimal left, decimal right) => left != default && right != default ? 1.0m : default;
        BindOperator(andFn, '∧', (int)EvalPrecedence.LogicalAnd, ExpressionType.AndAlso);
        BindOperator(andFn, "and", (int)EvalPrecedence.LogicalAnd, ExpressionType.AndAlso);
        BindOperator(andFn, "And", (int)EvalPrecedence.LogicalAnd, ExpressionType.AndAlso);
        BindOperator(andFn, "AND", (int)EvalPrecedence.LogicalAnd, ExpressionType.AndAlso);

        static decimal orFn(decimal left, decimal right) => left != default || right != default ? 1.0m : default;
        BindOperator(orFn, '∨', (int)EvalPrecedence.LogicalOr, ExpressionType.OrElse);
        BindOperator(orFn, "or", (int)EvalPrecedence.LogicalOr, ExpressionType.OrElse);
        BindOperator(orFn, "Or", (int)EvalPrecedence.LogicalOr, ExpressionType.OrElse);
        BindOperator(orFn, "OR", (int)EvalPrecedence.LogicalOr, ExpressionType.OrElse);

        static decimal xorFn(decimal left, decimal right) => left != default ^ right != default ? 1.0m : default;
        BindOperator(xorFn, '⊕', (int)EvalPrecedence.LogicalXor, ExpressionType.ExclusiveOr);
        BindOperator(xorFn, "xor", (int)EvalPrecedence.LogicalXor, ExpressionType.ExclusiveOr);
        BindOperator(xorFn, "Xor", (int)EvalPrecedence.LogicalXor, ExpressionType.ExclusiveOr);
        BindOperator(xorFn, "XOR", (int)EvalPrecedence.LogicalXor, ExpressionType.ExclusiveOr);

        static decimal logicalNegationFn(decimal right) => right == default ? 1.0m : default;
        BindOperandOperator(logicalNegationFn, '¬');

        static decimal notFn(decimal left, decimal right) => right == default ? 1.0m : default;
        BindOperator(notFn, "not", (int)EvalPrecedence.LogicalNot, ExpressionType.Not);
        BindOperator(notFn, "Not", (int)EvalPrecedence.LogicalNot, ExpressionType.Not);
        BindOperator(notFn, "NOT", (int)EvalPrecedence.LogicalNot, ExpressionType.Not);

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

    private static long Factorial(decimal n)
    {
        if (n < 0.0m)
            throw new ArgumentException($"Negative number {n} isn't allowed by the factorial function.");

        if (n % 1.0m > 0m)
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
