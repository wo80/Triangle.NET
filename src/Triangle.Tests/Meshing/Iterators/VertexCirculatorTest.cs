using NUnit.Framework;
using System.Linq;
using TriangleNet.Geometry;
using TriangleNet.IO;
using TriangleNet.Meshing;
using TriangleNet.Meshing.Iterators;

namespace TriangleNet.Tests.Meshing.Iterators
{
    public class VertexCirculatorTest
    {
        [Test]
        public void TestEnumerateVertices()
        {
            //           5
            //          /\
            //         /  \
            //        /    \
            //      3/______\4
            //      /\      /\
            //     /  \    /  \
            //    /    \  /    \
            //  0/______\/______\2
            //           1

            var mesh = CreateMesh(out var vertices);

            var circulator = new VertexCirculator(mesh);

            var p = vertices[0];
            
            var list =  circulator.EnumerateVertices(p).ToList();

            Assert.That(list.Count, Is.EqualTo(2));
        }

        [Test]
        public void TestEnumerateTriangles()
        {
            var mesh = CreateMesh(out var vertices);

            var circulator = new VertexCirculator(mesh);

            var p = vertices[0];

            var list = circulator.EnumerateTriangles(p).ToList();

            Assert.That(list.Count, Is.EqualTo(1));
        }

        private Mesh CreateMesh(out Vertex[] vertices)
        {
            var poly = new Polygon();

            vertices = new Vertex[]
            {
                new Vertex(-2.0, 0.0, 0),
                new Vertex(0.0, 0.0, 1),
                new Vertex(2.0, 0.0, 2),
                new Vertex(-1.0, 1.0, 3),
                new Vertex(1.0, 1.0, 4),
                new Vertex(0.0, 2.0, 5)
            };

            poly.Points.AddRange(vertices);

            var triangles = new InputTriangle[]
            {
                new InputTriangle(3, 0, 1) { ID = 1 },
                new InputTriangle(3, 1, 4) { ID = 2 },
                new InputTriangle(4, 1, 2) { ID = 3 },
                new InputTriangle(5, 3, 4) { ID = 4 },
            };

            return Converter.Instance.ToMesh(poly, triangles);
        }
    }
}
