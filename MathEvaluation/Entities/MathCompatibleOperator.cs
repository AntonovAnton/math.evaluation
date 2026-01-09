using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;

namespace MathEvaluation.Entities;

/// <summary>
///     The math operator processes the left and right expressions.
/// </summary>
internal class MathCompatibleOperator : MathEntity
{
    private static readonly Dictionary<OperatorType, ExpressionType> ExpressionTypeByOperatorType = [];
    private static readonly Dictionary<OperatorType, EvalPrecedence> EvalPrecedenceByOperatorType = [];

    private readonly bool _isProcessingOperand;

    /// <summary>Gets the type of the expression node.</summary>
    /// <value>The type of the expression node.</value>
    public ExpressionType ExpressionType { get; }

    /// <inheritdoc />
    public override int Precedence { get; }

    /// <summary>Gets the type of the operator.</summary>
    /// <value>The type of the operator.</value>
    public OperatorType OperatorType { get; }

    static MathCompatibleOperator()
    {
        foreach (var fieldInfo in typeof(OperatorType).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var attributes = fieldInfo.GetCustomAttributes(typeof(OperatorAttribute), false);
            var operatorAttribute = (OperatorAttribute?)attributes.FirstOrDefault(a => a is OperatorAttribute)
                                    ?? throw new Exception($"The Operator attribute isn't set for {fieldInfo.Name}.");

            var expressionType = operatorAttribute.ExpressionType;
            var precedence = operatorAttribute.Precedence;

            var operatorType = Enum.Parse<OperatorType>(fieldInfo.Name);

            ExpressionTypeByOperatorType[operatorType] = expressionType;
            EvalPrecedenceByOperatorType[operatorType] = precedence;
        }
    }

    /// <summary>Initializes a new instance of the <see cref="MathOperator{T}" /> class.</summary>
    /// <param name="key">The key (the operator notation).</param>
    /// <param name="operatorType">
    ///     The specified type of the operator allows to improve performance if it matches a C#
    ///     operator.
    /// </param>
    /// <exception cref="ArgumentNullException" />
    public MathCompatibleOperator(string? key, OperatorType operatorType)
        : base(key)
    {
        OperatorType = operatorType;
        ExpressionType = ExpressionTypeByOperatorType[operatorType];

        var precedence = EvalPrecedenceByOperatorType[operatorType];
        Precedence = (int)precedence;
        _isProcessingOperand = precedence is EvalPrecedence.OperandUnaryOperator or EvalPrecedence.Exponentiation;
    }

    /// <inheritdoc />
    public override double Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, double left)
    {
        var tokenPosition = i;
        if (OperatorType is OperatorType.LogicalNot or OperatorType.BitwiseNegation)
            start = tokenPosition;

        i += Key.Length;
        var right = _isProcessingOperand
            ? mathExpression.EvaluateOperand(ref i, separator, closingSymbol)
            : mathExpression.Evaluate(ref i, separator, closingSymbol, Precedence);

        if (_isProcessingOperand)
        {
            //for case such as 2^3^2 we should evaluate first 3^2, so start position = 1 + 1 in this example.
            var startExponentiation = OperatorType == OperatorType.Power ? tokenPosition + Key.Length : start;
            right = mathExpression.EvaluateExponentiation(startExponentiation, ref i, separator, closingSymbol, right);
        }

        var value = Calculate(OperatorType, left, right);

        mathExpression.OnEvaluating(start, i, value);
        return value;
    }

    /// <inheritdoc />
    public override decimal Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, decimal left)
    {
        var tokenPosition = i;
        if (OperatorType is OperatorType.LogicalNot or OperatorType.BitwiseNegation)
            start = tokenPosition;

        i += Key.Length;
        var right = _isProcessingOperand
            ? mathExpression.EvaluateOperandDecimal(ref i, separator, closingSymbol)
            : mathExpression.EvaluateDecimal(ref i, separator, closingSymbol, Precedence);

        if (_isProcessingOperand)
        {
            //for case such as 2^3^2 we should evaluate first 3^2, so start position = 1 + 1 in this example.
            var startExponentiation = OperatorType == OperatorType.Power ? tokenPosition + Key.Length : start;
            right = mathExpression.EvaluateExponentiationDecimal(startExponentiation, ref i, separator, closingSymbol, right);
        }

        var value = Calculate(OperatorType, left, right);

        mathExpression.OnEvaluating(start, i, value);
        return value;
    }

