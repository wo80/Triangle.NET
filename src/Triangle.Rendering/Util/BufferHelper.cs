
namespace TriangleNet.Rendering.Util
{
    using System.Collections.Generic;
    using TriangleNet.Topology;
    using TriangleNet.Geometry;
    using TriangleNet.Rendering.Buffer;
    using System.Linq;

    internal static class BufferHelper
    {
        public static IBuffer<float> CreateVertexBuffer(ICollection<Point> points)
        {
            var buffer = new VertexBuffer(2 * points.Count);

            var data = buffer.Data;

            float x, y;

            int i = 0;

            foreach (var p in points)
            {
                x = (float)p.X;
                y = (float)p.Y;

                data[2 * i] = x;
                data[2 * i + 1] = y;

                i++;
            }

            return buffer;
        }

        public static IBuffer<float> CreateVertexBuffer(ICollection<Vertex> points)
        {
            var buffer = new VertexBuffer(2 * points.Count);

            var data = buffer.Data;

            int i = 0;

            foreach (var p in points)
            {
                data[2 * i] = (float)p.X;
                data[2 * i + 1] = (float)p.Y;

                i++;
            }

            return buffer;
        }

        public static IBuffer<int> CreateIndexBuffer(IEnumerable<IEdge> edges, int size)
        {
            var buffer = new IndexBuffer(size * edges.Count(), size);

            var data = buffer.Data;

            int i = 0;

            foreach (var e in edges)
            {
                data[size * i + 0] = e.P0;
                data[size * i + 1] = e.P1;

                i++;
            }

            return buffer;
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

            return buffer;
        }
    }
}
