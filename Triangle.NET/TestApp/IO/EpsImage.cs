// -----------------------------------------------------------------------
// <copyright file="EpsImage.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// Original Matlab code by John Burkardt, Florida State University
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.IO
{
    using System;
    using System.IO;
    using System.Text;
    using TriangleNet;
    using TriangleNet.Data;
    using TriangleNet.Geometry;

    /// <summary>
    /// Writes a mesh to an EPS file.
    /// </summary>
    public class EpsImage
    {
        // EPS page metrics
        int x_ps_max = 576;
        int x_ps_max_clip = 594;
        int x_ps_min = 36;
        int x_ps_min_clip = 18;
        int y_ps_max = 666;
        int y_ps_max_clip = 684;
        int y_ps_min = 126;
        int y_ps_min_clip = 108;

        // Mesh metrics
        double x_max, x_min;
        double y_max, y_min;
        double x_scale, y_scale;

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

            using (StreamWriter eps = new StreamWriter(filename))
            {
                WriteHeader(filename, eps);

                DrawClip(eps);

                DrawTriangles(eps, mesh, false);

                DrawSegments(eps, mesh);

                DrawPoints(eps, mesh, false);

                WriteTrailer(eps);
            }
        }

        private void WriteHeader(string filename, StreamWriter eps)
        {
            eps.WriteLine("%!PS-Adobe-3.0 EPSF-3.0");
            eps.WriteLine("%%Creator: Triangle.NET Mesh Explorer");
            eps.WriteLine("%%Title: {0}", filename);
            eps.WriteLine("%%Pages: 1");
            eps.WriteLine("%%BoundingBox:  {0}  {1}  {2}  {3}", x_ps_min, y_ps_min, x_ps_max, y_ps_max);
            eps.WriteLine("%%Document-Fonts: Times-Roman");
            eps.WriteLine("%%LanguageLevel: 1");
            eps.WriteLine("%%EndComments");
            eps.WriteLine("%%BeginProlog");
            eps.WriteLine("/inch {72 mul} def");
            eps.WriteLine("%%EndProlog");
            eps.WriteLine("%%Page: 1 1");
        }

        private static void WriteTrailer(StreamWriter eps)
        {
            eps.WriteLine("%");
            eps.WriteLine("restore  showpage");
            eps.WriteLine("%");
            eps.WriteLine("%  End of page.");
            eps.WriteLine("%");
            eps.WriteLine("%%Trailer");
            eps.WriteLine("%%EOF");
        }

        private void DrawClip(StreamWriter eps)
        {
            eps.WriteLine("save");
            eps.WriteLine("%");
            eps.WriteLine("%  Set the RGB color to very light gray.");
            eps.WriteLine("%");
            eps.WriteLine("0.900  0.900  0.900 setrgbcolor");
            eps.WriteLine("%");
            eps.WriteLine("%  Draw a gray border around the page.");
            eps.WriteLine("%");
            eps.WriteLine("newpath");
            eps.WriteLine("  {0}  {1}  moveto", x_ps_min, y_ps_min);
            eps.WriteLine("  {0}  {1}  lineto", x_ps_max, y_ps_min);
            eps.WriteLine("  {0}  {1}  lineto", x_ps_max, y_ps_max);
            eps.WriteLine("  {0}  {1}  lineto", x_ps_min, y_ps_max);
            eps.WriteLine("  {0}  {1}  lineto", x_ps_min, y_ps_min);
            eps.WriteLine("stroke");
            eps.WriteLine("%");
            eps.WriteLine("%  Set the RGB color to black.");
            eps.WriteLine("%");
            eps.WriteLine("0.000  0.000  0.000 setrgbcolor");
            eps.WriteLine("%");
            eps.WriteLine("%  Set the font and its size.");
            eps.WriteLine("%");
            eps.WriteLine("/Times-Roman findfont");
            eps.WriteLine("0.50 inch scalefont");
            eps.WriteLine("setfont");
            eps.WriteLine("%");
            eps.WriteLine("%  Print a title.");
            eps.WriteLine("%");
            eps.WriteLine("%210  702  moveto");
            eps.WriteLine("%(Triangulation)  show");
            eps.WriteLine("%");
            eps.WriteLine("%  Define a clipping polygon.");
            eps.WriteLine("%");
            eps.WriteLine("newpath");
            eps.WriteLine("  {0}  {1}  moveto", x_ps_min_clip, y_ps_min_clip);
            eps.WriteLine("  {0}  {1}  lineto", x_ps_max_clip, y_ps_min_clip);
            eps.WriteLine("  {0}  {1}  lineto", x_ps_max_clip, y_ps_max_clip);
            eps.WriteLine("  {0}  {1}  lineto", x_ps_min_clip, y_ps_max_clip);
            eps.WriteLine("  {0}  {1}  lineto", x_ps_min_clip, y_ps_min_clip);
            eps.WriteLine("clip newpath");
        }

        private void DrawTriangles(StreamWriter eps, Mesh mesh, bool label)
        {
            eps.WriteLine("%");
            eps.WriteLine("%  Set the triangle line color and width.");
            eps.WriteLine("%");
            eps.WriteLine("0.6  0.6  0.6 setrgbcolor");
            eps.WriteLine("0.5 setlinewidth");
            eps.WriteLine("%");
            eps.WriteLine("%  Draw the triangles.");
            eps.WriteLine("%");

            StringBuilder labels = new StringBuilder();

            Vertex v1, v2, v3;
            double x1, y1, x2, y2, x3, y3, xa, ya;
            int x_ps, y_ps;

            foreach (var tri in mesh.Triangles)
            {
                eps.WriteLine("newpath");

                v1 = tri.GetVertex(0);
                v2 = tri.GetVertex(1);
                v3 = tri.GetVertex(2);

                x1 = v1.X; y1 = v1.Y;
                x2 = v2.X; y2 = v2.Y;
                x3 = v3.X; y3 = v3.Y;

                x_ps = (int)Math.Floor(((x_max - x1) * x_ps_min + (x1 - x_min) * x_ps_max) / (x_max - x_min));
                y_ps = (int)Math.Floor(((y_max - y1) * y_ps_min + (y1 - y_min) * y_ps_max) / (y_max - y_min));
                eps.WriteLine("  {0}  {1}  moveto", x_ps, y_ps);

                x_ps = (int)Math.Floor(((x_max - x2) * x_ps_min + (x2 - x_min) * x_ps_max) / (x_max - x_min));
                y_ps = (int)Math.Floor(((y_max - y2) * y_ps_min + (y2 - y_min) * y_ps_max) / (y_max - y_min));
                eps.WriteLine("  {0}  {1}  lineto", x_ps, y_ps);

                x_ps = (int)Math.Floor(((x_max - x3) * x_ps_min + (x3 - x_min) * x_ps_max) / (x_max - x_min));
                y_ps = (int)Math.Floor(((y_max - y3) * y_ps_min + (y3 - y_min) * y_ps_max) / (y_max - y_min));
                eps.WriteLine("  {0}  {1}  lineto", x_ps, y_ps);

                x_ps = (int)Math.Floor(((x_max - x1) * x_ps_min + (x1 - x_min) * x_ps_max) / (x_max - x_min));
                y_ps = (int)Math.Floor(((y_max - y1) * y_ps_min + (y1 - y_min) * y_ps_max) / (y_max - y_min));
                eps.WriteLine("  {0}  {1}  lineto", x_ps, y_ps);

                if (label)
                {
                    xa = (x1 + x2 + x3) / 3.0;
                    ya = (y1 + y2 + y3) / 3.0;

                    x_ps = (int)Math.Floor(((x_max - xa) * x_ps_min + (xa - x_min) * x_ps_max) / (x_max - x_min));
                    y_ps = (int)Math.Floor(((y_max - ya) * y_ps_min + (ya - y_min) * y_ps_max) / (y_max - y_min));

                    labels.AppendFormat("  {0}  {1}  moveto ({2}) show", x_ps, y_ps, tri.ID);
                    labels.AppendLine();
                }

                eps.WriteLine("stroke");
            }

            //  Label the triangles.
            if (label)
            {
                eps.WriteLine("%");
                eps.WriteLine("%  Label the triangles.");
                eps.WriteLine("%");
                eps.WriteLine("%  Set the RGB color to darker red.");
                eps.WriteLine("%");
                eps.WriteLine("0.950  0.250  0.150 setrgbcolor");
                eps.WriteLine("/Times-Roman findfont");
                eps.WriteLine("0.20 inch scalefont");
                eps.WriteLine("setfont");
                eps.WriteLine("%");

                eps.Write(labels.ToString());
            }
        }

        private void DrawSegments(StreamWriter eps, Mesh mesh)
        {
            eps.WriteLine("%");
            eps.WriteLine("%  Set the triangle line color and width.");
            eps.WriteLine("%");
            eps.WriteLine("0.27  0.5  0.7 setrgbcolor");
            eps.WriteLine("0.75 setlinewidth");
            eps.WriteLine("%");
            eps.WriteLine("%  Draw the triangles.");
            eps.WriteLine("%");

            StringBuilder labels = new StringBuilder();

            double x1, y1, x2, y2;
            int x_ps, y_ps;

            foreach (var seg in mesh.Segments)
            {
                eps.WriteLine("newpath");

                x1 = seg.GetVertex(0).X; y1 = seg.GetVertex(0).Y;
                x2 = seg.GetVertex(1).X; y2 = seg.GetVertex(1).Y;

                x_ps = (int)Math.Floor(((x_max - x1) * x_ps_min + (x1 - x_min) * x_ps_max) / (x_max - x_min));
                y_ps = (int)Math.Floor(((y_max - y1) * y_ps_min + (y1 - y_min) * y_ps_max) / (y_max - y_min));
                eps.WriteLine("  {0}  {1}  moveto", x_ps, y_ps);

                x_ps = (int)Math.Floor(((x_max - x2) * x_ps_min + (x2 - x_min) * x_ps_max) / (x_max - x_min));
                y_ps = (int)Math.Floor(((y_max - y2) * y_ps_min + (y2 - y_min) * y_ps_max) / (y_max - y_min));
                eps.WriteLine("  {0}  {1}  lineto", x_ps, y_ps);


                eps.WriteLine("stroke");
            }
        }

        private void DrawPoints(StreamWriter eps, Mesh mesh, bool label)
        {
            int n = mesh.Vertices.Count;

            int circle_size = 1;

            if (n < 100)
            {
                circle_size = 3;
            }
            else if (n < 500)
            {
                circle_size = 2;
            }

            eps.WriteLine("%");
            eps.WriteLine("%  Draw filled dots at the nodes.");
            eps.WriteLine("%");
            eps.WriteLine("%  Set the RGB color to blue.");
            eps.WriteLine("%");
            eps.WriteLine("0.0  0.4  0.0 setrgbcolor");
            eps.WriteLine("%");

            double x, y;
            int x_ps, y_ps;

            StringBuilder labels = new StringBuilder();

            foreach (var node in mesh.Vertices)
            {
                x = node.X;
                y = node.Y;

                x_ps = (int)Math.Floor(((x_max - x) * x_ps_min + (x - x_min) * x_ps_max) / (x_max - x_min));
                y_ps = (int)Math.Floor(((y_max - y) * y_ps_min + (y - y_min) * y_ps_max) / (y_max - y_min));

                eps.WriteLine("  newpath  {0}  {1}  {2} 0 360 arc closepath fill", x_ps, y_ps, circle_size);

                if (label)
                {
                    labels.AppendFormat("  {0}  {1}  moveto ({2}) show", x_ps, y_ps + 5, node);
                    labels.AppendLine();
                }
            }

            //  Label the nodes.
            if (label)
            {
                eps.WriteLine("%");
                eps.WriteLine("%  Label the nodes.");
                eps.WriteLine("%");
                eps.WriteLine("%  Set the RGB color to darker blue.");
                eps.WriteLine("%");
                eps.WriteLine("0.000  0.250  0.850 setrgbcolor");
                eps.WriteLine("/Times-Roman findfont");
                eps.WriteLine("0.20 inch scalefont");
                eps.WriteLine("setfont");
                eps.WriteLine("%");

                eps.Write(labels.ToString());
            }
        }

        private void UpdateMetrics(Rectangle bounds)
        {
            x_max = bounds.Right;
            x_min = bounds.Left;
            y_max = bounds.Top;
            y_min = bounds.Bottom;

            // Enlarge width 5% on each side
            x_scale = x_max - x_min;
            x_max = x_max + 0.05 * x_scale;
            x_min = x_min - 0.05 * x_scale;
            x_scale = x_max - x_min;

            // Enlarge height 5% on each side
            y_scale = y_max - y_min;
            y_max = y_max + 0.05 * y_scale;
            y_min = y_min - 0.05 * y_scale;
            y_scale = y_max - y_min;

            if (x_scale < y_scale)
            {
                int delta = (int)Math.Round((x_ps_max - x_ps_min) * (y_scale - x_scale) / (2.0 * y_scale));

                x_ps_max = x_ps_max - delta;
                x_ps_min = x_ps_min + delta;

                x_ps_max_clip = x_ps_max_clip - delta;
                x_ps_min_clip = x_ps_min_clip + delta;

                x_scale = y_scale;
            }
            else
            {
                int delta = (int)Math.Round((y_ps_max - y_ps_min) * (x_scale - y_scale) / (2.0 * x_scale));

                y_ps_max = y_ps_max - delta;
                y_ps_min = y_ps_min + delta;

                y_ps_max_clip = y_ps_max_clip - delta;
                y_ps_min_clip = y_ps_min_clip + delta;

                y_scale = x_scale;
            }
        }
    }
}
