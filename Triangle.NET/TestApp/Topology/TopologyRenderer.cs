
namespace MeshExplorer.Topology
{
    using System;
    using System.Drawing;
    using TriangleNet;
    using TriangleNet.Geometry;
    using TriangleNet.Rendering;
    
    public class TopologyRenderer
    {
        Projection zoom;
        Mesh mesh;
        PointF[] points;

        // Colors
        Color Background = Color.FromArgb(0, 0, 0);
        Brush Point = new SolidBrush(Color.FromArgb(0, 80, 0));
        Brush Triangle = new SolidBrush(Color.FromArgb(50, 50, 50));
        Pen Line = new Pen(Color.FromArgb(30, 30, 30));
        Pen Segment = new Pen(Color.DarkBlue);

        Brush SelectedTriangle = new SolidBrush(Color.FromArgb(50, 0, 0));
        Pen SelectedEdge = new Pen(Color.DarkRed, 2.0f);

        Font font, fontTri;

        ITriangle currentTri;
        Vertex currentOrg, currentDest;

        /// <summary>
        /// Initializes a new instance of the <see cref="MeshRenderer" /> class.
        /// </summary>
        public TopologyRenderer(Mesh mesh)
        {
            this.mesh = mesh;

            points = new PointF[mesh.Vertices.Count];

            int k = 0;

            foreach (var v in mesh.Vertices)
            {
                points[k++] = new PointF((float)v.X, (float)v.Y);
            }

            font = new Font("Arial", 7.5f);
            fontTri = new Font("Arial", 12f, FontStyle.Bold);
        }

        /// <summary>
        /// Renders the mesh.
        /// </summary>
        public void Render(Graphics g, Projection zoom)
        {
            this.zoom = zoom;

            if (mesh.Edges != null)
            {
                this.RenderSelectedTriangle(g);
                this.RenderEdges(g);
                this.RenderTriangleIds(g);
            }
            else if (mesh.Triangles != null)
            {
                this.RenderTriangles(g);
            }

            if (mesh.Segments != null)
            {
                this.RenderSegments(g);
            }

            RenderSelectedEdge(g);

            if (mesh.Vertices != null)
            {
                this.RenderPoints(g);
            }
        }

        public void SelectTriangle(ITriangle tri, Vertex org, Vertex dest)
        {
            currentTri = tri;
            currentOrg = org;
            currentDest = dest;
        }

        #region Helpers

        private PointF GetIncenter(PointF p0, PointF p1, PointF p2)
        {
            double cx, cy, a, b, c,
                dax = p1.X - p0.X,
                dbx = p2.X - p1.X,
                dcx = p0.X - p2.X,
                day = p1.Y - p0.Y,
                dby = p2.Y - p1.Y,
                dcy = p0.Y - p2.Y;

            a = Math.Sqrt(dax * dax + day * day);
            b = Math.Sqrt(dbx * dbx + dby * dby);
            c = Math.Sqrt(dcx * dcx + dcy * dcy);

            cx = (a * p2.X + b * p0.X + c * p1.X) / (a + b + c);
            cy = (a * p2.Y + b * p0.Y + c * p1.Y) / (a + b + c);

            return new PointF((float)cx, (float)cy);
        }

        private PointF GetCentroid(PointF p0, PointF p1, PointF p2)
        {
            double cx, cy;

            cx = (p0.X + p1.X + p2.X) / 3;
            cy = (p0.Y + p1.Y + p2.Y) / 3;

            return new PointF((float)cx, (float)cy);
        }

        private PointF GetPoint(ITriangle tri, int index)
        {
            var v = tri.GetVertex(index);

            return new PointF((float)v.X, (float)v.Y);
        }

        private PointF GetPoint(ISegment seg, int index)
        {
            var v = seg.GetVertex(index);

            return new PointF((float)v.X, (float)v.Y);
        }

        #endregion

