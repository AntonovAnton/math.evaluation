using System;
using System.Linq.Expressions;

namespace MathEvaluation.Entities;

/// <summary>
/// The attribute of the math operator. 
/// </summary>
/// <seealso cref="System.Attribute" />
[AttributeUsage(AttributeTargets.Field)]
internal class OperatorAttribute : Attribute
{
    /// <summary>
    /// Gets the expression type of the operator that allows improve performance if it matches a C# operator.
    /// </summary>
    /// <value>
    /// The type of the expression.
    /// </value>
    public ExpressionType ExpressionType { get; }

    /// <summary>
    /// Gets the precedence of the math operator.
    /// </summary>
    /// <value>
    /// The precedence of the math operator.
    /// </value>
    public EvalPrecedence Precedence { get; }

    /// <summary>Initializes a new instance of the <see cref="OperatorAttribute" /> class.</summary>
    /// <param name="expressionType">Type of the expression.</param>
    /// <param name="precedence">The precedence of the math operator.</param>
    public OperatorAttribute(ExpressionType expressionType, EvalPrecedence precedence)
    {
        Precedence = precedence;
        ExpressionType = expressionType;
    }
}

