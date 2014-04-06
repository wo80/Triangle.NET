// -----------------------------------------------------------------------
// <copyright file="GeometryWriter.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.IO
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using TriangleNet.Geometry;

    /// <summary>
    /// Writes an InputGeometry to standard Triangle format.
    /// </summary>
    public static class GeometryWriter
    {
        private static int OFFSET = 0;

        /// <summary>
        /// Writes an InputGeometry to a Triangle format .poly file.
        /// </summary>
        /// <param name="geometry">The InputGeometry to write.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="compatibleMode">If true, indices will start at 1 (compatible with original C code).</param>
        public static void Write(InputGeometry geometry, string filename, bool compatibleMode = false)
        {
            OFFSET = compatibleMode ? 1 : 0;

            using (StreamWriter writer = new StreamWriter(filename))
            {
                WritePoints(writer, geometry.Points, geometry.Count);
                WriteSegments(writer, geometry.Segments);
                WriteHoles(writer, geometry.Holes);
            }
        }

        private static void WritePoints(StreamWriter writer, IEnumerable<Point> points, int count)
        {
            int attributes = 0, index = OFFSET;

            var first = points.FirstOrDefault();

            if (first.Attributes != null)
            {
                attributes = first.Attributes.Length;
            }

            writer.WriteLine("{0} {1} {2} {3}", count, 2, attributes, 1);

            foreach (var item in points)
            {
                // Vertex number, x and y coordinates.
                writer.Write("{0} {1} {2}", index, item.X.ToString(Util.Nfi), item.Y.ToString(Util.Nfi));

                // Write attributes.
                for (int j = 0; j < attributes; j++)
                {
                    writer.Write(" {0}", item.Attributes[j].ToString(Util.Nfi));
                }

                // Write the boundary marker.
                writer.WriteLine(" {0}", item.Boundary);

                index++;
            }
        }

        private static void WriteSegments(StreamWriter writer, IEnumerable<Edge> edges)
        {
            int index = OFFSET;

            writer.WriteLine("{0} {1}", edges.Count(), 1);

            foreach (var item in edges)
            {
                writer.WriteLine("{0} {1} {2} {3}", index, item.P0 + OFFSET, item.P1 + OFFSET, item.Boundary);

                index++;
            }
        }

        private static void WriteHoles(StreamWriter writer, IEnumerable<Point> holes)
        {
            int index = OFFSET;

            writer.WriteLine("{0}", holes.Count());

            foreach (var item in holes)
            {
                writer.WriteLine("{0} {1} {2}", index, item.X.ToString(Util.Nfi), item.Y.ToString(Util.Nfi));

                index++;
            }
        }
    }
}