#if NET8_0_OR_GREATER

    /// <inheritdoc />
    public override TResult Evaluate<TResult>(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, TResult left)
    {
        var tokenPosition = i;
        if (OperatorType is OperatorType.LogicalNot or OperatorType.BitwiseNegation)
            start = tokenPosition;

        i += Key.Length;
        var right = _isProcessingOperand
            ? mathExpression.EvaluateOperand<TResult>(ref i, separator, closingSymbol)
            : mathExpression.Evaluate<TResult>(ref i, separator, closingSymbol, Precedence);

        if (_isProcessingOperand)
        {
            //for case such as 2^3^2 we should evaluate first 3^2, so start position = 1 + 1 in this example.
            var startExponentiation = OperatorType == OperatorType.Power ? tokenPosition + Key.Length : start;
            right = mathExpression.EvaluateExponentiation(startExponentiation, ref i, separator, closingSymbol, right);
        }

        var value = Calculate(OperatorType, left, right);

        mathExpression.OnEvaluating(start, i, value);
        return value;
    }

#endif

    /// <inheritdoc />
    public override Complex Evaluate(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, Complex left)
    {
        var tokenPosition = i;
        if (OperatorType is OperatorType.LogicalNot or OperatorType.BitwiseNegation)
            start = tokenPosition;

        i += Key.Length;
        var right = _isProcessingOperand
            ? mathExpression.EvaluateOperandComplex(ref i, separator, closingSymbol)
            : mathExpression.EvaluateComplex(ref i, separator, closingSymbol, Precedence);

        if (_isProcessingOperand)
        {
            //for case such as 2^3^2 we should evaluate first 3^2, so start position = 1 + 1 in this example.
            var startExponentiation = OperatorType == OperatorType.Power ? tokenPosition + Key.Length : start;
            right = mathExpression.EvaluateExponentiationComplex(startExponentiation, ref i, separator, closingSymbol, right);
        }

        var value = Calculate(OperatorType, left, right);

        mathExpression.OnEvaluating(start, i, value);
        return value;
    }

    /// <inheritdoc />
    public override Expression Build<TResult>(MathExpression mathExpression, int start, ref int i, char? separator, char? closingSymbol, Expression left)
    {
        var tokenPosition = i;
        if (OperatorType is OperatorType.LogicalNot or OperatorType.BitwiseNegation)
            start = tokenPosition;

        i += Key.Length;
        var right = _isProcessingOperand
            ? mathExpression.BuildOperand<TResult>(ref i, separator, closingSymbol)
            : mathExpression.Build<TResult>(ref i, separator, closingSymbol, Precedence);

        if (_isProcessingOperand)
        {
            //for case such as 2^3^2 we should evaluate first 3^2, so start position = 1 + 1 in this example.
            var startExponentiation = OperatorType == OperatorType.Power ? tokenPosition + Key.Length : start;
            right = mathExpression.BuildExponentiation<TResult>(startExponentiation, ref i, separator, closingSymbol, right);
        }

        var expression = Build<TResult>(OperatorType, left, right);

        mathExpression.OnEvaluating(start, i, expression.NodeType == ExpressionType.Convert ? ((UnaryExpression)expression).Operand : expression);
        return expression;
    }

    internal static Expression Build<TResult>(OperatorType type, Expression left, Expression right)
#if NET8_0_OR_GREATER
        where TResult : struct, INumberBase<TResult>
