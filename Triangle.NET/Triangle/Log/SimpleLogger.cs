// -----------------------------------------------------------------------
// <copyright file="SimpleLogger.cs" company="">
// Triangle.NET code by Christian Woltering, http://home.edo.tu-dortmund.de/~woltering/triangle/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Log
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A simple logger, which logs messages to a List<string>.
    /// </summary>
    /// <remarks>Using singleton pattern as proposed by Jon Skeet.
    /// http://csharpindepth.com/Articles/General/Singleton.aspx
    /// </remarks>
    public sealed class SimpleLogger : ILog<string>
    {
        private List<string> log = new List<string>();

        #region Singleton pattern

        private static readonly SimpleLogger instance = new SimpleLogger();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static SimpleLogger() { }

        private SimpleLogger() { }

        public static ILog<string> Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        public void Add(string item)
        {
            log.Add(item);
        }

        public void Info(string message)
        {
            log.Add(message);
        }

        public void Trace(string message, string location)
        {
            log.Add(message);
        }

        public void Warning(string message, string location)
        {
            log.Add(message);
        }

        public void Error(string message, string location)
        {
            log.Add(message);
        }

        public IList<string> Data
        {
            get { return log; }
        }
    }
}
