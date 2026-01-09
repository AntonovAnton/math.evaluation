using System.Linq.Expressions;
using System.Numerics;

namespace MathEvaluation.Entities;

/// <summary>
///     The math variable uses as a parameter, that should be evaluated as an expression.
/// </summary>
internal class MathExpressionVariable(string? key, string mathString) : MathEntity(key)
{
    /// <inheritdoc />
    public override int Precedence => (int)EvalPrecedence.Variable;

    /// <summary>Gets the math expression string that defineds variable.</summary>
    public string MathString => mathString;

    /// <inheritdoc />
    public override double Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, double value)
    {
        var tokenPosition = i;
        i += Key.Length;

        using var varMathExpression = new MathExpression(MathString, mathExpression.Context, mathExpression.Provider);
        varMathExpression.Evaluating += (sender, args) =>
        {
            // Forward the evaluating event to the math expression.
            mathExpression.OnEvaluating(args.Start, args.End + 1, args.Value, mathString, false);
        };
        var result = varMathExpression.Evaluate(mathExpression.Parameters);

        // Bind the variable to the math expression parameters to ensure it can be used in further evaluations.
        mathExpression.Parameters!.BindVariable(result, Key);

        mathExpression.OnEvaluating(tokenPosition, i, result);

        result = mathExpression.EvaluateExponentiation(tokenPosition, ref i, separator, closingSymbol, result);
        value = value == default ? result : value * result;

        if (value != result && !double.IsNaN(value))
            mathExpression.OnEvaluating(start, i, value);

        return value;
    }

    /// <inheritdoc />
    public override decimal Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, decimal value)
    {
        var tokenPosition = i;
        i += Key.Length;

        using var varMathExpression = new MathExpression(MathString, mathExpression.Context, mathExpression.Provider);
        varMathExpression.Evaluating += (sender, args) =>
        {
            // Forward the evaluating event to the math expression.
            mathExpression.OnEvaluating(args.Start, args.End + 1, args.Value, mathString, false);
        };
        var result = varMathExpression.EvaluateDecimal(mathExpression.Parameters);

        // Bind the variable to the math expression parameters to ensure it can be used in further evaluations.
        mathExpression.Parameters!.BindVariable(result, Key);

        mathExpression.OnEvaluating(tokenPosition, i, result);

        result = mathExpression.EvaluateExponentiationDecimal(tokenPosition, ref i, separator, closingSymbol, result);
        value = value == default ? result : value * result;

        if (value != result)
            mathExpression.OnEvaluating(start, i, value);

        return value;
    }

#if NET8_0_OR_GREATER

    public override TResult Evaluate<TResult>(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, TResult value)
    {
        var tokenPosition = i;
        i += Key.Length;

        using var varMathExpression = new MathExpression(MathString, mathExpression.Context, mathExpression.Provider);
        varMathExpression.Evaluating += (sender, args) =>
        {
            // Forward the evaluating event to the math expression.
            mathExpression.OnEvaluating(args.Start, args.End + 1, args.Value, mathString, false);
        };
        var result = varMathExpression.Evaluate<TResult>(mathExpression.Parameters);

        // Bind the variable to the math expression parameters to ensure it can be used in further evaluations.
        mathExpression.Parameters!.BindVariable(result, Key);

        mathExpression.OnEvaluating(tokenPosition, i, result);

        result = mathExpression.EvaluateExponentiation(tokenPosition, ref i, separator, closingSymbol, result);
        value = value == default ? result : value * result;

        if (value != result && !(value is Complex c && (double.IsNaN(c.Real) || double.IsNaN(c.Imaginary))))
            mathExpression.OnEvaluating(start, i, value);

        return value;
    }

#endif

    /// <inheritdoc />
    public override Complex Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, Complex value)
    {
        var tokenPosition = i;
        i += Key.Length;

        using var varMathExpression = new MathExpression(MathString, mathExpression.Context, mathExpression.Provider);
        varMathExpression.Evaluating += (sender, args) =>
        {
            // Forward the evaluating event to the math expression.
            mathExpression.OnEvaluating(args.Start, args.End + 1, args.Value, mathString, false);
        };
        var result = varMathExpression.EvaluateComplex(mathExpression.Parameters);

        // Bind the variable to the math expression parameters to ensure it can be used in further evaluations.
        mathExpression.Parameters!.BindVariable(result, Key);

        mathExpression.OnEvaluating(tokenPosition, i, result);

        result = mathExpression.EvaluateExponentiationComplex(tokenPosition, ref i, separator, closingSymbol, result);
        value = value == default ? result : value * result;

        if (value != result && !double.IsNaN(value.Real) && !double.IsNaN(value.Imaginary))
            mathExpression.OnEvaluating(start, i, value);

        return value;
    }

    /// <inheritdoc />
    public override Expression Build<TResult>(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, Expression left)
    {
        var tokenPosition = i;
        i += Key.Length;

        // If the variable is not already in the ExpressionVariables dictionary, add it.
        if (mathExpression.ExpressionVariables?.TryGetValue(Key, out var parameterExpression) != true)
        {
            mathExpression.ExpressionVariables ??= [];
            mathExpression.ExpressionStatements ??= [];

            using var varMathExpression = new MathExpression(MathString, mathExpression.Context, mathExpression.Provider);
            varMathExpression.Evaluating += (sender, args) =>
            {
                mathExpression.OnEvaluating(args.Start, args.End + 1, args.Value, mathString, false);
            };

            // Build the right-hand side expression (like: 'a + b')
            var result = varMathExpression.Build<TResult>(
                mathExpression.ParameterExpression!,
                mathExpression.Parameters!);

            // Declare the variable (like: 'var x')
            parameterExpression = Expression.Variable(typeof(TResult), Key);

            // Create an assignment expression (like: 'x = a + b')
            var assignExpr = Expression.Assign(parameterExpression, result);

            // Store the variable for later use
            mathExpression.ExpressionVariables[Key] = parameterExpression;
            mathExpression.ExpressionStatements.Add(assignExpr); // <-- assuming you have a list for body expressions

            mathExpression.OnEvaluating(tokenPosition, i, result);
        }

        var right = BuildConvert<TResult>(parameterExpression!);
        right = mathExpression.BuildExponentiation<TResult>(tokenPosition, ref i, separator, closingSymbol, right);
        var expression = MathExpression.BuildMultiplyIfLeftNotDefault<TResult>(left, right);

        if (expression != right)
            mathExpression.OnEvaluating(start, i, expression);

        return expression;
    }
}