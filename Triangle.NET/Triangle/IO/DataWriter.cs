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

    /// <summary>
    /// Generates a mesh representaion using arrays.
    /// </summary>
    public static class DataWriter
    {
        static NumberFormatInfo nfi = CultureInfo.InvariantCulture.NumberFormat;

        static int verticesCount;
        static int elementsCount;

        #region Library

        /// <summary>
        /// Number the vertices and write them to raw output data.
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="data"></param>
        public static void WriteNodes(Mesh mesh, MeshData data)
        {
            Vertex vertex;
            int outvertices = mesh.vertices.Count;

            if (Behavior.Jettison)
            {
                outvertices = mesh.vertices.Count - mesh.undeads;
            }

            verticesCount = outvertices;

            // Allocate memory for output vertices if necessary.
            data.Points = new double[outvertices][];

            // Allocate memory for output vertex attributes if necessary.
            if (mesh.nextras > 0)
            {
                data.PointAttributes = new double[outvertices][];
            }
            // Allocate memory for output vertex markers if necessary.
            if (Behavior.UseBoundaryMarkers)
            {
                data.PointMarkers = new int[outvertices];
            }
            
            int i = 0;
            foreach (var item in mesh.vertices.Values)
            {
                vertex = item;

                if (!Behavior.Jettison || vertex.type != VertexType.UndeadVertex)
                {
                    // X and y coordinates.
                    data.Points[i] = new double[] { vertex.pt.X, vertex.pt.Y };

                    // Vertex attributes.
                    if (data.PointAttributes != null)
                    {
                        data.PointAttributes[i] = vertex.attributes;
                    }

                    if (Behavior.UseBoundaryMarkers)
                    {
                        // Save the boundary marker.
                        data.PointMarkers[i] = vertex.mark;
                    }

                    // Assign array index to vertex ID for later use.
                    vertex.ID = i++;
                }
            }
        }

        /// <summary>
        /// Write the triangles to raw output data.
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="data"></param>
        public static void WriteElements(Mesh mesh, MeshData data)
        {
            Otri tri = default(Otri);
            Vertex p1, p2, p3;
            
            elementsCount = mesh.triangles.Count;

            // Allocate memory for output triangles if necessary.
            data.Triangles = new int[elementsCount][];

            // Allocate memory for output triangle attributes if necessary.
            if (mesh.eextras > 0)
            {
                data.TriangleAttributes = new double[mesh.triangles.Count][];
            }

            tri.orient = 0;
            
            int i = 0;
            foreach (var item in mesh.triangles.Values)
            {
                tri.triangle = item;

                p1 = tri.Org();
                p2 = tri.Dest();
                p3 = tri.Apex();

                // Triangle order is always 1 (no higher order elements supported)
                data.Triangles[i] = new int[] { p1.ID, p2.ID, p3.ID };

                if (data.TriangleAttributes != null)
                {
                    data.TriangleAttributes[i] = tri.triangle.attributes;
                }

                // Update ID for later use
                item.ID = i++;
            }
        }

        /// <summary>
        /// Write the segments and holes to raw output data.
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="data"></param>
        public static void WritePoly(Mesh mesh, MeshData data)
        {
            Osub subseg = default(Osub);
            Vertex pt1, pt2;
            int n = mesh.subsegs.Count;

            // Allocate memory for output segments if necessary.
            data.Segments = new int[n][];

            // Allocate memory for output segment markers if necessary.
            if (Behavior.UseBoundaryMarkers)
            {
                data.SegmentMarkers = new int[n];
            }

            subseg.ssorient = 0;

            int i = 0;
            foreach (var item in mesh.subsegs.Values)
            {
                subseg.ss = item;

                pt1 = subseg.Org();
                pt2 = subseg.Dest();

                // Copy indices of the segment's two endpoints.
                data.Segments[i] = new int[] { pt1.ID, pt2.ID };

                // Copy the boundary marker.
                if (Behavior.UseBoundaryMarkers)
                {
                    data.SegmentMarkers[i] = subseg.ss.boundary;
                }

                i++;
            }

            n = mesh.holes.Count;

            if (n > 0)
            {
                data.Holes = new double[n][];

                i = 0;
                foreach (var hole in mesh.holes)
                {
                    data.Holes[i] = new double[] { hole.X, hole.Y };
                    i++;
                }
            }
        }

        /// <summary>
        /// Write the edges to raw output data.
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="data"></param>
        public static void WriteEdges(Mesh mesh, MeshData data)
        {
            Otri tri = default(Otri), trisym = default(Otri);
            Osub checkmark = default(Osub);
            Vertex p1, p2;

            // Allocate memory for edges if necessary.
            data.Edges = new int[mesh.edges][];

            // Allocate memory for edge markers if necessary.
            if (Behavior.UseBoundaryMarkers)
            {
                data.EdgeMarkers = new int[mesh.edges];
            }

            int index = 0;
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
                    if ((tri.triangle.ID < trisym.triangle.ID) || (trisym.triangle == Mesh.dummytri))
                    {
                        p1 = tri.Org();
                        p2 = tri.Dest();

                        data.Edges[index] = new int[] { p1.ID, p2.ID };
                        if (Behavior.UseBoundaryMarkers)
                        {
                            // Edge number, indices of two endpoints, and a boundary marker.
                            // If there's no subsegment, the boundary marker is zero.
                            if (Behavior.UseSegments)
                            {
                                tri.SegPivot(ref checkmark);
                                if (checkmark.ss == Mesh.dummysub)
                                {
                                    data.EdgeMarkers[index] = 0;
                                }
                                else
                                {
                                    data.EdgeMarkers[index] = checkmark.ss.boundary;
                                }
                            }
                            else
                            {
                                data.EdgeMarkers[index] = (trisym.triangle == Mesh.dummytri ? 1 : 0);
                            }
                        }
                        index++;
                    }
                }
            }
        }

        /// <summary>
        /// Write the triangle neighbors to raw output data.
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="data"></param>
        /// <remarks>WARNING: Be sure WriteElements has been called before, 
        /// so the elements are numbered right!</remarks>
        public static void WriteNeighbors(Mesh mesh, MeshData data)
        {
            Otri tri = default(Otri), trisym = default(Otri);

            // Allocate memory for neighbors if necessary.
            data.Neighbors = new int[mesh.triangles.Count][];

            Mesh.dummytri.ID = -1;

            int i = 0;
            foreach (var item in mesh.triangles.Values)
            {
                data.Neighbors[i] = new int[3];

                tri.triangle = item;

                tri.orient = 1;
                tri.Sym(ref trisym);
                data.Neighbors[i][0] = trisym.triangle.ID;

                tri.orient = 2;
                tri.Sym(ref trisym);
                data.Neighbors[i][1] = trisym.triangle.ID;

                tri.orient = 0;
                tri.Sym(ref trisym);
                data.Neighbors[i][2] = trisym.triangle.ID;

                i++;
            }
        }

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
            Point2 circumcenter;
            double xi = 0, eta = 0;
            int p1, p2;

            int i = 0;

            // Copy input points (actually not part of the voronoi diagram)
            data.InputPoints = new double[mesh.vertices.Count][];

            foreach (var item in mesh.vertices.Values)
            {
                if (item.type != VertexType.UndeadVertex)
                {
                    data.InputPoints[i] = new double[] { item.pt.X, item.pt.Y };
                    i++;
                }
            }

            // Allocate memory for Voronoi vertices.
            data.Points = new double[mesh.triangles.Count][];

            int index = 0;

            tri.orient = 0;
            
            i = 0;
            foreach (var item in mesh.triangles.Values)
            {
                tri.triangle = item;
                torg = tri.Org();
                tdest = tri.Dest();
                tapex = tri.Apex();
                circumcenter = Primitives.FindCircumcenter(torg.pt, tdest.pt, tapex.pt, ref xi, ref eta, false);

                // X and y coordinates.
                data.Points[i] = new double[] { circumcenter.X, circumcenter.Y };

                // Update element id
                tri.triangle.ID = i++;
            }

            // Allocate memory for output Voronoi edges.
            data.Edges = new int[mesh.edges][];

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
                    if ((tri.triangle.ID < trisym.triangle.ID) || (trisym.triangle == Mesh.dummytri))
                    {
                        // Find the number of this triangle (and Voronoi vertex).
                        p1 = tri.triangle.ID;

                        if (trisym.triangle == Mesh.dummytri)
                        {
                            torg = tri.Org();
                            tdest = tri.Dest();

                            // Copy an infinite ray. Index of one endpoint, and -1.
                            data.Edges[index] = new int[] { p1, -1};
                            data.Directions[index] = new double[] { tdest[1] - torg[1], torg[0] - tdest[0] };
                        }
                        else
                        {
                            // Find the number of the adjacent triangle (and Voronoi vertex).
                            p2 = trisym.triangle.ID;
                            // Finite edge. Write indices of two endpoints.

                            data.Edges[index] = new int[] { p1, p2 };
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
