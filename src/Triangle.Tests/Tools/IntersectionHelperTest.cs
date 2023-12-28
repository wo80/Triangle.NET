using NUnit.Framework;
using System;
using TriangleNet.Geometry;
using TriangleNet.Tools;

namespace TriangleNet.Tests.Tools
{
    public class IntersectionHelperTest
    {
        private const double EPS = 1e-8;

        Rectangle box;
        Point p;

        [SetUp]
        public void Initialize()
        {
            // Square centered at origin.
            box = new Rectangle(-1.0, -1.0, 2.0, 2.0);

            // Starting point of ray for box-ray intersection.
            p = new Point(0.0, 0.0);
        }

        [Test]
        public void TestBoxRayIntersectionCorners()
        {
            var c = new Point(0.0, 0.0);

            var check = (Point c, double x, double y) => Math.Abs(c.X - x) < EPS && Math.Abs(c.Y - y) < EPS;

            IntersectionHelper.BoxRayIntersection(box, p, -0.2, -0.2, ref c);
            Assert.That(check(c, -1.0, -1.0), Is.True);
            IntersectionHelper.BoxRayIntersection(box, p, 0.2, -0.2, ref c);
            Assert.That(check(c, 1.0, -1.0), Is.True);
            IntersectionHelper.BoxRayIntersection(box, p, 0.2, 0.2, ref c);
            Assert.That(check(c, 1.0, 1.0), Is.True);
            IntersectionHelper.BoxRayIntersection(box, p, -0.2, 0.2, ref c);
            Assert.That(check(c, -1.0, 1.0), Is.True);
        }

        [Test]
        public void TestBoxRayIntersectionHorizontalVertical()
        {
            var a = new Point(0.0, 0.0);

            var check = (Point c, double x, double y) => Math.Abs(c.X - x) < EPS && Math.Abs(c.Y - y) < EPS;

            IntersectionHelper.BoxRayIntersection(box, p, -0.2, 0.0, ref a);
            Assert.That(check(a, -1.0, 0.0), Is.True);
            IntersectionHelper.BoxRayIntersection(box, p, 0.2, 0.0, ref a);
            Assert.That(check(a, 1.0, 0.0), Is.True);
            IntersectionHelper.BoxRayIntersection(box, p, 0.0, -0.2, ref a);
            Assert.That(check(a, 0.0, -1.0), Is.True);
            IntersectionHelper.BoxRayIntersection(box, p, 0.0, 0.2, ref a);
            Assert.That(check(a, 0.0, 1.0), Is.True);
        }

        [Test]
        public void TestBoxRayIntersectionBottom()
        {
            var a = new Point(0.0, 0.0);

            var check = (Point c, Rectangle r) => Math.Abs(c.Y - r.Bottom) < EPS && c.X > r.Left - EPS && c.X < r.Right + EPS;

            IntersectionHelper.BoxRayIntersection(box, p, -0.4, -0.5, ref a);
            Assert.That(check(a, box), Is.True);
            IntersectionHelper.BoxRayIntersection(box, p, -0.2, -0.5, ref a);
            Assert.That(check(a, box), Is.True);
            IntersectionHelper.BoxRayIntersection(box, p, -0.1, -0.5, ref a);
            Assert.That(check(a, box), Is.True);
            IntersectionHelper.BoxRayIntersection(box, p, 0.1, -0.5, ref a);
            Assert.That(check(a, box), Is.True);
            IntersectionHelper.BoxRayIntersection(box, p, 0.2, -0.5, ref a);
            Assert.That(check(a, box), Is.True);
            IntersectionHelper.BoxRayIntersection(box, p, 0.4, -0.5, ref a);
            Assert.That(check(a, box), Is.True);
        }

        [Test]
        public void TestBoxRayIntersectionTop()
        {
            var a = new Point(0.0, 0.0);

            var check = (Point c, Rectangle r) => Math.Abs(c.Y - r.Top) < EPS && c.X > r.Left - EPS && c.X < r.Right + EPS;

            IntersectionHelper.BoxRayIntersection(box, p, -0.4, 0.5, ref a);
            Assert.That(check(a, box), Is.True);
            IntersectionHelper.BoxRayIntersection(box, p, -0.2, 0.5, ref a);
            Assert.That(check(a, box), Is.True);
            IntersectionHelper.BoxRayIntersection(box, p, -0.1, 0.5, ref a);
            Assert.That(check(a, box), Is.True);
            IntersectionHelper.BoxRayIntersection(box, p, 0.1, 0.5, ref a);
            Assert.That(check(a, box), Is.True);
            IntersectionHelper.BoxRayIntersection(box, p, 0.2, 0.5, ref a);
            Assert.That(check(a, box), Is.True);
            IntersectionHelper.BoxRayIntersection(box, p, 0.4, 0.5, ref a);
            Assert.That(check(a, box), Is.True);
        }

