// -----------------------------------------------------------------------
// <copyright file="MeshRenderer.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Rendering.GDI
{
    using System.Drawing;
    using TriangleNet.Rendering.GDI.Native;

    /// <summary>
    /// Renders a mesh.
    /// </summary>
    public class MeshRenderer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MeshRenderer" /> class.
        /// </summary>
        public MeshRenderer()
        {
        }

        public Graphics RenderTarget { get; set; }

        public IRenderContext Context { get; set; }

        public void RenderPoints(float[] points, int size, int limit = 0)
        {
            int n = points.Length / size;
            int m = limit > 0 ? limit : n;

            // Draw unchanged points
            RenderPoints(points, size, 0, m, Context.ColorManager.Point);

            // Draw new (Steiner) points
            if (limit > 0)
            {
                RenderPoints(points, size, m, n, Context.ColorManager.SteinerPoint);
            }
        }

        public void RenderPoints(float[] points, int size, int start, int end, Brush brush)
        {
            var g = this.RenderTarget;
            var zoom = this.Context.Zoom;

            int i, k, n = points.Length / size;
            PointF p = new PointF();

            // Render points
            for (i = start; i < end; i++)
            {
                k = size * i;

                p.X = points[k];
                p.Y = points[k + 1];

                if (zoom.Viewport.Contains(p))
                {
                    zoom.WorldToScreen(ref p);
                    g.FillEllipse(brush, p.X - 1.5f, p.Y - 1.5f, 3, 3);
                }
            }
        }

        public void RenderSegments(float[] points, int[] indices, Pen pen)
        {
            RenderLines(points, indices, pen);
        }

        public void RenderEdges(float[] points, int[] indices, Pen pen)
        {
            RenderLines(points, indices, pen);
        }

        public void RenderElements(float[] points, int[] indices, int size, int[] partition)
        {
            var g = this.RenderTarget;
            var zoom = this.Context.Zoom;

            int n = indices.Length / size;
            int k0, k1, k2;

            var tri = new PointF[size];

            bool filled = partition != null;

            var brushes = filled ? Context.ColorManager.GetBrushDictionary() : null;

            // TODO: remove hardcoded color
            var pen = new Pen(Color.FromArgb(20, 20, 20));

            // Draw triangles
            for (int i = 0; i < n; i++)
            {
                k0 = 2 * indices[3 * i];
                k1 = 2 * indices[3 * i + 1];
                k2 = 2 * indices[3 * i + 2];

                tri[0].X = points[k0];
                tri[0].Y = points[k0 + 1];

                tri[1].X = points[k1];
                tri[1].Y = points[k1 + 1];

                tri[2].X = points[k2];
                tri[2].Y = points[k2 + 1];

                if (zoom.Viewport.Intersects(tri[0], tri[1], tri[2]))
                {
                    zoom.WorldToScreen(ref tri[0]);
                    zoom.WorldToScreen(ref tri[1]);
                    zoom.WorldToScreen(ref tri[2]);

                    if (filled)
                    {
                        var b = brushes[partition[i]];

                        if (b.Color.A > 0)
                        {
                            g.FillPolygon(b, tri);
                        }
                    }
                    else
                    {
                        g.DrawPolygon(pen, tri);
                    }
                }
            }

            pen.Dispose();

            if (filled)
            {
                Context.ColorManager.Dispose(brushes);
            }
        }

        public void RenderLines(float[] points, int[] indices, Pen pen)
        {
            var g = this.RenderTarget;
            var zoom = this.Context.Zoom;

            int n = indices.Length / 2;
            int k0, k1;

            PointF p0 = new PointF();
            PointF p1 = new PointF();

            // Draw edges
            for (int i = 0; i < n; i++)
            {
                k0 = 2 * indices[2 * i];
                k1 = 2 * indices[2 * i + 1];

                p0.X = points[k0];
                p0.Y = points[k0 + 1];

                p1.X = points[k1];
                p1.Y = points[k1 + 1];

                if (zoom.Viewport.Intersects(p0, p1))
                {
                    zoom.WorldToScreen(ref p0);
                    zoom.WorldToScreen(ref p1);

                    g.DrawLine(pen, p0, p1);
                }
            }
        }
    }
}
