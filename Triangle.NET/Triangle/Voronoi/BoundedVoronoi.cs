// -----------------------------------------------------------------------
// <copyright file="BoundedVoronoi.cs">
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Voronoi
{
    using System;
    using System.Collections.Generic;
    using TriangleNet.Geometry;

    public class BoundedVoronoi : VoronoiBase
    {
        int offset;

        public BoundedVoronoi(Mesh mesh)
            : base(mesh, true)
        {
            // We explicitly told the base constructor to call the Generate method, so
            // at this point the basic Voronoi diagram is already created.
            offset = base.vertices.Length;

            // Each vertex of the hull will be part of a Voronoi cell.
            Array.Resize(ref base.vertices, offset + mesh.hullsize);

            // Create bounded Voronoi diagram.
            PostProcess();

            Array.Resize(ref base.vertices, offset);
        }

        private void PostProcess()
        {
            // Compute edge intersections with mesh boundary edges.
            ProcessBoundaryEdges();
        }

        private void ProcessBoundaryEdges()
        {
            var infEdges = new List<DCEL.HalfEdge>();

            // TODO: save the half-infinite boundary edge in base class
            // so we don't have to process the complete list here.
            foreach (var edge in base.edges)
            {
                if (edge.next == null)
                {
                    infEdges.Add(edge);
                }
            }

            foreach (var edge in infEdges)
            {
                var v1 = (Vertex)edge.face.generator;
                var v2 = (Vertex)edge.twin.face.generator;

                double dir = RobustPredicates.CounterClockwise(v1, v2, edge.origin);

                if (dir <= 0)
                {
                    HandleCase1(edge, v1, v2);
                }
                else
                {
                    HandleCase2(edge, v1, v2);
                }
            }
        }

        private void HandleCase1(DCEL.HalfEdge edge, Vertex v1, Vertex v2)
        {
            //int mark = GetBoundaryMark(v1);

            // The infinite vertex.
            var v = (Point)edge.twin.origin;

            // The half-edge is the bisector of v1 and v2, so the projection onto the
            // boundary segment is actually its midpoint.
            v.x = (v1.x + v2.x) / 2.0;
            v.y = (v1.y + v2.y) / 2.0;

            // Close the cell connected to edge.
            var gen = new DCEL.Vertex(v1.x, v1.y);

            var h1 = new DCEL.HalfEdge(edge.twin.origin, edge.face);
            var h2 = new DCEL.HalfEdge(gen, edge.face);

            h1.next = h2;
            h2.next = edge.face.edge;

            // Let the face edge point to the edge leaving at generator.
            edge.face.edge = h2;

            base.edges.Add(h1);
            base.edges.Add(h2);

            int count = base.edges.Count;

            h1.id = count;
            h2.id = count + 1;

            base.vertices[offset] = gen;
            gen.id = offset++;
        }

        private void HandleCase2(DCEL.HalfEdge e1, Vertex v1, Vertex v2)
        {
            var e2 = e1.twin.next;
            var ei = e2.twin.next;

            // The vertices of the infinite edge.
            var p1 = (Point)e1.origin;
            var pi = (Point)e1.twin.origin;

            // Find the two intersections with boundary edge.
            IntersectSegments(v1, v2, e2.origin, e2.twin.origin, ref pi);
            IntersectSegments(v1, v2, ei.origin, ei.twin.origin, ref p1);

            // The infinite edge will now lie on the boundary. Update pointers:
            e2.twin.next = e1.twin;
            e1.twin.next = ei;
            e1.twin.face = ei.face;

            e2.origin = e1.twin.origin;

            e1.twin.twin = null;
            e1.twin = null;

            // Close the cell.
            var gen = new DCEL.Vertex(v1.x, v1.y);
            var he = new DCEL.HalfEdge(gen, e1.face);

            e1.next = he;
            he.next = e1.face.edge;

            // Let the face edge point to the edge leaving at generator.
            e1.face.edge = he;

            base.edges.Add(he);

            he.id = base.edges.Count;

            base.vertices[offset] = gen;
            gen.id = offset++;
        }

        /*
        private int GetBoundaryMark(Vertex v)
        {
            Otri tri = default(Otri);
            Otri next = default(Otri);
            Osub seg = default(Osub);

            // Get triangle connected to generator.
            v.tri.Copy(ref tri);
            v.tri.Copy(ref next);

            // Find boundary triangle.
            while (next.triangle.id != -1)
            {
                next.Copy(ref tri);
                next.OnextSelf();
            }

            // Find edge dual to current half-edge.
            tri.LnextSelf();
            tri.LnextSelf();

            tri.SegPivot(ref seg);

            return seg.seg.boundary;
        }
        //*/

        /// <summary>
        /// Compute intersection of two segments.
        /// </summary>
        /// <param name="p0">Segment 1 start point.</param>
        /// <param name="p1">Segment 1 end point.</param>
        /// <param name="q0">Segment 2 start point.</param>
        /// <param name="q1">Segment 2 end point.</param>
        /// <param name="i0">The intersection point.</param>
        /// <remarks>
        /// This is a special case of segment intersection. Since the calling algorithm assures
        /// that a valid intersection exists, there's no need to check for any special cases.
        /// </remarks>
        private static void IntersectSegments(Point p0, Point p1, Point q0, Point q1, ref Point i0)
        {
            double ux = p1.x - p0.x;
            double uy = p1.y - p0.y;
            double vx = q1.x - q0.x;
            double vy = q1.y - q0.y;
            double wx = p0.x - q0.x;
            double wy = p0.y - q0.y;

            double d = (ux * vy - uy * vx);
            double s = (vx * wy - vy * wx) / d;

            // Intersection point
            i0.x = p0.X + s * ux;
            i0.y = p0.Y + s * uy;
        }

        protected override IEnumerable<IEdge> EnumerateEdges()
        {
            var edges = new List<IEdge>(this.edges.Count / 2);

            foreach (var edge in this.edges)
            {
                var twin = edge.twin;

                // Report edge only once.
                if (twin == null)
                {
                    edges.Add(new Edge(edge.origin.id, edge.next.origin.id));
                }
                else if (edge.id < twin.id)
                {
                    edges.Add(new Edge(edge.origin.id, twin.origin.id));
                }
            }

            return edges;
        }
    }
}
