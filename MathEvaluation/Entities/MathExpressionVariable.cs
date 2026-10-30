using System.Diagnostics.CodeAnalysis;
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
    /// Stores the evaluated variable for later use to avoid re-evaluating the same expression multiple times.
    /// </summary>
    private IMathEntity? _evaluatedVariable;

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
        if (_evaluatedVariable != null)
        {
            // Use the pre-evaluated variable if available.
            return _evaluatedVariable.Evaluate(mathExpression, start, ref i, depth, separator, closingSymbol, value);
        }

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

        // Store the evaluated variable for later use
        _evaluatedVariable = new MathVariable<TResult>(Key, result);

        mathExpression.RaiseEvaluatingStep(tokenPosition, i, result);

        result = mathExpression.EvaluateExponentiation(tokenPosition, ref i, depth, separator, closingSymbol, result);
        value = value == default ? result : value * result;

        if (value != result && !(value is Complex c && (double.IsNaN(c.Real) || double.IsNaN(c.Imaginary))))
            mathExpression.RaiseEvaluatingStep(start, i, value);

        return value;
    }

    /// <inheritdoc />
    [RequiresUnreferencedCode("Building expressions requires reflection which may access trimmed properties")]
    [RequiresDynamicCode("Building expressions in some cases requires dynamic code generation")]
    public override Expression Build<TResult>(MathExpression mathExpression, int start, ref int i, int depth, char? separator, char? closingSymbol,
        Expression left)
    {
        var tokenPosition = i;
        i += Key.Length;

        // If the variable is not already assigned by the statements, add it.
        mathExpression.Statements ??= new();
        if (!mathExpression.Statements.TryGetVariable(Key, out var parameterExpression))
        {
            using var varMathExpression = new MathExpression(_mathString, mathExpression.Context, mathExpression.Provider);
            varMathExpression.ParameterExpression = mathExpression.ParameterExpression;
            // Share the statements, so the variables and awaits of the right-hand side are executed before this assignment.
            varMathExpression.Statements = mathExpression.Statements;
            varMathExpression.Evaluating += (_, args) =>
            {
                // Forward the evaluating event to the math expression.
                mathExpression.RaiseEvaluatingStep(args.Start, args.End + 1, args.Value, _mathString, true);
            };

            // Build the right-hand side expression (like: 'a + b'),
            // it is built on the same call stack, so it continues the recursion depth of this math expression.
            var result = varMathExpression.Build<TResult>(mathExpression.Parameters, depth);

            // Declare the variable and create an assignment expression (like: 'x = a + b')
            parameterExpression = mathExpression.Statements.AddAssignment(Key, result);

            mathExpression.RaiseEvaluatingStep(tokenPosition, i, result);
        }

        var right = BuildConvert<TResult>(parameterExpression);
        right = mathExpression.BuildExponentiation<TResult>(tokenPosition, ref i, depth, separator, closingSymbol, right);
        var expression = MathExpression.BuildMultiplyIfLeftNotDefault<TResult>(left, right);

        if (expression != right)
            mathExpression.RaiseEvaluatingStep(start, i, expression);

        return expression;
    }
}