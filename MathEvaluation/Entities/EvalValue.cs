using System;
using System.Runtime.CompilerServices;
using System.Text;

internal struct EvalValue : IEquatable<EvalValue>
{
    public double? DoubleValue;

    public decimal? DecimalValue;

    public bool? BooleanValue;

    public EvalValue(double value) : this()
    {
        DoubleValue = value;
    }

    public EvalValue(decimal value) : this()
    {
        DecimalValue = value;
    }

    public EvalValue(bool value) : this()
    {
        BooleanValue = value;
    }

    public EvalValue(EvalValue value) : this()
    {
        DoubleValue = value.DoubleValue;
        DecimalValue = value.DecimalValue;
        BooleanValue = value.BooleanValue;
    }

    public bool IsDefault
        => DoubleValue.HasValue
            ? DoubleValue.Value == default
            : (DecimalValue.HasValue ? DecimalValue.Value == default : BooleanValue == default);

    /// <summary>
    /// Returns a boolean indicating whether the given Object is equal to this EvalValue instance.
    /// </summary>
    /// <param name="obj">The Object to compare against.</param>
    /// <returns>True if the Object is equal to this EvalValue; False otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object obj)
    {
        if (obj is not EvalValue)
            return false;

        return Equals((EvalValue)obj);
    }

    /// <summary>
    /// Returns a boolean indicating whether the given EvalValue is equal to this EvalValue instance.
    /// </summary>
    /// <param name="other">The EvalValue to compare against.</param>
    /// <returns>True if the EvalValue is equal to this EvalValue; False otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(EvalValue other)
    {
        return DoubleValue == other.DoubleValue &&
            DecimalValue == other.DecimalValue &&
            BooleanValue == other.BooleanValue;
    }

    /// <summary>
    /// Returns a String representing this EvalValue instance.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("double: ");
        sb.Append(DoubleValue.ToString());
        sb.Append(',');
        sb.Append(' ');

        sb.Append("decimal: ");
        sb.Append(DecimalValue.ToString());
        sb.Append(',');
        sb.Append(' ');

        sb.Append("boolean: ");
        sb.Append(BooleanValue.ToString());
        return sb.ToString();
    }

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>The hash code.</returns>
    public override int GetHashCode()
    {
        int hash = DoubleValue.GetHashCode();
        hash = CombineHashCodes(hash, DecimalValue.GetHashCode());
        hash = CombineHashCodes(hash, BooleanValue.GetHashCode());
        return hash;
    }

    private static int CombineHashCodes(int h1, int h2)
    {
        return ((h1 << 5) + h1) ^ h2;
    }

    #region Public Static Operators

    public static implicit operator EvalValue(double v) => new(v);
    public static implicit operator EvalValue(decimal v) => new(v);
    public static implicit operator EvalValue(bool v) => new(v);

    public static implicit operator double(EvalValue v) => v.DoubleValue ?? (v.DecimalValue.HasValue ? (double)v.DecimalValue.Value : default);
    public static implicit operator decimal(EvalValue v) => v.DecimalValue ?? (v.DoubleValue.HasValue ? (decimal)v.DoubleValue.Value : default);
    public static implicit operator bool(EvalValue v) => v.BooleanValue ?? default;

    /// <summary>
    /// Adds two values together.
    /// </summary>
    /// <param name="left">The first source value.</param>
    /// <param name="right">The second source value.</param>
    /// <returns>The summed value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EvalValue operator +(EvalValue left, EvalValue right)
    {
        if (left.DoubleValue.HasValue)
            return new EvalValue(left.DoubleValue.Value + (double)right);

        if (left.DecimalValue.HasValue)
            return new EvalValue(left.DecimalValue.Value + (decimal)right);

        return new EvalValue();
    }

    /// <summary>
    /// Subtracts the second value from the first.
    /// </summary>
    /// <param name="left">The first source value.</param>
    /// <param name="right">The second source value.</param>
    /// <returns>The difference value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EvalValue operator -(EvalValue left, EvalValue right)
    {
        if (left.DoubleValue.HasValue)
            return new EvalValue(left.DoubleValue.Value - (double)right);

        if (left.DecimalValue.HasValue)
            return new EvalValue(left.DecimalValue.Value - (decimal)right);

        return new EvalValue();
    }

    /// <summary>
    /// Multiplies two values together.
    /// </summary>
    /// <param name="left">The first source value.</param>
    /// <param name="right">The second source value.</param>
    /// <returns>The product value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EvalValue operator *(EvalValue left, EvalValue right)
    {
        if (left.DoubleValue.HasValue)
            return new EvalValue(left.DoubleValue.Value * (double)right);

        if (left.DecimalValue.HasValue)
            return new EvalValue(left.DecimalValue.Value * (decimal)right);

        return new EvalValue();
    }

    /// <summary>
    /// Divides the first value by the second.
    /// </summary>
    /// <param name="left">The first source value.</param>
    /// <param name="right">The second source value.</param>
    /// <returns>The value resulting from the division.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EvalValue operator /(EvalValue left, EvalValue right)
    {
        if (left.DoubleValue.HasValue)
            return new EvalValue(left.DoubleValue.Value / (double)right);

        if (left.DecimalValue.HasValue)
            return new EvalValue(left.DecimalValue.Value / (decimal)right);

        return new EvalValue();
    }

    /// <summary>
    /// Negates a given value.
    /// </summary>
    /// <param name="value">The source value.</param>
    /// <returns>The negated value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EvalValue operator -(EvalValue value)
    {
        if (value.DoubleValue.HasValue)
            return new EvalValue(-value.DoubleValue.Value);

        if (value.DecimalValue.HasValue)
            return new EvalValue(-value.DecimalValue.Value);

        return value;
    }

    /// <summary>
    /// Returns a boolean indicating whether the two given values are equal.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>True if the values are equal; False otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(EvalValue left, EvalValue right)
    {
        return left.DoubleValue == right.DoubleValue &&
            left.DecimalValue == right.DecimalValue &&
            left.BooleanValue == right.BooleanValue;
    }

    /// <summary>
    /// Returns a boolean indicating whether the two given values are not equal.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>True if the values are not equal; False if they are equal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(EvalValue left, EvalValue right)
    {
        return left.DoubleValue != right.DoubleValue ||
            left.DecimalValue != right.DecimalValue ||
            left.BooleanValue != right.BooleanValue;
    }

    #endregion Public Static Operators
}