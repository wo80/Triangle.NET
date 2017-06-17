// -----------------------------------------------------------------------
// <copyright file="EpsImage.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// Original Matlab code by John Burkardt, Florida State University
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Rendering.Text
{
    using System;
    using System.IO;
    using TriangleNet;
    using TriangleNet.Geometry;

    using Color = System.Drawing.Color;
    using IntPoint = System.Drawing.Point;
    using IntRectangle = System.Drawing.Rectangle;

    /// <summary>
    /// Writes a mesh to an EPS file.
    /// </summary>
    public class EpsImage
    {
        // EPS page metrics
        PageSize ps = new PageSize(36, 126, 576, 666);
        PageSize clip = new PageSize(18, 108, 594, 684);

        // Mesh metrics
        double x_max, x_min;
        double y_max, y_min;

        // TODO: use color manager
        private static Color ColorPoints = Color.FromArgb(0, 100, 0);
        private static Color ColorLines = Color.FromArgb(150, 150, 150);
        private static Color ColorSegments = Color.FromArgb(70, 130, 180);
        private static Color ColorBorder = Color.FromArgb(230, 230, 230);

        /// <summary>
        /// Export the mesh to EPS format.
        /// </summary>
        /// <param name="mesh">The current mesh.</param>
        /// <param name="filename">The EPS filename.</param>
        /// <param name="width">The desired width of the image (currently ignored).</param>
        public void Export(Mesh mesh, string filename, int width)
        {
            // Check file name
            if (String.IsNullOrWhiteSpace(filename))
            {
                filename = String.Format("mesh-{0}.eps", DateTime.Now.ToString("yyyy-M-d-hh-mm-ss"));
            }

            if (!filename.EndsWith(".eps"))
            {
                filename = Path.ChangeExtension(filename, ".eps");
            }

            UpdateMetrics(mesh.Bounds);

            using (var eps = new EpsDocument(filename, ps))
            {
                int n = mesh.Vertices.Count;

                // Size of the points.
                eps.DefaultPointSize = (n < 100) ? 3 : ((n < 500) ? 2 : 1);

                eps.WriteHeader();

                // Draw a gray border around the page.
                eps.SetColor(ColorBorder);
                eps.DrawRectangle(GetRectangle(ps));

                // Define a clipping polygon.
                eps.SetClip(GetRectangle(clip));

                // Draw edges.
                eps.AddComment("Draw edges.");
                eps.SetStroke(0.4f, ColorLines);

                foreach (var e in EdgeIterator.EnumerateEdges(mesh))
                {
                    eps.DrawLine(Transform(e.GetVertex(0)), Transform(e.GetVertex(1)));
                }

                // Draw Segments.
                eps.AddComment("Draw Segments.");
                eps.SetStroke(0.8f, ColorSegments);

                foreach (var s in mesh.Segments)
                {
                    eps.DrawLine(Transform(s.GetVertex(0)), Transform(s.GetVertex(1)));
                }

                // Draw points.
                eps.AddComment("Draw points.");
                eps.SetColor(ColorPoints);

                foreach (var node in mesh.Vertices)
                {
                    eps.DrawPoint(Transform(node));
                }
            }
        }

        private IntRectangle GetRectangle(PageSize size)
        {
            return new IntRectangle((int)size.X, (int)size.Y, (int)size.Width, (int)size.Height);
        }

        private IntPoint Transform(Point p)
        {
            return Transform(p.X, p.Y);
        }

        private IntPoint Transform(double x, double y)
        {
            return new IntPoint(
                (int)Math.Floor(((x_max - x) * ps.X + (x - x_min) * ps.Right) / (x_max - x_min)),
                (int)Math.Floor(((y_max - y) * ps.Y + (y - y_min) * ps.Bottom) / (y_max - y_min))
            );
        }

        private void UpdateMetrics(Rectangle bounds)
        {
            x_max = bounds.Right;
            x_min = bounds.Left;
            y_max = bounds.Top;
            y_min = bounds.Bottom;

            // Enlarge width 5% on each side
            double x_scale = x_max - x_min;
            x_max = x_max + 0.05 * x_scale;
            x_min = x_min - 0.05 * x_scale;
            x_scale = x_max - x_min;

            // Enlarge height 5% on each side
            double y_scale = y_max - y_min;
            y_max = y_max + 0.05 * y_scale;
            y_min = y_min - 0.05 * y_scale;
            y_scale = y_max - y_min;

            if (x_scale < y_scale)
            {
                int delta = (int)Math.Round((ps.Right - ps.X) * (y_scale - x_scale) / (2.0 * y_scale));

                ps.Expand(-delta, 0);
                clip.Expand(-delta, 0);
            }
            else
            {
                int delta = (int)Math.Round((ps.Bottom - ps.Y) * (x_scale - y_scale) / (2.0 * x_scale));

                ps.Expand(0, -delta);
                clip.Expand(0, -delta);
            }
        }
    }
}
