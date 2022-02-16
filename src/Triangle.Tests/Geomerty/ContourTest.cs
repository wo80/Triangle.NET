using NUnit.Framework;
using TriangleNet.Geometry;

namespace TriangleNet.Tests.Geometry
{
    public class ContourTest
    {
        [Test]
        public void TestFindInteriorPoint()
        {
            // The vertices  that define the contour (have to be in order, obviously).
            var vertices = new Vertex[]
            {
                new Vertex(0d, 0d),
                new Vertex(1d ,0d),
                new Vertex(1d, 1d),
                new Vertex(0d, 1d),
            };

            var contour = new Contour(vertices);

            var p = contour.FindInteriorPoint();

            Assert.IsTrue(p.X > 0d && p.X < 1d && p.Y > 0d && p.Y < 1d);
        }
    }
}
