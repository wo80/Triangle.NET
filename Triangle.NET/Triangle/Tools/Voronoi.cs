// -----------------------------------------------------------------------
// <copyright file="Voronoi.cs">
// Original Triangle code by Jonathan Richard Shewchuk, http://www.cs.cmu.edu/~quake/triangle.html
// Triangle.NET code by Christian Woltering, http://home.edo.tu-dortmund.de/~woltering/index.html
// </copyright>
// -----------------------------------------------------------------------

using TriangleNet.Data;
using TriangleNet.Geometry;

namespace TriangleNet.Tools
{
    /// <summary>
    /// The Voronoi Diagram is the dual of a pointset triangulation.
    /// </summary>
    public class Voronoi
    {
        Mesh mesh;
        Point[] points;
        Point[] directions; // Stores the direction for infinite voronoi edges
        Edge[] edges;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoundedVoronoi" /> class.
        /// </summary>
        /// <param name="mesh"></param>
        /// <remarks>
        /// Be sure MakeVertexMap has been called (should always be the case).
        /// </remarks>
        public Voronoi(Mesh mesh)
        {
            this.mesh = mesh;
        }

        /// <summary>
        /// Gets the list of Voronoi vertices.
        /// </summary>
        public Point[] Points
        {
            get { return points; }
        }

        /// <summary>
        /// Gets the directions for infinite Voronoi edges.
        /// </summary>
        public Point[] Directions
        {
            get { return directions; }
        }

        /// <summary>
        /// Gets the list of Voronoi edges.
        /// </summary>
        public Edge[] Edges
        {
            get { return edges; }
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
        ///</remarks>
        public void Generate()
        {
            Otri tri = default(Otri), trisym = default(Otri);
            Vertex torg, tdest, tapex;
            Point circumcenter;
            double xi = 0, eta = 0;
            int p1, p2;

            // Allocate memory for Voronoi vertices.
            this.points = new Point[mesh.triangles.Count];

            tri.orient = 0;

            int i = 0;
            foreach (var item in mesh.triangles.Values)
            {
                tri.triangle = item;
                torg = tri.Org();
                tdest = tri.Dest();
                tapex = tri.Apex();
                circumcenter = Primitives.FindCircumcenter(torg, tdest, tapex, ref xi, ref eta, false);

                // X and y coordinates.
                this.points[i] = new Point(circumcenter.x, circumcenter.y);

                // Update element id
                tri.triangle.id = i++;
            }

            // Allocate memory for output Voronoi edges.
            this.edges = new Edge[mesh.edges];

            // Allocate memory for output Voronoi norms.
            this.directions = new Point[mesh.edges];

            i = 0;
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
                            this.edges[i] = new Edge(p1, -1);
                            this.directions[i] = new Point(tdest.y - torg.y, torg.x - tdest.x);
                        }
                        else
                        {
                            // Find the number of the adjacent triangle (and Voronoi vertex).
                            p2 = trisym.triangle.id;
                            // Finite edge. Write indices of two endpoints.

                            this.edges[i] = new Edge(p1, p2);
                            this.directions[i] = new Point(0, 0);
                        }

                        i++;
                    }
                }
            }
        }
    }
}
