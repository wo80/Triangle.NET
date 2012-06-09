// -----------------------------------------------------------------------
// <copyright file="ImageWriter.cs" company="">
// TODO: Update copyright text.
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
    public class ImageWriter
    {
        RenderColors colors = RenderColors.Default;

        /// <summary>
        /// Sets the color scheme.
        /// </summary>
        /// <param name="background">Background color.</param>
        /// <param name="points">Points color.</param>
        /// <param name="steiner">Steiner points color.</param>
        /// <param name="lines">Line color.</param>
        /// <param name="segments">Segment color.</param>
        public void SetColorScheme(Color background, Color points, Color steiner,
            Color lines, Color segments, Color triangles)
        {
            colors.Background = background;
            colors.Point = new SolidBrush(points);
            colors.SteinerPoint = new SolidBrush(steiner);
            colors.Triangle = new SolidBrush(triangles);
            colors.Line = new Pen(lines);
            colors.Segment = new Pen(segments);
        }

        /// <summary>
        /// Set a color scheme with white background.
        /// </summary>
        public void SetColorSchemeLight()
        {
            colors.Background = Color.White;
            colors.Point = new SolidBrush(Color.MidnightBlue);
            colors.SteinerPoint = new SolidBrush(Color.DarkGreen);
            colors.Triangle = new SolidBrush(Color.FromArgb(230, 240, 250));
            colors.Line = new Pen(Color.FromArgb(150, 150, 150));
            colors.Segment = new Pen(Color.SteelBlue);
        }

        /// <summary>
        /// Set a color scheme with black background.
        /// </summary>
        public void SetColorSchemeDark()
        {
            colors.Background = Color.Black;
            colors.Point = new SolidBrush(Color.Green);
            colors.SteinerPoint = new SolidBrush(Color.Peru);
            colors.Triangle = new SolidBrush(Color.FromArgb(30, 40, 50));
            colors.Line = new Pen(Color.FromArgb(30, 30, 30));
            colors.Segment = new Pen(Color.Blue);
        }

        /// <summary>
        /// Draws the mesh and writes the image file.
        /// </summary>
        /// <param name="mesh">The mesh to visualize.</param>
        public void WritePng(Mesh mesh)
        {
            WritePng(mesh, "", 1000);
        }

        /// <summary>
        /// Draws the mesh and writes the image file.
        /// </summary>
        /// <param name="mesh">The mesh to visualize.</param>
        /// <param name="filename">The filename (only PNG supported).</param>
        public void WritePng(Mesh mesh, string filename)
        {
            WritePng(mesh, filename, 1000);
        }

        /// <summary>
        /// Draws the mesh and writes the image file.
        /// </summary>
        /// <param name="mesh">The mesh to visualize.</param>
        /// <param name="width">The target width of the image (pixel).</param>
        public void WritePng(Mesh mesh, int width)
        {
            WritePng(mesh, "", width);
        }

        /// <summary>
        /// Draws the mesh and writes the image file.
        /// </summary>
        /// <param name="mesh">The mesh to visualize.</param>
        /// <param name="filename">The filename (only PNG supported).</param>
        /// <param name="width">The target width of the image (pixel).</param>
        public void WritePng(Mesh mesh, string filename, int width)
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
