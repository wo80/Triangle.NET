using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TriangleNet.Geometry;
using TriangleNet.Meshing;
using TriangleNet.Tools;

namespace TriangleNet.Tests.Tools
{
    public class AdjacencyMatrixTest
    {
        [Test]
        public void TestAdjacencyMatrix()
        {
            var p = GetVertices(false);
            var mesher = new GenericMesher();
            var mesh = (Mesh)mesher.Triangulate(p);

            Assert.That(mesh.Vertices.Max(v => v.ID), Is.EqualTo(5));

            mesh.Renumber();

            var matrix = new AdjacencyMatrix(mesh);

            // Highest vertex id after renumbering is 4, since there
            // is no duplicate vertex.
            Assert.That(matrix.RowIndices.Max(), Is.EqualTo(4));
            Assert.That(matrix.ColumnCount, Is.EqualTo(5));
        }

        [Test]
        public void TestAdjacencyMatrixDuplicate()
        {
            var p = GetVertices(true);
            var mesher = new GenericMesher();
            var mesh = (Mesh)mesher.Triangulate(p);

            mesh.Renumber();

            var matrix = new AdjacencyMatrix(mesh);

            var ai = matrix.RowIndices;

            // Highest vertex id after renumbering is 4, duplicates
            // are ignored.
            Assert.That(ai.Max(), Is.EqualTo(4));

            // Get the single, duplicate vertex.
            var dup = mesh.Vertices
                .Where(v => v.Type == VertexType.UndeadVertex)
                .Single();

            // Side effect: undead vertices will have negative indices
            // after computing the adjacency matrix.
            Assert.That(dup.id, Is.LessThan(0));
            Assert.That(!ai.Contains(dup.id), Is.True);
        }

        private List<Vertex> GetVertices(bool includeDuplicate)
        {
            var list = new List<Vertex>()
            {
                new Vertex(0.0, 0.0) { ID = 5 },
                new Vertex(1.0, 0.0) { ID = 4 },
                new Vertex(1.0, 1.0) { ID = 3 }
            };

            if (includeDuplicate)
            {
                list.Add(new Vertex(1.0, 1.0) { ID = 2 });
            }

            list.Add(new Vertex(0.0, 1.0) { ID = 1 });
            list.Add(new Vertex(0.5, 0.5) { ID = 0 });

            return list;
        }
    }
}