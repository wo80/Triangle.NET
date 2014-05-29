// -----------------------------------------------------------------------
// <copyright file="Voronoi.cs">
// Original Triangle code by Jonathan Richard Shewchuk, http://www.cs.cmu.edu/~quake/triangle.html
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using TriangleNet.Data;
using TriangleNet.Geometry;

namespace TriangleNet.Tools
{
    /// <summary>
    /// The Voronoi Diagram is the dual of a pointset triangulation.
    /// </summary>
    public class Voronoi : IVoronoi
    {
        Mesh mesh;

        Point[] points;
        Dictionary<int, VoronoiRegion> regions;

        // Stores the endpoints of rays of unbounded Voronoi cells
        Dictionary<int, Point> rayPoints;
        int rayIndex;

        // Bounding box of the triangles circumcenters.
        Rectangle bounds;

        /// <summary>
        /// Initializes a new instance of the <see cref="Voronoi" /> class.
        /// </summary>
        /// <param name="mesh"></param>
        /// <remarks>
        /// Be sure MakeVertexMap has been called (should always be the case).
        /// </remarks>
        public Voronoi(Mesh mesh)
        {
            this.mesh = mesh;

            Generate();
        }

        /// <summary>
        /// Gets the list of Voronoi vertices.
        /// </summary>
        public Point[] Points
        {
            get { return points; }
        }

