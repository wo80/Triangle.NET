// -----------------------------------------------------------------------
// <copyright file="GeometryWriter.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.IO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.IO;
    using TriangleNet.Geometry;

    /// <summary>
    /// Writes an InputGeometry to standard Triangle format.
    /// </summary>
    public static class GeometryWriter
    {
        public static void Write(InputGeometry geometry, string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                WritePoints(writer, geometry.Points, geometry.Count);
                WriteSegments(writer, geometry.Segments);
                WriteHoles(writer, geometry.Holes);
            }
        }

        private static void WritePoints(StreamWriter writer, IEnumerable<Point> points, int count)
        {
            int attributes = 0, index = 0;

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
            int index = 0;

            writer.WriteLine("{0} {1}", edges.Count(), 1);

            foreach (var item in edges)
            {
                writer.WriteLine("{0} {1} {2} {3}", index, item.P0, item.P1, item.Boundary);

                index++;
            }
        }

        private static void WriteHoles(StreamWriter writer, IEnumerable<Point> holes)
        {
            int index = 0;

            writer.WriteLine("{0}", holes.Count());

            foreach (var item in holes)
            {
                writer.WriteLine("{0} {1} {2}", index, item.X.ToString(Util.Nfi), item.Y.ToString(Util.Nfi));

                index++;
            }
        }
    }
}
