using System.Collections.Generic;
using System.Linq;

namespace DimaMazur.Core.DomainPrimitives
{
    /// <summary>
    /// Entry creation result.
    /// </summary>
    public class OperationResult
    {
        /// <summary>
        /// Simple operation result.
        /// </summary>
        public OperationResult()
        {
        }

        /// <summary>
        /// Creates failed operation result with errors inside.
        /// </summary>
        /// <param name="errors">Value for <see cref="Errors"/>.</param>
        public OperationResult(IReadOnlyCollection<ExecutionError> errors)
        {
            Errors = errors;
        }

        /// <summary>
        /// Creation errors.
        /// </summary>
        public IReadOnlyCollection<ExecutionError> Errors { get; protected set; }

        /// <summary>
        /// Indicator if the operation is succeeded.
        /// </summary>
        public bool Success => Errors is null || !Errors.Any();

        /// <summary>
        /// Creates successful result with an entry.
        /// </summary>
        /// <returns>Successful result.</returns>
        public static OperationResult Succeeded => new();

        /// <summary>
        /// Creates failed result with an error inside.
        /// </summary>
        /// <param name="errorMessage">Error message.</param>
        /// <returns>Failed result.</returns>
        public static OperationResult Failed(string errorMessage)
        {
            return new OperationResult(new [] { new ExecutionError(errorMessage) });
        }

        /// <summary>
        /// Creates failed result with an errors.
        /// </summary>
        /// <param name="errors">Details for current error.</param>
        /// <returns>Failed result.</returns>
        public static OperationResult Failed(IReadOnlyCollection<ExecutionError> errors)
        {
            return new OperationResult(errors);
        }
    }
}
