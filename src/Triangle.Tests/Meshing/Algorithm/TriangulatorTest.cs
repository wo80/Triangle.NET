using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TriangleNet.Geometry;
using TriangleNet.Meshing.Algorithm;

namespace TriangleNet.Tests.Meshing.Algorithm
{
    public class TriangulatorTest
    {
        [Test]
        public void TestTriangulateIncremental()
        {
            var t = new Incremental();

            var vertices = GetVertices();

            var mesh = t.Triangulate(vertices, new Configuration());

            Assert.AreEqual(6, vertices.Count);
            Assert.AreEqual(6, mesh.Vertices.Count);
            Assert.AreEqual(1, mesh.Vertices
                .Where(v => v.Type == VertexType.UndeadVertex)
                .Count());
        }

        [Test]
        public void TestTriangulateSweepLine()
        {
            var t = new SweepLine();

            var vertices = GetVertices();

            var mesh = t.Triangulate(vertices, new Configuration());

            Assert.AreEqual(6, vertices.Count);
            Assert.AreEqual(6, mesh.Vertices.Count);
            Assert.AreEqual(1, mesh.Vertices
                .Where(v => v.Type == VertexType.UndeadVertex)
                .Count());
        }

        [Test]
        public void TestTriangulateDwyer()
        {
            var t = new Dwyer();

            var vertices = GetVertices();

            var mesh = t.Triangulate(vertices, new Configuration());

            Assert.AreEqual(6, vertices.Count);
            Assert.AreEqual(6, mesh.Vertices.Count);
            Assert.AreEqual(1, mesh.Vertices
                .Where(v => v.Type == VertexType.UndeadVertex)
                .Count());
        }

        private List<Vertex> GetVertices()
        {
            return new List<Vertex>()
            {
                new Vertex(0.0, 0.0),
                new Vertex(1.0, 0.0),
                new Vertex(1.0, 1.0),
                new Vertex(1.0, 1.0), // duplicate
                new Vertex(0.0, 1.0),
                new Vertex(0.5, 0.5)
            };
        }
    }
}
