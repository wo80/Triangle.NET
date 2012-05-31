// -----------------------------------------------------------------------
// <copyright file="io.cs" company="">
// Original Triangle code by Jonathan Richard Shewchuk, http://www.cs.cmu.edu/~quake/triangle.html
// Triangle.NET code by Christian Woltering, http://home.edo.tu-dortmund.de/~woltering/triangle/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.IO
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Globalization;
    using TriangleNet.Data;
    using TriangleNet.Geometry;

    /// <summary>
    /// Generates a mesh representaion using arrays.
    /// </summary>
    public static class DataWriter
    {
        #region Library

        /// <summary>
        /// Gets the Voronoi diagram as raw output data.
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        /// <remarks>
        /// The Voronoi diagram is the geometric dual of the Delaunay triangulation.
        /// Hence, the Voronoi vertices are listed by traversing the Delaunay
        /// triangles, and the Voronoi edges are listed by traversing the Delaunay
        /// edges.
        ///
        /// WARNING:  In order to assign numbers to the Voronoi vertices, this
        /// procedure messes up the subsegments or the extra nodes of every
        /// element.  Hence, you should call this procedure last.</remarks>
        public static VoronoiData WriteVoronoi(Mesh mesh)
        {
            VoronoiData data = new VoronoiData();

            Otri tri = default(Otri), trisym = default(Otri);
            Vertex torg, tdest, tapex;
            Point circumcenter;
            double xi = 0, eta = 0;
            int p1, p2;

            int i = 0;

            // Copy input points (actually not part of the voronoi diagram)
            data.InputPoints = new Vertex[mesh.vertices.Count];

            foreach (var item in mesh.vertices.Values)
            {
                if (item.type != VertexType.UndeadVertex)
                {
                    data.InputPoints[i] = item;
                    i++;
                }
            }

            // Allocate memory for Voronoi vertices.
            data.Points = new Vertex[mesh.triangles.Count];

            int index = 0;

            tri.orient = 0;

            i = 0;
            foreach (var item in mesh.triangles.Values)
            {
                tri.triangle = item;
                torg = tri.Org();
                tdest = tri.Dest();
                tapex = tri.Apex();
                circumcenter = Primitives.FindCircumcenter(torg, tdest, tapex, ref xi, ref eta, false);

                // X and y coordinates.
                data.Points[i] = new Point(circumcenter.x, circumcenter.y);

                // Update element id
                tri.triangle.id = i++;
            }

            // Allocate memory for output Voronoi edges.
            data.Edges = new Edge[mesh.edges];

            // Allocate memory for output Voronoi norms.
            data.Directions = new double[mesh.edges][];

            index = 0;
            // To loop over the set of edges, loop over all triangles, and look at
            // the three edges of each triangle.  If there isn't another triangle
            // adjacent to the edge, operate on the edge.  If there is another
            // adjacent triangle, operate on the edge only if the current triangle
            // has a smaller pointer than its neighbor.  This way, each edge is
            // considered only once.
            foreach (var item in mesh.triangles.Values)
            {
                tri.triangle = item;

                for (tri.orient = 0; tri.orient < 3; tri.orient++)
                {
                    tri.Sym(ref trisym);
                    if ((tri.triangle.id < trisym.triangle.id) || (trisym.triangle == Mesh.dummytri))
                    {
                        // Find the number of this triangle (and Voronoi vertex).
                        p1 = tri.triangle.id;

                        if (trisym.triangle == Mesh.dummytri)
                        {
                            torg = tri.Org();
                            tdest = tri.Dest();

                            // Copy an infinite ray. Index of one endpoint, and -1.
                            data.Edges[index] = new Edge(p1, -1);
                            data.Directions[index] = new double[] { tdest[1] - torg[1], torg[0] - tdest[0] };
                        }
                        else
                        {
                            // Find the number of the adjacent triangle (and Voronoi vertex).
                            p2 = trisym.triangle.id;
                            // Finite edge. Write indices of two endpoints.

                            data.Edges[index] = new Edge(p1, p2);
                            data.Directions[index] = new double[] { 0, 0 };
                        }

                        index++;
                    }
                }
            }

            return data;
        }

        #endregion
    }
}
