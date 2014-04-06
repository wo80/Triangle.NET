// -----------------------------------------------------------------------
// <copyright file="MeshValidator.cs">
// Original Triangle code by Jonathan Richard Shewchuk, http://www.cs.cmu.edu/~quake/triangle.html
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet
{
    using System;
    using TriangleNet.Data;
    using TriangleNet.Log;

    public static class MeshValidator
    {
        /// <summary>
        /// Test the mesh for topological consistency.
        /// </summary>
        public static bool IsConsistent(Mesh mesh)
        {
            Otri tri = default(Otri);
            Otri oppotri = default(Otri), oppooppotri = default(Otri);
            Vertex triorg, tridest, triapex;
            Vertex oppoorg, oppodest;
            int horrors;
            bool saveexact;

            var logger = SimpleLog.Instance;

            // Temporarily turn on exact arithmetic if it's off.
            saveexact = Behavior.NoExact;
            Behavior.NoExact = false;
            horrors = 0;

            // Run through the list of triangles, checking each one.
            foreach (var t in mesh.triangles.Values)
            {
                tri.triangle = t;

                // Check all three edges of the triangle.
                for (tri.orient = 0; tri.orient < 3; tri.orient++)
                {
                    triorg = tri.Org();
                    tridest = tri.Dest();
                    if (tri.orient == 0)
                    {   // Only test for inversion once.
                        // Test if the triangle is flat or inverted.
                        triapex = tri.Apex();
                        if (Primitives.CounterClockwise(triorg, tridest, triapex) <= 0.0)
                        {
                            logger.Warning("Triangle is flat or inverted.",
                                "Quality.CheckMesh()");
                            horrors++;
                        }
                    }
                    // Find the neighboring triangle on this edge.
                    tri.Sym(ref oppotri);
                    if (oppotri.triangle != Mesh.dummytri)
                    {
                        // Check that the triangle's neighbor knows it's a neighbor.
                        oppotri.Sym(ref oppooppotri);
                        if ((tri.triangle != oppooppotri.triangle) || (tri.orient != oppooppotri.orient))
                        {
                            if (tri.triangle == oppooppotri.triangle)
                            {
                                logger.Warning("Asymmetric triangle-triangle bond: (Right triangle, wrong orientation)",
                                    "Quality.CheckMesh()");
                            }

                            horrors++;
                        }
                        // Check that both triangles agree on the identities
                        // of their shared vertices.
                        oppoorg = oppotri.Org();
                        oppodest = oppotri.Dest();
                        if ((triorg != oppodest) || (tridest != oppoorg))
                        {
                            logger.Warning("Mismatched edge coordinates between two triangles.",
                                "Quality.CheckMesh()");

                            horrors++;
                        }
                    }
                }
            }

            // Check for unconnected vertices
            mesh.MakeVertexMap();
            foreach (var v in mesh.vertices.Values)
            {
                if (v.tri.triangle == null)
                {
                    logger.Warning("Vertex (ID " + v.id + ") not connected to mesh (duplicate input vertex?)",
                                "Quality.CheckMesh()");
                }
            }

            if (horrors == 0) // && Behavior.Verbose
            {
                logger.Info("Mesh topology appears to be consistent.");
            }

            // Restore the status of exact arithmetic.
            Behavior.NoExact = saveexact;

            return (horrors == 0);
        }

        /// <summary>
        /// Ensure that the mesh is (constrained) Delaunay.
        /// </summary>
        public static bool IsDelaunay(Mesh mesh)
        {
            Otri loop = default(Otri);
            Otri oppotri = default(Otri);
            Osub opposubseg = default(Osub);
            Vertex triorg, tridest, triapex;
            Vertex oppoapex;
            bool shouldbedelaunay;
            int horrors;
            bool saveexact;

            var logger = SimpleLog.Instance;

            // Temporarily turn on exact arithmetic if it's off.
            saveexact = Behavior.NoExact;
            Behavior.NoExact = false;
            horrors = 0;

            // Run through the list of triangles, checking each one.
            foreach (var tri in mesh.triangles.Values)
            {
                loop.triangle = tri;

                // Check all three edges of the triangle.
                for (loop.orient = 0; loop.orient < 3;
                     loop.orient++)
                {
                    triorg = loop.Org();
                    tridest = loop.Dest();
                    triapex = loop.Apex();
                    loop.Sym(ref oppotri);
                    oppoapex = oppotri.Apex();
                    // Only test that the edge is locally Delaunay if there is an
                    // adjoining triangle whose pointer is larger (to ensure that
                    // each pair isn't tested twice).
                    shouldbedelaunay = (oppotri.triangle != Mesh.dummytri) &&
                          !Otri.IsDead(oppotri.triangle) && loop.triangle.id < oppotri.triangle.id &&
                          (triorg != mesh.infvertex1) && (triorg != mesh.infvertex2) &&
                          (triorg != mesh.infvertex3) &&
                          (tridest != mesh.infvertex1) && (tridest != mesh.infvertex2) &&
                          (tridest != mesh.infvertex3) &&
                          (triapex != mesh.infvertex1) && (triapex != mesh.infvertex2) &&
                          (triapex != mesh.infvertex3) &&
                          (oppoapex != mesh.infvertex1) && (oppoapex != mesh.infvertex2) &&
                          (oppoapex != mesh.infvertex3);
                    if (mesh.checksegments && shouldbedelaunay)
                    {
                        // If a subsegment separates the triangles, then the edge is
                        // constrained, so no local Delaunay test should be done.
                        loop.SegPivot(ref opposubseg);
                        if (opposubseg.seg != Mesh.dummysub)
                        {
                            shouldbedelaunay = false;
                        }
                    }
                    if (shouldbedelaunay)
                    {
                        if (Primitives.NonRegular(triorg, tridest, triapex, oppoapex) > 0.0)
                        {
                            logger.Warning(String.Format("Non-regular pair of triangles found (IDs {0}/{1}).",
                                loop.triangle.id, oppotri.triangle.id), "Quality.CheckDelaunay()");
                            horrors++;
                        }
                    }
                }

            }

            if (horrors == 0) // && Behavior.Verbose
            {
                logger.Info("Mesh is Delaunay.");
            }

            // Restore the status of exact arithmetic.
            Behavior.NoExact = saveexact;

            return (horrors == 0);
        }
    }
}
