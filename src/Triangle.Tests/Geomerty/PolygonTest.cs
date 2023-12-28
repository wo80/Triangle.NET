using NUnit.Framework;
using TriangleNet.Geometry;

namespace TriangleNet.Tests.Geometry
{
    public class PolygonTest
    {
        // The vertices  that define the polygon contour (triangle shape).
        Vertex[] vertices = new Vertex[]
        {
                new Vertex(0d, 0d),
                new Vertex(2d ,0d),
                new Vertex(1d, 1.5)
        };

        [Test]
        public void TestAddContour()
        {
            var contour = new Contour(vertices);

            var p = new Polygon();

            p.Add(contour);

            Assert.That(p.Points.Count, Is.EqualTo(3));
            Assert.That(p.Segments.Count, Is.EqualTo(3));
            Assert.That(p.Holes.Count, Is.EqualTo(0));
            Assert.That(p.Regions.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestAddContourAsHole()
        {
            var contour = new Contour(vertices);

            var p = new Polygon();

            p.Add(contour, true);

            Assert.That(p.Points.Count, Is.EqualTo(3));
            Assert.That(p.Segments.Count, Is.EqualTo(3));
            Assert.That(p.Holes.Count, Is.EqualTo(1));
            Assert.That(p.Regions.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestAddContourAsRegion()
        {
            var contour = new Contour(vertices, 1);

            var p = new Polygon();

            p.Add(contour, 1);

            Assert.That(p.Points.Count, Is.EqualTo(3));
            Assert.That(p.Segments.Count, Is.EqualTo(3));
            Assert.That(p.Holes.Count, Is.EqualTo(0));
            Assert.That(p.Regions.Count, Is.EqualTo(1));
        }

        [Test]
        public void TestBounds()
        {
            var contour = new Contour(vertices);

            var p = new Polygon();

            p.Add(contour);

            var bounds = p.Bounds();

            Assert.That(bounds.Width, Is.EqualTo(2d));
            Assert.That(bounds.Height, Is.EqualTo(1.5));
        }
    }
}