using MathEvaluation.Context;
using MathEvaluation.Extensions;
using MathEvaluation.Parameters;

namespace MathEvaluation.Tests.Compilation;

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
    [InlineData("1.0 >= 0.1 and 5.4 <= 5.4", 1.0 >= 0.1 & 5.4 <= 5.4)]
    [InlineData("1 > -0 And 2 < 3 Or 2 > 1", 1 > -0 && 2 < 1 || 2 > 1)]
    [InlineData("5.4 < 5.4", 5.4 < 5.4)]
    [InlineData("1.0 > 1.0 + -0.7 AND 5.4 < 5.5", 1.0 > 1.0 + -0.7 && 5.4 < 5.5)]
    [InlineData("1.0 - 1.95 >= 0.1", 1.0 - 1.95 >= 0.1)]
    [InlineData("2 ** 3 = 8", true)]
    [InlineData("3 % 2 <> 1.1", true)]
    [InlineData("4 <> 4 OR 5.4 = 5.4", true)]
    [InlineData("4 <> 4 OR 5.4 = 5.4 AND NOT true", false)]
    [InlineData("4 <> 4 OR 5.4 = 5.4 AND NOT 0 < 1 XOR 1.0 - 1.95 * 2 >= -12.9 + 0.1 / 0.01", true)]
    public void MathExpression_CompileBooleanThenInvoke_HasProgrammingBooleanLogic_ExpectedValue(string expression, bool expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var fn = new MathExpression(expression, _programmingContext).CompileBoolean();
        var value = fn();

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
    [InlineData("1 > -0 And 2 < 3 Or 2 > 1", 1 > -0 && 2 < 1 || 2 > 1)]
    [InlineData("5.4 < 5.4", 5.4 < 5.4)]
    [InlineData("1.0 > 1.0 + -0.7 AND 5.4 < 5.5", 1.0 > 1.0 + -0.7 && 5.4 < 5.5)]
    [InlineData("1.0 - 1.95 ⪰ 0.1", 1.0 - 1.95 >= 0.1)]
    [InlineData("2^3 = 8", true)]
    [InlineData("3 mod 2 ≠ 1.1", true)]
    [InlineData("4 ≠ 4 OR 5.4 = 5.4", true)]
    [InlineData("4 ≠ 4 OR 5.4 = 5.4 AND NOT true", false)]
    [InlineData("4 ≠ 4 OR 5.4 = 5.4 AND NOT 0 < 1 XOR 1.0 - 1.95 * 2 ⪰ -12.9 + 0.1 / 0.01", true)]
    public void MathExpression_CompileBooleanThenInvoke_HasEngineeringBooleanLogic_ExpectedValue(string expression, bool expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var fn = new MathExpression(expression, _scientificContext).CompileBoolean();
        var value = fn();

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
    public void MathExpression_CompileBooleanThenInvoke_HasScientificBooleanLogic_ExpectedValue(string expression, bool expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var fn = new MathExpression(expression, _scientificContext).CompileBoolean();
        var value = fn();

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("A or not B and C", false, false, true, true)]
    [InlineData("A or not B and C", true, false, true, true)]
    [InlineData("A or not B and C", false, true, true, false)]
    [InlineData("A or not B and C", true, false, false, true)]
    [InlineData("A or not B and (C or B)", true, false, true, true)]
    [InlineData("A or not B and (C or B)", false, true, false, false)]
    public void MathExpression_CompileBooleanThenInvoke_HasVariables_ExpectedValue(string expression, bool a, bool b, bool c, bool expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var getC = () => c;
        var fn = expression.CompileBoolean(new { A = a, B = b, C = getC }, _programmingContext);
        var value = fn(new { A = a, B = b, C = getC });

        Assert.Equal(expectedValue, value);
    }

    [Theory]
    [InlineData("if(3 % a = 1, true, false)", 2d, true)]
    [InlineData("if(3 % a = 1, true, false)", 1d, false)]
    public void MathExpression_CompileBooleanThenInvoke_HasCustomFunction_ExpectedValue(string expression, double a, bool expectedValue)
    {
        testOutputHelper.WriteLine($"{expression} = {expectedValue}");

        var context = new ProgrammingMathContext();
        context.BindFunction((c, v1, v2) => c != 0.0 ? v1 : v2, "if");

        var fn = expression.CompileBoolean(new { a }, context);
        var value = fn(new { a });

        Assert.Equal(expectedValue, value);
    }
}