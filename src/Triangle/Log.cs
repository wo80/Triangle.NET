// -----------------------------------------------------------------------
// <copyright file="Log.cs" company="">
// Triangle.NET Copyright (c) 2012-2022 Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The log level.
    /// </summary>
    public enum LogLevel { Info, Warning, Error }

    /// <summary>
    /// Represents an item stored in the log.
    /// </summary>
    public class LogItem
    {
        /// <summary>
        /// Gets the <see cref="DateTime"/> the item was logged.
        /// </summary>
        public DateTime Time { get; }

        /// <summary>
        /// Gets the <see cref="LogLevel"/>.
        /// </summary>
        public LogLevel Level { get; }

        /// <summary>
        /// Gets the log message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets further details of the log message.
        /// </summary>
        public string Details { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="LogItem"/> class.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <param name="message">The log message.</param>
        public LogItem(LogLevel level, string message)
            : this(level, message, "")
        { }

        /// <summary>
        /// Creates a new instance of the <see cref="LogItem"/> class.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <param name="message">The log message.</param>
        /// <param name="details">The message details.</param>
        public LogItem(LogLevel level, string message, string details)
        {
            Time = DateTime.Now;
            Level = level;
            Message = message;
            Details = details;
        }
    }

    /// <summary>
    /// A simple logger, which logs messages to a List.
    /// </summary>
    public sealed class Log
    {
        /// <summary>
        /// Log detailed information.
        /// </summary>
        public static bool Verbose { get; set; }

        /// <summary>
        /// Gets all log messages.
        /// </summary>
        public IList<LogItem> Data { get; } = new List<LogItem>();

        #region Singleton pattern

        // Singleton pattern as proposed by Jon Skeet:
        // https://csharpindepth.com/Articles/Singleton

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Log() { }

        private Log() { }

        /// <summary>
        /// Gets the <see cref="Log"/> instance.
        /// </summary>
        public static Log Instance { get; } = new Log();

        #endregion

        /// <summary>
        /// Adds a <see cref="LogItem"/> to the log.
        /// </summary>
        /// <param name="item"></param>
        public void Add(LogItem item)
        {
            Data.Add(item);
        }

        /// <summary>
        /// Clear all messages from the log.
        /// </summary>
        public void Clear()
        {
            Data.Clear();
        }

        /// <summary>
        /// Log info message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info(string message)
        {
            Data.Add(new LogItem(LogLevel.Info, message));
        }

        /// <summary>
        /// Log warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="details">Message details, for example the code location where the error occurred (class, method).</param>
        public void Warning(string message, string details)
        {
            Data.Add(new LogItem(LogLevel.Warning, message, details));
        }

        /// <summary>
        /// Log error message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="details">Message details, for example the code location where the error occurred (class, method).</param>
        public void Error(string message, string details)
        {
            Data.Add(new LogItem(LogLevel.Error, message, details));
        }
    }
}
