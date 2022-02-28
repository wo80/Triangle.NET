
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
            get { return Right - Left; }
        }

        public float Height
        {
            get { return Top - Bottom; }
        }

        public BoundingBox()
        {
            Reset();
        }

        public BoundingBox(float left, float right, float bottom, float top)
        {
            Left = left;
            Right = right;
            Bottom = bottom;
            Top = top;
        }

        public void Update(Point pt)
        {
            Update(pt.X, pt.Y);
        }

        public void Update(PointF pt)
        {
            Update(pt.X, pt.Y);
        }

        public void Update(double x, double y)
        {
            Update((float)x, (float)y);
        }

        public void Update(float x, float y)
        {
            // Update bounding box
            if (Left > x) Left = x;
            if (Right < x) Right = x;
            if (Bottom > y) Bottom = y;
            if (Top < y) Top = y;
        }

        public void Reset()
        {
            Left = float.MaxValue;
            Right = -float.MaxValue;
            Bottom = float.MaxValue;
            Top = -float.MaxValue;
        }
    }
}
