﻿// -----------------------------------------------------------------------
// <copyright file="Voronoi.cs">
// Triangle Copyright (c) 1993, 1995, 1997, 1998, 2002, 2005 Jonathan Richard Shewchuk
// Triangle.NET code by Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Voronoi.Legacy
{
    using System;
    using System.Collections.Generic;
    using Topology;
    using Geometry;
    using Tools;

    /// <summary>
    /// The Voronoi Diagram is the dual of a pointset triangulation.
    /// </summary>
    [Obsolete("Use TriangleNet.Voronoi.StandardVoronoi class instead.")]
    public class SimpleVoronoi : IVoronoi
    {
        private IPredicates predicates = RobustPredicates.Default;

        private Mesh mesh;

        private Dictionary<int, VoronoiRegion> regions;

        // Stores the endpoints of rays of unbounded Voronoi cells
        private Dictionary<int, Point> rayPoints;
        private int rayIndex;

        // Bounding box of the triangles circumcenters.
        private Rectangle bounds;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleVoronoi" /> class.
        /// </summary>
        /// <param name="mesh"></param>
        /// <remarks>
        /// Be sure MakeVertexMap has been called (should always be the case).
        /// </remarks>
        public SimpleVoronoi(Mesh mesh)
        {
            this.mesh = mesh;

            Generate();
        }

        /// <summary>
        /// Gets the list of Voronoi vertices.
        /// </summary>
        public Point[] Points { get; private set; }

        /// <summary>
        /// Gets the list of Voronoi regions.
        /// </summary>
        public ICollection<VoronoiRegion> Regions => regions.Values;

        /// <summary>
        /// Enumerates the edges of the Voronoi diagram.
        /// </summary>
        public IEnumerable<IEdge> Edges => EnumerateEdges();

        /// <summary>
        /// Generate the Voronoi diagram.
        /// </summary>
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
            Points = new Point[mesh.triangles.Count + mesh.hullsize];
            regions = new Dictionary<int, VoronoiRegion>(mesh.vertices.Count);

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
                    ConstructCell(region);
                }
            }
        }

        private void ComputeCircumCenters()
        {
            var tri = default(Otri);
            double xi = 0, eta = 0;
            Point pt;

            // Compue triangle circumcenters
            foreach (var item in mesh.triangles)
            {
                tri.tri = item;

                pt = predicates.FindCircumcenter(tri.Org(), tri.Dest(), tri.Apex(), ref xi, ref eta);
                pt.id = item.id;

                Points[item.id] = pt;

                bounds.Expand(pt);
            }

            var ds = Math.Max(bounds.Width, bounds.Height);
            bounds.Resize(ds / 10, ds / 10);
        }

        /// <summary>
        /// Construct Voronoi region for given vertex.
        /// </summary>
        /// <param name="region"></param>
        private void ConstructCell(VoronoiRegion region)
        {
            var vertex = region.Generator as Vertex;

            var vpoints = new List<Point>();

            var f = default(Otri);
            var f_init = default(Otri);
            var f_next = default(Otri);
            var f_prev = default(Otri);

            var sub = default(Osub);

            // Call f_init a triangle incident to x
            vertex.tri.Copy(ref f_init);

            f_init.Copy(ref f);
            f_init.Onext(ref f_next);

            // Check if f_init lies on the boundary of the triangulation.
            if (f_next.tri.id == Mesh.DUMMY)
            {
                f_init.Oprev(ref f_prev);

                if (f_prev.tri.id != Mesh.DUMMY)
                {
                    f_init.Copy(ref f_next);
                    // Move one triangle clockwise
                    f_init.Oprev();
                    f_init.Copy(ref f);
                }
            }

            // Go counterclockwise until we reach the border or the initial triangle.
            while (f_next.tri.id != Mesh.DUMMY)
            {
                // Add circumcenter of current triangle
                vpoints.Add(Points[f.tri.id]);

                region.AddNeighbor(f.tri.id, regions[f.Apex().id]);

                if (f_next.Equals(f_init))
                {
                    // Voronoi cell is complete (bounded case).
                    region.Add(vpoints);
                    return;
                }

                f_next.Copy(ref f);
                f_next.Onext();
            }

            // Voronoi cell is unbounded
            region.Bounded = false;

            Vertex torg, tdest, tapex;
            Point intersection;
            int sid, n = mesh.triangles.Count;

            // Find the boundary segment id (we use this id to number the endpoints of infinit rays).
            f.Lprev(ref f_next);
            f_next.Pivot(ref sub);
            sid = sub.seg.hash;

            // Last valid f lies at the boundary. Add the circumcenter.
            vpoints.Add(Points[f.tri.id]);
            region.AddNeighbor(f.tri.id, regions[f.Apex().id]);

            // Check if the intersection with the bounding box has already been computed.
            if (!rayPoints.TryGetValue(sid, out intersection))
            {
                torg = f.Org();
                tapex = f.Apex();
                intersection = IntersectionHelper.BoxRayIntersection(bounds, Points[f.tri.id], torg.y - tapex.y, tapex.x - torg.x);

                // Set the correct id for the vertex
                intersection.id = n + rayIndex;

                Points[n + rayIndex] = intersection;
                rayIndex++;

                rayPoints.Add(sid, intersection);
            }

            vpoints.Add(intersection);

            // Now walk from f_init clockwise till we reach the boundary.
            vpoints.Reverse();

            f_init.Copy(ref f);
            f.Oprev(ref f_prev);

            while (f_prev.tri.id != Mesh.DUMMY)
            {
                vpoints.Add(Points[f_prev.tri.id]);
                region.AddNeighbor(f_prev.tri.id, regions[f_prev.Apex().id]);

                f_prev.Copy(ref f);
                f_prev.Oprev();
            }

            // Find the boundary segment id.
            f.Pivot(ref sub);
            sid = sub.seg.hash;

            if (!rayPoints.TryGetValue(sid, out intersection))
            {
                // Intersection has not been computed yet.
                torg = f.Org();
                tdest = f.Dest();

                intersection = IntersectionHelper.BoxRayIntersection(bounds, Points[f.tri.id], tdest.y - torg.y, torg.x - tdest.x);

                // Set the correct id for the vertex
                intersection.id = n + rayIndex;

                rayPoints.Add(sid, intersection);

                Points[n + rayIndex] = intersection;
                rayIndex++;
            }

            vpoints.Add(intersection);
            region.AddNeighbor(intersection.id, regions[f.Dest().id]);

            // Add the new points to the region (in counter-clockwise order)
            vpoints.Reverse();
            region.Add(vpoints);
        }

        // TODO: Voronoi enumerate edges

        private IEnumerable<IEdge> EnumerateEdges()
        {
            // Copy edges
            Point first, last;
            var edges = new List<IEdge>(Regions.Count * 2);
            foreach (var region in Regions)
            {
                using var ve = region.Vertices.GetEnumerator();

                ve.MoveNext();

                first = last = ve.Current;

                while (ve.MoveNext())
                {
                    if (region.ID < region.GetNeighbor(last).ID)
                    {
                        edges.Add(new Edge(last.id, ve.Current.id));
                    }

                    last = ve.Current;
                }

                if (region.Bounded && region.ID < region.GetNeighbor(last).ID)
                {
                    edges.Add(new Edge(last.id, first.id));
                }
            }

            return edges;
        }
    }
}