#endif
        => left is ConstantExpression l && right is ConstantExpression r
            ? BuildConstant<TResult>(type, l, r)
            : BuildNotConstant<TResult>(type, left, right);

    #region private static Methods

    private static double Calculate(OperatorType type, double left, double right)
        => type switch
        {
            OperatorType.LogicalConditionalOr => left != default || right != default ? 1.0 : default,
            OperatorType.LogicalConditionalAnd => left != default && right != default ? 1.0 : default,
            OperatorType.LogicalOr => left != default || right != default ? 1.0 : default,
            OperatorType.BitwiseOr => (long)left | (long)right,
            OperatorType.LogicalXor => (left != default) ^ (right != default) ? 1.0 : default,
            OperatorType.BitwiseXor => (long)left ^ (long)right,
            OperatorType.LogicalAnd => left != default && right != default ? 1.0 : default,
            OperatorType.BitwiseAnd => (long)left & (long)right,
            OperatorType.LogicalNot or OperatorType.LogicalNegation => right == default ? 1.0 : default,
            OperatorType.BitwiseNegation => ~(long)right,
            OperatorType.Equal => left == right ? 1.0 : default,
            OperatorType.NotEqual => left != right ? 1.0 : default,
            OperatorType.LessThan => left < right ? 1.0 : default,
            OperatorType.LessThanOrEqual => left <= right ? 1.0 : default,
            OperatorType.GreaterThan => left > right ? 1.0 : default,
            OperatorType.GreaterThanOrEqual => left >= right ? 1.0 : default,
            OperatorType.Multiply => left * right,
            OperatorType.Divide => left / right,
            OperatorType.Add => left + right,
            OperatorType.Subtract => left - right,
            OperatorType.Modulo => left % right,
            OperatorType.Power => Math.Pow(left, right),
            OperatorType.Negate => -right,
            _ => throw new NotImplementedException()
        };

#if NET8_0_OR_GREATER

    private static TResult Calculate<TResult>(OperatorType type, TResult left, TResult right)
        where TResult : struct, INumberBase<TResult>
    {
        if (typeof(TResult) == typeof(Complex))
            return ConvertNumber<Complex, TResult>(Calculate(type, ConvertNumber<TResult, Complex>(left), ConvertNumber<TResult, Complex>(right)));

        return type switch
        {
            OperatorType.LogicalConditionalOr => left != default || right != default ? TResult.One : default,
            OperatorType.LogicalConditionalAnd => left != default && right != default ? TResult.One : default,
            OperatorType.LogicalOr => left != default || right != default ? TResult.One : default,
            OperatorType.BitwiseOr => ConvertNumber<long, TResult>(ConvertNumber<TResult, long>(left) | ConvertNumber<TResult, long>(right)),
            OperatorType.LogicalXor => (left != default) ^ (right != default) ? TResult.One : default,
            OperatorType.BitwiseXor => ConvertNumber<long, TResult>(ConvertNumber<TResult, long>(left) ^ ConvertNumber<TResult, long>(right)),
            OperatorType.LogicalAnd => left != default && right != default ? TResult.One : default,
            OperatorType.BitwiseAnd => TResult.CreateChecked(ConvertNumber<TResult, long>(left) & ConvertNumber<TResult, long>(right)),
            OperatorType.LogicalNot or OperatorType.LogicalNegation => right == default ? TResult.One : default,
            OperatorType.BitwiseNegation => ConvertNumber<long, TResult>(~ConvertNumber<TResult, long>(right)),
            OperatorType.Equal => left == right ? TResult.One : default,
            OperatorType.NotEqual => left != right ? TResult.One : default,
            OperatorType.LessThan => left is IComparable<TResult> l && l.CompareTo(right) < 0 ? TResult.One : default,
            OperatorType.LessThanOrEqual => left is IComparable<TResult> l && l.CompareTo(right) <= 0 ? TResult.One : default,
            OperatorType.GreaterThan => left is IComparable<TResult> l && l.CompareTo(right) > 0 ? TResult.One : default,
            OperatorType.GreaterThanOrEqual => left is IComparable<TResult> l && l.CompareTo(right) >= 0 ? TResult.One : default,
            OperatorType.Multiply => left * right,
            OperatorType.Divide => left / right,
            OperatorType.Add => left + right,
            OperatorType.Subtract => left - right,
            OperatorType.Modulo when left is BigInteger l => ConvertNumber<BigInteger, TResult>(l % ConvertNumber<TResult, BigInteger>(right)),
            OperatorType.Modulo when TResult.IsInteger(left) && TResult.IsInteger(right) => ConvertNumber<long, TResult>(ConvertNumber<TResult, long>(left) % ConvertNumber<TResult, long>(right)),
            OperatorType.Modulo => ConvertNumber<double, TResult>(ConvertNumber<TResult, double>(left) % ConvertNumber<TResult, double>(right)),
            OperatorType.Power when left is BigInteger l => ConvertNumber<BigInteger, TResult>(BigInteger.Pow(l, ConvertNumber<TResult, int>(right))),
            OperatorType.Power => ConvertNumber<double, TResult>(Math.Pow(ConvertNumber<TResult, double>(left), ConvertNumber<TResult, double>(right))),
            OperatorType.Negate => -right,
            _ => throw new NotImplementedException()
        };
    }

