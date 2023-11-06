
using System.Collections.Generic;
using System.Linq;
using TriangleNet.Geometry;

namespace TriangleNet.Rendering.Buffer
{
    public class IndexBuffer : BufferBase<uint>
    {
        #region Static methods

        public static IBuffer<uint> Create(IEnumerable<IEdge> edges, int size)
        {
            var buffer = new IndexBuffer(size * edges.Count(), size);

            var data = buffer.Data;

            int i = 0;

            foreach (var e in edges)
            {
                data[size * i + 0] = (uint)e.P0;
                data[size * i + 1] = (uint)e.P1;

                i++;
            }

            return buffer;
        }

        public static IBuffer<uint> Create(IEnumerable<ITriangle> elements, int size)
        {
            var buffer = new IndexBuffer(size * elements.Count(), size);

            var data = buffer.Data;

            int i = 0;

            foreach (var e in elements)
            {
                data[size * i + 0] = (uint)e.GetVertexID(0);
                data[size * i + 1] = (uint)e.GetVertexID(1);
                data[size * i + 2] = (uint)e.GetVertexID(2);

                i++;
            }

            return buffer;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexBuffer"/> class.
        /// </summary>
        /// <param name="capacity">The buffer capacity.</param>
        /// <param name="size">The size of one element in the buffer (i.e. 2 for 2D points)</param>
        public IndexBuffer(int capacity, int size)
            : base(capacity, size)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexBuffer"/> class.
        /// </summary>
        /// <param name="data">The buffer data.</param>
        /// <param name="size">The size of one element in the buffer (i.e. 2 for 2D points)</param>
        public IndexBuffer(uint[] data, int size)
            : base(data, size)
        {
        }

        /// <inheritdoc/>
        public override int Size => size;

        /// <inheritdoc/>
        public override BufferTarget Target => BufferTarget.IndexBuffer;
    }
}
