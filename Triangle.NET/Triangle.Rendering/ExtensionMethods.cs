
namespace TriangleNet.Rendering
{
    using System.Drawing;

    internal static class ExtensionMethods
    {
        /// <summary>
        /// Check if segment (a, b) intersects rectangle.
        /// </summary>
        public static bool Intersects(this RectangleF rect, PointF a, PointF b)
        {
            // TODO: implement intersection.
            return rect.Contains(a) || rect.Contains(b);
        }

        /// <summary>
        /// Check if triangle (a, b, c) intersects rectangle.
        /// </summary>
        public static bool Intersects(this RectangleF rect, PointF a, PointF b, PointF c)
        {
            // TODO: implement intersection.
            return rect.Contains(a) || rect.Contains(b) || rect.Contains(c);
        }
    }
}
