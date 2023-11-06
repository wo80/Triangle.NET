
using System;
using System.Collections.Generic;
using TriangleNet.Geometry;

namespace TriangleNet.Rendering.Buffer
{
    public class VertexBuffer : BufferBase<float>
    {
        #region Static methods

        /// <summary>
        /// Create a vertex buffer from given point collection.
        /// </summary>
        /// <param name="points">The points to render.</param>
        /// <returns>Returns the vertex buffer.</returns>
        public static IBuffer<float> Create(ICollection<Point> points)
        {
            return Create(points, new Rectangle(0d, 0d, 1d, 1d));
        }

        /// <summary>
        /// Create a normalized vertex buffer from given point collection.
        /// </summary>
        /// <param name="points">The points to render.</param>
        /// <param name="bounds">The bounding box used for normalization.</param>
        /// <returns>Returns a buffer of normalized coordinates.</returns>
        public static IBuffer<float> Create(ICollection<Point> points, Rectangle bounds)
        {
            var buffer = new VertexBuffer(2 * points.Count);

            var data = buffer.Data;

            double dx = bounds.X;
            double dy = bounds.Y;

            double scale = 1.0 / Math.Max(bounds.Width, bounds.Height);

            int i = 0;

            double x, y;

            foreach (var p in points)
            {
                x = (p.X - dx) * scale;
                y = (p.Y - dy) * scale;

                data[2 * i] = (float)x;
                data[2 * i + 1] = (float)y;

                i++;
            }

            return buffer;
        }

        /// <summary>
        /// Create a vertex buffer from given point collection.
        /// </summary>
        /// <param name="points">The points to render.</param>
        /// <param name="size">The size of one element in the buffer (i.e. 2 for 2D points)</param>
        /// <returns>Returns the vertex buffer.</returns>
        public static IBuffer<float> Create(ICollection<Vertex> points, int size = 2)
        {
            return Create(points, new Rectangle(0d, 0d, 1d, 1d), size);
        }

        /// <summary>
        /// Create a normalized vertex buffer from given vertex collection.
        /// </summary>
        /// <param name="points">The vertices to render.</param>
        /// <param name="bounds">The bounding box used for normalization.</param>
        /// <param name="size">The size of one element in the buffer (i.e. 2 for 2D points)</param>
        /// <returns>Returns a buffer of normalized coordinates.</returns>
        public static IBuffer<float> Create(ICollection<Vertex> points, Rectangle bounds, int size = 2)
        {
            var buffer = new VertexBuffer(size * points.Count);

            var data = buffer.Data;

            double dx = bounds.X;
            double dy = bounds.Y;

            double scale = 1.0 / Math.Max(bounds.Width, bounds.Height);

            int i = 0;

            double x, y;

            foreach (var p in points)
            {
                x = (p.X - dx) * scale;
                y = (p.Y - dy) * scale;

                data[size * i] = (float)x;
                data[size * i + 1] = (float)y;

                i++;
            }

            return buffer;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexBuffer"/> class.
        /// </summary>
        /// <param name="capacity">The buffer capacity.</param>
        /// <param name="size">The size of one element in the buffer (i.e. 2 for 2D points)</param>
        public VertexBuffer(int capacity, int size = 2)
            : base(capacity, size)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexBuffer"/> class.
        /// </summary>
        /// <param name="data">The buffer data.</param>
        /// <param name="size">The size of one element in the buffer (i.e. 2 for 2D points)</param>
        public VertexBuffer(float[] data, int size = 2)
            : base(data, size)
        {
        }

        /// <inheritdoc/>
        public override int Size => size;

        /// <inheritdoc/>
        public override BufferTarget Target => BufferTarget.VertexBuffer;
    }
}