#endif

    private static decimal Calculate(OperatorType type, decimal left, decimal right)
        => type switch
        {
            OperatorType.LogicalConditionalOr => left != default || right != default ? 1.0m : default,
            OperatorType.LogicalConditionalAnd => left != default && right != default ? 1.0m : default,
            OperatorType.LogicalOr => left != default || right != default ? 1.0m : default,
            OperatorType.BitwiseOr => (long)left | (long)right,
            OperatorType.LogicalXor => (left != default) ^ (right != default) ? 1.0m : default,
            OperatorType.BitwiseXor => (long)left ^ (long)right,
            OperatorType.LogicalAnd => left != default && right != default ? 1.0m : default,
            OperatorType.BitwiseAnd => (long)left & (long)right,
            OperatorType.LogicalNot or OperatorType.LogicalNegation => right == default ? 1.0m : default,
            OperatorType.BitwiseNegation => ~(long)right,
            OperatorType.Equal => left == right ? 1.0m : default,
            OperatorType.NotEqual => left != right ? 1.0m : default,
            OperatorType.LessThan => left < right ? 1.0m : default,
            OperatorType.LessThanOrEqual => left <= right ? 1.0m : default,
            OperatorType.GreaterThan => left > right ? 1.0m : default,
            OperatorType.GreaterThanOrEqual => left >= right ? 1.0m : default,
            OperatorType.Multiply => left * right,
            OperatorType.Divide => left / right,
            OperatorType.Add => left + right,
            OperatorType.Subtract => left - right,
            OperatorType.Modulo => left % right,
            OperatorType.Power => (decimal)Math.Pow((double)left, (double)right),
            OperatorType.Negate => -right,
            _ => throw new NotImplementedException()
        };

    private static Complex Calculate(OperatorType type, Complex left, Complex right)
        => type switch
        {
            OperatorType.LogicalConditionalOr => ConvertToBoolean(left) || ConvertToBoolean(right) ? Complex.One : default,
            OperatorType.LogicalConditionalAnd => ConvertToBoolean(left) && ConvertToBoolean(right) ? Complex.One : default,
            OperatorType.LogicalOr => ConvertToBoolean(left) || ConvertToBoolean(right) ? Complex.One : default,
            OperatorType.BitwiseOr => (long)ConvertToDouble(left) | (long)ConvertToDouble(right),
            OperatorType.LogicalXor => ConvertToBoolean(left) ^ ConvertToBoolean(right) ? Complex.One : default,
            OperatorType.BitwiseXor => (long)ConvertToDouble(left) ^ (long)ConvertToDouble(right),
            OperatorType.LogicalAnd => ConvertToBoolean(left) && ConvertToBoolean(right) ? Complex.One : default,
            OperatorType.BitwiseAnd => (long)ConvertToDouble(left) & (long)ConvertToDouble(right),
            OperatorType.LogicalNot or OperatorType.LogicalNegation => ConvertToBoolean(right) ? default : Complex.One,
            OperatorType.BitwiseNegation => ~(long)ConvertToDouble(right),
            OperatorType.Equal => left == right ? Complex.One : default,
            OperatorType.NotEqual => left != right ? Complex.One : default,
            OperatorType.LessThan => ConvertToDouble(left) < ConvertToDouble(right) ? Complex.One : default,
            OperatorType.LessThanOrEqual => ConvertToDouble(left) <= ConvertToDouble(right) ? Complex.One : default,
            OperatorType.GreaterThan => ConvertToDouble(left) > ConvertToDouble(right) ? Complex.One : default,
            OperatorType.GreaterThanOrEqual => ConvertToDouble(left) >= ConvertToDouble(right) ? Complex.One : default,
            OperatorType.Multiply => left * right,
            OperatorType.Divide => left / right,
            OperatorType.Add => left + right,
            OperatorType.Subtract => left - right,
            OperatorType.Modulo => ConvertToDouble(left) % ConvertToDouble(right),
            OperatorType.Power => Complex.Pow(left, right),
            OperatorType.Negate => -right,
            _ => throw new NotImplementedException()
        };

    private static Expression BuildNotConstant<TResult>(OperatorType type, Expression left, Expression right)
    {
        switch (type)
        {
            case OperatorType.LogicalConditionalAnd or OperatorType.LogicalAnd
                or OperatorType.LogicalConditionalOr or OperatorType.LogicalOr:
                left = BuildConvert<bool>(left);
                right = BuildConvert<bool>(right);
                break;
            case OperatorType.BitwiseAnd or OperatorType.BitwiseOr
                or OperatorType.LogicalXor or OperatorType.BitwiseXor:
                left = BuildConvert<long>(left);
                right = BuildConvert<long>(right);
                break;
            case OperatorType.LogicalNot or OperatorType.LogicalNegation:
                right = BuildConvert<bool>(right);
                break;
            case OperatorType.BitwiseNegation:
                right = BuildConvert<long>(right);
                break;
#if NET8_0_OR_GREATER
            case OperatorType.Power when typeof(TResult) == typeof(BigInteger):
                left = BuildConvert<BigInteger>(left);
                right = BuildConvert<int>(right);
                return Expression.Call(typeof(BigInteger).GetMethod(nameof(BigInteger.Pow), [left.Type, typeof(int)])!, left, right);
#endif
            case OperatorType.Power when typeof(TResult) == typeof(Complex):
                left = BuildConvert<Complex>(left);
                right = BuildConvert<Complex>(right);
                return Expression.Call(typeof(Complex).GetMethod(nameof(Complex.Pow), [left.Type, right.Type])!, left, right);
            case OperatorType.Modulo or OperatorType.LessThan or OperatorType.LessThanOrEqual or OperatorType.GreaterThan or OperatorType.GreaterThanOrEqual
                when typeof(TResult) == typeof(Complex):
            case OperatorType.Power:
                left = BuildConvert<double>(left);
                right = BuildConvert<double>(right);
                break;
        }

        var expression = type is OperatorType.LogicalNot or OperatorType.LogicalNegation or OperatorType.BitwiseNegation or OperatorType.Negate
            ? Expression.MakeUnary(ExpressionTypeByOperatorType[type], right, null!).Reduce() // null is okey here because the type is not needed for these operators
            : Expression.MakeBinary(ExpressionTypeByOperatorType[type], left, right).Reduce();

        return BuildConvert<TResult>(expression);
    }

    private static Expression BuildConstant<TResult>(OperatorType type, ConstantExpression left, ConstantExpression right)
#if NET8_0_OR_GREATER
        where TResult : struct, INumberBase<TResult>
#endif
    {
        object value;
        switch (left.Value)
        {
#if NET8_0_OR_GREATER
            case TResult ld:
                {
                    var rd = right.Value is TResult d ? d : (TResult)ChangeType(right.Value, typeof(TResult));
                    value = Calculate(type, ld, rd);
                    break;
                }
#endif
            case Complex lc:
                {
                    var rc = right.Value is Complex c ? c : ConvertToDouble(right.Value);
                    value = Calculate(type, lc, rc);
                    break;
                }
            case decimal ld:
                {
                    var rd = right.Value is decimal d ? d : ConvertToDecimal(right.Value);
                    value = Calculate(type, ld, rd);
                    break;
                }
            default:
                value = Calculate(type, ConvertToDouble(left.Value), ConvertToDouble(right.Value));
                break;
        }

        return BuildConvert<TResult>(Expression.Constant(value));
    }

    private static bool ConvertToBoolean(Complex value)
        => (bool)ChangeType(value, typeof(bool));

    #endregion
}