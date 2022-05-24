// -----------------------------------------------------------------------
// <copyright file="MeshValidator.cs">
// Triangle Copyright (c) 1993, 1995, 1997, 1998, 2002, 2005 Jonathan Richard Shewchuk
// Triangle.NET code by Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet
{
    using System;
    using System.Collections.Generic;
    using TriangleNet.Geometry;
    using TriangleNet.Meshing;
    using TriangleNet.Tools;
    using TriangleNet.Topology;

    /// <summary>
    /// Mesh validation helper.
    /// </summary>
    public static class MeshValidator
    {
        private static RobustPredicates predicates = RobustPredicates.Default;

        /// <summary>
        /// Test the mesh for topological consistency.
        /// </summary>
        /// <param name="mesh">The mesh.</param>
        /// <returns>True, if mesh is topologically consistent.</returns>
        public static bool IsConsistent(Mesh mesh)
        {
            Otri tri = default(Otri);
            Otri oppotri = default(Otri), oppooppotri = default(Otri);
            Vertex org, dest, apex;
            Vertex oppoorg, oppodest;

            var logger = Log.Instance;

            // Temporarily turn on exact arithmetic if it's off.
            bool saveexact = Behavior.NoExact;
            Behavior.NoExact = false;

            int horrors = 0;

            // Run through the list of triangles, checking each one.
            foreach (var t in mesh.triangles)
            {
                tri.tri = t;

                // Check all three edges of the triangle.
                for (tri.orient = 0; tri.orient < 3; tri.orient++)
                {
                    org = tri.Org();
                    dest = tri.Dest();
                    if (tri.orient == 0)
                    {
                        // Only test for inversion once.
                        // Test if the triangle is flat or inverted.
                        apex = tri.Apex();
                        if (predicates.CounterClockwise(org, dest, apex) <= 0.0)
                        {
                            if (Log.Verbose)
                            {
                                logger.Warning(string.Format("Triangle is flat or inverted (ID {0}).", t.id),
                                    "MeshValidator.IsConsistent()");
                            }

                            horrors++;
                        }
                    }

                    // Find the neighboring triangle on this edge.
                    tri.Sym(ref oppotri);
                    if (oppotri.tri.id != Mesh.DUMMY)
                    {
                        // Check that the triangle's neighbor knows it's a neighbor.
                        oppotri.Sym(ref oppooppotri);
                        if ((tri.tri != oppooppotri.tri) || (tri.orient != oppooppotri.orient))
                        {
                            if (tri.tri == oppooppotri.tri && Log.Verbose)
                            {
                                logger.Warning("Asymmetric triangle-triangle bond: (Right triangle, wrong orientation)",
                                    "MeshValidator.IsConsistent()");
                            }

                            horrors++;
                        }
                        // Check that both triangles agree on the identities
                        // of their shared vertices.
                        oppoorg = oppotri.Org();
                        oppodest = oppotri.Dest();
                        if ((org != oppodest) || (dest != oppoorg))
                        {
                            if (Log.Verbose)
                            {
                                logger.Warning("Mismatched edge coordinates between two triangles.",
                                    "MeshValidator.IsConsistent()");
                            }

                            horrors++;
                        }
                    }
                }
            }

            // Check for unconnected vertices
            mesh.MakeVertexMap();
            foreach (var v in mesh.vertices.Values)
            {
                if (v.tri.tri == null && Log.Verbose)
                {
                    logger.Warning("Vertex (ID " + v.id + ") not connected to mesh (duplicate input vertex?)",
                                "MeshValidator.IsConsistent()");
                }
            }

            // Restore the status of exact arithmetic.
            Behavior.NoExact = saveexact;

            return (horrors == 0);
        }

        /// <summary>
        /// Check whether the mesh is (conforming) Delaunay.
        /// </summary>
        /// <param name="mesh">The mesh.</param>
        /// <returns>True, if mesh is (conforming) Delaunay.</returns>
        public static bool IsDelaunay(Mesh mesh)
        {
            return IsDelaunay(mesh, false);
        }

        /// <summary>
        /// Check whether the mesh is (constrained) Delaunay.
        /// </summary>
        /// <param name="mesh">The mesh.</param>
        /// <returns>True, if mesh is (constrained) Delaunay.</returns>
        public static bool IsConstrainedDelaunay(Mesh mesh)
        {
            return IsDelaunay(mesh, true);
        }

        /// <summary>
        /// Ensure that the mesh is (constrained) Delaunay.
        /// </summary>
        private static bool IsDelaunay(Mesh mesh, bool constrained)
        {
            Otri loop = default(Otri);
            Otri oppotri = default(Otri);
            Osub opposubseg = default(Osub);
            Vertex org, dest, apex;
            Vertex oppoapex;

            bool shouldbedelaunay;

            var logger = Log.Instance;

            // Temporarily turn on exact arithmetic if it's off.
            bool saveexact = Behavior.NoExact;
            Behavior.NoExact = false;

            int horrors = 0;

            var inf1 = mesh.infvertex1;
            var inf2 = mesh.infvertex2;
            var inf3 = mesh.infvertex3;

            // Run through the list of triangles, checking each one.
            foreach (var tri in mesh.triangles)
            {
                loop.tri = tri;

                // Check all three edges of the triangle.
                for (loop.orient = 0; loop.orient < 3; loop.orient++)
                {
                    org = loop.Org();
                    dest = loop.Dest();
                    apex = loop.Apex();

                    loop.Sym(ref oppotri);
                    oppoapex = oppotri.Apex();

                    // Only test that the edge is locally Delaunay if there is an
                    // adjoining triangle whose pointer is larger (to ensure that
                    // each pair isn't tested twice).
                    shouldbedelaunay = (loop.tri.id < oppotri.tri.id) &&
                           !Otri.IsDead(oppotri.tri) && (oppotri.tri.id != Mesh.DUMMY) &&
                          (org != inf1) && (org != inf2) && (org != inf3) &&
                          (dest != inf1) && (dest != inf2) && (dest != inf3) &&
                          (apex != inf1) && (apex != inf2) && (apex != inf3) &&
                          (oppoapex != inf1) && (oppoapex != inf2) && (oppoapex != inf3);

                    if (constrained && mesh.checksegments && shouldbedelaunay)
                    {
                        // If a subsegment separates the triangles, then the edge is
                        // constrained, so no local Delaunay test should be done.
                        loop.Pivot(ref opposubseg);

                        if (opposubseg.seg.hash != Mesh.DUMMY)
                        {
                            shouldbedelaunay = false;
                        }
                    }

                    if (shouldbedelaunay)
                    {
                        if (predicates.NonRegular(org, dest, apex, oppoapex) > 0.0)
                        {
                            if (Log.Verbose)
                            {
                                logger.Warning(string.Format("Non-regular pair of triangles found (IDs {0}/{1}).",
                                    loop.tri.id, oppotri.tri.id), "MeshValidator.IsDelaunay()");
                            }

                            horrors++;
                        }
                    }
                }

            }

            // Restore the status of exact arithmetic.
            Behavior.NoExact = saveexact;

            return (horrors == 0);
        }

        /// <summary>
        /// Check whether the mesh has degenerate boundary triangles.
        /// </summary>
        /// <param name="mesh">The mesh.</param>
        /// <param name="threshold">Threshold for what angle is considered invalid (too small).</param>
        /// <returns></returns>
        public static IEnumerable<ITriangle> GetDegenerateBoundaryTriangles(IMesh mesh, double threshold = 1e-8)
        {
            // We will compare against the squared cosine of the maximum angle.
            threshold = Math.Sqrt(threshold);

            var data = new double[6];

            foreach (var triangle in mesh.Triangles)
            {
                for (int i = 0; i < 3; i++)
                {
                    var neighbor = triangle.GetNeighbor(i);

                    // Triangle lies on mesh boundary.
                    if (neighbor == null)
                    {
                        Statistic.ComputeAngles(triangle, data);
                        
                        // The squared cosine of the maximum angle will be near 1.0 only
                        // if the maximum angle is near 180 degrees.
                        if (Math.Abs(1.0 - data[1]) < threshold)
                        {
                            yield return triangle;
                        }

                        // Next triangle.
                        break;
                    }
                }
            }
        }
    }
}
