﻿using System;
using MathTrigonometric;

namespace MathEvaluation.Context
{
    public class ScientificMathContext : MathContext, IScientificMathContext
    {
        public ScientificMathContext()
        {
            BindConstant(Math.PI, "π");
            BindConstant(Math.PI, "pi");
            BindConstant(Math.PI, "Pi");
            BindConstant(Math.PI, "PI");
            BindConstant(Math.E, "e");
            BindConstant(Math.PI * 2, "τ");
            BindConstant(double.PositiveInfinity, "\u221e"); //infinity symbol

            BindFunction(Math.Abs, '|', '|');
            BindFunction(Math.Ceiling, '⌈', '⌉');
            BindFunction(Math.Floor, '⌊', '⌋');
            BindFunction(value => value, '[', ']');

            BindFunction(Math.Abs, "abs");
            BindFunction(Math.Abs, "Abs");
            BindFunction(Math.Abs, "ABS");
            BindFunction(Math.Log, "ln");
            BindFunction(Math.Log, "Ln");
            BindFunction(Math.Log, "LN");
            BindFunction(Math.Log10, "log");
            BindFunction(Math.Log10, "Log");
            BindFunction(Math.Log10, "LOG");

            #region trigonometric functions

            BindConverter(MathTrig.DegreesToRadians, "\u00b0"); //degree symbol

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

            BindFunction(MathTrig.Asin, "asin");
            BindFunction(MathTrig.Asin, "Asin");
            BindFunction(MathTrig.Asin, "ASIN");
            BindFunction(MathTrig.Acos, "acos");
            BindFunction(MathTrig.Acos, "Acos");
            BindFunction(MathTrig.Acos, "ACOS");
            BindFunction(MathTrig.Atan, "atan");
            BindFunction(MathTrig.Atan, "Atan");
            BindFunction(MathTrig.Atan, "ATAN");
            BindFunction(MathTrig.Asec, "asec");
            BindFunction(MathTrig.Asec, "Asec");
            BindFunction(MathTrig.Asec, "ASEC");
            BindFunction(MathTrig.Acsc, "acsc");
            BindFunction(MathTrig.Acsc, "Acsc");
            BindFunction(MathTrig.Acsc, "ACSC");
            BindFunction(MathTrig.Acot, "acot");
            BindFunction(MathTrig.Acot, "Acot");
            BindFunction(MathTrig.Acot, "ACOT");

            BindFunction(MathTrig.Asinh, "asinh");
            BindFunction(MathTrig.Asinh, "Asinh");
            BindFunction(MathTrig.Asinh, "ASINH");
            BindFunction(MathTrig.Acosh, "acosh");
            BindFunction(MathTrig.Acosh, "Acosh");
            BindFunction(MathTrig.Acosh, "ACOSH");
            BindFunction(MathTrig.Atanh, "atanh");
            BindFunction(MathTrig.Atanh, "Atanh");
            BindFunction(MathTrig.Atanh, "ATANH");
            BindFunction(MathTrig.Asech, "asech");
            BindFunction(MathTrig.Asech, "Asech");
            BindFunction(MathTrig.Asech, "ASECH");
            BindFunction(MathTrig.Acsch, "acsch");
            BindFunction(MathTrig.Acsch, "Acsch");
            BindFunction(MathTrig.Acsch, "ACSCH");
            BindFunction(MathTrig.Acoth, "acoth");
            BindFunction(MathTrig.Acoth, "Acoth");
            BindFunction(MathTrig.Acoth, "ACOTH");

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

        protected override bool TryEvaluate(ReadOnlySpan<char> expression, IFormatProvider provider, ref int i, char? separator,
            bool isEvaluatedFirst, ref double value)
        {
            if (base.TryEvaluate(expression, provider, ref i, separator, isEvaluatedFirst, ref value))
                return true;

            if (TryEvaluateModulus(expression, provider, ref i, separator, isEvaluatedFirst, ref value))
                return true;

            return false;
        }

        private bool TryEvaluateModulus(ReadOnlySpan<char> expression, IFormatProvider provider, ref int i, char? separator,
            bool isEvaluatedFirst, ref double value)
        {
            const string fn = "mod";
            if (!expression[i..].StartsWith(fn, StringComparison.InvariantCultureIgnoreCase))
                return false;

            if (isEvaluatedFirst)
            {
                return true;
            }

            i += 3;
            if (expression[i..].StartsWith("ulo", StringComparison.InvariantCultureIgnoreCase))
                i += 3;

            value %= MathEvaluator.EvaluateBasic(expression, this, provider, ref i, separator, true);
            return true;
        }
    }
}
