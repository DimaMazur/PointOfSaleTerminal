using System;

namespace DimaMazur.Core.Logging
{
    /// <summary>
    /// Abstraction for logging. Implementation could use Microsoft.Extensions.Logging Nuget package etc.
    /// </summary>
    public interface ILogger<TEntity>
    {
        /// <summary>
        /// Logs information representing by message that might contain different parameters.
        /// </summary>
        /// <param name="message">Description message.</param>
        /// <param name="args">Possible parameters might be used in message with {}</param>
        public void LogInfo(string message, params object[] args);

        /// <summary>
        /// Logs warning representing by message that might contain different parameters.
        /// </summary>
        /// <param name="message">Description message.</param>
        /// <param name="args">Possible parameters might be used in message with {}</param>
        public void LogWarning(string message, params object[] args);

        /// <summary>
        /// Logs error representing by message that might contain different parameters.
        /// </summary>
        /// <param name="message">Description message.</param>
        /// <param name="args">Possible parameters might be used in message with {}</param>
        public void LogError(string message, params object[] args);

        /// <summary>
        /// Logs exception with message that might contain different parameters.
        /// </summary>
        /// <param name="exception">Exception to be logged.</param>
        /// <param name="message">Description message.</param>
        /// <param name="args">Possible parameters might be used in message with {}</param>
        public void LogException(Exception exception, string message, params object[] args);
    }
}
