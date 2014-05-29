
namespace MeshRenderer.Core
{
    using System.Drawing;
    using Rectangle = TriangleNet.Geometry.Rectangle;

    public struct BoundingBox
    {
        public float Left;
        public float Right;
        public float Bottom;
        public float Top;

        public float Width
        {
            get { return this.Right - this.Left; }
        }

        public float Height
        {
            get { return this.Top - this.Bottom; }
        }

        public BoundingBox(float left, float right, float bottom, float top)
        {
            this.Left = left;
            this.Right = right;
            this.Bottom = bottom;
            this.Top = top;
        }

        public BoundingBox(Rectangle rectangle)
        {
            this.Left = (float)rectangle.Left;
            this.Right = (float)rectangle.Right;
            this.Bottom = (float)rectangle.Bottom;
            this.Top = (float)rectangle.Top;
        }

        public void Update(Point pt)
        {
            this.Update(pt.X, pt.Y);
        }

        public void Update(PointF pt)
        {
            this.Update(pt.X, pt.Y);
        }

        public void Update(double x, double y)
        {
            // Update bounding box
            if (this.Left > x) this.Left = (float)x;
            if (this.Right < x) this.Right = (float)x;
            if (this.Bottom > y) this.Bottom = (float)y;
            if (this.Top < y) this.Top = (float)y;
        }

        public void Reset()
        {
            this.Left = float.MaxValue;
            this.Right = -float.MaxValue;
            this.Bottom = float.MaxValue;
            this.Top = -float.MaxValue;
        }
    }
}
