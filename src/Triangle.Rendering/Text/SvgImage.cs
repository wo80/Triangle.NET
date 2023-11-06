// -----------------------------------------------------------------------
// <copyright file="SvgImage.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Rendering.Text
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using TriangleNet.Geometry;
    using TriangleNet.Meshing;
    using TriangleNet.Meshing.Iterators;

    /// <summary>
    /// Writes a mesh to an SVG file.
    /// </summary>
    public class SvgImage
    {
        // Iterations to insert a line break in SVG path.
        private const int LINEBREAK_COUNT = 10;

        float scale = 1f;

        /// <summary>
        /// Exports a mesh to SVG format.
        /// </summary>
        /// <param name="mesh">The mesh.</param>
        /// <param name="file">The SVG filename.</param>
        /// <param name="width">The desired width (pixel) of the image.</param>
        /// <param name="regions">Enable rendering of regions.</param>
        /// <param name="points">Enable rendering of points.</param>
        public static void Save(IMesh mesh, string file = null, int width = 800,
            bool regions = false, bool points = true)
        {
            new SvgImage().Export(mesh, file, width, regions, points);
        }

        /// <summary>
        /// Export a mesh to SVG format.
        /// </summary>
        /// <param name="mesh">The current mesh.</param>
        /// <param name="filename">The SVG filename.</param>
        /// <param name="width">The desired width of the image.</param>
        /// <param name="regions">Enable rendering of regions.</param>
        /// <param name="points">Enable rendering of points.</param>
        public void Export(IMesh mesh, string filename, int width,
            bool regions = false, bool points = true)
        {
            // Check file name
            if (string.IsNullOrWhiteSpace(filename))
            {
                filename = string.Format("mesh-{0}.svg", DateTime.Now.ToString("yyyy-M-d-hh-mm-ss"));
            }

            if (!filename.EndsWith(".svg"))
            {
                filename = Path.ChangeExtension(filename, ".svg");
            }

            if (width < 200)
            {
                width = 200;
            }

            var bounds = mesh.Bounds;

            float margin = 0.05f * (float)bounds.Width;

            scale = width / ((float)bounds.Width + 2 * margin);

            int x_offset = -(int)((bounds.Left - margin) * scale - 0.5);
            int y_offset = (int)((bounds.Top + margin) * scale + 0.5);

            int height = (int)((bounds.Height + 2 * margin) * scale + 0.5);

            using (var svg = new FormattingStreamWriter(filename))
            {
                svg.WriteLine("<svg version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\"");
                svg.WriteLine("\twidth=\"{0}px\" height=\"{1}px\"", width, height);
                svg.WriteLine("\tviewBox=\"0 0 {0} {1}\">", width, height);

                svg.WriteLine("<g transform=\"translate({0}, {1}) scale(1,-1)\">", x_offset, y_offset);

                DrawTriangles(svg, mesh, regions, false);
                //DrawEdges(svg, mesh);

                DrawSegments(svg, mesh);

                if (points)
                {
                    DrawPoints(svg, mesh, false);
                }

                svg.WriteLine("</g>");

                svg.WriteLine("</svg>");
            }
        }

        private void DrawTriangles(StreamWriter svg, IMesh mesh, bool regions, bool label)
        {
            var labels = new StringBuilder();
            var edges = new StringBuilder();

            var filled = new Dictionary<uint, StringBuilder>();

            Vertex v1, v2, v3;
            double x1, y1, x2, y2, x3, y3, xa, ya;

            int i = 1;

            edges.Append("\t<path d=\"");

            var format = svg.FormatProvider;

            foreach (var tri in mesh.Triangles)
            {
                v1 = tri.GetVertex(0);
                v2 = tri.GetVertex(1);
                v3 = tri.GetVertex(2);

                x1 = scale * v1.X;
                y1 = scale * v1.Y;
                x2 = scale * v2.X;
                y2 = scale * v2.Y;
                x3 = scale * v3.X;
                y3 = scale * v3.Y;

                var s = string.Format(format, "M {0:0.#},{1:0.#} L {2:0.#},{3:0.#} {4:0.#},{5:0.#} Z ",
                    x1, y1, x2, y2, x3, y3);

                if (i % LINEBREAK_COUNT == 0)
                {
                    s += Environment.NewLine + "\t";
                }

                edges.Append(s);

                i++;

                if (regions && tri.Label != 0)
                {
                    if (!filled.TryGetValue((uint)tri.Label, out var sb))
                    {
                        sb = new StringBuilder();
                        filled.Add((uint)tri.Label, sb);
                    }
                    sb.Append(s);
                }

                if (label)
                {
                    xa = (x1 + x2 + x3) / 3.0;
                    ya = (y1 + y2 + y3) / 3.0;

                    labels.AppendFormat(format, "<text x=\"{0:0.#}\" y=\"{1:0.#}\">{2}</text>",
                        xa, ya, tri.ID);
                    labels.AppendLine();
                }
            }

            edges.AppendLine("\" style=\"stroke:#c2c2c2; fill:none; stroke-linejoin:bevel;\"/>");

            if (regions)
            {
                var colors = new ColorManager().CreateColorDictionary(filled.Keys);

                foreach (var r in filled)
                {
                    var c = colors[r.Key];
                    svg.Write("\t<path d=\"");
                    svg.Write(r.Value.ToString());
                    svg.WriteLine("\" fill=\"rgb({0},{1},{2})\" fill-opacity=\"{3:0.##}\"/>", c.R, c.G, c.B, c.A / 255f);
                }
            }

            svg.Write(edges.ToString());

            //  Label the triangles.
            if (label)
            {
                svg.WriteLine("\t<g font-family=\"Verdana\" font-size=\"11\" fill=\"black\">");
                svg.Write(labels.ToString());
                svg.WriteLine("\t<g/>");
            }
        }

        private void DrawEdges(StreamWriter svg, IMesh mesh)
        {
            svg.Write("\t<path d=\"");

            Vertex v1, v2;
            double x1, y1, x2, y2;

            int i = 1;

            foreach (var e in EdgeIterator.EnumerateEdges(mesh))
            {
                v1 = e.GetVertex(0);
                v2 = e.GetVertex(1);

                x1 = scale * v1.X;
                y1 = scale * v1.Y;
                x2 = scale * v2.X;
                y2 = scale * v2.Y;

                svg.Write("M {0:0.#},{1:0.#} L {2:0.#},{3:0.#} ",
                    x1, y1, x2, y2);

                if (i % LINEBREAK_COUNT == 0)
                {
                    svg.WriteLine();
                    svg.Write("\t");
                }

                i++;
            }

            svg.WriteLine("\" style=\"stroke:#c2c2c2; fill:none; stroke-linejoin:bevel;\"/>");
        }

        private void DrawSegments(StreamWriter svg, IMesh mesh)
        {
            svg.Write("\t<path d=\"");

            double x1, y1, x2, y2;

            int i = 1;

            foreach (var seg in mesh.Segments)
            {
                x1 = scale * seg.GetVertex(0).X;
                y1 = scale * seg.GetVertex(0).Y;
                x2 = scale * seg.GetVertex(1).X;
                y2 = scale * seg.GetVertex(1).Y;

                svg.Write("M {0:0.#},{1:0.#} L {2:0.#},{3:0.#} ",
                    x1, y1, x2, y2);

                if (i % LINEBREAK_COUNT == 0)
                {
                    svg.WriteLine();
                    svg.Write("\t");
                }

                i++;
            }

            svg.WriteLine("\" style=\"stroke:#4682B4; fill:none; stroke-linejoin:bevel; stroke-width:2px;\"/>");
        }

        private void DrawPoints(StreamWriter svg, IMesh mesh, bool label)
        {
            var format = svg.FormatProvider;

            int n = mesh.Vertices.Count;

            float circle_size = 1.5f;

            if (n < 100)
            {
                circle_size = 3;
            }
            else if (n < 500)
            {
                circle_size = 2;
            }

            svg.WriteLine("\t<g style=\"fill: #006400\">");

            double x, y;

            var labels = new StringBuilder();

            foreach (var node in mesh.Vertices)
            {
                x = scale * node.X;
                y = scale * node.Y;

                svg.WriteLine("\t\t<circle cx=\"{0:0.#}\" cy=\"{1:0.#}\" r=\"{2:0.#}\" />",
                    x, y, circle_size);

                if (label)
                {
                    labels.AppendFormat(format, "<text x=\"{0:0.#}\" y=\"{1:0.#}\">{2}</text>",
                        x, y, node.ID);
                    labels.AppendLine();
                }
            }

            svg.WriteLine("\t</g>");

            //  Label the nodes.
            if (label)
            {
                svg.WriteLine("\t<g font-family=\"Verdana\" font-size=\"11\" fill=\"black\">");
                svg.Write(labels.ToString());
                svg.WriteLine("\t</g>");
            }
        }
    }
}
