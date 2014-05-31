// -----------------------------------------------------------------------
// <copyright file="RasterImage.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.IO
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using TriangleNet;
    using TriangleNet.Rendering;
    using TriangleNet.Rendering.GDI;

    /// <summary>
    /// Writes an image of the mesh to disk.
    /// </summary>
    public class RasterImage
    {
        ColorManager colors = RasterImage.LightScheme();

        public ColorManager ColorScheme
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
            // Check file name
            if (String.IsNullOrWhiteSpace(filename))
            {
                filename = String.Format("mesh-{0}.png", DateTime.Now.ToString("yyyy-M-d-hh-mm-ss"));
            }

            Bitmap bitmap;

            // Check if the specified width is reasonable
            if (width < 2 * Math.Sqrt(mesh.Vertices.Count))
            {
                bitmap = new Bitmap(400, 200);
                Graphics g = Graphics.FromImage(bitmap);
                g.Clear(Color.White);

                string message = String.Format("Sorry, I won't render {0} points on such a small image!", mesh.Vertices.Count);

                SizeF sz = g.MeasureString(message, SystemFonts.DefaultFont);

                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawString(message, SystemFonts.DefaultFont, Brushes.Black,
                    200 - sz.Width / 2, 100 - sz.Height / 2);

                g.Dispose();
            }
            else
            {
                var bounds = mesh.Bounds;
                // World margin on each side
                float margin = (float)bounds.Height * 0.05f;
                float scale = width / ((float)bounds.Width + 2 * margin);

                var target = new Rectangle(0, 0, width, (int)((bounds.Height + 2 * margin) * scale));

                bitmap = new Bitmap(width, target.Height, PixelFormat.Format32bppPArgb);

                Graphics g = Graphics.FromImage(bitmap);
                g.Clear(colors.Background);

                g.SmoothingMode = SmoothingMode.HighQuality;

                var context = new RenderContext(new Projection(target), colors);
                context.Add(mesh, true);

                var renderer = new LayerRenderer();
                renderer.Context = context;
                renderer.RenderTarget = g;
                renderer.Render();

                g.Dispose();
            }

            if (Path.GetExtension(filename) != ".png")
            {
                filename += ".png";
            }

            bitmap.Save(filename, ImageFormat.Png);
        }

        public static ColorManager LightScheme()
        {
            var colors = new ColorManager();

            colors.Background = Color.White;
            colors.Point = new SolidBrush(Color.FromArgb(60, 80, 120));
            colors.SteinerPoint = new SolidBrush(Color.DarkGreen);
            colors.Line = new Pen(Color.FromArgb(150, 150, 150));
            colors.Segment = new Pen(Color.SteelBlue);
            colors.VoronoiLine = new Pen(Color.FromArgb(160, 170, 180));

            return colors;
        }
    }
}
