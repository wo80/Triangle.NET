
namespace TriangleNet.Rendering.GDI
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using TriangleNet.Meshing;

    /// <summary>
    /// Enables rendering of polygons or meshes to a bitmap.
    /// </summary>
    public class ImageRenderer
    {
        ColorManager colors = LightScheme();

        public ColorManager ColorScheme
        {
            get { return colors; }
            set { colors = value; }
        }

        public bool EnableRegions { get; set; }

        public bool EnablePoints { get; set; }
        
        /// <summary>
        /// Export the mesh to PNG format.
        /// </summary>
        /// <param name="mesh">The current mesh.</param>
        /// <param name="width">The desired width (pixel) of the image.</param>
        /// <param name="file">The PNG filename.</param>
        /// <param name="regions">Enable rendering of regions.</param>
        /// <param name="points">Enable rendering of points.</param>
        public static void Save(IMesh mesh, string file = null, int width = 800,
            bool regions = false, bool points = true)
        {
            // Check file name
            if (string.IsNullOrWhiteSpace(file))
            {
                file = string.Format("mesh-{0}.png", DateTime.Now.ToString("yyyy-M-d-hh-mm-ss"));
            }

            // Ensure .png extension.
            if (!file.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
            {
                Path.ChangeExtension(file, ".png");
            }

            var renderer = new ImageRenderer();

            renderer.EnableRegions = regions;
            renderer.EnablePoints = points;

            var bitmap = renderer.Render(mesh, width);

            bitmap.Save(file, ImageFormat.Png);
        }

        /// <summary>
        /// Renders the mesh to a bitmap.
        /// </summary>
        /// <param name="mesh">The current mesh.</param>
        /// <param name="width">The desired width (pixel) of the image.</param>
        /// <returns>The bitmap.</returns>
        /// <remarks>
        /// The width has to be at least 2 * sqrt(n), n the number of vertices.
        /// Otherwise, an empty bitmap
        /// </remarks>
        public Bitmap Render(IMesh mesh, int width = 800)
        {
            Bitmap bitmap;

            // Check if the specified width is reasonable
            if (width < 2 * Math.Sqrt(mesh.Vertices.Count))
            {
                return new Bitmap(1, 1);
            }

            var bounds = mesh.Bounds;

            // World margin on each side
            float margin = (float)bounds.Height * 0.05f;
            float scale = width / ((float)bounds.Width + 2 * margin);

            var target = new Rectangle(0, 0, width, (int)((bounds.Height + 2 * margin) * scale));

            bitmap = new Bitmap(width, target.Height, PixelFormat.Format32bppPArgb);

            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(colors.Background);
                g.SmoothingMode = SmoothingMode.HighQuality;

                var context = new RenderContext(new Projection(target), colors);
                context.Add(mesh, true);

                if (EnableRegions)
                {
                    context.Add(GetRegions(mesh));
                }

                if (!EnablePoints)
                {
                    context.Enable(3, false);
                }

                var renderer = new LayerRenderer();
                renderer.Context = context;
                renderer.RenderTarget = g;
                renderer.Render();
            }

            return bitmap;
        }

        private int[] GetRegions(IMesh mesh)
        {
            mesh.Renumber();

            var labels = new int[mesh.Triangles.Count];
            var regions = new SortedSet<int>();

            foreach (var t in mesh.Triangles)
            {
                labels[t.ID] = t.Label;
                regions.Add(t.Label);
            }

            if (colors.ColorDictionary == null)
            {
                colors.CreateColorDictionary(regions, regions.Count);
            }

            return labels;
        }

        public static ColorManager LightScheme()
        {
            var colors = new ColorManager();

            colors.Background = Color.White;
            colors.Point = new SolidBrush(Color.FromArgb(60, 80, 120));
            colors.SteinerPoint = new SolidBrush(Color.DarkGreen);
            colors.Line = new Pen(Color.FromArgb(200, 200, 200));
            colors.Segment = new Pen(Color.SteelBlue);
            colors.VoronoiLine = new Pen(Color.FromArgb(160, 170, 180));

            return colors;
        }
    }
}
