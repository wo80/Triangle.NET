
namespace TriangleNet.Rendering.Buffer
{
    public abstract class BufferBase<T> : IBuffer<T> where T : struct
    {
        protected T[] data;
        protected int size;

        public BufferBase(int capacity, int size)
            : this(new T[capacity], size)
        {
        }

        public BufferBase(T[] data, int size)
        {
            this.data = data;
            this.size = size;
        }

        /// <inheritdoc/>
        public T[] Data => data;

        /// <inheritdoc/>
        public int Count => data == null ? 0 : data.Length;

        /// <inheritdoc/>
        public abstract int Size { get; }

        /// <inheritdoc/>
        public abstract BufferTarget Target { get; }
    }
}
