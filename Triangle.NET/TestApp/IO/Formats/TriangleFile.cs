// -----------------------------------------------------------------------
// <copyright file="TriangleFile.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.IO.Formats
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TriangleNet.IO;
    using System.IO;
    using TriangleNet.Geometry;
    using TriangleNet;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TriangleFile : IMeshFile
    {
        TriangleFormat format = new TriangleFormat();

        /// <summary>
        /// Gets the supported file extensions.
        /// </summary>
        public string[] Extensions
        {
            get { return new string[] { ".node", ".poly", ".ele" }; }
        }

        public bool ContainsMeshData(string filename)
        {
            string ext = Path.GetExtension(filename);

            if (ext == ".node" || ext == ".poly")
            {
                if (File.Exists(Path.ChangeExtension(filename, ".ele")))
                {
                    return true;
                }
            }

            return (ext == ".ele");
        }

        public InputGeometry Read(string filename)
        {
            return format.Read(filename);
        }

        public Mesh Import(string filename)
        {
            return format.Import(filename);
        }

        public void Write(Mesh mesh, string filename)
        {
            if (mesh.NumberOfVertices > 0)
            {
                format.Write(mesh, filename);
            }
        }
    }
}
