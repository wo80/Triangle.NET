
namespace TriangleNet.Rendering.Buffer
{
    using System;
    using System.Drawing;

    public class ColorBuffer : BufferBase<Color>
    {
        public ColorBuffer(int capacity, int size)
            : base(capacity, size)
        {
        }

        public ColorBuffer(Color[] data, int size)
            : base(data, size)
        {
        }

        public override int Size
        {
            get { return 1; }
        }

        public override BufferTarget Target
        {
            get { return BufferTarget.ColorBuffer; }
        }
    }
}
