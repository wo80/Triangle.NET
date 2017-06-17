
namespace TriangleNet.Rendering.GDI
{
    using System;
    using System.Drawing;
    using TriangleNet.Rendering.GDI.Native;

    public class FunctionRenderer
    {
        TriVertex[] points;
        GradientTriangle[] elements;

        public Graphics RenderTarget { get; set; }

        public IRenderContext Context { get; set; }

        public void Render(IRenderLayer layer)
        {
            Create(layer);

            var hdc = RenderTarget.GetHdc();

            NativeMethods.GradientFill(hdc,
                points, (uint)points.Length, elements, (uint)elements.Length,
                GradientFillMode.GRADIENT_FILL_TRIANGLE);

            RenderTarget.ReleaseHdc(hdc);
        }

        private void Create(IRenderLayer layer)
        {
            var zoom = Context.Zoom;
            var colors = layer.Colors.Data;

            int length = colors.Length;

            int size = layer.Points.Size;
            var data = layer.Points.Data;

            if (length != data.Length / size)
            {
                throw new Exception();
            }

            this.points = new TriVertex[length];

            TriVertex vertex;
            Color color;
            PointF p = new PointF((float)data[0], (float)data[1]);

            zoom.WorldToScreen(ref p);

            // Get correction distance
            float dx = (p.X - (int)p.X) * 2.0f;
            float dy = (p.Y - (int)p.Y) * 2.0f;

            // Create vertices.
            for (int i = 0; i < length; i++)
            {
                p.X = (float)data[size * i];
                p.Y = (float)data[size * i + 1];

                zoom.WorldToScreen(ref p);

                color = colors[i];

                vertex = new TriVertex();

                vertex.x = (int)(p.X + dx);
                vertex.y = (int)(p.Y + dy);

                vertex.Red = (ushort)(color.R << 8);
                vertex.Green = (ushort)(color.G << 8);
                vertex.Blue = (ushort)(color.B << 8);
                vertex.Alpha = (ushort)(color.A << 8);

                this.points[i] = vertex;
            }

            var triangles = layer.Indices.Data;

            length = triangles.Length / 3;

            this.elements = new GradientTriangle[length];

            GradientTriangle e;

            // Create triangles.
            for (int i = 0; i < length; i++)
            {
                e = new GradientTriangle();

                e.Vertex1 = (uint)triangles[3 * i];
                e.Vertex2 = (uint)triangles[3 * i + 1];
                e.Vertex3 = (uint)triangles[3 * i + 2];

                this.elements[i] = e;
            }
        }
    }
}
