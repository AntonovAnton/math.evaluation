using System;

namespace MathEvaluation;

internal static class MathFn
{
    #region Fundamental Trigonometric Functions

    public static double Sin(double a)
    {
        return Math.Sin(a);
    }

    public static double Cos(double a)
    {
        return Math.Cos(a);
    }

    public static double Tan(double a)
    {
        var cos = Math.Cos(a);
        if (cos == 0d)
            return double.NaN;

        return Math.Sin(a) / cos;
    }

    /// <summary>
    ///     Cosecant
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static double Csc(double a)
    {
        var sin = Math.Sin(a);
        if (sin == 0d)
            return double.NaN;

        return 1 / sin;
    }

    /// <summary>
    ///     Secant
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static double Sec(double a)
    {
        var cos = Math.Cos(a);
        if (cos == 0d)
            return double.NaN;

        return 1 / cos;
    }

    /// <summary>
    ///     Cotangent
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static double Cot(double a)
    {
        var sin = Math.Sin(a);
        if (sin == 0d)
            return double.NaN;

        return Math.Cos(a) / sin;
    }

    #endregion

    #region Hyperbolic Trigonometric Functions

    public static double Sinh(double a)
    {
        return Math.Sinh(a);
    }


    public static double Cosh(double a)
    {
        return Math.Cosh(a);
    }


    public static double Tanh(double a)
    {
        var cos = Math.Cosh(a);
        if (cos == 0d)
            return double.NaN;

        return Math.Sinh(a) / cos;
    }

    /// <summary>
    ///     Cosecant
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static double Csch(double a)
    {
        var sin = Math.Sinh(a);
        if (sin == 0d)
            return double.NaN;

        return 1 / sin;
    }

    /// <summary>
    ///     Secant
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static double Sech(double a)
    {
        var cos = Math.Cosh(a);
        if (cos == 0d)
            return double.NaN;

        return 1 / cos;
    }

    /// <summary>
    ///     Cotangent
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static double Coth(double a)
    {
        var sin = Math.Sinh(a);
        if (sin == 0d)
            return double.NaN;

        return Math.Cosh(a) / sin;
    }

    #endregion

    public static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0d;
    }

    internal static bool TryGetTrigonometricFn(ReadOnlySpan<char> expression, ref int i,
        out Func<double, double>? fn)
    {
        if (expression.Length > i + 3 && expression[i + 3] is 'h' or 'H')
            return TryGetHyperbolicFn(expression, ref i, out fn);

        fn = null;
        if (expression.Length <= i + 2)
            return false;

        fn = expression[i] switch
        {
            's' or 'S' => expression[i + 1] switch
            {
                'i' or 'I' when expression[i + 2] is 'n' or 'N' => Sin,
                'e' or 'E' when expression[i + 2] is 'c' or 'C' => Sec,
                _ => null
            },
            'c' or 'C' => expression[i + 1] switch
            {
                'o' or 'O' when expression[i + 2] is 's' or 'S' => Cos,
                'o' or 'O' when expression[i + 2] is 't' or 'T' => Cot,
                's' or 'S' when expression[i + 2] is 'c' or 'C' => Csc,
                _ => null
            },
            't' or 'T' => expression[i + 1] switch
            {
                'a' or 'A' when expression[i + 2] is 'n' or 'N' => Tan,
                _ => null
            },
            _ => null
        };

        if (fn == null)
            return false;

        i += 3;
        if (expression.Length > i && expression[i] == '(')
            i++;

        return true;
    }

    private static bool TryGetHyperbolicFn(ReadOnlySpan<char> expression, ref int i,
        out Func<double, double>? fn)
    {
        fn = expression[i] switch
        {
            's' or 'S' => expression[i + 1] switch
            {
                'i' or 'I' when expression[i + 2] is 'n' or 'N' => Sinh,
                'e' or 'E' when expression[i + 2] is 'c' or 'C' => Sech,
                _ => null
            },
            'c' or 'C' => expression[i + 1] switch
            {
                'o' or 'O' when expression[i + 2] is 's' or 'S' => Cosh,
                'o' or 'O' when expression[i + 2] is 't' or 'T' => Coth,
                's' or 'S' when expression[i + 2] is 'c' or 'C' => Csch,
                _ => null
            },
            't' or 'T' => expression[i + 1] switch
            {
                'a' or 'A' when expression[i + 2] is 'n' or 'N' => Tanh,
                _ => null
            },
            _ => null
        };

        if (fn == null)
            return false;

        i += 4;
        if (expression.Length > i && expression[i] == '(')
            i++;

        return true;
    }
}