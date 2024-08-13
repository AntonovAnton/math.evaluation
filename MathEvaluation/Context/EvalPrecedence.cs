using System;

namespace MathEvaluation.Context
{
    /// <summary>
    /// Enumering precedence of math evaluation
    /// </summary>
    [Serializable]
    public enum EvalPrecedence : int
    {
        /// <summary>
        /// The unknown precedence.
        /// </summary>
        Unknown = int.MinValue,

        /// <summary>
        /// The precedence of the logical conditional OR operator ||.
        /// </summary>
        LogicalConditionalOr = -700,

        /// <summary>
        /// The precedence of the logical conditional AND operator.
        /// </summary>
        LogicalConditionalAnd = -600,

        /// <summary>
        /// The precedence of the logical OR operator |.
        /// </summary>
        LogicalOr = -500,

        /// <summary>
        /// The precedence of the logical XOR operator ^.
        /// </summary>
        LogicalXor = -400,

        /// <summary>
        /// The precedence of the logical AND operator.
        /// </summary>
        LogicalAnd = -300,

        /// <summary>
        /// The precedence of the logical negation NOT operator.
        /// </summary>
        LogicalNot = -200,

        /// <summary>
        /// The precedence of the &lt; (less than), 
        /// &gt; (greater than), 
        /// &lt;= (less than or equal), 
        /// and &gt;= (greater than or equal) comparison, 
        /// also known as relational, operators compare their operands.
        /// </summary>
        Comparison = -100,

        /// <summary>
        /// The precedence of equality and inequality operators.
        /// </summary>
        Equality = Comparison,

        /// <summary>
        /// The precedence of arithmetic addition and subtraction operators.
        /// </summary>
        LowestBasic = 0,

        /// <summary>
        /// The precedence of arithmetic multiplication, division, and modulus operators.
        /// </summary>
        Basic = 100,

        /// <summary>
        /// The precedence of a math function.
        /// </summary>
        Function = 200,

        /// <summary>
        /// The precedence of a variable.
        /// </summary>
        Variable = 250,

        /// <summary>
        /// The precedence of the exponentiation operation.
        /// </summary>
        Exponentiation = 300,

        /// <summary>
        /// The precedence of converting a math operand to another value 
        /// (for example, converting degrees to radians).
        /// </summary>
        Convertation = 400,
    }
}
