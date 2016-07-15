// -----------------------------------------------------------------------
// <copyright file="EpsImage.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// Original Matlab code by John Burkardt, Florida State University
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using TriangleNet;
    using TriangleNet.Geometry;

    using IntPoint = System.Drawing.Point;

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

            using (var eps = new FormattingStreamWriter(filename))
            {
                WriteHeader(filename, eps);

                DrawClip(eps);

                DrawEdges(eps, mesh);

                DrawSegments(eps, mesh);

                DrawPoints(eps, mesh);

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
            eps.WriteLine("%%LanguageLevel: 2");
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

        private void DrawEdges(StreamWriter eps, Mesh mesh)
        {
            IntPoint a, b;

            eps.WriteLine("%");
            eps.WriteLine("%  Draw the triangles (mesh edges).");
            eps.WriteLine("%");

            SetStroke(eps, 0.6f, 0.6f, 0.6f, 0.4f);

            eps.WriteLine(@"/L {
2 dict begin
/y2 exch def
/x2 exch def
/y1 exch def
/x1 exch def
gsave
newpath x1 y1 moveto x2 y2 lineto stroke
grestore
end
} def");

            foreach (var e in EnumerateEdges(mesh))
            {
                a = Transform(e.GetVertex(0));
                b = Transform(e.GetVertex(1));

                eps.WriteLine("{0} {1} {2} {3} L", a.X, a.Y, b.X, b.Y);
            }
        }

        private void DrawSegments(StreamWriter eps, Mesh mesh)
        {
            IntPoint a, b;

            eps.WriteLine("%");
            eps.WriteLine("%  Draw the segments.");
            eps.WriteLine("%");

            SetStroke(eps, 0.27f, 0.5f, 0.7f, 0.8f);

            foreach (var s in mesh.Segments)
            {
                a = Transform(s.GetVertex(0));
                b = Transform(s.GetVertex(1));

                eps.WriteLine("{0} {1} {2} {3} L", a.X, a.Y, b.X, b.Y);
            }
        }

        private void DrawPoints(StreamWriter eps, Mesh mesh)
        {
            IntPoint p;

            int n = mesh.Vertices.Count;

            // Size of the points.
            int size = (n < 100) ? 3 : ((n < 500) ? 2 : 1);

            eps.WriteLine("%");
            eps.WriteLine("%  Draw the vertices.");
            eps.WriteLine("%");

            SetColor(eps, 0.0f, 0.4f, 0.0f);

            eps.WriteLine(@"/P {
2 dict begin
/y exch def
/x exch def
gsave
newpath x y 1 0 360 arc fill
grestore
end
} def");

            // TODO: EPS point size.
            //       newpath x y {size} 0 360 arc fill

            foreach (var node in mesh.Vertices)
            {
                p = Transform(node);

                eps.WriteLine("{0} {1} P", p.X, p.Y);
            }
        }

        /*
        private void DrawPointLabels(StreamWriter eps, Mesh mesh)
        {
            int n = mesh.Vertices.Count;

            IntPoint p;

            StringBuilder labels = new StringBuilder();

            foreach (var node in mesh.Vertices)
            {
                p = Transform(node);

                labels.AppendFormat("  {0}  {1}  moveto ({2}) show", p.X, p.Y + 5, node.ID);
                labels.AppendLine();
            }

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

            eps.WriteLine(labels.ToString());
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

            IntPoint a, b, c;

            foreach (var t in mesh.Triangles)
            {
                a = Transform(t.GetVertex(0));
                b = Transform(t.GetVertex(1));
                c = Transform(t.GetVertex(2));

                eps.WriteLine("newpath");

                eps.WriteLine("  {0}  {1}  moveto", a.X, a.Y);
                eps.WriteLine("  {0}  {1}  lineto", b.X, b.Y);
                eps.WriteLine("  {0}  {1}  lineto", c.X, c.Y);
                eps.WriteLine("  {0}  {1}  lineto", a.X, a.Y);

                eps.WriteLine("stroke");
            }
        }

        private void DrawTriangleLabels(StreamWriter eps, Mesh mesh)
        {
            var labels = new StringBuilder();

            IntPoint a, b, c;

            foreach (var t in mesh.Triangles)
            {
                a = Transform(t.GetVertex(0));
                b = Transform(t.GetVertex(1));
                c = Transform(t.GetVertex(2));

                eps.WriteLine("newpath");

                a = Transform((a.X + b.X + c.X) / 3.0, (a.Y + b.Y + c.Y) / 3.0);

                labels.AppendFormat("  {0}  {1}  moveto ({2}) show", a.X, a.Y, t.ID);
                labels.AppendLine();

                eps.WriteLine("stroke");
            }

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

            eps.WriteLine(labels.ToString());
        }
        //*/

        private void SetColor(StreamWriter eps, float r, float g, float b)
        {
            eps.WriteLine("{0} {1} {2} setrgbcolor", r, g, b);
            eps.WriteLine("%");
        }

        private void SetStroke(StreamWriter eps, float r, float g, float b, float width)
        {
            eps.WriteLine("{0} {1} {2} setrgbcolor", r, g, b);
            eps.WriteLine("{0} setlinewidth", width);
            eps.WriteLine("%");
        }

        private IntPoint Transform(Point p)
        {
            return Transform(p.X, p.Y);
        }

        private IntPoint Transform(double x, double y)
        {
            return new IntPoint(
                (int)Math.Floor(((x_max - x) * x_ps_min + (x - x_min) * x_ps_max) / (x_max - x_min)),
                (int)Math.Floor(((y_max - y) * y_ps_min + (y - y_min) * y_ps_max) / (y_max - y_min))
            );
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

        public IEnumerable<ISegment> EnumerateEdges(Mesh mesh, bool segments = false)
        {
            foreach (var t in mesh.Triangles)
            {
                for (int i = 0; i < 3; i++)
                {
                    int nid = t.GetNeighborID(i);

                    if ((t.ID < nid) || (nid < 0))
                    {
                        if (segments || t.GetSegment(i) == null)
                        {
                            yield return new Segment(
                                t.GetVertex((i + 1) % 3),
                                t.GetVertex((i + 2) % 3));
                        }
                    }
                }
            }
        }
    }
}