        /// <summary>
        /// Gets the list of Voronoi regions.
        /// </summary>
        public ICollection<VoronoiRegion> Regions
        {
            get { return regions.Values; }
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
        private void Generate()
        {
            mesh.Renumber();
            mesh.MakeVertexMap();

            // Allocate space for voronoi diagram
            this.points = new Point[mesh.triangles.Count + mesh.hullsize];
            this.regions = new Dictionary<int, VoronoiRegion>(mesh.vertices.Count);

            rayPoints = new Dictionary<int, Point>();
            rayIndex = 0;

            bounds = new Rectangle();

            // Compute triangles circumcenters and setup bounding box
            ComputeCircumCenters();

            // Add all Voronoi regions to the map.
            foreach (var vertex in mesh.vertices.Values)
            {
                regions.Add(vertex.id, new VoronoiRegion(vertex));
            }

            // Loop over the mesh vertices (Voronoi generators).
            foreach (var region in regions.Values)
            {
                //if (item.Boundary == 0)
                {
                    ConstructVoronoiRegion(region);
                }
            }
        }

        private void ComputeCircumCenters()
        {
            Otri tri = default(Otri);
            double xi = 0, eta = 0;
            Point pt;

            // Compue triangle circumcenters
            foreach (var item in mesh.triangles.Values)
            {
                tri.triangle = item;

                pt = Primitives.FindCircumcenter(tri.Org(), tri.Dest(), tri.Apex(), ref xi, ref eta);
                pt.id = item.id;

                points[item.id] = pt;

                bounds.Expand(pt);
            }

            double ds = Math.Max(bounds.Width, bounds.Height);
            bounds.Resize(ds, ds);
        }

        /// <summary>
        /// Construct Voronoi region for given vertex.
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns>The circumcenter indices which make up the cell.</returns>
        private void ConstructVoronoiRegion(VoronoiRegion region)
        {
            var vertex = region.Generator as Vertex;

            var vpoints = new List<Point>();

            Otri f = default(Otri);
            Otri f_init = default(Otri);
            Otri f_next = default(Otri);
            Otri f_prev = default(Otri);

            Osub sub = default(Osub);

            // Call f_init a triangle incident to x
            vertex.tri.Copy(ref f_init);

            f_init.Copy(ref f);
            f_init.Onext(ref f_next);

            // Check if f_init lies on the boundary of the triangulation.
            if (f_next.triangle == Mesh.dummytri)
            {
                f_init.Oprev(ref f_prev);

                if (f_prev.triangle != Mesh.dummytri)
                {
                    f_init.Copy(ref f_next);
                    // Move one triangle clockwise
                    f_init.OprevSelf();
                    f_init.Copy(ref f);
                }
            }

            // Go counterclockwise until we reach the border or the initial triangle.
            while (f_next.triangle != Mesh.dummytri)
            {
                // Add circumcenter of current triangle
                vpoints.Add(points[f.triangle.id]);

                region.AddNeighbor(f.triangle.id, regions[f.Apex().id]);

                if (f_next.Equal(f_init))
                {
                    // Voronoi cell is complete (bounded case).
                    region.Add(vpoints);
                    return;
                }

                f_next.Copy(ref f);
                f_next.OnextSelf();
            }

            // Voronoi cell is unbounded
            region.Bounded = false;

            Vertex torg, tdest, tapex;
            Point intersection;
            int sid, n = mesh.triangles.Count;

            // Find the boundary segment id (we use this id to number the endpoints of infinit rays).
            f.Lprev(ref f_next);
            f_next.SegPivot(ref sub);
            sid = sub.seg.hash;

            // Last valid f lies at the boundary. Add the circumcenter.
            vpoints.Add(points[f.triangle.id]);
            region.AddNeighbor(f.triangle.id, regions[f.Apex().id]);

            // Check if the intersection with the bounding box has already been computed.
            if (!rayPoints.TryGetValue(sid, out intersection))
            {
                torg = f.Org();
                tapex = f.Apex();
                BoxRayIntersection(points[f.triangle.id], torg.y - tapex.y, tapex.x - torg.x, out intersection);

                // Set the correct id for the vertex
                intersection.id = n + rayIndex;

                points[n + rayIndex] = intersection;
                rayIndex++;

                rayPoints.Add(sid, intersection);
            }

            vpoints.Add(intersection);

            // Now walk from f_init clockwise till we reach the boundary.
            vpoints.Reverse();

            f_init.Copy(ref f);
            f.Oprev(ref f_prev);

            while (f_prev.triangle != Mesh.dummytri)
            {
                vpoints.Add(points[f_prev.triangle.id]);
                region.AddNeighbor(f_prev.triangle.id, regions[f_prev.Apex().id]);

                f_prev.Copy(ref f);
                f_prev.OprevSelf();
            }

            // Find the boundary segment id.
            f.SegPivot(ref sub);
            sid = sub.seg.hash;

            if (!rayPoints.TryGetValue(sid, out intersection))
            {
                // Intersection has not been computed yet.
                torg = f.Org();
                tdest = f.Dest();

                BoxRayIntersection(points[f.triangle.id], tdest.y - torg.y, torg.x - tdest.x, out intersection);

                // Set the correct id for the vertex
                intersection.id = n + rayIndex;

                rayPoints.Add(sid, intersection);

                points[n + rayIndex] = intersection;
                rayIndex++;
            }

            vpoints.Add(intersection);
            region.AddNeighbor(intersection.id, regions[f.Dest().id]);

            // Add the new points to the region (in counter-clockwise order)
            vpoints.Reverse();
            region.Add(vpoints);
        }

        private bool BoxRayIntersection(Point pt, double dx, double dy, out Point intersect)
        {
            double x = pt.X;
            double y = pt.Y;

            double t1, x1, y1, t2, x2, y2;

            // Bounding box
            double minX = bounds.Left;
            double maxX = bounds.Right;
            double minY = bounds.Bottom;
            double maxY = bounds.Top;

            // Check if point is inside the bounds
            if (x < minX || x > maxX || y < minY || y > maxY)
            {
                intersect = null;
                return false;
            }

            // Calculate the cut through the vertical boundaries
            if (dx < 0)
            {
                // Line going to the left: intersect with x = minX
                t1 = (minX - x) / dx;
                x1 = minX;
                y1 = y + t1 * dy;
            }
            else if (dx > 0)
            {
                // Line going to the right: intersect with x = maxX
                t1 = (maxX - x) / dx;
                x1 = maxX;
                y1 = y + t1 * dy;
            }
            else
            {
                // Line going straight up or down: no intersection possible
                t1 = double.MaxValue;
                x1 = y1 = 0;
            }

            // Calculate the cut through upper and lower boundaries
            if (dy < 0)
            {
                // Line going downwards: intersect with y = minY
                t2 = (minY - y) / dy;
                x2 = x + t2 * dx;
                y2 = minY;
            }
            else if (dy > 0)
            {
                // Line going upwards: intersect with y = maxY
                t2 = (maxY - y) / dy;
                x2 = x + t2 * dx;
                y2 = maxY;
            }
            else
            {
                // Horizontal line: no intersection possible
                t2 = double.MaxValue;
                x2 = y2 = 0;
            }

            if (t1 < t2)
            {
                intersect = new Point(x1, y1);
            }
            else
            {
                intersect = new Point(x2, y2);
            }

            return true;
        }
    }
}
