// -----------------------------------------------------------------------
// <copyright file="SimpleLogItem.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Log
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SimpleLogItem : ILogItem
    {
        DateTime time;
        LogLevel level;
        string message;
        string info;

        public DateTime Time
        {
            get { return time; }
        }

        public LogLevel Level
        {
            get { return level; }
        }

        public string Message
        {
            get { return message; }
        }

        public string Info
        {
            get { return info; }
        }

        public SimpleLogItem(LogLevel level, string message)
            : this(level, message, "")
        { }

        public SimpleLogItem(LogLevel level, string message, string info)
        {
            this.time = DateTime.Now;
            this.level = level;
            this.message = message;
            this.info = info;
        }
    }
}
