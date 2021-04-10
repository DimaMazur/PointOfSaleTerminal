
namespace DimaMazur.Core.DomainPrimitives
{
    /// <summary>
    /// Error in execution to avoid raising exceptions.
    /// </summary>
    public class ExecutionError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionError"/> class.
        /// </summary>
        /// <param name="message">Reason of failure.</param>
        public ExecutionError(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Reason of failure.
        /// </summary>
        public string Message { get; init; } = string.Empty;

        /// <summary>
        /// Message error.
        /// </summary>
        /// <returns>Converted String.</returns>
        public override string ToString() => Message;
    }
}
