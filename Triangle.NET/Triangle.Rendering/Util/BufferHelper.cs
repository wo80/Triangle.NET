
namespace TriangleNet.Rendering.Util
{
    using System.Collections.Generic;
    using TriangleNet.Topology;
    using TriangleNet.Geometry;
    using TriangleNet.Rendering.Buffer;

    internal static class BufferHelper
    {
        public static IBuffer<float> CreateVertexBuffer(double[] points, ref BoundingBox bounds)
        {
            int length = points.Length;

            var buffer = new VertexBuffer(length);

            bounds.Reset();

            var data = buffer.Data;

            float x, y;

            length = length >> 1;

            for (int i = 0; i < length; i++)
            {
                x = (float)points[2 * i];
                y = (float)points[2 * i + 1];

                data[2 * i] = x;
                data[2 * i + 1] = y;

                bounds.Update(x, y);
            }

            return buffer as IBuffer<float>;
        }

        public static IBuffer<float> CreateVertexBuffer(ICollection<Point> points, ref BoundingBox bounds)
        {
            var buffer = new VertexBuffer(2 * points.Count);

            bounds.Reset();

            var data = buffer.Data;

            float x, y;

            int i = 0;

            foreach (var p in points)
            {
                x = (float)p.X;
                y = (float)p.Y;

                data[2 * i] = x;
                data[2 * i + 1] = y;

                bounds.Update(x, y);

                i++;
            }

            return buffer as IBuffer<float>;
        }

        public static IBuffer<float> CreateVertexBuffer(ICollection<Vertex> points, ref BoundingBox bounds)
        {
            var buffer = new VertexBuffer(2 * points.Count);

            bounds.Reset();

            var data = buffer.Data;

            int i = 0;

            foreach (var p in points)
            {
                data[2 * i] = (float)p.X;
                data[2 * i + 1] = (float)p.Y;

                bounds.Update(p.X, p.Y);

                i++;
            }

            return buffer as IBuffer<float>;
        }

        public static IBuffer<int> CreateIndexBuffer(IList<IEdge> segments, int size)
        {
            var buffer = new IndexBuffer(size * segments.Count, size);

            var data = buffer.Data;

            int i = 0;

            foreach (var e in segments)
            {
                data[size * i + 0] = e.P0;
                data[size * i + 1] = e.P1;

                i++;
            }

            return buffer as IBuffer<int>;
        }

        public static IBuffer<int> CreateIndexBuffer(IEnumerable<IEdge> edges, int size)
        {
            var data = new List<int>();

            foreach (var e in edges)
            {
                data.Add(e.P0);
                data.Add(e.P1);
            }

            return new IndexBuffer(data.ToArray(), size) as IBuffer<int>;
        }

        public static IBuffer<int> CreateIndexBuffer(ICollection<Triangle> elements, int size)
        {
            var buffer = new IndexBuffer(size * elements.Count, size);

            var data = buffer.Data;

            int i = 0;

            foreach (var e in elements)
            {
                data[size * i + 0] = e.GetVertexID(0);
                data[size * i + 1] = e.GetVertexID(1);
                data[size * i + 2] = e.GetVertexID(2);

                i++;
            }

            return buffer as IBuffer<int>;
        }
    }
}
