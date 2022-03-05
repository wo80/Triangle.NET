
namespace TriangleNet.Rendering.Buffer
{
    using System.Drawing;

    public class ColorBuffer : BufferBase<Color>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorBuffer"/> class.
        /// </summary>
        /// <param name="capacity">The buffer capacity.</param>
        /// <param name="size">The size of one element in the buffer (i.e. 2 for 2D points)</param>
        public ColorBuffer(int capacity, int size)
            : base(capacity, size)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorBuffer"/> class.
        /// </summary>
        /// <param name="data">The buffer data.</param>
        /// <param name="size">The size of one element in the buffer (i.e. 2 for 2D points)</param>
        public ColorBuffer(Color[] data, int size)
            : base(data, size)
        {
        }

        /// <inheritdoc/>
        public override int Size => 1;

        /// <inheritdoc/>
        public override BufferTarget Target => BufferTarget.ColorBuffer; 
    }
}
