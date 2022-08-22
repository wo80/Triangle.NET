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

            Assert.AreEqual(quality.Area.Minimum, quality.Area.Maximum);
            Assert.AreEqual(1.0, quality.Alpha.Minimum);
            Assert.AreEqual(1.0, quality.Alpha.Maximum);
            Assert.AreEqual(1.0, quality.Eta.Minimum);
            Assert.AreEqual(1.0, quality.Eta.Maximum);
            Assert.AreEqual(1.0, quality.Q.Minimum);
            Assert.AreEqual(1.0, quality.Q.Maximum);
        }
    }
}
