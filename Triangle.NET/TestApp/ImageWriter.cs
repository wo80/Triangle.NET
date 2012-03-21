// -----------------------------------------------------------------------
// <copyright file="ImageWriter.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace TestApp
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using TriangleNet;
    using TriangleNet.Data;
    using TriangleNet.IO;

    /// <summary>
    /// Writes an image of the mesh to disk.
    /// </summary>
    public static class ImageWriter
    {
        // Number of input points
        static int NumberOfInputPoints = 0;

        // Default color scheme (dark)
        static Color bgColor = Color.Black;
        static Color ptColor = Color.Green;
        static Color spColor = Color.Peru;
        static Color lnColor = Color.FromArgb(30, 30, 30);
        static Color sgColor = Color.Blue;
        static Color trColor = Color.FromArgb(30, 40, 50);
            
        /// <summary>
        /// Sets the color scheme.
        /// </summary>
        /// <param name="background">Background color.</param>
        /// <param name="points">Points color.</param>
        /// <param name="steiner">Steiner points color.</param>
        /// <param name="lines">Line color.</param>
        /// <param name="segments">Segment color.</param>
        public static void SetColorScheme(Color background, Color points, Color steiner, 
            Color lines, Color segments, Color triangles)
        {
            bgColor = background;
            ptColor = points;
            spColor = steiner;
            lnColor = lines;
            sgColor = segments;
            trColor = triangles;
        }

        /// <summary>
        /// Set a color scheme with white background.
        /// </summary>
        public static void SetColorSchemeLight()
        {
            bgColor = Color.White;
            ptColor = Color.MidnightBlue;
            spColor = Color.DarkGreen;
            lnColor = Color.FromArgb(150, 150, 150);
            sgColor = Color.SteelBlue;
            trColor = Color.FromArgb(230, 240, 250);
        }

        /// <summary>
        /// Set a color scheme with black background.
        /// </summary>
        public static void SetColorSchemeDark()
        {
            bgColor = Color.Black;
            ptColor = Color.Green;
            spColor = Color.Peru;
            lnColor = Color.FromArgb(30, 30, 30);
            sgColor = Color.Blue;
            trColor = Color.FromArgb(30, 40, 50);
        }

        /// <summary>
        /// Draws the mesh and writes the image file.
        /// </summary>
        /// <param name="mesh">The mesh to visualize.</param>
        public static void WritePng(Mesh mesh)
        {
            WritePng(mesh, "", 1000);
        }

        /// <summary>
        /// Draws the mesh and writes the image file.
        /// </summary>
        /// <param name="mesh">The mesh to visualize.</param>
        /// <param name="filename">The filename (only PNG supported).</param>
        public static void WritePng(Mesh mesh, string filename)
        {
            WritePng(mesh, filename, 1000);
        }

        /// <summary>
        /// Draws the mesh and writes the image file.
        /// </summary>
        /// <param name="mesh">The mesh to visualize.</param>
        /// <param name="width">The target width of the image (pixel).</param>
        public static void WritePng(Mesh mesh, int width)
        {
            WritePng(mesh, "", width);
        }

        /// <summary>
        /// Draws the mesh and writes the image file.
        /// </summary>
        /// <param name="mesh">The mesh to visualize.</param>
        /// <param name="filename">The filename (only PNG supported).</param>
        /// <param name="width">The target width of the image (pixel).</param>
        public static void WritePng(Mesh mesh, string filename, int width)
        {
            NumberOfInputPoints = mesh.NumberOfInputPoints;

            if (String.IsNullOrWhiteSpace(filename))
            {
                filename = String.Format("mesh-{0}.png", DateTime.Now.ToString("yyyy-M-d-hh-mm-ss"));
            }

            MeshData data = mesh.GetMeshData(true, true, false);

            // Mesh bounds
            float minx = float.MaxValue;
            float maxx = float.MinValue;
            float miny = float.MaxValue;
            float maxy = float.MinValue;

            float x, y;

            int n = data.Points.Length;

            // Calculate bounds
            for (int i = 0; i < n; i++)
            {
                x = (float)data.Points[i][0];
                y = (float)data.Points[i][1];

                // Update bounding box
                if (minx > x) minx = x;
                if (maxx < x) maxx = x;
                if (miny > y) miny = y;
                if (maxy < y) maxy = y;
            }

            Bitmap bitmap;

            // Check if the specified width is reasonable
            if (width < 2 * Math.Sqrt(n))
            {
                bitmap = new Bitmap(400, 200);
                Graphics g = Graphics.FromImage(bitmap);
                g.Clear(Color.Black);

                string message = String.Format("Sorry, I won't render {0} points on such a small image!", n);

                SizeF sz = g.MeasureString(message, SystemFonts.DefaultFont);

                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawString(message, SystemFonts.DefaultFont, Brushes.White,
                    200 - sz.Width / 2, 100 - sz.Height / 2);

                g.Dispose();
            }
            else
            {
                // World margin on each side
                float margin = (maxy - miny) * 0.05f;
                float scale = width / (maxx - minx + 2 * margin);

                bitmap = new Bitmap(width, (int)((maxy - miny + 2 * margin) * scale), PixelFormat.Format32bppArgb);

                Graphics g = Graphics.FromImage(bitmap);
                g.Clear(bgColor);

                // Transform world to screen
                g.ScaleTransform(scale, -scale);
                g.TranslateTransform(-minx + margin, -maxy - margin);

                DrawMesh(g, data, scale);

                g.Dispose();
            }

            if (Path.GetExtension(filename) != ".png")
            {
                filename += ".png";
            }

            bitmap.Save(filename, ImageFormat.Png);
        }

        /// <summary>
        /// Draw mesh to the graphics object.
        /// </summary>
        private static void DrawMesh(Graphics g, MeshData mesh, float scale)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            // Colors
            
            Brush bgBrush = new SolidBrush(bgColor);
            Brush ptBrush = new SolidBrush(ptColor);
            Brush spBrush = new SolidBrush(spColor);
            Brush trBrush = new SolidBrush(trColor);

            // Scale the pens to 1 pixel width
            //Pen ptBrush = new Pen(ptColor, 1 / scale);
            //Pen spBrush = new Pen(spColor, 1 / scale);
            Pen lnBrush = new Pen(lnColor, 1 / scale);
            Pen sgBrush = new Pen(sgColor, 1 / scale);

            PointF p1, p2, p3;

            int[] tmp;

            // Draw triangle edges
            int n = mesh.Triangles == null ? 0 : mesh.Triangles.Length;

            for (int i = 0; i < n; i++)
            {
                tmp = mesh.Triangles[i];

                p1 = new PointF((float)mesh.Points[tmp[0]][0], (float)mesh.Points[tmp[0]][1]);
                p2 = new PointF((float)mesh.Points[tmp[1]][0], (float)mesh.Points[tmp[1]][1]);
                p3 = new PointF((float)mesh.Points[tmp[2]][0], (float)mesh.Points[tmp[2]][1]);

                // Fill triangle
                g.FillPolygon(trBrush, new PointF[] { p1, p2, p3 });
            }

            // Draw edges
            n = mesh.Edges == null ? 0 : mesh.Edges.Length;

            for (int i = 0; i < n; i++)
            {
                tmp = mesh.Edges[i];

                p1 = new PointF((float)mesh.Points[tmp[0]][0], (float)mesh.Points[tmp[0]][1]);
                p2 = new PointF((float)mesh.Points[tmp[1]][0], (float)mesh.Points[tmp[1]][1]);

                // Draw line
                g.DrawLine(lnBrush, p1, p2);
            }

            // Draw segments
            n = mesh.Segments == null ? 0 : mesh.Segments.Length;

            for (int i = 0; i < n; i++)
            {
                tmp = mesh.Segments[i];

                p1 = new PointF((float)mesh.Points[tmp[0]][0], (float)mesh.Points[tmp[0]][1]);
                p2 = new PointF((float)mesh.Points[tmp[1]][0], (float)mesh.Points[tmp[1]][1]);

                // Draw line
                g.DrawLine(sgBrush, p1, p2);
            }

            // Scale the points radius to 2 pixel.
            float radius = 1.5f / scale, x, y;

            // Draw points
            n = mesh.Points.Length;

            if (NumberOfInputPoints <= 0)
            {
                NumberOfInputPoints = n;
            }

            for (int i = 0; i < n; i++)
            {
                x = (float)mesh.Points[i][0];
                y = (float)mesh.Points[i][1];

                if (i < NumberOfInputPoints)
                {
                    g.FillEllipse(ptBrush, x - radius, y - radius, 2 * radius, 2 * radius);
                    //g.DrawEllipse(ptBrush, x - radius, y - radius, 2 * radius, 2 * radius);
                }
                else
                {
                    g.FillEllipse(spBrush, x - radius, y - radius, 2 * radius, 2 * radius);
                    //g.DrawEllipse(ptBrush, x - radius, y - radius, 2 * radius, 2 * radius);
                }
            }

            bgBrush.Dispose();
            ptBrush.Dispose();
            spBrush.Dispose();
            lnBrush.Dispose();
            sgBrush.Dispose();
            trBrush.Dispose();
        }
    }
}
