// -----------------------------------------------------------------------
// <copyright file="DebugWriter.cs" company="">
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.IO
{
    using System;
    using System.IO;
    using System.Globalization;
    using TriangleNet.Data;

    /// <summary>
    /// Writes a the current mesh into a text file.
    /// </summary>
    /// <remarks>
    /// File format:
    /// 
    /// num_nodes
    /// nid_1 nx ny mark
    /// ...
    /// nid_n nx ny mark
    /// 
    /// num_segs
    /// sid_1 p1 p2 mark
    /// ...
    /// sid_n p1 p2 mark
    /// 
    /// num_tris
    /// tid_1 p1 p2 p3 n1 n2 n3
    /// ...
    /// tid_n p1 p2 p3 n1 n2 n3
    /// </remarks>
    class DebugWriter
    {
        static NumberFormatInfo nfi = CultureInfo.InvariantCulture.NumberFormat;

        Mesh mesh;
        int iteration;
        string name;

        public DebugWriter(Mesh mesh)
        {
            this.mesh = mesh;

            this.iteration = 0;
            this.name = "debug-{0}.mesh";
        }

        /// <summary>
        /// Start a new session with given name.
        /// </summary>
        /// <param name="name">Name of the session (and output files).</param>
        public void NewSession(string name)
        {
            this.iteration = 0;
            this.name = name + "-{0}.mesh";
        }
        
        /// <summary>
        /// Write complete mesh to a .mesh file.
        /// </summary>
        public void Write()
        {
            Vertex p1, p2, p3;

            string file = String.Format(name, iteration++);

            using (StreamWriter writer = new StreamWriter(file))
            {
                // Number of vertices.
                writer.WriteLine("{0}", mesh.vertices.Count);

                foreach (var v in mesh.vertices.Values)
                {
                    // Vertex number, x and y coordinates and marker.
                    writer.WriteLine("{0} {1} {2} {3}", v.hash, v.x.ToString(nfi), v.y.ToString(nfi), v.mark);
                }

                // Number of segments.
                writer.WriteLine("{0}", mesh.subsegs.Count);

                Osub subseg = default(Osub);
                subseg.orient = 0;

                foreach (var item in mesh.subsegs.Values)
                {
                    if (item.hash <= 0)
                    {
                        continue;
                    }

                    subseg.seg = item;

                    p1 = subseg.Org();
                    p2 = subseg.Dest();

                    // Segment number, indices of its two endpoints, and marker.
                    writer.WriteLine("{0} {1} {2} {3}", subseg.seg.hash, p1.hash, p2.hash, subseg.seg.boundary);
                }

                Otri tri = default(Otri), trisym = default(Otri);
                tri.orient = 0;

                int n1, n2, n3, hash3;

                // Number of triangles.
                writer.WriteLine("{0}", mesh.triangles.Count);

                foreach (var item in mesh.triangles.Values)
                {
                    if (item.hash <= 0)
                    {
                        continue;
                    }

                    tri.triangle = item;

                    p1 = tri.Org();
                    p2 = tri.Dest();
                    p3 = tri.Apex();

                    if (p3 == null)
                    {
                        if (p1 == null || p2 == null)
                        {
                            continue;
                        }

                        hash3 = -1;
                    }
                    else
                    {
                        hash3 = p3.hash;
                    }
                    
                    if (p1 == null || p2 == null)
                    {
                        continue;
                    }

                    // Triangle number, indices for three vertices.
                    writer.Write("{0} {1} {2} {3}", tri.triangle.hash, p1.hash, p2.hash, hash3);

                    tri.orient = 1;
                    tri.Sym(ref trisym);
                    n1 = trisym.triangle.hash;

                    tri.orient = 2;
                    tri.Sym(ref trisym);
                    n2 = trisym.triangle.hash;

                    tri.orient = 0;
                    tri.Sym(ref trisym);
                    n3 = trisym.triangle.hash;

                    // Neighboring triangle numbers.
                    writer.WriteLine(" {0} {1} {2}", n1, n2, n3);
                }
            }
        }
    }
}
