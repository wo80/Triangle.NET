// -----------------------------------------------------------------------
// <copyright file="IMeshFile.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.IO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TriangleNet;
    using TriangleNet.IO;

    /// <summary>
    /// Defines an interface for mesh file formats.
    /// </summary>
    public interface IMeshFile : IPolygonFormat, IMeshFormat
    {
        /// <summary>
        /// The supported file extensions.
        /// </summary>
        string[] Extensions { get; }

        /// <summary>
        /// Return true, if a (previously saved) mesh is associated with the given file.
        /// </summary>
        /// <param name="filename">The file name.</param>
        /// <returns>True, if a mesh is associated with the given file.</returns>
        bool ContainsMeshData(string filename);
    }
}
