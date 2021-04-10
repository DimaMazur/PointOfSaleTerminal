using System;

namespace DimaMazur.Core.DomainPrimitives
{
    /// <summary>
    /// Opeartioan status result.
    /// </summary>
    /// <typeparam name="T">Type of the entity.</typeparam>
    public class OperationResult<T> : OperationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult{T}"/> class.
        /// </summary>
        /// <param name="entry">Value for <see cref="Entity"/>.</param>
        public OperationResult(T entry)
        {
            Entity = entry;
            Errors = Array.Empty<ExecutionError>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult{T}"/> class.
        /// </summary>
        /// <param name="errors">Value for <see cref="Errors"/>.</param>
        public OperationResult(params ExecutionError[] errors)
            : base(errors)
        {
        }

        /// <summary>
        /// Created entry.
        /// </summary>
        public T Entity { get; }

        /// <summary>
        /// Creates successful result with an entry.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <returns>Successful result.</returns>
        public static new OperationResult<T> Succeeded(T entity)
        {
            return new OperationResult<T>(entity);
        }
    }
}
