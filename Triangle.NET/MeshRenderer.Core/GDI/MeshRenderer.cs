// -----------------------------------------------------------------------
// <copyright file="MeshRenderer.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace MeshRenderer.Core.GDI
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
        ColorManager renderColors;

        // If true, even geometry parts outside of bounds will be rendered.
        bool ignoreBounds;

        /// <summary>
        /// Initializes a new instance of the <see cref="MeshRenderer" /> class.
        /// </summary>
        public MeshRenderer(RenderData data, ColorManager renderColors)
        {
            this.data = data;
            this.renderColors = renderColors;

            int featureCount = data.Points.Length;

            if (data.MeshEdges != null)
            {
                featureCount += data.MeshEdges.Length;
            }
            else if (data.Triangles != null)
            {
                featureCount += 2 * data.Triangles.Length;
            }

            this.ignoreBounds = featureCount < 1000;

            if (data.Triangles != null && data.NumberOfRegions > 0)
            {
                renderColors.MakeRegionMap(data.TrianglePartition, data.NumberOfRegions);
            }
        }

        /// <summary>
        /// Renders the mesh.
        /// </summary>
        public void Render(Graphics g, Zoom zoom)
        {
            Render(g, zoom, false);
        }

        /// <summary>
        /// Renders the mesh.
        /// </summary>
        public void Render(Graphics g, Zoom zoom, bool fillTriangles)
        {
            this.zoom = zoom;

            if (data.MeshEdges != null && !fillTriangles)
            {
                this.RenderEdges(g);
            }

            if (fillTriangles && data.Triangles != null)
            {
                this.RenderTriangles(g, fillTriangles && (data.NumberOfRegions > 0));
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
        public void RenderMesh(Graphics g, Zoom zoom)
        {
            this.zoom = zoom;

            if (data.MeshEdges != null)
            {
                this.RenderEdges(g);
            }
            else if (data.Triangles != null)
            {
                this.RenderTriangles(g, false);
            }
        }

        /// <summary>
        /// Renders only points and segments (no mesh triangles).
        /// </summary>
        public void RenderGeometry(Graphics g, Zoom zoom)
        {
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
            int i, k, n;
            PointF pt;
            float[] pts = data.Points;

            // Draw input points
            n = data.NumberOfInputPoints;
            for (i = 0; i < n; i++)
            {
                k = 2 * i;
                if (ignoreBounds || zoom.ViewportContains(pts[k], pts[k + 1]))
                {
                    pt = zoom.WorldToScreen(pts[k], pts[k + 1]);
                    g.FillEllipse(renderColors.Point, pt.X - 1.5f, pt.Y - 1.5f, 3, 3);
                }
            }

            // Draw Steiner points
            n = pts.Length / 2;
            for (; i < n; i++)
            {
                k = 2 * i;
                if (ignoreBounds || zoom.ViewportContains(pts[k], pts[k + 1]))
                {
                    pt = zoom.WorldToScreen(pts[k], pts[k + 1]);
                    g.FillEllipse(renderColors.SteinerPoint, pt.X - 1.5f, pt.Y - 1.5f, 3, 3);
                }
            }
        }

        private void RenderTriangles(Graphics g, bool fillTriangles)
        {
            int n = data.Triangles.Length / 3;
            uint k0, k1, k2;
            PointF[] tri = new PointF[3];
            float[] pts = data.Points;

            // Draw triangles
            for (int i = 0; i < n; i++)
            {
                k0 = 2 * data.Triangles[3 * i];
                k1 = 2 * data.Triangles[3 * i + 1];
                k2 = 2 * data.Triangles[3 * i + 2];

                if (ignoreBounds ||
                    (zoom.ViewportContains(pts[k0], pts[k0 + 1]) ||
                    zoom.ViewportContains(pts[k1], pts[k1 + 1]) ||
                    zoom.ViewportContains(pts[k2], pts[k2 + 1])))
                {
                    tri[0] = zoom.WorldToScreen(pts[k0], pts[k0 + 1]);
                    tri[1] = zoom.WorldToScreen(pts[k1], pts[k1 + 1]);
                    tri[2] = zoom.WorldToScreen(pts[k2], pts[k2 + 1]);

                    g.DrawPolygon(renderColors.Line, tri);

                    if (fillTriangles)
                    {
                        g.FillPolygon(renderColors.GetRegionBrush(data.TrianglePartition[i]), tri);
                    }
                }
            }
        }

        private void RenderEdges(Graphics g)
        {
            int n = data.MeshEdges.Length / 2;
            uint k0, k1;
            PointF p0, p1;
            float[] pts = data.Points;

            // Draw edges
            for (int i = 0; i < n; i++)
            {
                k0 = 2 * data.MeshEdges[2 * i];
                k1 = 2 * data.MeshEdges[2 * i + 1];

                if (ignoreBounds ||
                    (zoom.ViewportContains(pts[k0], pts[k0 + 1]) ||
                    zoom.ViewportContains(pts[k1], pts[k1 + 1])))
                {
                    p0 = zoom.WorldToScreen(pts[k0], pts[k0 + 1]);
                    p1 = zoom.WorldToScreen(pts[k1], pts[k1 + 1]);

                    g.DrawLine(renderColors.Line, p0, p1);
                }
            }
        }

        private void RenderSegments(Graphics g)
        {
            int n = data.Segments.Length / 2;
            uint k0, k1;
            PointF p0, p1;
            float[] pts = data.Points;

            // Draw edges
            for (int i = 0; i < n; i++)
            {
                k0 = 2 * data.Segments[2 * i];
                k1 = 2 * data.Segments[2 * i + 1];

                if (ignoreBounds ||
                    (zoom.ViewportContains(pts[k0], pts[k0 + 1]) ||
                    zoom.ViewportContains(pts[k1], pts[k1 + 1])))
                {
                    p0 = zoom.WorldToScreen(pts[k0], pts[k0 + 1]);
                    p1 = zoom.WorldToScreen(pts[k1], pts[k1 + 1]);

                    g.DrawLine(renderColors.Segment, p0, p1);
                }
            }
        }
    }
}
