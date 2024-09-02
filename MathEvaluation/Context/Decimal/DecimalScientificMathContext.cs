using System;
using MathEvaluation.Entities;
using MathTrigonometric;

namespace MathEvaluation.Context.Decimal;

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
        BindOperator(modFn, "mod");
        BindOperator(modFn, "Mod");
        BindOperator(modFn, "MOD");
        BindOperator(modFn, "modulo");
        BindOperator(modFn, "Modulo");
        BindOperator(modFn, "MODULO");

        static decimal divisionFn(decimal left, decimal right) => left / right;
        BindOperator(divisionFn, '÷');

        static decimal floorDivisionFn(decimal left, decimal right) => Math.Floor(left / right);
        BindOperator(floorDivisionFn, "//");

        static decimal multiplicationFn(decimal left, decimal right) => left * right;
        BindOperator(multiplicationFn, '×');
        BindOperator(multiplicationFn, '·');

        BindOperandsOperator(Math.Pow, '^', (int)EvalPrecedence.Exponentiation);

        BindFunction((decimal value) => Math.Abs(value), '|', '|');
        BindFunction((decimal value) => Math.Ceiling(value), '⌈', '⌉');
        BindFunction((decimal value) => Math.Floor(value), '⌊', '⌋');
        BindFunction(Math.Sqrt, '\u221a'); //square root symbol
        BindFunction(value => Math.Pow(value, 1 / 3d), '\u221b'); //cube root symbol
        BindFunction(value => Math.Pow(value, 0.25d), '\u221c'); //fourth root symbol

        BindFunction((decimal value) => Math.Abs(value), "abs");
        BindFunction((decimal value) => Math.Abs(value), "Abs");
        BindFunction((decimal value) => Math.Abs(value), "ABS");
        BindFunction((double value) => Math.Log(value), "ln");
        BindFunction((double value) => Math.Log(value), "Ln");
        BindFunction((double value) => Math.Log(value), "LN");
        BindFunction(Math.Log10, "log");
        BindFunction(Math.Log10, "Log");
        BindFunction(Math.Log10, "LOG");

        BindOperandOperator(n => Factorial(n), '!', true);

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
        BindOperator(equalToFn, '=', (int)EvalPrecedence.Equality);
        BindOperator(equalToFn, '↔', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator(equalToFn, '⇔', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator(equalToFn, '≡', (int)EvalPrecedence.Equivalence);

        static decimal notEqualToFn(decimal left, decimal right) => left != right ? 1.0m : default;
        BindOperator(notEqualToFn, '≠', (int)EvalPrecedence.Equality);
        BindOperator(notEqualToFn, '↮', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator(notEqualToFn, '⇎', (int)EvalPrecedence.BiconditionalLogicalEquivalence);
        BindOperator(notEqualToFn, '≢', (int)EvalPrecedence.Equivalence);

        static decimal greaterThanFn(decimal left, decimal right) => left > right ? 1.0m : default;
        BindOperator(greaterThanFn, '>', (int)EvalPrecedence.RelationalOperator);

        static decimal lessThanFn(decimal left, decimal right) => left < right ? 1.0m : default;
        BindOperator(lessThanFn, '<', (int)EvalPrecedence.RelationalOperator);

        static decimal greaterThanOrEqualToFn(decimal left, decimal right) => left >= right ? 1.0m : default;
        BindOperator(greaterThanOrEqualToFn, '≥', (int)EvalPrecedence.RelationalOperator);
        BindOperator(greaterThanOrEqualToFn, '⪰', (int)EvalPrecedence.RelationalOperator);
        BindOperator(greaterThanOrEqualToFn, ">=", (int)EvalPrecedence.RelationalOperator);

        static decimal lessThanOrEqualToFn(decimal left, decimal right) => left <= right ? 1.0m : default;
        BindOperator(lessThanOrEqualToFn, '≤', (int)EvalPrecedence.RelationalOperator);
        BindOperator(lessThanOrEqualToFn, '⪯', (int)EvalPrecedence.RelationalOperator);
        BindOperator(lessThanOrEqualToFn, "<=", (int)EvalPrecedence.RelationalOperator);

        static decimal implicationFn(decimal left, decimal right) => left == default || right != default ? 1.0m : default;
        BindOperator(implicationFn, '→', (int)EvalPrecedence.LogicalImplication);
        BindOperator(implicationFn, '⇒', (int)EvalPrecedence.LogicalImplication);

        static decimal reverseImplicationFn(decimal left, decimal right) => left != default || right == default ? 1.0m : default;
        BindOperator(reverseImplicationFn, '←', (int)EvalPrecedence.LogicalImplication);
        BindOperator(reverseImplicationFn, '⟸', (int)EvalPrecedence.LogicalImplication);

        static decimal andFn(decimal left, decimal right) => left != default && right != default ? 1.0m : default;
        BindOperator(andFn, '∧', (int)EvalPrecedence.LogicalAnd);
        BindOperator(andFn, "and", (int)EvalPrecedence.LogicalAnd);
        BindOperator(andFn, "And", (int)EvalPrecedence.LogicalAnd);
        BindOperator(andFn, "AND", (int)EvalPrecedence.LogicalAnd);

        static decimal orFn(decimal left, decimal right) => left != default || right != default ? 1.0m : default;
        BindOperator(orFn, '∨', (int)EvalPrecedence.LogicalOr);
        BindOperator(orFn, "or", (int)EvalPrecedence.LogicalOr);
        BindOperator(orFn, "Or", (int)EvalPrecedence.LogicalOr);
        BindOperator(orFn, "OR", (int)EvalPrecedence.LogicalOr);

        static decimal xorFn(decimal left, decimal right) => left != default ^ right != default ? 1.0m : default;
        BindOperator(xorFn, '⊕', (int)EvalPrecedence.LogicalXor);
        BindOperator(xorFn, "xor", (int)EvalPrecedence.LogicalXor);
        BindOperator(xorFn, "Xor", (int)EvalPrecedence.LogicalXor);
        BindOperator(xorFn, "XOR", (int)EvalPrecedence.LogicalXor);

        static decimal logicalNegationFn(decimal right) => right == default ? 1.0m : default;
        BindOperandOperator(logicalNegationFn, '¬');

        static decimal notFn(decimal left, decimal right) => right == default ? 1.0m : default;
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
