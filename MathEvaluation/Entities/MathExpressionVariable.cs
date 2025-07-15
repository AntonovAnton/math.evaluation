using System;
using System.Linq.Expressions;
using System.Numerics;

namespace MathEvaluation.Entities;

/// <summary>
///     The math variable uses as a parameter, that should be evaluated as an expression.
/// </summary>
internal class MathExpressionVariable(string? key, string mathString, bool isDictinaryItem = false) : MathEntity(key)
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

        Expression right;
        if (isDictinaryItem)
        {
            // Fix: Use Expression.MakeIndex with appropriate arguments
            var dictionaryProperty = mathExpression.ParameterExpression!.Type.GetProperty("Item");
            if (dictionaryProperty == null)
                throw new InvalidOperationException("The parameter expression does not have an indexer property.");

            var keyExpression = Expression.Constant(Key);
            right = Expression.MakeIndex(mathExpression.ParameterExpression!, dictionaryProperty, [keyExpression]);
        }
        else
        {
            right = Expression.Property(mathExpression.ParameterExpression!, Key);
        }

        right = BuildConvert<TResult>(right);
        mathExpression.OnEvaluating(tokenPosition, i, right);

        right = mathExpression.BuildExponentiation<TResult>(tokenPosition, ref i, separator, closingSymbol, right);
        var expression = MathExpression.BuildMultiplyIfLeftNotDefault<TResult>(left, right);

        if (expression != right)
            mathExpression.OnEvaluating(start, i, expression);

        return expression;
    }
}