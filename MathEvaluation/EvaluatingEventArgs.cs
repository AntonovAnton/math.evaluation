using System;

namespace MathEvaluation
{
    /// <summary>
    /// Args of the Evaluating event.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class EvaluatingEventArgs : EventArgs
    {
        /// <summary>
        /// The math expression string.
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
        ///   <c>true</c> if this evaluating is completed; otherwise, <c>false</c>.</value>
        public bool IsCompleted { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluatingEventArgs"/> class.
        /// </summary>
        /// <param name="mathString">The math expression string.</param>
        /// <param name="start">The start position of the token in the math expression string.</param>
        /// <param name="end">The end position of the token in the math expression string.</param>
        /// <param name="step">The evaluation step number.</param>
        /// <param name="value">The value of the token.</param>
        public EvaluatingEventArgs(string mathString, int start, int end, int step, object value)
        {
            MathString = mathString;
            Start = start;
            End = end;
            Step = step;
            Value = value;
            IsCompleted = start == 0 && mathString.Length == end + 1;
        }
    }
}
