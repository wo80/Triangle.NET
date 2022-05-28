// -----------------------------------------------------------------------
// <copyright file="IFileFormat.cs" company="">
// Triangle.NET Copyright (c) 2012-2022 Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.IO
{
    /// <summary>
    /// Interface used to indicate support for file formats in the <see cref="FileProcessor" />.
    /// </summary>
    public interface IFileFormat
    {
        /// <summary>
        /// Test whether the given file is supported.
        /// </summary>
        /// <param name="file">The file to read.</param>
        /// <returns>Returns true if the file can be read.</returns>
        bool IsSupported(string file);
    }
}
