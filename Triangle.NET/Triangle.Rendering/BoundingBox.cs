
namespace TriangleNet.Rendering
{
    using System.Drawing;

    public class BoundingBox
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

        public BoundingBox()
        {
            Reset();
        }

        public BoundingBox(float left, float right, float bottom, float top)
        {
            this.Left = left;
            this.Right = right;
            this.Bottom = bottom;
            this.Top = top;
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
            Update((float)x, (float)y);
        }

        public void Update(float x, float y)
        {
            // Update bounding box
            if (this.Left > x) this.Left = x;
            if (this.Right < x) this.Right = x;
            if (this.Bottom > y) this.Bottom = y;
            if (this.Top < y) this.Top = y;
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
