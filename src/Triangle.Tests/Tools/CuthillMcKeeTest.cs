using NUnit.Framework;
using TriangleNet.Meshing;
using TriangleNet.Tools;

namespace TriangleNet.Tests.Tools
{
    public class CuthillMcKeeTest
    {
        [Test]
        public void TestCuthillMcKee()
        {
            var mesh = (Mesh)GenericMesher.StructuredMesh(4.0, 3.0, 2, 1);

            var matrix = new AdjacencyMatrix(mesh);

            CollectionAssert.AreEqual(matrix.ColumnPointers, new int[] { 0, 4, 7, 11, 17, 21, 24 });
            CollectionAssert.AreEqual(matrix.RowIndices, new int[] {
                0, 1, 2, 3,
                0, 1, 3,
                0, 2, 3, 4,
                0, 1, 2, 3, 4, 5,
                2, 3, 4, 5,
                3, 4, 5 });

            Assert.AreEqual(7, matrix.Bandwidth());

            var p = new CuthillMcKee().Renumber(matrix);

            foreach (var node in mesh.Vertices)
            {
                node.ID = p[node.ID];
            }

            var pmatrix = new AdjacencyMatrix(mesh, false);

            CollectionAssert.AreEqual(pmatrix.ColumnPointers, new int[] { 0, 3, 7, 11, 15, 21, 24 });
            CollectionAssert.AreEqual(pmatrix.RowIndices, new int[] {
                0, 2, 4,
                1, 2, 3, 4,
                0, 1, 2, 4,
                1, 3, 4, 5,
                0, 1, 2, 3, 4, 5,
                3, 4, 5 });

            // For structured meshes we cannot expect an improved bandwidth.
            Assert.AreEqual(9, pmatrix.Bandwidth());
        }
    }
}