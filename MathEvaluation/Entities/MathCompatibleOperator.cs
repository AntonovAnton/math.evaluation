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
        where TResult : struct, INumberBase<TResult>
        => left is ConstantExpression l && right is ConstantExpression r
            ? BuildConstant<TResult>(type, l, r)
            : BuildNotConstant<TResult>(type, left, right);

    #region private static Methods

    private static TResult Calculate<TResult>(OperatorType type, TResult left, TResult right)
        where TResult : struct, INumberBase<TResult>
    {
        if (typeof(TResult) == typeof(Complex))
            return ConvertNumber<Complex, TResult>(Calculate(type, ConvertNumber<TResult, Complex>(left), ConvertNumber<TResult, Complex>(right)));

        return type switch
        {
            OperatorType.Multiply => left * right,
            OperatorType.Divide => left / right,
            OperatorType.Add => left + right,
            OperatorType.Subtract => left - right,

            OperatorType.LogicalConditionalOr => left != default || right != default ? TResult.One : default,
            OperatorType.LogicalConditionalAnd => left != default && right != default ? TResult.One : default,

            OperatorType.LogicalOr => left != default || right != default ? TResult.One : default,

            OperatorType.BitwiseOr when left is Int128 l => ConvertNumber<Int128, TResult>(l | ConvertNumber<TResult, Int128>(right)),
            OperatorType.BitwiseOr when left is UInt128 l => ConvertNumber<UInt128, TResult>(l | ConvertNumber<TResult, UInt128>(right)),
            OperatorType.BitwiseOr when left is BigInteger l => ConvertNumber<BigInteger, TResult>(l | ConvertNumber<TResult, BigInteger>(right)),
            OperatorType.BitwiseOr => ConvertNumber<long, TResult>(ConvertNumber<TResult, long>(left) | ConvertNumber<TResult, long>(right)),

            OperatorType.LogicalXor => (left != default) ^ (right != default) ? TResult.One : default,

            OperatorType.BitwiseXor when left is Int128 l => ConvertNumber<Int128, TResult>(l ^ ConvertNumber<TResult, Int128>(right)),
            OperatorType.BitwiseXor when left is UInt128 l => ConvertNumber<UInt128, TResult>(l ^ ConvertNumber<TResult, UInt128>(right)),
            OperatorType.BitwiseXor when left is BigInteger l => ConvertNumber<BigInteger, TResult>(l ^ ConvertNumber<TResult, BigInteger>(right)),
            OperatorType.BitwiseXor => ConvertNumber<long, TResult>(ConvertNumber<TResult, long>(left) ^ ConvertNumber<TResult, long>(right)),

            OperatorType.LogicalAnd => left != default && right != default ? TResult.One : default,

            OperatorType.BitwiseAnd when left is Int128 l => ConvertNumber<Int128, TResult>(l & ConvertNumber<TResult, Int128>(right)),
            OperatorType.BitwiseAnd when left is UInt128 l => ConvertNumber<UInt128, TResult>(l & ConvertNumber<TResult, UInt128>(right)),
            OperatorType.BitwiseAnd when left is BigInteger l => ConvertNumber<BigInteger, TResult>(l & ConvertNumber<TResult, BigInteger>(right)),
            OperatorType.BitwiseAnd => ConvertNumber<long, TResult>(ConvertNumber<TResult, long>(left) & ConvertNumber<TResult, long>(right)),

            OperatorType.LogicalNot or OperatorType.LogicalNegation => right == default ? TResult.One : default,

            OperatorType.BitwiseNegation when right is Int128 r => ConvertNumber<Int128, TResult>(~r),
            OperatorType.BitwiseNegation when right is UInt128 r => ConvertNumber<UInt128, TResult>(~r),
            OperatorType.BitwiseNegation when right is BigInteger r => ConvertNumber<BigInteger, TResult>(~r),
            OperatorType.BitwiseNegation => ConvertNumber<long, TResult>(~ConvertNumber<TResult, long>(right)),

            OperatorType.Equal => left == right ? TResult.One : default,
            OperatorType.NotEqual => left != right ? TResult.One : default,

            OperatorType.Modulo when left is Int128 l => ConvertNumber<Int128, TResult>(l % ConvertNumber<TResult, Int128>(right)),
            OperatorType.Modulo when left is UInt128 l => ConvertNumber<UInt128, TResult>(l % ConvertNumber<TResult, UInt128>(right)),
            OperatorType.Modulo when left is BigInteger l => ConvertNumber<BigInteger, TResult>(l % ConvertNumber<TResult, BigInteger>(right)),
            OperatorType.Modulo when TResult.IsInteger(left) && TResult.IsInteger(right) => ConvertNumber<long, TResult>(ConvertNumber<TResult, long>(left) % ConvertNumber<TResult, long>(right)),
            OperatorType.Modulo => ConvertNumber<double, TResult>(ConvertNumber<TResult, double>(left) % ConvertNumber<TResult, double>(right)),

            OperatorType.Power when left is BigInteger l => ConvertNumber<BigInteger, TResult>(BigInteger.Pow(l, ConvertNumber<TResult, int>(right))),
            OperatorType.Power => ConvertNumber<double, TResult>(Math.Pow(ConvertNumber<TResult, double>(left), ConvertNumber<TResult, double>(right))),

            OperatorType.Negate => -right,

            OperatorType.LessThan when left is double l => l < ConvertNumber<TResult, double>(right) ? TResult.One : default,
            OperatorType.LessThanOrEqual when left is double l => l <= ConvertNumber<TResult, double>(right) ? TResult.One : default,
            OperatorType.GreaterThan when left is double l => l > ConvertNumber<TResult, double>(right) ? TResult.One : default,
            OperatorType.GreaterThanOrEqual when left is double l => l >= ConvertNumber<TResult, double>(right) ? TResult.One : default,

            OperatorType.LessThan when left is decimal l => l < ConvertNumber<TResult, decimal>(right) ? TResult.One : default,
            OperatorType.LessThanOrEqual when left is decimal l => l <= ConvertNumber<TResult, decimal>(right) ? TResult.One : default,
            OperatorType.GreaterThan when left is decimal l => l > ConvertNumber<TResult, decimal>(right) ? TResult.One : default,
            OperatorType.GreaterThanOrEqual when left is decimal l => l >= ConvertNumber<TResult, decimal>(right) ? TResult.One : default,

            OperatorType.LessThan when left is float l => l < ConvertNumber<TResult, float>(right) ? TResult.One : default,
            OperatorType.LessThanOrEqual when left is float l => l <= ConvertNumber<TResult, float>(right) ? TResult.One : default,
            OperatorType.GreaterThan when left is float l => l > ConvertNumber<TResult, float>(right) ? TResult.One : default,
            OperatorType.GreaterThanOrEqual when left is float l => l >= ConvertNumber<TResult, float>(right) ? TResult.One : default,

            OperatorType.LessThan when left is Half l => l < ConvertNumber<TResult, Half>(right) ? TResult.One : default,
            OperatorType.LessThanOrEqual when left is Half l => l <= ConvertNumber<TResult, Half>(right) ? TResult.One : default,
            OperatorType.GreaterThan when left is Half l => l > ConvertNumber<TResult, Half>(right) ? TResult.One : default,
            OperatorType.GreaterThanOrEqual when left is Half l => l >= ConvertNumber<TResult, Half>(right) ? TResult.One : default,

            OperatorType.LessThan when left is int l => l < ConvertNumber<TResult, int>(right) ? TResult.One : default,
            OperatorType.LessThanOrEqual when left is int l => l <= ConvertNumber<TResult, int>(right) ? TResult.One : default,
            OperatorType.GreaterThan when left is int l => l > ConvertNumber<TResult, int>(right) ? TResult.One : default,
            OperatorType.GreaterThanOrEqual when left is int l => l >= ConvertNumber<TResult, int>(right) ? TResult.One : default,

            OperatorType.LessThan when left is uint l => l < ConvertNumber<TResult, uint>(right) ? TResult.One : default,
            OperatorType.LessThanOrEqual when left is uint l => l <= ConvertNumber<TResult, uint>(right) ? TResult.One : default,
            OperatorType.GreaterThan when left is uint l => l > ConvertNumber<TResult, uint>(right) ? TResult.One : default,
            OperatorType.GreaterThanOrEqual when left is uint l => l >= ConvertNumber<TResult, uint>(right) ? TResult.One : default,

            OperatorType.LessThan when left is long l => l < ConvertNumber<TResult, long>(right) ? TResult.One : default,
            OperatorType.LessThanOrEqual when left is long l => l <= ConvertNumber<TResult, long>(right) ? TResult.One : default,
            OperatorType.GreaterThan when left is long l => l > ConvertNumber<TResult, long>(right) ? TResult.One : default,
            OperatorType.GreaterThanOrEqual when left is long l => l >= ConvertNumber<TResult, long>(right) ? TResult.One : default,

            OperatorType.LessThan when left is ulong l => l < ConvertNumber<TResult, ulong>(right) ? TResult.One : default,
            OperatorType.LessThanOrEqual when left is ulong l => l <= ConvertNumber<TResult, ulong>(right) ? TResult.One : default,
            OperatorType.GreaterThan when left is ulong l => l > ConvertNumber<TResult, ulong>(right) ? TResult.One : default,
            OperatorType.GreaterThanOrEqual when left is ulong l => l >= ConvertNumber<TResult, ulong>(right) ? TResult.One : default,

            OperatorType.LessThan when left is short l => l < ConvertNumber<TResult, short>(right) ? TResult.One : default,
            OperatorType.LessThanOrEqual when left is short l => l <= ConvertNumber<TResult, short>(right) ? TResult.One : default,
            OperatorType.GreaterThan when left is short l => l > ConvertNumber<TResult, short>(right) ? TResult.One : default,
            OperatorType.GreaterThanOrEqual when left is short l => l >= ConvertNumber<TResult, short>(right) ? TResult.One : default,

            OperatorType.LessThan when left is ushort l => l < ConvertNumber<TResult, ushort>(right) ? TResult.One : default,
            OperatorType.LessThanOrEqual when left is ushort l => l <= ConvertNumber<TResult, ushort>(right) ? TResult.One : default,
            OperatorType.GreaterThan when left is ushort l => l > ConvertNumber<TResult, ushort>(right) ? TResult.One : default,
            OperatorType.GreaterThanOrEqual when left is ushort l => l >= ConvertNumber<TResult, ushort>(right) ? TResult.One : default,

            OperatorType.LessThan when left is byte l => l < ConvertNumber<TResult, byte>(right) ? TResult.One : default,
            OperatorType.LessThanOrEqual when left is byte l => l <= ConvertNumber<TResult, byte>(right) ? TResult.One : default,
            OperatorType.GreaterThan when left is byte l => l > ConvertNumber<TResult, byte>(right) ? TResult.One : default,
            OperatorType.GreaterThanOrEqual when left is byte l => l >= ConvertNumber<TResult, byte>(right) ? TResult.One : default,

            OperatorType.LessThan when left is sbyte l => l < ConvertNumber<TResult, sbyte>(right) ? TResult.One : default,
            OperatorType.LessThanOrEqual when left is sbyte l => l <= ConvertNumber<TResult, sbyte>(right) ? TResult.One : default,
            OperatorType.GreaterThan when left is sbyte l => l > ConvertNumber<TResult, sbyte>(right) ? TResult.One : default,
            OperatorType.GreaterThanOrEqual when left is sbyte l => l >= ConvertNumber<TResult, sbyte>(right) ? TResult.One : default,

            OperatorType.LessThan when left is nint l => l < ConvertNumber<TResult, nint>(right) ? TResult.One : default,
            OperatorType.LessThanOrEqual when left is nint l => l <= ConvertNumber<TResult, nint>(right) ? TResult.One : default,
            OperatorType.GreaterThan when left is nint l => l > ConvertNumber<TResult, nint>(right) ? TResult.One : default,
            OperatorType.GreaterThanOrEqual when left is nint l => l >= ConvertNumber<TResult, nint>(right) ? TResult.One : default,

            OperatorType.LessThan when left is nuint l => l < ConvertNumber<TResult, nuint>(right) ? TResult.One : default,
            OperatorType.LessThanOrEqual when left is nuint l => l <= ConvertNumber<TResult, nuint>(right) ? TResult.One : default,
            OperatorType.GreaterThan when left is nuint l => l > ConvertNumber<TResult, nuint>(right) ? TResult.One : default,
            OperatorType.GreaterThanOrEqual when left is nuint l => l >= ConvertNumber<TResult, nuint>(right) ? TResult.One : default,

            OperatorType.LessThan when left is Int128 l => l < ConvertNumber<TResult, Int128>(right) ? TResult.One : default,
            OperatorType.LessThanOrEqual when left is Int128 l => l <= ConvertNumber<TResult, Int128>(right) ? TResult.One : default,
            OperatorType.GreaterThan when left is Int128 l => l > ConvertNumber<TResult, Int128>(right) ? TResult.One : default,
            OperatorType.GreaterThanOrEqual when left is Int128 l => l >= ConvertNumber<TResult, Int128>(right) ? TResult.One : default,

            OperatorType.LessThan when left is UInt128 l => l < ConvertNumber<TResult, UInt128>(right) ? TResult.One : default,
            OperatorType.LessThanOrEqual when left is UInt128 l => l <= ConvertNumber<TResult, UInt128>(right) ? TResult.One : default,
            OperatorType.GreaterThan when left is UInt128 l => l > ConvertNumber<TResult, UInt128>(right) ? TResult.One : default,
            OperatorType.GreaterThanOrEqual when left is UInt128 l => l >= ConvertNumber<TResult, UInt128>(right) ? TResult.One : default,

            OperatorType.LessThan when left is BigInteger l => l < ConvertNumber<TResult, BigInteger>(right) ? TResult.One : default,
            OperatorType.LessThanOrEqual when left is BigInteger l => l <= ConvertNumber<TResult, BigInteger>(right) ? TResult.One : default,
            OperatorType.GreaterThan when left is BigInteger l => l > ConvertNumber<TResult, BigInteger>(right) ? TResult.One : default,
            OperatorType.GreaterThanOrEqual when left is BigInteger l => l >= ConvertNumber<TResult, BigInteger>(right) ? TResult.One : default,

            OperatorType.LessThan => left is IComparable<TResult> l && l.CompareTo(right) < 0 ? TResult.One : default,
            OperatorType.LessThanOrEqual => left is IComparable<TResult> l && l.CompareTo(right) <= 0 ? TResult.One : default,
            OperatorType.GreaterThan => left is IComparable<TResult> l && l.CompareTo(right) > 0 ? TResult.One : default,
            OperatorType.GreaterThanOrEqual => left is IComparable<TResult> l && l.CompareTo(right) >= 0 ? TResult.One : default,

            _ => throw new NotImplementedException()
        };
    }

    private static Complex Calculate(OperatorType type, Complex left, Complex right)
        => type switch
        {
            OperatorType.Multiply => left * right,
            OperatorType.Divide => left / right,
            OperatorType.Add => left + right,
            OperatorType.Subtract => left - right,
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
            OperatorType.Modulo => ConvertToDouble(left) % ConvertToDouble(right),
            OperatorType.Power => Complex.Pow(left, right),
            OperatorType.Negate => -right,
            OperatorType.LessThan => ConvertToDouble(left) < ConvertToDouble(right) ? Complex.One : default,
            OperatorType.LessThanOrEqual => ConvertToDouble(left) <= ConvertToDouble(right) ? Complex.One : default,
            OperatorType.GreaterThan => ConvertToDouble(left) > ConvertToDouble(right) ? Complex.One : default,
            OperatorType.GreaterThanOrEqual => ConvertToDouble(left) >= ConvertToDouble(right) ? Complex.One : default,
            _ => throw new NotImplementedException()
        };

    private static Expression BuildNotConstant<TResult>(OperatorType type, Expression left, Expression right)
        where TResult : INumberBase<TResult>
    {
        switch (type)
        {
            case OperatorType.LogicalConditionalAnd or OperatorType.LogicalAnd
                or OperatorType.LogicalConditionalOr or OperatorType.LogicalOr or OperatorType.LogicalXor:
                left = Expression.NotEqual(left, Expression.Default(left.Type)).Reduce();
                right = Expression.NotEqual(right, Expression.Default(right.Type)).Reduce();
                break;
            case OperatorType.LogicalNot or OperatorType.LogicalNegation:
                right = Expression.NotEqual(right, Expression.Default(right.Type)).Reduce();
                break;
            case OperatorType.Power when typeof(TResult) == typeof(BigInteger):
                left = BuildConvert<BigInteger>(left);
                right = BuildConvert<int>(right);
                return Expression.Call(typeof(BigInteger).GetMethod(nameof(BigInteger.Pow), [left.Type, typeof(int)])!, left, right);
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
            case OperatorType.BitwiseAnd or OperatorType.BitwiseOr or OperatorType.BitwiseXor when typeof(TResult) == typeof(Int128):
                left = BuildConvert<Int128>(left);
                right = BuildConvert<Int128>(right);
                break;
            case OperatorType.BitwiseAnd or OperatorType.BitwiseOr or OperatorType.BitwiseXor when typeof(TResult) == typeof(UInt128):
                left = BuildConvert<UInt128>(left);
                right = BuildConvert<UInt128>(right);
                break;
            case OperatorType.BitwiseAnd or OperatorType.BitwiseOr or OperatorType.BitwiseXor when typeof(BigInteger) == typeof(BigInteger):
                left = BuildConvert<BigInteger>(left);
                right = BuildConvert<BigInteger>(right);
                break;
            case OperatorType.BitwiseAnd or OperatorType.BitwiseOr or OperatorType.BitwiseXor:
                left = BuildConvert<long>(left);
                right = BuildConvert<long>(right);
                break;
            case OperatorType.BitwiseNegation when typeof(TResult) == typeof(Int128):
                right = BuildConvert<Int128>(right);
                break;
            case OperatorType.BitwiseNegation when typeof(TResult) == typeof(UInt128):
                right = BuildConvert<UInt128>(right);
                break;
            case OperatorType.BitwiseNegation when typeof(TResult) == typeof(BigInteger):
                right = BuildConvert<BigInteger>(right);
                break;
            case OperatorType.BitwiseNegation:
                right = BuildConvert<long>(right);
                break;
        }

        var expression = type is OperatorType.LogicalNot or OperatorType.LogicalNegation or OperatorType.BitwiseNegation or OperatorType.Negate
            ? Expression.MakeUnary(ExpressionTypeByOperatorType[type], right, null!).Reduce() // null is okey here because the type is not needed for these operators
            : Expression.MakeBinary(ExpressionTypeByOperatorType[type], left, right).Reduce();

        return BuildConvert<TResult>(expression);
    }

    private static Expression BuildConstant<TResult>(OperatorType type, ConstantExpression left, ConstantExpression right)
        where TResult : struct, INumberBase<TResult>
    {
        var l = left.Value is TResult lr ? lr : (TResult)ChangeType(left.Value, typeof(TResult));
        var r = right.Value is TResult rr ? rr : (TResult)ChangeType(right.Value, typeof(TResult));
        var value = Calculate(type, l, r);

        return BuildConvert<TResult>(Expression.Constant(value, typeof(TResult)));
    }

    private static bool ConvertToBoolean(Complex value)
        => ConvertToDouble(value) != default;

    #endregion
}