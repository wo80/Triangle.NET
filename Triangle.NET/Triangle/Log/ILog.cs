// -----------------------------------------------------------------------
// <copyright file="ILogger.cs" company="">
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
    /// TODO: Update summary.
    /// </summary>
    public interface ILog<T>
    {
        void Add(T item);
        void Info(string message);
        void Trace(string message, string location);
        void Error(string message, string location);
        void Warning(string message, string location);

        IList<T> Data { get; }
    }
}
