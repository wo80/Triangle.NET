
namespace TriangleNet.Rendering.GDI
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using TriangleNet.Meshing;

    /// <summary>
    /// Enables rendering of polygons or meshes to a bitmap.
    /// </summary>
    public class ImageRenderer
    {
        public ColorManager ColorScheme { get; set; } = LightScheme();

        public bool EnableRegions { get; set; }

        public bool EnablePoints { get; set; }

        /// <summary>
        /// Exports a polygon to PNG format.
        /// </summary>
        /// <param name="poly">The polygon.</param>
        /// <param name="width">The desired width (pixel) of the image.</param>
        /// <param name="file">The PNG filename.</param>
        /// <param name="regions">Enable rendering of regions.</param>
        /// <param name="points">Enable rendering of points.</param>
        public static void Save(Geometry.IPolygon poly, string file = null, int width = 800,
            bool points = true)
        {
            // Check file name
            if (string.IsNullOrWhiteSpace(file))
            {
                file = string.Format("poly-{0}.png", DateTime.Now.ToString("yyyy-M-d-hh-mm-ss"));
            }

            // Ensure .png extension.
            if (file.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
            {
                Path.ChangeExtension(file, ".png");
            }

            var renderer = new ImageRenderer();

            renderer.EnableRegions = false;
            renderer.EnablePoints = points;

            var bitmap = renderer.Render(poly, width);

            bitmap.Save(file, ImageFormat.Png);
        }

        /// <summary>
        /// Exports a mesh to PNG format.
        /// </summary>
        /// <param name="mesh">The mesh.</param>
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
        /// Renders the polygon to a bitmap.
        /// </summary>
        /// <param name="poly">The polygon.</param>
        /// <param name="width">The desired width (pixel) of the image.</param>
        /// <returns>The bitmap.</returns>
        /// <remarks>
        /// The width has to be at least 2 * sqrt(n), n the number of vertices.
        /// Otherwise, an empty bitmap
        /// </remarks>
        public Bitmap Render(Geometry.IPolygon poly, int width = 800)
        {
            Bitmap bitmap;

            // Check if the specified width is reasonable
            if (width < 2 * Math.Sqrt(poly.Points.Count))
            {
                return new Bitmap(1, 1);
            }

            var bounds = poly.Bounds();

            // World margin on each side
            float margin = (float)bounds.Height * 0.05f;
            float scale = width / ((float)bounds.Width + 2 * margin);

            var target = new Rectangle(0, 0, width, (int)((bounds.Height + 2 * margin) * scale));

            bitmap = new Bitmap(width, target.Height, PixelFormat.Format32bppPArgb);

            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(ColorScheme.Background);
                g.SmoothingMode = SmoothingMode.HighQuality;

                var context = new RenderContext(new Projection(target), ColorScheme);
                context.Add(poly);
                
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
                g.Clear(ColorScheme.Background);
                g.SmoothingMode = SmoothingMode.HighQuality;

                var context = new RenderContext(new Projection(target), ColorScheme);
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

        public Bitmap Render(IMesh mesh, Topology.DCEL.DcelMesh dcel, int width = 800)
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
                g.Clear(ColorScheme.Background);
                g.SmoothingMode = SmoothingMode.HighQuality;

                var context = new RenderContext(new Projection(target), ColorScheme);
                context.Add(mesh, true);
                context.Add(dcel.Vertices.ToArray(), dcel.Edges, false);

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

        private uint[] GetRegions(IMesh mesh)
        {
            mesh.Renumber();

            var labels = new uint[mesh.Triangles.Count()];
            var regions = new SortedSet<uint>();

            foreach (var t in mesh.Triangles)
            {
                labels[t.ID] = (uint)t.Label;
                regions.Add((uint)t.Label);
            }

            if (ColorScheme.ColorDictionary == null)
            {
                ColorScheme.CreateColorDictionary(regions);
            }

            return labels;
        }

        public static ColorManager LightScheme()
        {
            var colors = new ColorManager();

            colors.Background = Color.White;
            colors.Point = Color.FromArgb(60, 80, 120);
            colors.SteinerPoint = Color.DarkGreen;
            colors.Line = Color.FromArgb(200, 200, 200);
            colors.Segment = Color.SteelBlue;
            colors.VoronoiLine = Color.FromArgb(160, 170, 180);

            return colors;
        }
    }
}
