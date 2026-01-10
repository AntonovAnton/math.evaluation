using System.Linq.Expressions;
using System.Numerics;

namespace MathEvaluation.Entities;

/// <summary>
///     Math entity interface.
/// </summary>
internal interface IMathEntity
{
    /// <summary>Gets the key.</summary>
    /// <value>The key (name, notation, or symbol).</value>
    string Key { get; }

    /// <summary>
    ///     Gets the evaluation precedence.
    /// </summary>
    /// <value>
    ///     The evaluation precedence.
    /// </value>
    int Precedence { get; }

    /// <summary>
    ///     Evaluates the part in which the math entity is defined in the math expression.
    /// </summary>
    /// <param name="mathExpression">The math expression.</param>
    /// <param name="start">The starting char index of the evaluating.</param>
    /// <param name="i">The current char index.</param>
    /// <param name="separator">The parameter separator.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <returns></returns>
    /// <param name="value">The value.</param>
    TResult Evaluate<TResult>(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, TResult value)
        where TResult : struct, INumberBase<TResult>;

    /// <summary>
    ///     Builds the part of the expression tree in which the math entity is defined in the math expression.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="mathExpression">The math expression.</param>
    /// <param name="start">The starting char index of the evaluating.</param>
    /// <param name="i">The current char index.</param>
    /// <param name="separator">The parameter separator.</param>
    /// <param name="closingSymbol">The closing symbol.</param>
    /// <returns></returns>
    /// <param name="left">The expression tree of the left operand.</param>
    Expression Build<TResult>(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, Expression left)
        where TResult : struct, INumberBase<TResult>;
}