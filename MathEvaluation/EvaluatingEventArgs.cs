﻿using System;

namespace MathEvaluation;

/// <summary>
///     Args of the Evaluating event.
/// </summary>
/// <seealso cref="System.EventArgs" />
public class EvaluatingEventArgs : EventArgs
{
    /// <summary>
    ///     The math expression string.
    /// </summary>
    public string MathString { get; }

    /// <summary>Gets the start position of the token in the math expression string.</summary>
    /// <value>Index of the starting character.</value>
    public int Start { get; }

    /// <summary>Gets the end position of the token in the math expression string.</summary>
    /// <value>Index of the ending character.</value>
    public int End { get; }

    /// <summary>Gets the evaluation step number.</summary>
    /// <value>The evaluation step number.</value>
    public int Step { get; }

    /// <summary>Gets the evaluated value.</summary>
    /// <value>The value.</value>
    public object Value { get; }

    /// <summary>Gets a value indicating whether evaluating is completed.</summary>
    /// <value>
    ///     <c>true</c> if this evaluating is completed; otherwise, <c>false</c>.
    /// </value>
    public bool IsCompleted { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="EvaluatingEventArgs" /> class.
    /// </summary>
    /// <param name="mathString">The math expression string.</param>
    /// <param name="start">The start position of the token in the math expression string.</param>
    /// <param name="end">The end position of the token in the math expression string.</param>
    /// <param name="step">The evaluation step number.</param>
    /// <param name="value">The value of the token.</param>
    /// <param name="isCompleted">if set to <c>null</c> then it will be calculated based on the start and end positions.</param>
    internal EvaluatingEventArgs(string mathString, int start, int end, int step, object value, bool? isCompleted = null)
    {
        MathString = mathString;
        Start = start;
        End = end;
        Step = step;
        Value = value;
        IsCompleted = isCompleted ?? start == 0 && mathString.Length == end + 1;
    }

    /// <summary> Converts to string.</summary>
    /// <returns>
    ///     A <see cref="System.String" /> that represents this instance.
    /// </returns>
    public override string ToString()
        => $"{nameof(MathString)}: \"{MathString}\", {nameof(Start)}: {Start}, {nameof(End)}: {End}, {nameof(Step)}: {Step}, {nameof(Value)}: {Value}, {nameof(IsCompleted)}: {IsCompleted}";
}