// -----------------------------------------------------------------------
// <copyright file="MeshRenderer.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.Rendering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Drawing;
    using TriangleNet;

    /// <summary>
    /// Renders a mesh.
    /// </summary>
    public class MeshRenderer
    {
        Zoom zoom;
        RenderData data;
        RenderColors renderColors;

        /// <summary>
        /// Initializes a new instance of the <see cref="MeshRenderer" /> class.
        /// </summary>
        public MeshRenderer(RenderData data)
        {
            this.data = data;
        }

        /// <summary>
        /// Renders the mesh.
        /// </summary>
        public void Render(Graphics g, Zoom zoom, RenderColors renderColors)
        {
            this.renderColors = renderColors;
            this.zoom = zoom;

            if (data.Edges != null)
            {
                this.RenderEdges(g);
            }
            else if (data.Triangles != null)
            {
                this.RenderTriangles(g);
            }

            if (data.Segments != null)
            {
                this.RenderSegments(g);
            }

            if (data.Points != null)
            {
                this.RenderPoints(g);
            }
        }

        /// <summary>
        /// Renders only the mesh edges (no points or segments).
        /// </summary>
        public void RenderMesh(Graphics g, Zoom zoom, RenderColors renderColors)
        {
            this.renderColors = renderColors;
            this.zoom = zoom;

            if (data.Edges != null)
            {
                this.RenderEdges(g);
            }
            else if (data.Triangles != null)
            {
                this.RenderTriangles(g);
            }
        }

        /// <summary>
        /// Renders only points and segments (no mesh triangles).
        /// </summary>
        public void RenderGeometry(Graphics g, Zoom zoom, RenderColors renderColors)
        {
            this.renderColors = renderColors;
            this.zoom = zoom;

            if (data.Segments != null)
            {
                this.RenderSegments(g);
            }

            if (data.Points != null)
            {
                this.RenderPoints(g);
            }
        }

        private void RenderPoints(Graphics g)
        {
            PointF pt;
            PointF[] pts = data.Points;
            int i, n;

            // Draw input points
            n = data.NumberOfInputPoints;
            for (i = 0; i < n; i++)
            {
                if (zoom.ViewportContains(pts[i]))
                {
                    pt = zoom.WorldToScreen(pts[i]);
                    g.FillEllipse(renderColors.Point, pt.X - 1.5f, pt.Y - 1.5f, 3, 3);
                }
            }

            // Draw Steiner points
            n = pts.Length;
            for (; i < n; i++)
            {
                if (zoom.ViewportContains(pts[i]))
                {
                    pt = zoom.WorldToScreen(pts[i]);
                    g.FillEllipse(renderColors.SteinerPoint, pt.X - 1.5f, pt.Y - 1.5f, 3, 3);
                }
            }
        }

        private void RenderTriangles(Graphics g)
        {
            PointF p0, p1, p2;
            PointF[] pts = data.Points;

            var triangles = data.Triangles;

            // Draw triangles
            foreach (var tri in triangles)
            {
                if (zoom.ViewportContains(pts[tri.P0]) ||
                    zoom.ViewportContains(pts[tri.P1]) ||
                    zoom.ViewportContains(pts[tri.P2]))
                {
                    p0 = zoom.WorldToScreen(pts[tri.P0]);
                    p1 = zoom.WorldToScreen(pts[tri.P1]);
                    p2 = zoom.WorldToScreen(pts[tri.P2]);

                    g.DrawLine(renderColors.Line, p0, p1);
                    g.DrawLine(renderColors.Line, p1, p2);
                    g.DrawLine(renderColors.Line, p2, p0);
                }
            }
        }

        private void RenderEdges(Graphics g)
        {
            PointF p0, p1;
            PointF[] pts = data.Points;

            var edges = data.Edges;

            // Draw edges
            foreach (var edge in edges)
            {
                if (zoom.ViewportContains(pts[edge.P0]) ||
                    zoom.ViewportContains(pts[edge.P1]))
                {
                    p0 = zoom.WorldToScreen(pts[edge.P0]);
                    p1 = zoom.WorldToScreen(pts[edge.P1]);

                    g.DrawLine(renderColors.Line, p0, p1);
                }
            }
        }

        private void RenderSegments(Graphics g)
        {
            PointF p0, p1;
            PointF[] pts = data.Points;

            var segments = data.Segments;

            foreach (var seg in segments)
            {
                if (zoom.ViewportContains(pts[seg.P0]) ||
                    zoom.ViewportContains(pts[seg.P1]))
                {
                    p0 = zoom.WorldToScreen(pts[seg.P0]);
                    p1 = zoom.WorldToScreen(pts[seg.P1]);

                    g.DrawLine(renderColors.Segment, p0, p1);
                }
            }
        }
    }
}
