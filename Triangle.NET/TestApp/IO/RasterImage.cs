// -----------------------------------------------------------------------
// <copyright file="RasterImage.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.IO
{
    using MeshExplorer.Rendering;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using TriangleNet;
    using TriangleNet.Data;
    using TriangleNet.IO;
    using TriangleNet.Tools;

    /// <summary>
    /// Writes an image of the mesh to disk.
    /// </summary>
    public class RasterImage
    {
        RenderColors colors = RenderColors.Default();

        public RenderColors ColorScheme
        {
            get { return colors; }
            set { colors = value; }
        }

        /// <summary>
        /// Export the mesh to PNG format.
        /// </summary>
        /// <param name="mesh">The current mesh.</param>
        /// <param name="filename">The PNG filename.</param>
        /// <param name="width">The desired width (pixel) of the image.</param>
        public void Export(Mesh mesh, string filename, int width)
        {
            // Get mesh data -- TODO: Use RenderControl's RenderData
            RenderData data = new RenderData();
            data.SetData(mesh);

            // Check file name
            if (String.IsNullOrWhiteSpace(filename))
            {
                filename = String.Format("mesh-{0}.png", DateTime.Now.ToString("yyyy-M-d-hh-mm-ss"));
            }

            Bitmap bitmap;

            // Check if the specified width is reasonable
            if (width < 2 * Math.Sqrt(mesh.NumberOfVertices))
            {
                bitmap = new Bitmap(400, 200);
                Graphics g = Graphics.FromImage(bitmap);
                g.Clear(colors.Background);

                string message = String.Format("Sorry, I won't render {0} points on such a small image!", mesh.NumberOfVertices);

                SizeF sz = g.MeasureString(message, SystemFonts.DefaultFont);

                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawString(message, SystemFonts.DefaultFont, colors.Point,
                    200 - sz.Width / 2, 100 - sz.Height / 2);

                g.Dispose();
            }
            else
            {
                var bounds = data.Bounds;
                // World margin on each side
                float margin = (float)bounds.Height * 0.05f;
                float scale = width / ((float)bounds.Width + 2 * margin);

                var target = new Rectangle(0, 0, width, (int)((bounds.Height + 2 * margin) * scale));

                bitmap = new Bitmap(width, target.Height, PixelFormat.Format32bppPArgb);

                Zoom zoom = new Zoom();
                zoom.Initialize(target, bounds);

                Graphics g = Graphics.FromImage(bitmap);
                g.Clear(colors.Background);

                g.SmoothingMode = SmoothingMode.HighQuality;

                MeshRenderer meshRenderer = new MeshRenderer(data);
                meshRenderer.Render(g, zoom, colors);

                g.Dispose();
            }

            if (Path.GetExtension(filename) != ".png")
            {
                filename += ".png";
            }

            bitmap.Save(filename, ImageFormat.Png);
        }
    }
}
