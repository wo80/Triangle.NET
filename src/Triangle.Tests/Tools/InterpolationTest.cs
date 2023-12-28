using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TriangleNet.Geometry;
using TriangleNet.Meshing.Algorithm;
using TriangleNet.Tools;

namespace TriangleNet.Tests.Tools
{
    public class InterpolationTest
    {
        [Test, DefaultFloatingPointTolerance(1e-8)]
        public void TestInterpolatePoint()
        {
            var vertices = new List<Vertex>()
            {
                new Vertex(0.0, 0.0) { ID = 0 },
                new Vertex(2.0, 0.0) { ID = 1 },
                new Vertex(0.5, 1.0) { ID = 2 }
            };

            // The z-values.
            var values = new double[] { 1d, -1d, 2d };

            var mesh = new Dwyer().Triangulate(vertices, new Configuration());

            var tri = mesh.Triangles.First();

            // Check the corners.

            double actual, expected;

            for (int i = 0; i < 3; i++)
            {
                actual = Interpolation.InterpolatePoint(tri, vertices[i], values);
                expected = values[i];

                Assert.That(actual, Is.EqualTo(expected));
            }

            // Check the edge midpoints.

            double x, y;

            for (int i = 0; i < 3; i++)
            {
                x = (vertices[i].X + vertices[(i + 1) % 3].X) / 2;
                y = (vertices[i].Y + vertices[(i + 1) % 3].Y) / 2;

                var p = new Point(x, y);

                actual = Interpolation.InterpolatePoint(tri, p, values);
                expected = (values[i] + values[(i + 1) % 3]) / 2;

                Assert.That(actual, Is.EqualTo(expected));
            }

            // Check centroid.
            x = (vertices[0].X + vertices[1].X + vertices[2].X) / 3;
            y = (vertices[0].Y + vertices[1].Y + vertices[2].Y) / 3;

            actual = Interpolation.InterpolatePoint(tri, new Point(x, y), values);
            expected = (values[0] + values[1] + values[2]) / 3;

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
