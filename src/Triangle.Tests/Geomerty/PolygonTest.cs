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

            Assert.AreEqual(3, p.Points.Count);
            Assert.AreEqual(3, p.Segments.Count);
            Assert.AreEqual(0, p.Holes.Count);
            Assert.AreEqual(0, p.Regions.Count);
        }

        [Test]
        public void TestAddContourAsHole()
        {
            var contour = new Contour(vertices);

            var p = new Polygon();

            p.Add(contour, true);

            Assert.AreEqual(3, p.Points.Count);
            Assert.AreEqual(3, p.Segments.Count);
            Assert.AreEqual(1, p.Holes.Count);
            Assert.AreEqual(0, p.Regions.Count);
        }

        [Test]
        public void TestAddContourAsRegion()
        {
            var contour = new Contour(vertices, 1);

            var p = new Polygon();

            p.Add(contour, 1);

            Assert.AreEqual(3, p.Points.Count);
            Assert.AreEqual(3, p.Segments.Count);
            Assert.AreEqual(0, p.Holes.Count);
            Assert.AreEqual(1, p.Regions.Count);
        }

        [Test]
        public void TestBounds()
        {
            var contour = new Contour(vertices);

            var p = new Polygon();

            p.Add(contour);

            var bounds = p.Bounds();

            Assert.AreEqual(2d, bounds.Width);
            Assert.AreEqual(1.5, bounds.Height);
        }
    }
}