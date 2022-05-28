// -----------------------------------------------------------------------
// <copyright file="TriangleFormat.cs" company="">
// Triangle.NET Copyright (c) 2012-2022 Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using TriangleNet.Geometry;
    using TriangleNet.Meshing;

    /// <summary>
    /// Implements geometry and mesh file formats of the original Triangle project.
    /// </summary>
    public class TriangleFormat : IPolygonFormat, IMeshFormat
    {
        /// <inheritdoc />
        public bool IsSupported(string file)
        {
            string ext = Path.GetExtension(file).ToLower();

            if (ext == ".node" || ext == ".poly" || ext == ".ele")
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public IMesh Import(string filename)
        {
            string ext = Path.GetExtension(filename);

            if (ext == ".node" || ext == ".poly" || ext == ".ele")
            {
                List<ITriangle> triangles;
                Polygon geometry;

                (new TriangleReader()).Read(filename, out geometry, out triangles);

                if (geometry != null && triangles != null)
                {
                    return Converter.Instance.ToMesh(geometry, triangles.ToArray());
                }
            }

            throw new NotSupportedException("Could not load '" + filename + "' file.");
        }

        /// <inheritdoc />
        public void Write(IMesh mesh, string filename)
        {
            var writer = new TriangleWriter();

            writer.WritePoly((Mesh)mesh, Path.ChangeExtension(filename, ".poly"));
            writer.WriteElements((Mesh)mesh, Path.ChangeExtension(filename, ".ele"));
        }

        /// <inheritdoc />
        public void Write(IMesh mesh, Stream stream)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IPolygon Read(string filename)
        {
            string ext = Path.GetExtension(filename);

            if (ext == ".node")
            {
                return (new TriangleReader()).ReadNodeFile(filename);
            }
            else if (ext == ".poly")
            {
                return (new TriangleReader()).ReadPolyFile(filename);
            }

            throw new NotSupportedException("File format '" + ext + "' not supported.");
        }


        /// <inheritdoc />
        public void Write(IPolygon polygon, string filename)
        {
            (new TriangleWriter()).WritePoly(polygon, filename);
        }

        /// <inheritdoc />
        public void Write(IPolygon polygon, Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
