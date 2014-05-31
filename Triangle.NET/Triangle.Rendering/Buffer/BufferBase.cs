
namespace TriangleNet.Rendering.Buffer
{
    public abstract class BufferBase<T> : IBuffer<T> where T : struct
    {
        protected T[] data;
        protected int size;
        
        public BufferBase(int capacity, int size)
        {
            this.data = new T[capacity];
            this.size = size;
        }

        public BufferBase(T[] data, int size)
        {
            this.data = data;
            this.size = size;
        }

        public T[] Data
        {
            get { return data; }
        }

        public int Count
        {
            get { return data == null ? 0 : data.Length; }
        }

        public abstract int Size
        {
            get;
        }

        public abstract BufferTarget Target
        {
            get;
        }
    }
}
