using System;
using MathTrigonometric;

namespace MathEvaluation.Context;

/// <summary>
/// The base scientific math context supports all trigonometric functions, logarithms, other scientific math functions, and constants. 
/// For a complete list of features and supported functions, please refer to the documentation at <see href="https://github.com/AntonovAnton/math.evaluation"/>.
/// </summary>
/// <seealso cref="MathEvaluation.Context.MathContext" />
public class ScientificMathContext : MathContext
{
    /// <summary>Initializes a new instance of the <see cref="ScientificMathContext" /> class.</summary>
    public ScientificMathContext()
    {
        BindVariable(Math.PI, 'π');
        BindVariable(Math.PI, "pi");
        BindVariable(Math.PI, "Pi");
        BindVariable(Math.PI, "PI");
        BindVariable(Math.E, 'e');
        BindVariable(Math.PI * 2, 'τ');
        BindVariable(double.PositiveInfinity, '\u221e'); //infinity symbol

        static double modFn(double left, double right) => left % right;
        BindOperator(modFn, "mod");
        BindOperator(modFn, "Mod");
        BindOperator(modFn, "MOD");
        BindOperator(modFn, "modulo");
        BindOperator(modFn, "Modulo");
        BindOperator(modFn, "MODULO");

        static double divisionFn(double left, double right) => left / right;
        BindOperator(divisionFn, '÷');

        static double floorDivisionFn(double left, double right) => Math.Floor(left / right);
        BindOperator(floorDivisionFn, "//");

        static double multiplicationFn(double left, double right) => left * right;
        BindOperator(multiplicationFn, '×');
        BindOperator(multiplicationFn, '·');

        BindOperandsOperator(Math.Pow, '^', (int)EvalPrecedence.Exponentiation);

        BindFunction((double value) => Math.Abs(value), '|', '|');
        BindFunction((double value) => Math.Ceiling(value), '⌈', '⌉');
        BindFunction((double value) => Math.Floor(value), '⌊', '⌋');
        BindFunction(Math.Sqrt, '\u221a'); //square root symbol
        BindFunction(value => Math.Pow(value, 1/3d), '\u221b'); //cube root symbol
        BindFunction(value => Math.Pow(value, 0.25d), '\u221c'); //fourth root symbol

        BindFunction((double value) => Math.Abs(value), "abs");
        BindFunction((double value) => Math.Abs(value), "Abs");
        BindFunction((double value) => Math.Abs(value), "ABS");
        BindFunction(Math.Log, "ln");
        BindFunction(Math.Log, "Ln");
        BindFunction(Math.Log, "LN");
        BindFunction(Math.Log10, "log");
        BindFunction(Math.Log10, "Log");
        BindFunction(Math.Log10, "LOG");

        BindOperandOperator(n => Factorial(n), '!', true);

        #region boolean logic

        BindVariable(1d, "true");
        BindVariable(1d, "True");
        BindVariable(1d, "TRUE");
        BindVariable(1d, 'T');
        BindVariable(1d, '⊤');

        BindVariable(0d, "false");
        BindVariable(0d, "False");
        BindVariable(0d, "FALSE");
        BindVariable(0d, 'F');
        BindVariable(0d, '⊥');

        static double equalToFn(double left, double right) => left == right ? 1.0 : default;
        BindOperator(equalToFn, '=', (int)EvalPrecedence.Equality);
        BindOperator(equalToFn, '↔', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator(equalToFn, '⇔', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator(equalToFn, '≡', (int)EvalPrecedence.Equivalence);

        static double notEqualToFn(double left, double right) => left != right ? 1.0 : default;
        BindOperator(notEqualToFn, '≠', (int)EvalPrecedence.Equality);
        BindOperator(notEqualToFn, '↮', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator(notEqualToFn, '⇎', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator(notEqualToFn, '≢', (int)EvalPrecedence.Equivalence);

        static double greaterThanFn(double left, double right) => left > right ? 1.0 : default;
        BindOperator(greaterThanFn, '>', (int)EvalPrecedence.RelationalOperator);

        static double lessThanFn(double left, double right) => left < right ? 1.0 : default;
        BindOperator(lessThanFn, '<', (int)EvalPrecedence.RelationalOperator);

        static double greaterThanOrEqualToFn(double left, double right) => left >= right ? 1.0 : default;
        BindOperator(greaterThanOrEqualToFn, '≥', (int)EvalPrecedence.RelationalOperator);
        BindOperator(greaterThanOrEqualToFn, '⪰', (int)EvalPrecedence.RelationalOperator);

        static double lessThanOrEqualToFn(double left, double right) => left <= right ? 1.0 : default;
        BindOperator(lessThanOrEqualToFn, '≤', (int)EvalPrecedence.RelationalOperator);
        BindOperator(lessThanOrEqualToFn, '⪯', (int)EvalPrecedence.RelationalOperator);

        static double implicationFn(double left, double right) => left == default || right != default ? 1.0 : default;
        BindOperator(implicationFn, '→', (int)EvalPrecedence.LogicalImplication);
        BindOperator(implicationFn, '⇒', (int)EvalPrecedence.LogicalImplication);

        static double reverseImplicationFn(double left, double right) => left != default || right == default ? 1.0 : default;
        BindOperator(reverseImplicationFn, '←', (int)EvalPrecedence.LogicalImplication);
        BindOperator(reverseImplicationFn, '⟸', (int)EvalPrecedence.LogicalImplication);

        static double andFn(double left, double right) => left != default && right != default ? 1.0 : default;
        BindOperator(andFn, '∧', (int)EvalPrecedence.LogicalAnd);
        BindOperator(andFn, "and", (int)EvalPrecedence.LogicalAnd);
        BindOperator(andFn, "And", (int)EvalPrecedence.LogicalAnd);
        BindOperator(andFn, "AND", (int)EvalPrecedence.LogicalAnd);

        static double orFn(double left, double right) => left != default || right != default ? 1.0 : default;
        BindOperator(orFn, '∨', (int)EvalPrecedence.LogicalOr);
        BindOperator(orFn, "or", (int)EvalPrecedence.LogicalOr);
        BindOperator(orFn, "Or", (int)EvalPrecedence.LogicalOr);
        BindOperator(orFn, "OR", (int)EvalPrecedence.LogicalOr);

        static double xorFn(double left, double right) => left != default ^ right != default ? 1.0 : default;
        BindOperator(xorFn, '⊕', (int)EvalPrecedence.LogicalXor);
        BindOperator(xorFn, "xor", (int)EvalPrecedence.LogicalXor);
        BindOperator(xorFn, "Xor", (int)EvalPrecedence.LogicalXor);
        BindOperator(xorFn, "XOR", (int)EvalPrecedence.LogicalXor);

        static double logicalNegationFn(double right) => right == default ? 1.0 : default;
        BindOperandOperator(logicalNegationFn, '¬');

        static double notFn(double left, double right) => right == default ? 1.0 : default;
        BindOperator(notFn, "not", (int)EvalPrecedence.LogicalNot);
        BindOperator(notFn, "Not", (int)EvalPrecedence.LogicalNot);
        BindOperator(notFn, "NOT", (int)EvalPrecedence.LogicalNot);

        #endregion

        #region trigonometric functions

        BindOperandOperator(MathTrig.DegreesToRadians, '\u00b0', true); //degree symbol

        BindFunction(MathTrig.Sin, "sin");
        BindFunction(MathTrig.Sin, "Sin");
        BindFunction(MathTrig.Sin, "SIN");
        BindFunction(MathTrig.Cos, "cos");
        BindFunction(MathTrig.Cos, "Cos");
        BindFunction(MathTrig.Cos, "COS");
        BindFunction(MathTrig.Tan, "tan");
        BindFunction(MathTrig.Tan, "Tan");
        BindFunction(MathTrig.Tan, "TAN");
        BindFunction(MathTrig.Sec, "sec");
        BindFunction(MathTrig.Sec, "Sec");
        BindFunction(MathTrig.Sec, "SEC");
        BindFunction(MathTrig.Csc, "csc");
        BindFunction(MathTrig.Csc, "Csc");
        BindFunction(MathTrig.Csc, "CSC");
        BindFunction(MathTrig.Cot, "cot");
        BindFunction(MathTrig.Cot, "Cot");
        BindFunction(MathTrig.Cot, "COT");

        BindFunction(MathTrig.Sinh, "sinh");
        BindFunction(MathTrig.Sinh, "Sinh");
        BindFunction(MathTrig.Sinh, "SINH");
        BindFunction(MathTrig.Cosh, "cosh");
        BindFunction(MathTrig.Cosh, "Cosh");
        BindFunction(MathTrig.Cosh, "COSH");
        BindFunction(MathTrig.Tanh, "tanh");
        BindFunction(MathTrig.Tanh, "Tanh");
        BindFunction(MathTrig.Tanh, "TANH");
        BindFunction(MathTrig.Sech, "sech");
        BindFunction(MathTrig.Sech, "Sech");
        BindFunction(MathTrig.Sech, "SECH");
        BindFunction(MathTrig.Csch, "csch");
        BindFunction(MathTrig.Csch, "Csch");
        BindFunction(MathTrig.Csch, "CSCH");
        BindFunction(MathTrig.Coth, "coth");
        BindFunction(MathTrig.Coth, "Coth");
        BindFunction(MathTrig.Coth, "COTH");

        BindFunction(MathTrig.Asin, "sin^-1");
        BindFunction(MathTrig.Asin, "Sin^-1");
        BindFunction(MathTrig.Asin, "SIN^-1");
        BindFunction(MathTrig.Acos, "cos^-1");
        BindFunction(MathTrig.Acos, "Cos^-1");
        BindFunction(MathTrig.Acos, "COS^-1");
        BindFunction(MathTrig.Atan, "tan^-1");
        BindFunction(MathTrig.Atan, "Tan^-1");
        BindFunction(MathTrig.Atan, "TAN^-1");
        BindFunction(MathTrig.Asec, "sec^-1");
        BindFunction(MathTrig.Asec, "Sec^-1");
        BindFunction(MathTrig.Asec, "SEC^-1");
        BindFunction(MathTrig.Acsc, "csc^-1");
        BindFunction(MathTrig.Acsc, "Csc^-1");
        BindFunction(MathTrig.Acsc, "CSC^-1");
        BindFunction(MathTrig.Acot, "cot^-1");
        BindFunction(MathTrig.Acot, "Cot^-1");
        BindFunction(MathTrig.Acot, "COT^-1");

        BindFunction(MathTrig.Asinh, "sinh^-1");
        BindFunction(MathTrig.Asinh, "Sinh^-1");
        BindFunction(MathTrig.Asinh, "SINH^-1");
        BindFunction(MathTrig.Acosh, "cosh^-1");
        BindFunction(MathTrig.Acosh, "Cosh^-1");
        BindFunction(MathTrig.Acosh, "COSH^-1");
        BindFunction(MathTrig.Atanh, "tanh^-1");
        BindFunction(MathTrig.Atanh, "Tanh^-1");
        BindFunction(MathTrig.Atanh, "TANH^-1");
        BindFunction(MathTrig.Asech, "sech^-1");
        BindFunction(MathTrig.Asech, "Sech^-1");
        BindFunction(MathTrig.Asech, "SECH^-1");
        BindFunction(MathTrig.Acsch, "csch^-1");
        BindFunction(MathTrig.Acsch, "Csch^-1");
        BindFunction(MathTrig.Acsch, "CSCH^-1");
        BindFunction(MathTrig.Acoth, "coth^-1");
        BindFunction(MathTrig.Acoth, "Coth^-1");
        BindFunction(MathTrig.Acoth, "COTH^-1");

        BindFunction(MathTrig.Asin, "arcsin");
        BindFunction(MathTrig.Asin, "Arcsin");
        BindFunction(MathTrig.Asin, "ARCSIN");
        BindFunction(MathTrig.Acos, "arccos");
        BindFunction(MathTrig.Acos, "Arccos");
        BindFunction(MathTrig.Acos, "ARCCOS");
        BindFunction(MathTrig.Atan, "arctan");
        BindFunction(MathTrig.Atan, "Arctan");
        BindFunction(MathTrig.Atan, "ARCTAN");
        BindFunction(MathTrig.Asec, "arcsec");
        BindFunction(MathTrig.Asec, "Arcsec");
        BindFunction(MathTrig.Asec, "ARCSEC");
        BindFunction(MathTrig.Acsc, "arccsc");
        BindFunction(MathTrig.Acsc, "Arccsc");
        BindFunction(MathTrig.Acsc, "ARCCSC");
        BindFunction(MathTrig.Acot, "arccot");
        BindFunction(MathTrig.Acot, "Arccot");
        BindFunction(MathTrig.Acot, "ARCCOT");

        BindFunction(MathTrig.Asinh, "arsinh");
        BindFunction(MathTrig.Asinh, "Arsinh");
        BindFunction(MathTrig.Asinh, "ARSINH");
        BindFunction(MathTrig.Acosh, "arcosh");
        BindFunction(MathTrig.Acosh, "Arcosh");
        BindFunction(MathTrig.Acosh, "ARCOSH");
        BindFunction(MathTrig.Atanh, "artanh");
        BindFunction(MathTrig.Atanh, "Artanh");
        BindFunction(MathTrig.Atanh, "ARTANH");
        BindFunction(MathTrig.Asech, "arsech");
        BindFunction(MathTrig.Asech, "Arsech");
        BindFunction(MathTrig.Asech, "ARSECH");
        BindFunction(MathTrig.Acsch, "arcsch");
        BindFunction(MathTrig.Acsch, "Arcsch");
        BindFunction(MathTrig.Acsch, "ARCSCH");
        BindFunction(MathTrig.Acoth, "arcoth");
        BindFunction(MathTrig.Acoth, "Arcoth");
        BindFunction(MathTrig.Acoth, "ARCOTH");

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
