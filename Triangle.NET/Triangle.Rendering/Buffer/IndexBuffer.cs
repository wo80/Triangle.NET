
namespace TriangleNet.Rendering.Buffer
{
    public class IndexBuffer : BufferBase<int>
    {
        public IndexBuffer(int capacity, int size)
            : base(capacity, size)
        {
        }

        public IndexBuffer(int[] data, int size)
            : base(data, size)
        {
        }

        /// <summary>
        /// Gets the number of indices for one element (i.e. 2 for segments
        /// or 3 for triangles).
        /// </summary>
        public override int Size
        {
            get { return size; }
        }

        public override BufferTarget Target
        {
            get { return BufferTarget.IndexBuffer; }
        }
    }
}
