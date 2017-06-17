// -----------------------------------------------------------------------
// <copyright file="VoronoiRenderer.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace MeshRenderer.Core.GDI
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
        RenderData data;
        ColorManager renderColors;

        /// <summary>
        /// Initializes a new instance of the <see cref="VoronoiRenderer" /> class.
        /// </summary>
        public VoronoiRenderer(RenderData data)
        {
            this.data = data;
        }

        /// <summary>
        /// Renders the voronoi diagram.
        /// </summary>
        public void Render(Graphics g, Zoom zoom, ColorManager renderColors)
        {
            this.renderColors = renderColors;

            var points = data.VoronoiPoints;
            var edges = data.VoronoiEdges;

            if (points != null && edges != null)
            {
                uint k;
                PointF p0, p1;
                int n = edges.Length / 2;

                for (int i = 0; i < n; i++)
                {
                    // First endpoint of voronoi edge
                    k = edges[2 * i];
                    p0 = new PointF(points[2 * k], points[2 * k + 1]);

                    // Second endpoint of voronoi edge
                    k = edges[2 * i + 1];
                    p1 = new PointF(points[2 * k], points[2 * k + 1]);

                    // Render the edge
                    if (zoom.ViewportContains(p0.X, p0.Y) ||
                        zoom.ViewportContains(p1.X, p1.Y))
                    {
                        p0 = zoom.WorldToScreen(p0.X, p0.Y);
                        p1 = zoom.WorldToScreen(p1.X, p1.Y);

                        g.DrawLine(renderColors.VoronoiLine, p0, p1);
                    }
                }
            }
        }
    }
}
