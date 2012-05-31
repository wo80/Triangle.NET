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
    public class TriangleFile : MeshExplorer.IO.IMeshFormat
    {
        /// <summary>
        /// Gets the supported file extensions.
        /// </summary>
        public string[] Extensions
        {
            get { return new string[] { ".node", ".poly" }; }
        }


        public InputGeometry Read(string file)
        {
            string supp = Path.ChangeExtension(file, ".ele");

            return FileReader.ReadFile(file, File.Exists(supp));
        }

        public void Write(string file, Mesh data)
        {
            if (data.NumberOfVertices > 0)
            {
                WritePoly(file, data);

                if (data.Triangles != null)
                {
                    file = Path.ChangeExtension(file, "ele");

                    WriteElements(file, data);
                }
            }
        }

        private void WritePoly(string file, Mesh data)
        {
            int i = 0;
            int n = data.NumberOfVertices;
            bool markers = true;

            using (StreamWriter writer = new StreamWriter(file))
            {
                // Write nodes
                writer.WriteLine("{0} 2 0 {1}", n, markers ? "1" : "0");

                // TODO: point attributes
                foreach (var item in data.Vertices)
                {
                    writer.Write("{0} {1} {2}", i++,
                        item.X.ToString(Util.Nfi),
                        item.Y.ToString(Util.Nfi));

                    if (markers)
                    {
                        writer.Write(" {0}", item.Boundary);
                    }

                    writer.WriteLine();
                }

                // Write segments
                n = data.NumberOfSegments;
                i = 0;

                // Number of segments, number of boundary markers (zero or one).
                writer.WriteLine("{0} {1}", n, markers ? "1" : "0");

                foreach (var item in data.Segments)
                {
                    writer.Write("{0} {1} {2}", i,
                        item.P0, item.P1);

                    if (markers)
                    {
                        writer.Write(" {0}", item.Boundary);
                    }

                    writer.WriteLine();
                }

                // Write holes
                n = data.Holes.Count;

                writer.WriteLine("{0}", n);

                foreach (var item in data.Holes)
                {
                    writer.WriteLine("{0} {1}",
                        data.Holes[i].X.ToString(Util.Nfi),
                        data.Holes[i].Y.ToString(Util.Nfi));
                }

                // TODO: Regions
            }
        }

        public void WriteElements(string file, Mesh data)
        {
            using (StreamWriter writer = new StreamWriter(file))
            {
                int i = 0;

                // TODO: attributes
                writer.WriteLine("{0} 3 0", data.NumberOfTriangles);

                foreach (var item in data.Triangles)
                {
                    writer.WriteLine("{0} {1} {2} {3}", i++,
                        item.P0, item.P1, item.P2);
                }
            }
        }
    }
}