        [Test]
        public void TestBoxRayIntersectionLeft()
        {
            var a = new Point(0.0, 0.0);

            var check = (Point c, Rectangle r) => Math.Abs(c.X - r.Left) < EPS && c.Y > r.Bottom - EPS && c.Y < r.Top + EPS;

            IntersectionHelper.BoxRayIntersection(box, p, -0.5, -0.4, ref a);
            Assert.That(check(a, box), Is.True);
            IntersectionHelper.BoxRayIntersection(box, p, -0.5, -0.2, ref a);
            Assert.That(check(a, box), Is.True);
            IntersectionHelper.BoxRayIntersection(box, p, -0.5, -0.1, ref a);
            Assert.That(check(a, box), Is.True);
            IntersectionHelper.BoxRayIntersection(box, p, -0.5, 0.1, ref a);
            Assert.That(check(a, box), Is.True);
            IntersectionHelper.BoxRayIntersection(box, p, -0.5, 0.2, ref a);
            Assert.That(check(a, box), Is.True);
            IntersectionHelper.BoxRayIntersection(box, p, -0.5, 0.4, ref a);
            Assert.That(check(a, box), Is.True);
        }

        [Test]
        public void TestBoxRayIntersectionRight()
        {
            var a = new Point(0.0, 0.0);

            var check = (Point c, Rectangle r) => Math.Abs(c.X - r.Right) < EPS && c.Y > r.Bottom - EPS && c.Y < r.Top + EPS;

            IntersectionHelper.BoxRayIntersection(box, p, 0.5, -0.4, ref a);
            Assert.That(check(a, box), Is.True);
            IntersectionHelper.BoxRayIntersection(box, p, 0.5, -0.2, ref a);
            Assert.That(check(a, box), Is.True);
            IntersectionHelper.BoxRayIntersection(box, p, 0.5, -0.1, ref a);
            Assert.That(check(a, box), Is.True);
            IntersectionHelper.BoxRayIntersection(box, p, 0.5, 0.1, ref a);
            Assert.That(check(a, box), Is.True);
            IntersectionHelper.BoxRayIntersection(box, p, 0.5, 0.2, ref a);
            Assert.That(check(a, box), Is.True);
            IntersectionHelper.BoxRayIntersection(box, p, 0.5, 0.4, ref a);
            Assert.That(check(a, box), Is.True);
        }
        
        [Test]
        public void TestIsPointOnSegment()
        {
            var a = new Vertex(1.0, 1.0);
            var b = new Vertex(2.0, 2.0);

            // Test point = segment start point.
            Assert.That(IntersectionHelper.IsPointOnSegment(a, b, new Vertex(1.0, 1.0)), Is.True);

            // Test point = segment end point.
            Assert.That(IntersectionHelper.IsPointOnSegment(a, b, new Vertex(2.0, 2.0)), Is.True);

            // Test point on segment.
            Assert.That(IntersectionHelper.IsPointOnSegment(a, b, new Vertex(1.5, 1.5)), Is.True);

            // Test point collinear, but not on segment.
            Assert.That(IntersectionHelper.IsPointOnSegment(a, b, new Vertex(0.0, 0.0)), Is.False);
            Assert.That(IntersectionHelper.IsPointOnSegment(a, b, new Vertex(3.0, 3.0)), Is.False);

            // Test point not on segment.
            Assert.That(IntersectionHelper.IsPointOnSegment(a, b, new Vertex(1.5, 0.5)), Is.False);

            double eps = 1e-12;

            // Test point collinear near endpoint, but not on segment.
            Assert.That(IntersectionHelper.IsPointOnSegment(a, b, new Vertex(2.0 + eps, 2.0 + eps)), Is.False);
            Assert.That(IntersectionHelper.IsPointOnSegment(a, b, new Vertex(2.0 - eps, 2.0 + eps)), Is.False);

            // Test point collinear near endpoint on segment.
            Assert.That(IntersectionHelper.IsPointOnSegment(a, b, new Vertex(2.0 - eps, 2.0 - eps)), Is.True);
        }
    }
}
