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

            Assert.That(vertices.Count, Is.EqualTo(6));
            Assert.That(mesh.Vertices.Count, Is.EqualTo(6));
            Assert.That(mesh.Vertices
                .Where(v => v.Type == VertexType.UndeadVertex)
                .Count(), Is.EqualTo(1));
        }

        [Test]
        public void TestTriangulateSweepLine()
        {
            var t = new SweepLine();

            var vertices = GetVertices();

            var mesh = t.Triangulate(vertices, new Configuration());

            Assert.That(vertices.Count, Is.EqualTo(6));
            Assert.That(mesh.Vertices.Count, Is.EqualTo(6));
            Assert.That(mesh.Vertices
                .Where(v => v.Type == VertexType.UndeadVertex)
                .Count(), Is.EqualTo(1));
        }

        [Test]
        public void TestTriangulateDwyer()
        {
            var t = new Dwyer();

            var vertices = GetVertices();

            var mesh = t.Triangulate(vertices, new Configuration());

            Assert.That(vertices.Count, Is.EqualTo(6));
            Assert.That(mesh.Vertices.Count, Is.EqualTo(6));
            Assert.That(mesh.Vertices
                .Where(v => v.Type == VertexType.UndeadVertex)
                .Count(), Is.EqualTo(1));
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
