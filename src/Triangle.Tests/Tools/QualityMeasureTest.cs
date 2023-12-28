using NUnit.Framework;
using System;
using System.Collections.Generic;
using TriangleNet.Geometry;
using TriangleNet.Meshing.Algorithm;
using TriangleNet.Tools;

namespace TriangleNet.Tests.Tools
{
    public class QualityMeasureTest
    {
        [Test, DefaultFloatingPointTolerance(1e-12)]
        public void TestEquilateralTriangles()
        {
            double sqrt3 = Math.Sqrt(3);

            var vertices = new List<Vertex>()
            {
                new Vertex(0.0, 0.0) { ID = 0 },
                new Vertex(1.0, sqrt3) { ID = 1 },
                new Vertex(2.0, 0.0) { ID = 2 },
                new Vertex(3.0, sqrt3) { ID = 3 }
            };

            var mesh = new Dwyer().Triangulate(vertices, new Configuration());

            var quality = new QualityMeasure();

            quality.Update(mesh);

            Assert.That(quality.Area.Maximum, Is.EqualTo(quality.Area.Minimum));
            Assert.That(quality.Alpha.Minimum, Is.EqualTo(1.0));
            Assert.That(quality.Alpha.Maximum, Is.EqualTo(1.0));
            Assert.That(quality.Eta.Minimum, Is.EqualTo(1.0));
            Assert.That(quality.Eta.Maximum, Is.EqualTo(1.0));
            Assert.That(quality.Q.Minimum, Is.EqualTo(1.0));
            Assert.That(quality.Q.Maximum, Is.EqualTo(1.0));
        }
    }
}
