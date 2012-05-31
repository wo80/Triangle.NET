// -----------------------------------------------------------------------
// <copyright file="ImageWriter.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer
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
        static PointF[] points;

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
            int i = 0, n = mesh.NumberOfVertices;

            points = new PointF[n];

            foreach (var pt in mesh.Vertices)
            {
                points[i++] = new PointF((float)pt.X, (float)pt.Y);
            }

            if (String.IsNullOrWhiteSpace(filename))
            {
                filename = String.Format("mesh-{0}.png", DateTime.Now.ToString("yyyy-M-d-hh-mm-ss"));
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
                var bounds = mesh.Bounds;
                // World margin on each side
                float margin = (float)bounds.Height * 0.05f;
                float scale = width / ((float)bounds.Width + 2 * margin);

                bitmap = new Bitmap(width, (int)((bounds.Height + 2 * margin) * scale), PixelFormat.Format32bppArgb);

                Graphics g = Graphics.FromImage(bitmap);
                g.Clear(bgColor);

                // Transform world to screen
                g.ScaleTransform(scale, -scale);
                g.TranslateTransform(-(float)bounds.Xmin + margin, -(float)bounds.Ymax - margin);

                DrawMesh(g, mesh, scale);

                g.Dispose();
            }

            if (Path.GetExtension(filename) != ".png")
            {
                filename += ".png";
            }

            bitmap.Save(filename, ImageFormat.Png);
        }

        /// <summary>
        /// Draws the voronoi diagram and writes the image file.
        /// </summary>
        /// <param name="mesh">The mesh to visualize.</param>
        /// <param name="filename">The filename (only PNG supported).</param>
        /// <param name="width">The target width of the image (pixel).</param>
        public static void WriteVoronoiPng(Mesh mesh, string filename, int width)
        {
            if (String.IsNullOrWhiteSpace(filename))
            {
                filename = String.Format("mesh-{0}.png", DateTime.Now.ToString("yyyy-M-d-hh-mm-ss"));
            }

            VoronoiData data = DataWriter.WriteVoronoi(mesh);

            int n = data.Points.Length;

            var bounds = mesh.Bounds;

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
                float margin = (float)bounds.Height * 0.05f;
                float scale = width / ((float)bounds.Width + 2 * margin);

                bitmap = new Bitmap(width, (int)((bounds.Height + 2 * margin) * scale), PixelFormat.Format32bppArgb);

                Graphics g = Graphics.FromImage(bitmap);
                g.Clear(bgColor);

                // Transform world to screen
                g.ScaleTransform(scale, -scale);
                g.TranslateTransform(-(float)bounds.Xmin + margin, -(float)bounds.Ymax - margin);

                DrawVoronoi(g, mesh, data, scale);

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
        private static void DrawMesh(Graphics g, Mesh mesh, float scale)
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

            // Draw triangle edges
            foreach (var tri in mesh.Triangles)
            {
                p1 = points[tri.P0];
                p2 = points[tri.P1];
                p3 = points[tri.P2];

                // Fill triangle
                g.FillPolygon(trBrush, new PointF[] { p1, p2, p3 });
            }

            // Draw edges
            /*
            n = mesh.Edges == null ? 0 : mesh.Edges.Length;

            for (int i = 0; i < n; i++)
            {
                tmp = mesh.Edges[i];

                p1 = new PointF((float)mesh.Points[tmp[0]].X, (float)mesh.Points[tmp[0]].Y);
                p2 = new PointF((float)mesh.Points[tmp[1]].X, (float)mesh.Points[tmp[1]].Y);

                // Draw line
                g.DrawLine(lnBrush, p1, p2);
            }
             * */

            // Draw segments
            foreach (var seg in mesh.Segments)
            {
                p1 = points[seg.P0];
                p2 = points[seg.P1];

                // Draw line
                g.DrawLine(sgBrush, p1, p2);
            }

            // Scale the points radius to 2 pixel.
            float radius = 1.5f / scale, x, y;

            // Draw points
            int n = mesh.NumberOfInputPoints;

            for (int i = 0; i < n; i++)
            {
                x = points[i].X;
                y = points[i].Y;

                if (i < n)
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

        /// <summary>
        /// Draw mesh to the graphics object.
        /// </summary>
        private static void DrawVoronoi(Graphics g, Mesh mesh, VoronoiData voronoi, float scale)
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

            PointF p1, p2;

            int[] tmp;

            BBox bounds = new BBox(mesh.Bounds);

            // Enlarge 50%
            bounds.Extend(0.5f);

            // Draw edges
            int n = voronoi.Edges == null ? 0 : voronoi.Edges.Length;

            for (int i = 0; i < n; i++)
            {
                var seg = voronoi.Edges[i];

                if (seg.P1 == -1)
                {
                    // Infinite voronoi edge
                    p1 = new PointF((float)voronoi.Points[seg.P0].X, (float)voronoi.Points[seg.P0].Y);
                    p2 = VoronoiBoxIntersection(bounds, voronoi.Points[seg.P0], voronoi.Directions[i]);
                }
                else
                {
                    p1 = new PointF((float)voronoi.Points[seg.P0].X, (float)voronoi.Points[seg.P0].Y);
                    p2 = new PointF((float)voronoi.Points[seg.P1].X, (float)voronoi.Points[seg.P1].Y);
                }

                // Draw line
                g.DrawLine(lnBrush, p1, p2);
            }

            // Shrink 50%
            bounds.Extend(-0.5f);

            // Scale the points radius to 2 pixel.
            float radius = 1.5f / scale, x, y;

            // Draw points
            n = voronoi.Points.Length;

            for (int i = 0; i < n; i++)
            {
                x = (float)voronoi.Points[i].X;
                y = (float)voronoi.Points[i].Y;

                g.FillEllipse(ptBrush, x - radius, y - radius, 2 * radius, 2 * radius);
                //g.DrawEllipse(ptBrush, x - radius, y - radius, 2 * radius, 2 * radius);
            }

            // Draw input points
            n = voronoi.InputPoints.Length;

            for (int i = 0; i < n; i++)
            {
                x = (float)voronoi.InputPoints[i].X;
                y = (float)voronoi.InputPoints[i].Y;

                g.FillEllipse(spBrush, x - radius, y - radius, 2 * radius, 2 * radius);
                //g.DrawEllipse(spBrush, x - radius, y - radius, 2 * radius, 2 * radius);
            }

            bgBrush.Dispose();
            ptBrush.Dispose();
            spBrush.Dispose();
            lnBrush.Dispose();
            sgBrush.Dispose();
            trBrush.Dispose();
        }

        private static PointF VoronoiBoxIntersection(BBox bounds, TriangleNet.Geometry.Point pt, double[] direction)
        {
            double x = pt.X;
            double y = pt.Y;
            double dx = direction[0];
            double dy = direction[1];

            double t1, x1, y1, t2, x2, y2;

            // Check if point is inside the bounds
            if (x < bounds.MinX || x > bounds.MaxX || y < bounds.MinY || y > bounds.MaxY)
            {
                throw new ArgumentException("Point must be located inside the bounding box.");
            }

            // Calculate the cut through the vertical boundaries
            if (dx < 0)
            {
                // Line going to the left: intersect with x = bounds.MinX
                t1 = (bounds.MinX - x) / dx;
                x1 = bounds.MinX;
                y1 = y + t1 * dy;
            }
            else if (dx > 0)
            {
                // Line going to the right: intersect with x = bounds.MaxX
                t1 = (bounds.MaxX - x) / dx;
                x1 = bounds.MaxX;
                y1 = y + t1 * dy;
            }
            else
            {
                // Line going straight up or down: no intersection possible
                t1 = double.MaxValue;
                x1 = y1 = 0;
            }

            // Calculate the cut through upper and lower boundaries
            if (dy < 0)
            {
                // Line going downwards: intersect with y = bounds.MinY
                t2 = (bounds.MinY - y) / dy;
                x2 = x + t2 * dx;
                y2 = bounds.MinY;
            }
            else if (dx > 0)
            {
                // Line going upwards: intersect with y = bounds.MaxY
                t2 = (bounds.MaxY - y) / dy;
                x2 = x + t2 * dx;
                y2 = bounds.MaxY;
            }
            else
            {
                // Horizontal line: no intersection possible
                t2 = double.MaxValue;
                x2 = y2 = 0;
            }

            if (t1 < t2)
            {
                return new PointF((float)x1, (float)y1);
            }

            return new PointF((float)x2, (float)y2);
        }

        /// <summary>
        /// Bounding box.
        /// </summary>
        struct BBox
        {
            public float MinX;
            public float MaxX;
            public float MinY;
            public float MaxY;

            public float Width { get { return MaxX - MinX; } }
            public float Height { get { return MaxY - MinY; } }

            public BBox(TriangleNet.Geometry.BoundingBox box)
            {
                MinX = (float)box.Xmin;
                MaxX = (float)box.Xmax;
                MinY = (float)box.Ymin;
                MaxY = (float)box.Ymax;
            }

            public void Reset()
            {
                MinX = float.MaxValue;
                MaxX = float.MinValue;
                MinY = float.MaxValue;
                MaxY = float.MinValue;
            }

            public void Update(TriangleNet.Geometry.Point pt)
            {
                float x = (float)pt.X;
                float y = (float)pt.Y;

                // Update bounding box
                if (MinX > x) MinX = x;
                if (MaxX < x) MaxX = x;
                if (MinY > y) MinY = y;
                if (MaxY < y) MaxY = y;
            }

            public void Extend(float amount)
            {
                float dx = amount * this.Width;
                float dy = amount * this.Height;

                MinX -= dx;
                MaxX += dx;
                MinY -= dy;
                MaxY += dy;
            }
        }
    }
}
