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
        [Test]
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

            var quality = new QualityMeasure(mesh);

            Assert.AreEqual(quality.AreaMinimum, quality.AreaMaximum);
            Assert.AreEqual(1.0, quality.AlphaMinimum);
            Assert.AreEqual(1.0, quality.AlphaMaximum);
            Assert.AreEqual(1.0, quality.Q_Minimum);
            Assert.AreEqual(1.0, quality.Q_Maximum);
        }
    }
}
