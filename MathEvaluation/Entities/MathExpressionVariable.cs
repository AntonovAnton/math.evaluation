using System.Linq.Expressions;
using System.Numerics;

namespace MathEvaluation.Entities;

/// <summary>
///     The math variable uses as a parameter, that should be evaluated as an expression.
/// </summary>
internal class MathExpressionVariable : MathEntity
{
    private readonly string _mathString;

    /// <summary>
    ///     The math variable uses as a parameter, that should be evaluated as an expression.
    /// </summary>
    public MathExpressionVariable(string? key, string mathString) : base(key)
    {
        _mathString = mathString;
    }

    /// <inheritdoc />
    public override int Precedence => (int)EvalPrecedence.Variable;

    /// <inheritdoc />
    public override TResult Evaluate<TResult>(MathExpression mathExpression, int start, ref int i, int depth, char? separator, char? closingSymbol,
        TResult value)
    {
        var tokenPosition = i;
        i += Key.Length;

        using var varMathExpression = new MathExpression(_mathString, mathExpression.Context, mathExpression.Provider);
        varMathExpression.Evaluating += (_, args) =>
        {
            // Forward the evaluating event to the math expression.
            mathExpression.RaiseEvaluatingStep(args.Start, args.End + 1, args.Value, _mathString, true);
        };
        // The variable is evaluated on the same call stack, so it continues the recursion depth of this math expression.
        var result = varMathExpression.Evaluate<TResult>(mathExpression.Parameters, depth);

        // Bind the variable to the math expression parameters to ensure it can be used in further evaluations.
        mathExpression.Parameters!.BindVariable(result, Key);

        mathExpression.RaiseEvaluatingStep(tokenPosition, i, result);

        result = mathExpression.EvaluateExponentiation(tokenPosition, ref i, depth, separator, closingSymbol, result);
        value = value == default ? result : value * result;

        if (value != result && !(value is Complex c && (double.IsNaN(c.Real) || double.IsNaN(c.Imaginary))))
            mathExpression.RaiseEvaluatingStep(start, i, value);

        return value;
    }

    /// <inheritdoc />
    public override Expression Build<TResult>(MathExpression mathExpression, int start, ref int i, int depth, char? separator, char? closingSymbol,
        Expression left)
    {
        var tokenPosition = i;
        i += Key.Length;

        // If the variable is not already in the ExpressionVariables dictionary, add it.
        if (mathExpression.ExpressionVariables?.TryGetValue(Key, out var parameterExpression) != true)
        {
            mathExpression.ExpressionVariables ??= [];
            mathExpression.ExpressionStatements ??= [];

            using var varMathExpression = new MathExpression(_mathString, mathExpression.Context, mathExpression.Provider);
            varMathExpression.Evaluating += (_, args) =>
            {
                // Forward the evaluating event to the math expression.
                mathExpression.RaiseEvaluatingStep(args.Start, args.End + 1, args.Value, _mathString, true);
            };

            // Build the right-hand side expression (like: 'a + b'),
            // it is built on the same call stack, so it continues the recursion depth of this math expression.
            var result = varMathExpression.Build<TResult>(
                mathExpression.ParameterExpression!,
                mathExpression.Parameters!,
                depth);

            // Declare the variable (like: 'var x')
            parameterExpression = Expression.Variable(typeof(TResult), Key);

            // Create an assignment expression (like: 'x = a + b')
            var assignExpr = Expression.Assign(parameterExpression, result);

            // Store the variable for later use
            mathExpression.ExpressionVariables[Key] = parameterExpression;
            mathExpression.ExpressionStatements.Add(assignExpr); // <-- assuming you have a list for body expressions

            mathExpression.RaiseEvaluatingStep(tokenPosition, i, result);
        }

        var right = BuildConvert<TResult>(parameterExpression!);
        right = mathExpression.BuildExponentiation<TResult>(tokenPosition, ref i, depth, separator, closingSymbol, right);
        var expression = MathExpression.BuildMultiplyIfLeftNotDefault<TResult>(left, right);

        if (expression != right)
            mathExpression.RaiseEvaluatingStep(start, i, expression);

        return expression;
    }
}