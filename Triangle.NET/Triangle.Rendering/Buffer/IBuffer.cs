
namespace TriangleNet.Rendering.Buffer
{
    public enum BufferTarget : byte
    {
        ColorBuffer,
        IndexBuffer,
        VertexBuffer
    }

    public interface IBuffer<T> where T : struct
    {
        /// <summary>
        /// Gets the contents of the buffer.
        /// </summary>
        T[] Data { get; }

        /// <summary>
        /// Gets the size of the buffer.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the size of one element in the buffer (i.e. 2 for 2D points
        /// or 3 for triangles).
        /// </summary>
        int Size { get; }

        /// <summary>
        /// Gets the buffer target (vertices or indices).
        /// </summary>
        BufferTarget Target { get; }
    }
}
