using NUnit.Framework;
using TriangleNet.Geometry;
using TriangleNet.Tools;

namespace TriangleNet.Tests.Tools
{
    public class IntersectionHelperTest
    {
        [Test]
        public void TestIsPointOnSegment()
        {
            var a = new Vertex(1.0, 1.0);
            var b = new Vertex(2.0, 2.0);

            // Test point = segment start point.
            Assert.IsTrue(IntersectionHelper.IsPointOnSegment(a, b, new Vertex(1.0, 1.0)));

            // Test point = segment end point.
            Assert.IsTrue(IntersectionHelper.IsPointOnSegment(a, b, new Vertex(2.0, 2.0)));

            // Test point on segment.
            Assert.IsTrue(IntersectionHelper.IsPointOnSegment(a, b, new Vertex(1.5, 1.5)));

            // Test point collinear, but not on segment.
            Assert.IsFalse(IntersectionHelper.IsPointOnSegment(a, b, new Vertex(0.0, 0.0)));
            Assert.IsFalse(IntersectionHelper.IsPointOnSegment(a, b, new Vertex(3.0, 3.0)));

            // Test point not on segment.
            Assert.IsFalse(IntersectionHelper.IsPointOnSegment(a, b, new Vertex(1.5, 0.5)));

            double eps = 1e-12;

            // Test point collinear near endpoint, but not on segment.
            Assert.IsFalse(IntersectionHelper.IsPointOnSegment(a, b, new Vertex(2.0 + eps, 2.0 + eps)));
            Assert.IsFalse(IntersectionHelper.IsPointOnSegment(a, b, new Vertex(2.0 - eps, 2.0 + eps)));

            // Test point collinear near endpoint on segment.
            Assert.IsTrue(IntersectionHelper.IsPointOnSegment(a, b, new Vertex(2.0 - eps, 2.0 - eps)));
        }
    }
}
