
namespace TriangleNet.Rendering.Buffer
{
    public class VertexBuffer : BufferBase<float>
    {
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
