using System;

namespace MathEvaluation;

public static class MathFunctions
{
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
                'i' or 'I' when expression[i + 2] is 'n' or 'N' => Sine,
                'e' or 'E' when expression[i + 2] is 'c' or 'C' => Secant,
                _ => null
            },
            'c' or 'C' => expression[i + 1] switch
            {
                'o' or 'O' when expression[i + 2] is 's' or 'S' => Cosine,
                'o' or 'O' when expression[i + 2] is 't' or 'T' => Cotangent,
                's' or 'S' when expression[i + 2] is 'c' or 'C' => Cosecant,
                _ => null
            },
            't' or 'T' => expression[i + 1] switch
            {
                'a' or 'A' when expression[i + 2] is 'n' or 'N' => Tangent,
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
        //TODO: Hyperbolic
        fn = null;
        return false;
    }

    public static double Sine(double a)
    {
        return Math.Sin(a);
    }

    public static double Cosine(double a)
    {
        return Math.Cos(a);
    }

    public static double Tangent(double a)
    {
        var cos = Math.Cos(a);
        if (cos == 0d)
            return double.NaN;

        return Math.Sin(a) / cos;
    }

    public static double Cosecant(double a)
    {
        var sin = Math.Sin(a);
        if (sin == 0d)
            return double.NaN;

        return 1 / sin;
    }

    public static double Secant(double a)
    {
        var cos = Math.Cos(a);
        if (cos == 0d)
            return double.NaN;

        return 1 / cos;
    }

    public static double Cotangent(double a)
    {
        var sin = Math.Sin(a);
        if (sin == 0d)
            return double.NaN;

        return Math.Cos(a) / sin;
    }

    public static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0d;
    }
}