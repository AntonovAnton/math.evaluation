using System;
using MathTrigonometric;

namespace MathEvaluation;

internal static class MathFnEvaluator
{
    internal static bool TryGetAbsFn(ReadOnlySpan<char> expression, ref int i,
        out Func<double, double>? fn)
    {
        fn = null;
        if (expression.Length <= i + 2)
            return false;

        if (expression[i] is 'a' or 'A' && expression[i + 1] is 'b' or 'B' && expression[i + 2] is 's' or 'S')
            fn = Math.Abs;

        if (fn == null)
            return false;

        i += 3;
        return true;
    }

    internal static bool TryGetLogarithmFn(ReadOnlySpan<char> expression, ref int i,
        out Func<double, double>? fn)
    {
        fn = null;

        if (expression.Length <= i + 1 || expression[i] is not ('l' or 'L'))
            return fn != null;

        if (expression[i + 1] is 'n' or 'N')
        {
            fn = Math.Log;
            i += 2;
        }
        else if (expression.Length > i + 2 && expression[i + 1] is 'o' or 'O' && expression[i + 2] is 'g' or 'G')
        {
            i += 3;
            fn = Math.Log10;
        }

        return fn != null;
    }

    internal static bool TryGetTrigonometricFn(ReadOnlySpan<char> expression, ref int i,
        out Func<double, double>? fn)
    {
        if (expression.Length > i + 5)
            if (expression[i] is 'a' or 'A' && expression[i + 1] is 'r' or 'R')
            {
                if (expression[i + 5] is 'h' or 'H')
                    return TryGetInverseHyperbolicFn(expression, ref i, out fn, 2);

                if (expression[i + 2] is 'c' or 'C')
                    return TryGetInverseTrigonometricFn(expression, ref i, out fn, 3);
            }

        if (expression.Length > i + 3)
        {
            if (expression[i] is 'a' or 'A')
            {
                if (expression.Length > i + 4 && expression[i + 4] is 'h' or 'H')
                    return TryGetInverseHyperbolicFn(expression, ref i, out fn, 1);

                return TryGetInverseTrigonometricFn(expression, ref i, out fn, 1);
            }

            if (expression[i + 3] is 'h' or 'H')
                return TryGetHyperbolicFn(expression, ref i, out fn);
        }

        fn = null;
        if (expression.Length <= i + 2)
            return false;

        fn = expression[i] switch
        {
            's' or 'S' => expression[i + 1] switch
            {
                'i' or 'I' when expression[i + 2] is 'n' or 'N' => MathTrig.Sin,
                'e' or 'E' when expression[i + 2] is 'c' or 'C' => MathTrig.Sec,
                _ => null
            },
            'c' or 'C' => expression[i + 1] switch
            {
                'o' or 'O' when expression[i + 2] is 's' or 'S' => MathTrig.Cos,
                'o' or 'O' when expression[i + 2] is 't' or 'T' => MathTrig.Cot,
                's' or 'S' when expression[i + 2] is 'c' or 'C' => MathTrig.Csc,
                _ => null
            },
            't' or 'T' => expression[i + 1] switch
            {
                'a' or 'A' when expression[i + 2] is 'n' or 'N' => MathTrig.Tan,
                _ => null
            },
            _ => null
        };

        if (fn == null)
            return false;

        i += 3;
        return true;
    }

    private static bool TryGetHyperbolicFn(ReadOnlySpan<char> expression, ref int i,
        out Func<double, double>? fn)
    {
        fn = expression[i] switch
        {
            's' or 'S' => expression[i + 1] switch
            {
                'i' or 'I' when expression[i + 2] is 'n' or 'N' => MathTrig.Sinh,
                'e' or 'E' when expression[i + 2] is 'c' or 'C' => MathTrig.Sech,
                _ => null
            },
            'c' or 'C' => expression[i + 1] switch
            {
                'o' or 'O' when expression[i + 2] is 's' or 'S' => MathTrig.Cosh,
                'o' or 'O' when expression[i + 2] is 't' or 'T' => MathTrig.Coth,
                's' or 'S' when expression[i + 2] is 'c' or 'C' => MathTrig.Csch,
                _ => null
            },
            't' or 'T' => expression[i + 1] switch
            {
                'a' or 'A' when expression[i + 2] is 'n' or 'N' => MathTrig.Tanh,
                _ => null
            },
            _ => null
        };

        if (fn == null)
            return false;

        i += 4;
        return true;
    }

    private static bool TryGetInverseTrigonometricFn(ReadOnlySpan<char> expression, ref int i,
        out Func<double, double>? fn, int indexShift)
    {
        fn = expression[i + indexShift] switch
        {
            's' or 'S' => expression[i + indexShift + 1] switch
            {
                'i' or 'I' when expression[i + indexShift + 2] is 'n' or 'N' => MathTrig.Asin,
                'e' or 'E' when expression[i + indexShift + 2] is 'c' or 'C' => MathTrig.Asec,
                _ => null
            },
            'c' or 'C' => expression[i + indexShift + 1] switch
            {
                'o' or 'O' when expression[i + indexShift + 2] is 's' or 'S' => MathTrig.Acos,
                'o' or 'O' when expression[i + indexShift + 2] is 't' or 'T' => MathTrig.Acot,
                's' or 'S' when expression[i + indexShift + 2] is 'c' or 'C' => MathTrig.Acsc,
                _ => null
            },
            't' or 'T' => expression[i + indexShift + 1] switch
            {
                'a' or 'A' when expression[i + indexShift + 2] is 'n' or 'N' => MathTrig.Atan,
                _ => null
            },
            _ => null
        };

        if (fn == null)
            return false;

        i = i + 3 + indexShift;
        return true;
    }

    private static bool TryGetInverseHyperbolicFn(ReadOnlySpan<char> expression, ref int i,
        out Func<double, double>? fn, int indexShift)
    {
        fn = expression[i + indexShift] switch
        {
            's' or 'S' => expression[i + indexShift + 1] switch
            {
                'i' or 'I' when expression[i + indexShift + 2] is 'n' or 'N' => MathTrig.Asinh,
                'e' or 'E' when expression[i + indexShift + 2] is 'c' or 'C' => MathTrig.Asech,
                _ => null
            },
            'c' or 'C' => expression[i + indexShift + 1] switch
            {
                'o' or 'O' when expression[i + indexShift + 2] is 's' or 'S' => MathTrig.Acosh,
                'o' or 'O' when expression[i + indexShift + 2] is 't' or 'T' => MathTrig.Acoth,
                's' or 'S' when expression[i + indexShift + 2] is 'c' or 'C' => MathTrig.Acsch,
                _ => null
            },
            't' or 'T' => expression[i + indexShift + 1] switch
            {
                'a' or 'A' when expression[i + indexShift + 2] is 'n' or 'N' => MathTrig.Atanh,
                _ => null
            },
            _ => null
        };

        if (fn == null)
            return false;

        i = i + 4 + indexShift;
        return true;
    }
}