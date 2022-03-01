
namespace TriangleNet.Rendering.Buffer
{
    using System.Collections.Generic;
    using TriangleNet.Geometry;

    public class VertexBuffer : BufferBase<float>
    {
        #region Static methods

        /// <summary>
        /// Create a vertex buffer from given point collection.
        /// </summary>
        /// <param name="points">The points to render.</param>
        /// <param name="bounds">The points bounding box.</param>
        /// <returns></returns>
        public static IBuffer<float> Create(ICollection<Point> points, Rectangle bounds)
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

        /// <summary>
        /// Create a vertex buffer from given vertex collection.
        /// </summary>
        /// <param name="points">The vertices to render.</param>
        /// <param name="bounds">The vertices bounding box.</param>
        /// <returns></returns>
        public static IBuffer<float> Create(ICollection<Vertex> points, Rectangle bounds)
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
