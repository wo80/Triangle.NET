using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TriangleNet.Geometry;
using TriangleNet.Meshing;

namespace TriangleNet.Tests.Meshing
{
    public class GenericMesherTest
    {
        [Test]
        public void TestTriangulateDwyer()
        {
            var m = new GenericMesher();

            var vertices = GetVertices();

            var mesh = m.Triangulate(vertices);

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
