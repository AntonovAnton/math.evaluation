﻿using MathEvaluation.Context;
using MathEvaluation.Extensions;
using System.Globalization;
// ReSharper disable EqualExpressionComparison
// ReSharper disable RedundantLogicalConditionalExpressionOperand

namespace MathEvaluation.FastExpressionCompiler.Tests.Compilation;

public partial class MathExpressionTests
{
    [Theory]
    [InlineData("false = true", false)]
    [InlineData("not(False)", true)]
    [InlineData("FALSE <> True", true)]
    [InlineData("false or TRUE", true)]
    [InlineData("True xor True", false)]
    [InlineData("200 >= 2.4", 200 >= 2.4)]
    [InlineData("200 <= 2.4", 200 <= 2.4)]
    [InlineData("1.0 >= 0.1 and 5.4 <= 5.4", (1.0 >= 0.1) & (5.4 <= 5.4))]
    [InlineData("1 > -0 And 2 < 3 Or 2 > 1", (1 > -0 && 2 < 1) || 2 > 1)]
    [InlineData("5.4 < 5.4", 5.4 < 5.4)]
    [InlineData("1.0 > 1.0 + -0.7 AND 5.4 < 5.5", 1.0 > 1.0 + -0.7 && 5.4 < 5.5)]
    [InlineData("1.0 - 1.95 >= 0.1", 1.0 - 1.95 >= 0.1)]
    [InlineData("2 ** 3 = 8", true)]
    [InlineData("3 % 2 <> 1.1", true)]
    [InlineData("4 <> 4 OR 5.4 = 5.4", true)]
    [InlineData("4 <> 4 OR 5.4 = 5.4 AND NOT true", false)]
    [InlineData("4 <> 4 OR 5.4 = 5.4 AND NOT 0 < 1 XOR 1.0 - 1.95 * 2 >= -12.9 + 0.1 / 0.01", true)]
    public void FastMathExpression_CompileBooleanThenInvoke_HasProgrammingBooleanLogic_ExpectedValue(string mathString, bool expectedValue)
    {
        using var expression = new FastMathExpression(mathString, _programmingContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileBoolean();
        var value = fn();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("false = true", false)]
    [InlineData("not(False)", true)]
    [InlineData("FALSE ≠ True", true)]
    [InlineData("false or TRUE", true)]
    [InlineData("True xor True", false)]
    [InlineData("200 ≥ 2.4", 200 >= 2.4)]
    [InlineData("200 ≤ 2.4", 200 <= 2.4)]
    [InlineData("1.0 ⪯ 0.1 and 5.4 ≤ 5.4", 1.0 <= 0.1 && 5.4 <= 5.4)]
    [InlineData("1 > -0 And 2 < 3 Or 2 > 1", (1 > -0 && 2 < 1) || 2 > 1)]
    [InlineData("5.4 < 5.4", 5.4 < 5.4)]
    [InlineData("1.0 > 1.0 + -0.7 AND 5.4 < 5.5", 1.0 > 1.0 + -0.7 && 5.4 < 5.5)]
    [InlineData("1.0 - 1.95 ⪰ 0.1", 1.0 - 1.95 >= 0.1)]
    [InlineData("2^3 = 8", true)]
    [InlineData("3 mod 2 ≠ 1.1", true)]
    [InlineData("4 ≠ 4 OR 5.4 = 5.4", true)]
    [InlineData("4 ≠ 4 OR 5.4 = 5.4 AND NOT true", false)]
    [InlineData("4 ≠ 4 OR 5.4 = 5.4 AND NOT 0 < 1 XOR 1.0 - 1.95 * 2 ⪰ -12.9 + 0.1 / 0.01", true)]
    public void FastMathExpression_CompileBooleanThenInvoke_HasEngineeringBooleanLogic_ExpectedValue(string mathString, bool expectedValue)
    {
        using var expression = new FastMathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileBoolean();
        var value = fn();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("¬F", true)]
    [InlineData("¬⊥", true)]
    [InlineData("¬T", false)]
    [InlineData("¬⊤", false)]
    [InlineData("T ≡ ¬F", true)]
    [InlineData("T ≢ F", true)]
    [InlineData("F ↔ F", true)]
    [InlineData("T ↔ F", false)]
    [InlineData("(T→F)∧(F→T)", false)]
    [InlineData("(((¬F)∧T)∨¬T)→¬T", false)]
    [InlineData("F∧T∨¬T", false)]
    [InlineData("¬F∧T∨¬T", true)]
    [InlineData("¬F∧T∨¬T→¬T", false)]
    [InlineData("F∧¬T∨¬T", false)]
    [InlineData("F∧¬T∨¬T→¬T", true)]
    [InlineData("(((F)∧T)∨¬T)→T", true)]
    [InlineData("(((F)∧T)∨¬T)⇒T", true)]
    [InlineData("(((F)∧T)∨¬T)←T", false)]
    [InlineData("F∧T∨¬T←T", false)]
    [InlineData("F∧T∨¬T⟸T", false)]
    [InlineData("F∧T∨¬T←¬T", true)]
    [InlineData("F∧T∨¬T⟸¬T", true)]
    [InlineData("0∧1∨¬1⟸¬1", true)]
    [InlineData("0.0∧1.0∨¬1.0⟸¬1.0", true)]
    [InlineData("0.0∧1.0∨¬1.0←¬1.0 ≢ F∧¬T∨¬T→T", false)]
    [InlineData("0.0∧1.0∨¬1.0←¬1.0 ≢ ¬F∧T∨¬T→¬T", true)]
    [InlineData("0.0∧1.0∨¬1.0←¬1.0 ≡ F∧¬T∨¬T→¬T", true)]
    [InlineData("(F∧¬T∨¬T→¬T ≡ ¬F∧T∨¬T→¬T)⇔F", true)]
    [InlineData("F∧¬T∨¬T→¬T ≡ ¬F∧T∨¬T→¬T⇔F", true)]
    [InlineData("F∧¬T∨¬T→¬T ≢ ¬F∧T∨¬T→¬T↔F", false)]
    [InlineData("F∧¬T∨¬T→¬T ≡ ¬F∧T∨¬T→¬T⇎F", false)]
    [InlineData("F∧¬T∨¬T→¬T ≡ ¬F∧T∨¬T→¬T ↮ 4 ≠ 4", false)]
    [InlineData("F∧¬T∨¬T→¬T ≡ ¬F∧T∨¬T→¬T ↮ 4 = 4", true)]
    [InlineData("¬F∧T∨¬T←¬T", true)]
    [InlineData("T⊕T", false)]
    [InlineData("F∧¬T∨¬T→¬T ≡ ¬F∧T∨¬T←¬T ⇎ 1 ⪰ 1 ⇔ 1 ⪯ 0", true)]
    [InlineData("F∧¬T∨¬T→¬T ≡ ¬F∧T∨¬T←¬T ⇎ 1 ≥ 1 ⇔ 1 ≤ 0", true)]
    [InlineData("F∧¬T∨¬T→¬T ≡ ¬F∧T∨¬T←¬T ⇎ 1 ≥ 1 ⇔ 1 ⊕ 1", true)]
    [InlineData("F∧¬T∨¬T→¬T ≡ ¬F∧T∨¬T←¬T ⇎ 1 > 0 ⇔ 1 ⊕ 0", false)]
    [InlineData("F∧¬T∨¬T→¬T ≡ ¬F∧T∨¬T←¬T ⇎ 1 < 0 ⇔ 1 ⊕ 0", true)]
    [InlineData("F∧¬T∨¬T→¬T ≡ ¬F∧T∨¬T←¬T ⇎ F ⇎ T⊕T", true)]
    [InlineData("F∧¬T∨¬T→¬T ≡ ¬F∧T∨¬T←¬T ↮ F ↮ T⊕T", true)]
    [InlineData("F ∨ T ∧ ¬T ⊕ F", false)]
    [InlineData("F ∨ T ∧ ¬F ⊕ T", false)]
    [InlineData("F ∨ T ∧ ¬T ⊕ T", true)]
    [InlineData("F ∨ T ∧ ¬T ⊕ ¬(F ⇎ F)", true)]
    [InlineData("¬F∧T∨¬T→¬T ≡ F∨T∧¬T⊕¬(F ↮ F) ↔ F", true)]
    [InlineData("¬⊥∧⊤∨¬⊤⇒¬⊤ ≡ ⊥∨⊤∧¬⊤⊕¬(⊥ ⇎ ⊥) ⇔ ⊥", true)]
    [InlineData("F ∨ T ∧ ¬(F < T) ⊕ F ≥ F", true)]
    [InlineData("4 ≠ 4 ∨ 5.4 = 5.4 ∧ ¬(0 < 1) ⊕ 1.0 - 1.95 * 2 ≥ -12.9 + 0.1 / 0.01", true)]
    public void FastMathExpression_CompileBooleanThenInvoke_HasScientificBooleanLogic_ExpectedValue(string mathString, bool expectedValue)
    {
        using var expression = new FastMathExpression(mathString, _scientificContext, CultureInfo.InvariantCulture);
        expression.Evaluating += SubscribeToEvaluating;

        var fn = expression.CompileBoolean();
        var value = fn();

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("A or not B and C", false, false, true, true)]
    [InlineData("A or not B and C", true, false, true, true)]
    [InlineData("A or not B and C", false, true, true, false)]
    [InlineData("A or not B and C", true, false, false, true)]
    [InlineData("A or not B and (C or B)", true, false, true, true)]
    [InlineData("A or not B and (C or B)", false, true, false, false)]
    public void FastMathExpression_CompileBooleanThenInvoke_HasVariables_ExpectedValue(string expression, bool a, bool b, bool c, bool expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var getC = () => c;
        var fn = expression.CompileBooleanFast(new { A = a, B = b, C = getC }, _programmingContext);
        var value = fn(new { A = a, B = b, C = getC });

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("if(3 % a = 1, true, false)", 2d, true)]
    [InlineData("if(3 % a = 1, true, false)", 1d, false)]
    public void FastMathExpression_CompileBooleanThenInvoke_HasCustomFunction_ExpectedValue(string expression, double a, bool expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var context = new ProgrammingMathContext();
        context.BindFunction((c, v1, v2) => c != 0.0 ? v1 : v2, "if");

        var fn = expression.CompileBooleanFast(new { a }, context);
        var value = fn(new { a });

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("IIF(3 % a = 1)", 2d, true)]
    [InlineData("Iif(3 % a = 1, true)", 1d, false)]
    [InlineData("iif(3 % a = 1, false, true)", 1d, true)]
    public void FastMathExpression_CompileBooleanThenInvoke_HasIif_ExpectedValue(string expression, double a, bool expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var fn = expression.CompileBooleanFast(new { a }, _programmingContext);
        var value = fn(new { a });

        testOutputHelper.WriteLine($"result: {value}");

        Assert.Equal(expectedValue, value);
    }
}