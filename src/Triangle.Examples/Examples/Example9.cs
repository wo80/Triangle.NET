
namespace TriangleNet.Examples
{
    using System.Collections.Generic;
    using TriangleNet;
    using TriangleNet.Meshing.Iterators;
    using TriangleNet.Tools;

    /// <summary>
    /// Compute the adjacency matrix of the mesh vertices.
    /// </summary>
    public class Example9 : IExample
    {
        public bool Run(bool print)
        {
            var mesh = (Mesh)Example4.CreateMesh();

            return FindAdjacencyMatrix(mesh);
        }

        private static bool FindAdjacencyMatrix(Mesh mesh)
        {
            mesh.Renumber();

            var ap = new List<int>(mesh.Vertices.Count); // Column pointers.
            var ai = new List<int>(4 * mesh.Vertices.Count); // Row indices.

            var circulator = new VertexCirculator(mesh);

            int k = 0;

            foreach (var vertex in mesh.Vertices)
            {
                var star = circulator.EnumerateVertices(vertex);

                ap.Add(k);

                // Each vertex is adjacent to itself.
                ai.Add(vertex.ID);
                k++;

                foreach (var item in star)
                {
                    ai.Add(item.ID);
                    k++;
                }
            }

            ap.Add(k);

            var matrix1 = new AdjacencyMatrix(ap.ToArray(), ai.ToArray());
            var matrix2 = new AdjacencyMatrix(mesh);

            // Column pointers should be exactly the same.
            if (!CompareArray(matrix1.ColumnPointers, matrix2.ColumnPointers))
            {
                return false;
            }

            return true;
        }

        private static bool CompareArray(int[] a, int[] b)
        {
            int length = a.Length;

            if (b.Length != length)
            {
                return false;
            }

            for (int i = 0; i < length; i++)
            {
                if (a[i] != b[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
