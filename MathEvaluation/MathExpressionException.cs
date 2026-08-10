using System;
using System.Text;

namespace MathEvaluation;

/// <summary>The exception to evaluating a math expression string.</summary>
/// <seealso cref="System.ApplicationException" />
public class MathExpressionException : ApplicationException
{
    /// <summary>The default error message.</summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public const string DefaultMessage = "Error of evaluating the expression.";

    /// <summary>Gets the invalid token position.</summary>
    /// <value>The invalid token position.</value>
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public int InvalidTokenPosition { get; }

    /// <summary>Gets the recursion depth of the parsing at which the evaluating was stopped.</summary>
    /// <value>
    ///     The nesting depth, or -1 if the exception isn't caused by a too deeply nested math expression string.
    ///     It counts the recursive calls of the parser, which is what consumes the call stack,
    ///     so it is proportional to, but not equal to, the nesting depth of the math expression string.
    /// </value>
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public int NestingDepth { get; init; } = -1;

    /// <summary>Initializes a new instance of the <see cref="MathExpressionException" /> class.</summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="invalidTokenPosition">The invalid token position.</param>
    public MathExpressionException(string message, int invalidTokenPosition = -1)
        : base(BuildMessage(message, invalidTokenPosition))
    {
        InvalidTokenPosition = invalidTokenPosition;
    }

    /// <summary>Initializes a new instance of the <see cref="MathExpressionException" /> class.</summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">
    ///     The exception that is the cause of the current exception.
    ///     If the <paramref name="innerException" /> parameter is not a null reference,
    ///     the current exception is raised in a <span class="keyword">catch</span> block that handles the inner exception.
    /// </param>
    /// <param name="invalidTokenPosition">The invalid token position.</param>
    public MathExpressionException(string message, Exception innerException, int invalidTokenPosition = -1)
        : base(BuildMessage(message, invalidTokenPosition), innerException)
    {
        InvalidTokenPosition = invalidTokenPosition;
    }

    private static string BuildMessage(string message, int invalidTokenPosition)
    {
        var sb = new StringBuilder(DefaultMessage);
        if (!string.IsNullOrWhiteSpace(message))
        {
            sb.Append(' ').Append(message);
        }

        if (invalidTokenPosition >= 0)
        {
            sb.Append(" Invalid token at position ").Append(invalidTokenPosition).Append('.');
        }

        return sb.ToString();
    }
}