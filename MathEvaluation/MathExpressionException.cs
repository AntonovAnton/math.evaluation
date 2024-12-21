using System;
using System.Runtime.Serialization;
using System.Text;

namespace MathEvaluation;

/// <summary>The exception to evaluating a math expression string.</summary>
/// <seealso cref="System.ApplicationException" />
[Serializable]
public class MathExpressionException : ApplicationException
{
    /// <summary>The default error message.</summary>
    public const string DefaultMessage = "Error of evaluating the expression.";

    /// <summary>Gets the invalid token position.</summary>
    /// <value>The invalid token position.</value>
    public int InvalidTokenPosition { get; }

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

    /// <summary>Initializes a new instance of the <see cref="MathExpressionException" /> class.</summary>
    /// <param name="info">The object that holds the serialized object data.</param>
    /// <param name="context">The contextual information about the source or destination.</param>
    protected MathExpressionException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
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
