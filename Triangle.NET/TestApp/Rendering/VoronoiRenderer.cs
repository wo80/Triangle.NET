// -----------------------------------------------------------------------
// <copyright file="VoronoiRenderer.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
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
        RenderColors renderColors;

        /// <summary>
        /// Initializes a new instance of the <see cref="VoronoiRenderer" /> class.
        /// </summary>
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

        /// <summary>
        /// Regenerates the voronoi diagram.
        /// </summary>
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

        /// <summary>
        /// Resets the voronoi display.
        /// </summary>
        public void Reset()
        {
            simpleVoro = null;
            boundedVoro = null;
        }

        /// <summary>
        /// Renders the voronoi diagram.
        /// </summary>
        public void Render(Graphics g, Zoom zoom, RenderColors renderColors)
        {
            this.renderColors = renderColors;

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

            TriangleNet.Geometry.Point[] points = simpleVoro.Points;

            // Draw edges
            int n = simpleVoro.Edges == null ? 0 : simpleVoro.Edges.Length;

            for (int i = 0; i < n; i++)
            {
                var seg = simpleVoro.Edges[i];

                if (seg.P1 == -1)
                {
                    // Infinite voronoi edge
                    p0 = new PointF((float)points[seg.P0].X, (float)points[seg.P0].Y);

                    if (zoom.ViewportContains(p0) &&
                        BoxRayIntersection(points[seg.P0], simpleVoro.Directions[i], out p1))
                    {
                        RenderEdge(g, zoom, p0, p1);
                    }
                }
                else
                {
                    p0 = new PointF((float)points[seg.P0].X, (float)points[seg.P0].Y);
                    p1 = new PointF((float)points[seg.P1].X, (float)points[seg.P1].Y);

                    RenderEdge(g, zoom, p0, p1);
                }
            }

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

                g.DrawLine(renderColors.VoronoiLine, p0, p1);
            }
        }

        private bool BoxRayIntersection(TriangleNet.Geometry.Point pt,
            TriangleNet.Geometry.Point direction, out PointF intersect)
        {
            double x = pt.X;
            double y = pt.Y;
            double dx = direction.X;
            double dy = direction.Y;

            double t1, x1, y1, t2, x2, y2;

            // Bounding box (50% enlarged)
            var box = mesh.Bounds;

            double dw = box.Width * 0.5f;
            double dh = box.Height * 0.5f;

            double minX = box.Xmin - dw;
            double maxX = box.Xmax + dw;
            double minY = box.Ymin - dh;
            double maxY = box.Ymax + dh;

            intersect = new PointF();

            // Check if point is inside the bounds
            if (x < minX || x > maxX || y < minY || y > maxY)
            {
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
            else if (dx > 0)
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
    }
}
