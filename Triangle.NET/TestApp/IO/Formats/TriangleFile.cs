// -----------------------------------------------------------------------
// <copyright file="TriangleFile.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.IO.Formats
{
    using System;
    using System.IO;
    using TriangleNet;
    using TriangleNet.Geometry;
    using TriangleNet.IO;

    /// <summary>
    /// Read and write files defined in classic Triangle format.
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

        public bool IsSupported(string file)
        {
            throw new NotImplementedException();
        }

        public InputGeometry Read(string filename)
        {
            return format.Read(filename);
        }

        public void Write(InputGeometry polygon, string filename)
        {
            format.Write(polygon, filename);
        }

        public void Write(InputGeometry polygon, StreamWriter stream)
        {
            format.Write(polygon, stream);
        }

        public Mesh Import(string filename)
        {
            return format.Import(filename);
        }

        public void Write(Mesh mesh, string filename)
        {
            if (mesh.Vertices.Count > 0)
            {
                format.Write(mesh, filename);
            }
        }

        public void Write(Mesh mesh, StreamWriter stream)
        {
            throw new NotImplementedException();
        }
    }
}
