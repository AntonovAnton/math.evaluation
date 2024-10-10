using System;
using System.Linq.Expressions;

namespace MathEvaluation.Entities;

/// <summary>
/// The specified compatible operator allows improve performance.
/// </summary>
[Serializable]
public enum OperatorType : int
{
    /// <summary>
    /// The conditional OR operator.
    /// </summary>
    [Operator(ExpressionType.OrElse, EvalPrecedence.LogicalConditionalOr)]
    LogicalConditionalOr = 1,

    /// <summary>
    /// The conditional AND operator.
    /// </summary>
    [Operator(ExpressionType.AndAlso, EvalPrecedence.LogicalConditionalAnd)]
    LogicalConditionalAnd = 2,

    /// <summary>
    /// The logical OR operator.
    /// </summary>
    [Operator(ExpressionType.OrElse, EvalPrecedence.LogicalOr)]
    LogicalOr = 3,

    /// <summary>
    /// The bitwise OR operator.
    /// </summary>
    [Operator(ExpressionType.Or, EvalPrecedence.BitwiseOr)]
    BitwiseOr = 4,

    /// <summary>
    /// The logical XOR operator.
    /// </summary>
    [Operator(ExpressionType.ExclusiveOr, EvalPrecedence.LogicalXor)]
    LogicalXor = 5,

    /// <summary>
    /// The bitwise XOR operator.
    /// </summary>
    [Operator(ExpressionType.ExclusiveOr, EvalPrecedence.BitwiseXor)]
    BitwiseXor = 6,

    /// <summary>
    /// The logical AND operator.
    /// </summary>
    [Operator(ExpressionType.AndAlso, EvalPrecedence.LogicalAnd)]
    LogicalAnd = 7,

    /// <summary>
    /// The bitwise AND operator.
    /// </summary>
    [Operator(ExpressionType.And, EvalPrecedence.BitwiseAnd)]
    BitwiseAnd = 8,

    /// <summary>
    /// The logical NOT operator. In Visual Basic, it is equivalent to (not a)
    /// </summary>
    [Operator(ExpressionType.Not, EvalPrecedence.LogicalNot)]
    LogicalNot = 9,

    /// <summary>
    /// The unary negation operator. In C#, it is equivalent to (!a) for Boolean values.
    /// </summary>
    [Operator(ExpressionType.Not, EvalPrecedence.OperandUnaryOperator)]
    LogicalNegation = 10,

    /// <summary>
    /// The unary negation operator. In C#, it is equivalent to (~a) for integral types.
    /// </summary>
    [Operator(ExpressionType.Not, EvalPrecedence.OperandUnaryOperator)]
    BitwiseNegation = 11,

    /// <summary>
    /// The equality comparison operator, such as (a == b) in C# or (a = b) in Visual Basic.
    /// </summary>
    [Operator(ExpressionType.Equal, EvalPrecedence.Equality)]
    Equal = 12,

    /// <summary>
    /// The inequality comparison operator, such as (a == b) in C# or (a = b) in Visual Basic.
    /// </summary>
    [Operator(ExpressionType.NotEqual, EvalPrecedence.Equality)]
    NotEqual = 13,

    /// <summary>
    /// A "less than" comparison, such as (a &lt; b).
    /// </summary>
    [Operator(ExpressionType.LessThan, EvalPrecedence.RelationalOperator)]
    LessThan = 14,

    /// <summary>
    /// A "less than or equal to" comparison, such as (a &lt;= b).
    /// </summary>
    [Operator(ExpressionType.LessThanOrEqual, EvalPrecedence.RelationalOperator)]
    LessThanOrEqual = 15,

    /// <summary>
    /// A "greater than" comparison, such as (a &gt; b).
    /// </summary>
    [Operator(ExpressionType.GreaterThan, EvalPrecedence.RelationalOperator)]
    GreaterThan = 16,

    /// <summary>
    /// A "greater than or equal to" comparison, such as (a &gt;= b).
    /// </summary>
    [Operator(ExpressionType.GreaterThanOrEqual, EvalPrecedence.RelationalOperator)]
    GreaterThanOrEqual = 17,

    /// <summary>
    /// A multiplication operation, such as (a * b), without overflow checking, for numeric operands.
    /// </summary>
    [Operator(ExpressionType.Multiply, EvalPrecedence.Basic)]
    Multiply = 18,

    /// <summary>
    /// A division operation, such as (a / b), for numeric operands.
    /// </summary>
    [Operator(ExpressionType.Divide, EvalPrecedence.Basic)]
    Divide = 19,

    /// <summary>
    /// An addition operation, such as (a + b), without overflow checking, for numeric operands.
    /// </summary>
    [Operator(ExpressionType.Add, EvalPrecedence.Basic)]
    Add = 20,

    /// <summary>
    /// A substraction operation, such as (a - b), without overflow checking, for numeric operands.
    /// </summary>
    [Operator(ExpressionType.Subtract, EvalPrecedence.Basic)]
    Subtract = 21,

    /// <summary>
    /// An arithmetic remainder operation, such as (a % b) in C# or (a Mod b) in Visual Basic.
    /// </summary>
    [Operator(ExpressionType.Modulo, EvalPrecedence.Basic)]
    Modulo = 22,

    /// <summary>
    /// An arithmetic remainder operation, such as (a % b) in C# or (a Mod b) in Visual Basic.
    /// </summary>
    [Operator(ExpressionType.Power, EvalPrecedence.Exponentiation)]
    Power = 23,

    /// <summary>
    /// An arithmetic negation operation, such as (-a).
    /// </summary>
    [Operator(ExpressionType.Negate, EvalPrecedence.Basic)]
    Negate = 24,
}
