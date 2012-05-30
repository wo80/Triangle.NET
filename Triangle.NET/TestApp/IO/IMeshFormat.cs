// -----------------------------------------------------------------------
// <copyright file="IMeshFormat.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.IO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TriangleNet;
    using TriangleNet.Geometry;

    /// <summary>
    /// Defines an interface for mesh file formats.
    /// </summary>
    public interface IMeshFormat
    {
        string[] Extensions { get; }

        InputGeometry Read(string file);
        void Write(string file, Mesh data);
    }
}
