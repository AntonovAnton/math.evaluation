using System;
using System.Linq.Expressions;

namespace MathEvaluation.Entities;

/// <summary>
/// Math entity interface.
/// </summary>
public interface IMathEntity
{
    /// <summary>Gets the key.</summary>
    /// <value>The key (name, notation, or symbol).</value>
    string Key { get; }

    /// <summary>
    /// Gets the evaluation precedence.
    /// </summary>
    /// <value>
    /// The evaluation precedence.
    /// </value>
    int Precedence { get; }

    /// <summary>
    /// Evaluates the part in which the math entity is defined in the math expression.
    /// </summary>
    /// <param name="mathExpression">The math expression.</param>
    /// <param name="i">The current char index.</param>
    /// <param name="separator">The parameter separator.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    double Evaluate(MathExpression mathExpression, ref int i, char? separator, char? closingSymbol, double value);

    /// <inheritdoc cref="Evaluate(MathExpression, ref int, char?, char?, double)"/>
    decimal Evaluate(MathExpression mathExpression, ref int i, char? separator, char? closingSymbol, decimal value);

    /// <summary>
    /// Builds the part of the expression tree in which the math entity is defined in the math expression.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="mathExpression">The math expression.</param>
    /// <param name="i">The current char index.</param>
    /// <param name="separator">The parameter separator.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <param name="left">The expression tree of the left operand.</param>
    /// <returns></returns>
    Expression Build<TResult>(MathExpression mathExpression, ref int i, char? separator, char? closingSymbol, Expression left)
        where TResult : struct, IConvertible;
}
