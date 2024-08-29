using System;

namespace MathEvaluation.Entities
{
    /// <summary>
    /// Enumering precedence of math evaluation.
    /// </summary>
    [Serializable]
    public enum EvalPrecedence : int
    {
        /// <summary>
        /// The unknown precedence.
        /// </summary>
        Unknown = int.MinValue,

        /// <summary>
        /// The precedence of the congruence or equivalence operator ≡. 
        /// It can indicate that two expressions are equivalent in a specific context (e.g., modulo arithmetic, logical equivalence).
        /// </summary>
        Equivalence = -1000,

        /// <summary>
        /// The precedence of the biconditional logical equivalence operator ⇔. 
        /// It can indicate that both sides imply each other; if one is true, the other must be true, and vice versa.
        /// </summary>
        BiconditionalLogicalEquivalence = -900,

        /// <summary>
        /// The precedence of the logical implication operator → or ←. 
        /// It returns false only if the first operand (antecedent) is true and the second operand (consequent) is false; otherwise, it returns true.
        /// </summary>
        LogicalImplication = -800,

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
        RelationalOperator = -100,

        /// <summary>
        /// The precedence of equality and inequality operators.
        /// </summary>
        Equality = RelationalOperator,

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
        Variable = 300,

        /// <summary>
        /// The precedence of the exponentiation operation.
        /// </summary>
        Exponentiation = 400,

        /// <summary>
        /// The precedence of a math operator that performs an action on one math operand. 
        /// For example, degrees, decrement, increment, factorial, or negation.
        /// </summary>
        OperandUnaryOperator = 500,
    }
}
