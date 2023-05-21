
namespace TriangleNet.Examples
{
    using System.Collections.Generic;
    using TriangleNet;
    using TriangleNet.Geometry;
    using TriangleNet.Meshing;
    using TriangleNet.Meshing.Iterators;
    using TriangleNet.Rendering.Text;

    /// <summary>
    /// Two ways finding boundary triangles.
    /// </summary>
    public class Example6 : IExample
    {
        public bool Run(bool print = false)
        {
            var mesh = Example4.CreateMesh();

            FindBoundary1(mesh);

            if (print) SvgImage.Save(mesh, "example-6-1.svg", 500, true, false);

            FindBoundary2(mesh);

            if (print) SvgImage.Save(mesh, "example-6-2.svg", 500, true, false);

            return mesh.Triangles.Count > 0;
        }

        /// <summary>
        /// Find boundary triangles using segments.
        /// </summary>
        private static void FindBoundary1(IMesh mesh, bool neigbours = true)
        {
            mesh.Renumber();

            var cache = new List<Vertex>(mesh.Segments.Count + 1);

            var circulator = new VertexCirculator((Mesh)mesh);

            foreach (var s in mesh.Segments)
            {
                int label = s.Label;

                for (int i = 0; i < 2; i++)
                {
                    var vertex = s.GetVertex(i);

                    // Check the vertex ID to see if it was processed already.
                    if (vertex.ID >= 0)
                    {
                        var star = circulator.EnumerateTriangles(vertex);

                        foreach (var triangle in star)
                        {
                            triangle.Label = label;
                        }

                        // Mark the vertex as "processed".
                        vertex.ID = -vertex.ID;

                        cache.Add(vertex);
                    }
                }
            }

            // Undo the vertex ID changes.
            foreach (var vertex in cache)
            {
                vertex.ID = -vertex.ID;
            }
        }

        /// <summary>
        /// Find boundary triangles using vertices.
        /// </summary>
        private static void FindBoundary2(IMesh mesh)
        {
            var circulator = new VertexCirculator((Mesh)mesh);

            foreach (var vertex in mesh.Vertices)
            {
                int label = vertex.Label;

                if (label > 0)
                {
                    var star = circulator.EnumerateTriangles(vertex);

                    foreach (var triangle in star)
                    {
                        triangle.Label = label;
                    }
                }
            }
        }
    }
}
