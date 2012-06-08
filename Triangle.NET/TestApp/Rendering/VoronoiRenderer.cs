// -----------------------------------------------------------------------
// <copyright file="VoronoiRenderer.cs" company="">
// Christian Woltering, http://home.edo.tu-dortmund.de/~woltering/triangle/
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.Rendering
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using TriangleNet;
    using TriangleNet.Tools;

    /// <summary>
    /// Renders a (bounded) Voronoi diagram.
    /// </summary>
    public class VoronoiRenderer
    {
        Mesh mesh;
        Voronoi simpleVoro;
        BoundedVoronoi boundedVoro;

        Pen lines = new Pen(Color.FromArgb(40, 50, 60));

        public VoronoiRenderer(Mesh mesh)
        {
            this.mesh = mesh;

            //if (mesh.NumberOfSegments > 0)
            if (mesh.IsPolygon)
            {
                boundedVoro = new BoundedVoronoi(mesh);
            }
            else
            {
                simpleVoro = new Voronoi(mesh);
            }
        }

        public void Update()
        {
            if (simpleVoro != null)
            {
                simpleVoro.Generate();
            }

            if (boundedVoro != null)
            {
                boundedVoro.Generate();
            }
        }

        public void Reset()
        {
            simpleVoro = null;
            boundedVoro = null;
        }

        internal void Render(Graphics g, Zoom zoom)
        {
            if (simpleVoro != null)
            {
                RenderSimple(g, zoom);
                return;
            }

            if (boundedVoro != null)
            {
                RenderBounded(g, zoom);
                return;
            }
        }

        private void RenderSimple(Graphics g, Zoom zoom)
        {
            PointF p0, p1;

            BBox bounds = new BBox(mesh.Bounds);

            TriangleNet.Geometry.Point[] points = simpleVoro.Points;

            // Enlarge 50%
            bounds.Extend(0.5f);

            // Draw edges
            int n = simpleVoro.Edges == null ? 0 : simpleVoro.Edges.Length;

            for (int i = 0; i < n; i++)
            {
                var seg = simpleVoro.Edges[i];

                if (seg.P1 == -1)
                {
                    // Infinite voronoi edge
                    p0 = new PointF((float)points[seg.P0].X, (float)points[seg.P0].Y);

                    if (!BoxRayIntersection(bounds, points[seg.P0], simpleVoro.Directions[i], out p1))
                    {
                        continue;
                    }
                }
                else
                {
                    p0 = new PointF((float)points[seg.P0].X, (float)points[seg.P0].Y);
                    p1 = new PointF((float)points[seg.P1].X, (float)points[seg.P1].Y);
                }

                // Draw line
                RenderEdge(g, zoom, p0, p1);
            }

            // Shrink 50%
            bounds.Extend(-0.5f);

            // Scale the points radius to 2 pixel.
            //float radius = 1.5f / scale, x, y;

            // Draw points
            //n = voro.Points.Length;

            //for (int i = 0; i < n; i++)
            //{
            //    x = (float)voro.Points[i].X;
            //    y = (float)voro.Points[i].Y;

            //    g.FillEllipse(Brushes.Blue, x - radius, y - radius, 2 * radius, 2 * radius);
            //}
        }

        private void RenderBounded(Graphics g, Zoom zoom)
        {
            PointF p0, p1;
            int n;

            foreach (var cell in boundedVoro.Cells)
            {
                n = cell.Length;

                for (int i = 1; i < n; i++)
                {
                    p0 = new PointF((float)cell[i - 1].X, (float)cell[i - 1].Y);
                    p1 = new PointF((float)cell[i].X, (float)cell[i].Y);

                    RenderEdge(g, zoom, p0, p1);
                }
            }
        }

        private void RenderEdge(Graphics g, Zoom zoom, PointF p0, PointF p1)
        {
            if (zoom.ViewportContains(p0) ||
                zoom.ViewportContains(p1))
            {
                p0 = zoom.WorldToScreen(p0);
                p1 = zoom.WorldToScreen(p1);

                g.DrawLine(lines, p0, p1);
            }
        }

        private bool BoxRayIntersection(BBox bounds, TriangleNet.Geometry.Point pt,
            TriangleNet.Geometry.Point direction, out PointF intersect)
        {
            double x = pt.X;
            double y = pt.Y;
            double dx = direction.X;
            double dy = direction.Y;

            double t1, x1, y1, t2, x2, y2;

            intersect = new PointF();

            // Check if point is inside the bounds
            if (x < bounds.MinX || x > bounds.MaxX || y < bounds.MinY || y > bounds.MaxY)
            {
                return false;
            }

            // Calculate the cut through the vertical boundaries
            if (dx < 0)
            {
                // Line going to the left: intersect with x = bounds.MinX
                t1 = (bounds.MinX - x) / dx;
                x1 = bounds.MinX;
                y1 = y + t1 * dy;
            }
            else if (dx > 0)
            {
                // Line going to the right: intersect with x = bounds.MaxX
                t1 = (bounds.MaxX - x) / dx;
                x1 = bounds.MaxX;
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
                // Line going downwards: intersect with y = bounds.MinY
                t2 = (bounds.MinY - y) / dy;
                x2 = x + t2 * dx;
                y2 = bounds.MinY;
            }
            else if (dx > 0)
            {
                // Line going upwards: intersect with y = bounds.MaxY
                t2 = (bounds.MaxY - y) / dy;
                x2 = x + t2 * dx;
                y2 = bounds.MaxY;
            }
            else
            {
                // Horizontal line: no intersection possible
                t2 = double.MaxValue;
                x2 = y2 = 0;
            }

            if (t1 < t2)
            {
                intersect.X = (float)x1;
                intersect.Y = (float)y1;
            }
            else
            {
                intersect.X = (float)x2;
                intersect.Y = (float)y2;
            }

            return true;
        }

        /// <summary>
        /// Bounding box.
        /// </summary>
        struct BBox
        {
            public float MinX;
            public float MaxX;
            public float MinY;
            public float MaxY;

            public float Width { get { return MaxX - MinX; } }
            public float Height { get { return MaxY - MinY; } }

            public BBox(TriangleNet.Geometry.BoundingBox box)
            {
                MinX = (float)box.Xmin;
                MaxX = (float)box.Xmax;
                MinY = (float)box.Ymin;
                MaxY = (float)box.Ymax;
            }

            public void Extend(float amount)
            {
                float dx = amount * this.Width;
                float dy = amount * this.Height;

                MinX -= dx;
                MaxX += dx;
                MinY -= dy;
                MaxY += dy;
            }
        }
    }
}