        private void RenderPoints(Graphics g)
        {
            int n = points.Length;
            PointF pt;

            int id = currentOrg != null ? currentOrg.ID : -1;

            for (int i = 0; i < n; i++)
            {
                var brush = i == id ? Brushes.DarkRed : Point;

                pt = points[i];
                zoom.WorldToScreen(ref pt);
                g.FillEllipse(brush, pt.X - 10f, pt.Y - 10f, 20, 20);

                pt.X -= i > 9 ? 7 : 4;
                pt.Y -= 6;
                g.DrawString(i.ToString(), font, Brushes.White, pt);
            }
        }

        private void RenderTriangles(Graphics g)
        {
            PointF p0, p1, p2, center;

            var triangles = mesh.Triangles;

            // Draw triangles
            foreach (var tri in triangles)
            {
                p0 = points[tri.GetVertexID(0)];
                p1 = points[tri.GetVertexID(1)];
                p2 = points[tri.GetVertexID(2)];

                zoom.WorldToScreen(ref p0);
                zoom.WorldToScreen(ref p1);
                zoom.WorldToScreen(ref p2);

                g.DrawLine(Line, p0, p1);
                g.DrawLine(Line, p1, p2);
                g.DrawLine(Line, p2, p0);

                center = GetIncenter(p0, p1, p2);
                center.X -= 5;
                center.Y -= 5;

                g.DrawString(tri.ID.ToString(), fontTri, Triangle, center);
            }
        }

        private void RenderTriangleIds(Graphics g)
        {
            PointF p0, p1, p2, center;

            var triangles = mesh.Triangles;

            // Draw triangles
            foreach (var tri in triangles)
            {
                p0 = points[tri.GetVertexID(0)];
                p1 = points[tri.GetVertexID(1)];
                p2 = points[tri.GetVertexID(2)];

                zoom.WorldToScreen(ref p0);
                zoom.WorldToScreen(ref p1);
                zoom.WorldToScreen(ref p2);

                center = GetIncenter(p0, p1, p2);
                center.X -= 5;
                center.Y -= 5;

                g.DrawString(tri.ID.ToString(), fontTri, Triangle, center);
            }
        }

        private void RenderEdges(Graphics g)
        {
            PointF p0, p1;

            var edges = mesh.Edges;

            // Draw edges
            foreach (var edge in edges)
            {
                p0 = points[edge.P0];
                p1 = points[edge.P1];

                zoom.WorldToScreen(ref p0);
                zoom.WorldToScreen(ref p1);

                g.DrawLine(Line, p0, p1);
            }
        }

        private void RenderSegments(Graphics g)
        {
            PointF p0, p1;

            var segments = mesh.Segments;

            foreach (var seg in segments)
            {
                p0 = points[seg.P0];
                p1 = points[seg.P1];

                zoom.WorldToScreen(ref p0);
                zoom.WorldToScreen(ref p1);

                g.DrawLine(Segment, p0, p1);
            }
        }

        private void RenderSelectedEdge(Graphics g)
        {
            if (currentTri != null)
            {
                PointF p0, p1;

                p0 = points[currentOrg.ID];
                p1 = points[currentDest.ID];

                zoom.WorldToScreen(ref p0);
                zoom.WorldToScreen(ref p1);

                g.DrawLine(SelectedEdge, p0, p1);
            }
        }

        private void RenderSelectedTriangle(Graphics g)
        {
            if (currentTri != null)
            {
                var p = new PointF[3];

                p[0] = points[currentTri.GetVertexID(0)];
                p[1] = points[currentTri.GetVertexID(1)];
                p[2] = points[currentTri.GetVertexID(2)];

                zoom.WorldToScreen(ref p[0]);
                zoom.WorldToScreen(ref p[1]);
                zoom.WorldToScreen(ref p[2]);

                g.FillPolygon(SelectedTriangle, p);
            }
        }
    }
}
