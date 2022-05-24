// -----------------------------------------------------------------------
// <copyright file="EdgeIterator.cs" company="">
// Triangle.NET Copyright (c) 2012-2022 Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Meshing.Iterators
{
    using System.Collections.Generic;
    using TriangleNet.Topology;
    using TriangleNet.Geometry;

    /// <summary>
    /// Enumerates the edges of a triangulation.
    /// </summary>
    public class EdgeIterator
    {
        /// <summary>
        /// Enumerate all edges of the given mesh.
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        public IEnumerable<Edge> EnumerateEdges(IMesh mesh)
        {
            Otri tri = default;
            Otri neighbor = default;
            Osub sub = default;

            Vertex p1, p2;

            foreach (var t in mesh.Triangles)
            {
                tri.tri = t;
                tri.orient = 0;

                for (int i = 0; i < 3; i++)
                {
                    tri.Sym(ref neighbor);

                    int nid = neighbor.tri.id;

                    if ((tri.tri.id < nid) || (nid == Mesh.DUMMY))
                    {
                        p1 = tri.Org();
                        p2 = tri.Dest();

                        tri.Pivot(ref sub);

                        // Boundary mark of dummysub is 0, so we don't need to worry about that.
                        yield return new Edge(p1.id, p2.id, sub.seg.boundary);
                    }

                    tri.orient++;
                }
            }
        }

        /// <summary>
        /// Enumerate all edges of the given mesh.
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="skipSegments"></param>
        /// <returns></returns>
        /// <remarks>
        /// In contrast to <see cref="EnumerateEdges(IMesh)"/> this method will return
        /// objects that include the vertex information (and not only the indices).
        /// </remarks>
        public static IEnumerable<ISegment> EnumerateEdges(IMesh mesh, bool skipSegments = true)
        {
            Otri tri = default;
            Otri neighbor = default;
            Osub sub = default;

            Vertex p1, p2;

            bool segments = !skipSegments;

            foreach (var t in mesh.Triangles)
            {
                tri.tri = t;
                tri.orient = 0;

                for (int i = 0; i < 3; i++)
                {
                    tri.Sym(ref neighbor);

                    int nid = neighbor.tri.id;

                    if ((tri.tri.id < nid) || (nid == Mesh.DUMMY))
                    {
                        p1 = tri.Org();
                        p2 = tri.Dest();

                        tri.Pivot(ref sub);

                        if (sub.seg.hash == Mesh.DUMMY)
                        {
                            yield return new Segment(p1, p2);
                        }
                        else if (segments)
                        {
                            // Segments might be processed separately, so only
                            // include them if requested.
                            yield return sub.seg;
                        }
                    }

                    tri.orient++;
                }
            }
        }
    }
}
