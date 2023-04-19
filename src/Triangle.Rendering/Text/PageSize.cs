
namespace TriangleNet.Rendering.Text
{
    using System.Drawing;

    /// <summary>
    /// Page size in millimeters.
    /// </summary>
    public struct PageSize
    {
        private const float MM_PER_INCH = 2.54f;

        public static readonly PageSize A3 = new PageSize(297.0f, 420.0f);
        public static readonly PageSize A4 = new PageSize(210.0f, 297.0f);
        public static readonly PageSize A5 = new PageSize(148.0f, 210.0f);
        public static readonly PageSize LETTER = new PageSize(8.5f * MM_PER_INCH, 11.0f * MM_PER_INCH);
        public static readonly PageSize LEGAL = new PageSize(8.5f * MM_PER_INCH, 14.0f * MM_PER_INCH);

        public float X { get; private set; }

        public float Y { get; private set; }

        public float Width => Right - X;

        public float Height => Bottom - Y;

        public float Right { get; private set; }

        public float Bottom { get; private set; }

        public PageSize(float left, float top, float right, float bottom)
        {
            this.X = left;
            this.Y = top;
            this.Right = right;
            this.Bottom = bottom;
        }

        public PageSize(float width, float height)
            : this(0.0f, 0.0f, width, height)
        {
        }

        public PageSize(Rectangle size)
            : this(size.Left, size.Right, size.Top, size.Bottom)
        {
        }

        public void Expand(float dx, float dy)
        {
            X -= dx;
            Y -= dy;

            Right += dx;
            Bottom += dy;
        }
    }
}
