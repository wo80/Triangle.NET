// -----------------------------------------------------------------------
// <copyright file="SvgImage.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.IO
{
    using System;
    using System.IO;
    using System.Text;
    using TriangleNet;
    using TriangleNet.Data;

    /// <summary>
    /// Writes a mesh to an SVG file.
    /// </summary>
    public class SvgImage
    {
        float scale = 1f;

        /// <summary>
        /// Export the mesh to SVG format.
        /// </summary>
        /// <param name="mesh">The current mesh.</param>
        /// <param name="filename">The SVG filename.</param>
        /// <param name="width">The desired width of the image.</param>
        public void Export(Mesh mesh, string filename, int width)
        {
            // Check file name
            if (String.IsNullOrWhiteSpace(filename))
            {
                filename = String.Format("mesh-{0}.svg", DateTime.Now.ToString("yyyy-M-d-hh-mm-ss"));
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

            int x_offset = -(int)((bounds.MinX - margin) * scale);
            int y_offset = (int)((bounds.MaxY + margin) * scale);

            int height = (int)((bounds.Height + 2 * margin) * scale);

            using (StreamWriter svg = new StreamWriter(filename))
            {
                svg.WriteLine("<svg version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\"");
                svg.WriteLine("    width=\"{0}px\" height=\"{1}px\"", width, height);
                svg.WriteLine("    viewBox=\"0 0 {0} {1}\">", width, height);

                svg.WriteLine("<g transform=\"translate({0}, {1}) scale(1,-1)\">", x_offset, y_offset);

                DrawTriangles(svg, mesh, false);

                DrawSegments(svg, mesh);

                DrawPoints(svg, mesh, false);

                svg.WriteLine("</g>");

                svg.WriteLine("</svg>");
            }
        }

        private void DrawTriangles(StreamWriter svg, Mesh mesh, bool label)
        {
            svg.Write("    <path d=\"");

            StringBuilder labels = new StringBuilder();

            Vertex v1, v2, v3;
            double x1, y1, x2, y2, x3, y3, xa, ya;

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

                svg.Write("M {0},{1} L {2},{3} {4},{5} Z ",
                    x1.ToString("0.0", Util.Nfi), y1.ToString("0.0", Util.Nfi),
                    x2.ToString("0.0", Util.Nfi), y2.ToString("0.0", Util.Nfi),
                    x3.ToString("0.0", Util.Nfi), y3.ToString("0.0", Util.Nfi));

                if (label)
                {
                    xa = (x1 + x2 + x3) / 3.0;
                    ya = (y1 + y2 + y3) / 3.0;

                    labels.AppendFormat("<text x=\"{0}\" y=\"{1}\">{2}</text>",
                        xa.ToString("0.0", Util.Nfi), ya.ToString("0.0", Util.Nfi), tri.ID);
                    labels.AppendLine();
                }

            }

            svg.WriteLine("\" style=\"stroke:#969696; fill:none; stroke-linejoin:bevel;\"/>");

            //  Label the triangles.
            if (label)
            {
                svg.WriteLine("    <g font-family=\"Verdana\" font-size=\"11\" fill=\"black\">");
                svg.Write(labels.ToString());
                svg.WriteLine("    <g/>");
            }
        }

        private void DrawSegments(StreamWriter svg, Mesh mesh)
        {
            svg.Write("    <path d=\"");

            StringBuilder labels = new StringBuilder();

            double x1, y1, x2, y2;

            foreach (var seg in mesh.Segments)
            {
                x1 = scale * seg.GetVertex(0).X;
                y1 = scale * seg.GetVertex(0).Y;
                x2 = scale * seg.GetVertex(1).X;
                y2 = scale * seg.GetVertex(1).Y;

                svg.Write("M {0},{1} L {2},{3} ",
                    x1.ToString("0.0", Util.Nfi), y1.ToString("0.0", Util.Nfi),
                    x2.ToString("0.0", Util.Nfi), y2.ToString("0.0", Util.Nfi));
            }

            svg.WriteLine("\" style=\"stroke:#4682B4; fill:none; stroke-linejoin:bevel; stroke-width:2px;\"/>");
        }

        private void DrawPoints(StreamWriter svg, Mesh mesh, bool label)
        {
            int n = mesh.Vertices.Count;

            int circle_size = 1;

            if (n < 100)
            {
                circle_size = 4;
            }
            else if (n < 500)
            {
                circle_size = 3;
            }
            else if (n < 1000)
            {
                circle_size = 2;
            }

            svg.WriteLine("    <g style=\"fill: #006400\">");

            double x, y;

            StringBuilder labels = new StringBuilder();

            foreach (var node in mesh.Vertices)
            {
                x = scale * node.X;
                y = scale * node.Y;

                svg.WriteLine("        <circle cx=\"{0}\" cy=\"{1}\" r=\"{2}\" />",
                    x.ToString("0.0", Util.Nfi), y.ToString("0.0", Util.Nfi), circle_size);

                if (label)
                {
                    labels.AppendFormat("<text x=\"{0}\" y=\"{1}\">{2}</text>",
                        x.ToString("0.0", Util.Nfi), y.ToString("0.0", Util.Nfi), node.ID);
                    labels.AppendLine();
                }
            }

            svg.WriteLine("    </g>");

            //  Label the nodes.
            if (label)
            {
                svg.WriteLine("    <g font-family=\"Verdana\" font-size=\"11\" fill=\"black\">");
                svg.Write(labels.ToString());
                svg.WriteLine("    <g/>");
            }
        }
    }
}
