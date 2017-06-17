
namespace TriangleNet.Rendering.Buffer
{
    public class VertexBuffer : BufferBase<float>
    {
        public VertexBuffer(int capacity, int size = 2)
            : base(capacity, size)
        {
        }

        public VertexBuffer(float[] data, int size = 2)
            : base(data, size)
        {
        }

        /// <summary>
        /// Gets the number of coordinates of one vertex in the buffer (i.e. 2 for
        /// 2D points or 3D points).
        /// </summary>
        public override int Size
        {
            get { return size; }
        }

        public override BufferTarget Target
        {
            get { return BufferTarget.VertexBuffer; }
        }
    }
}
