namespace TriangleNet.Examples
{
    using System.Linq;
    using TriangleNet;
    using TriangleNet.Geometry;
    using TriangleNet.Meshing.Iterators;
    using TriangleNet.Tools;
    using TriangleNet.Topology;

    /// <summary>
    /// Boolean operations on mesh regions (intersection, difference, xor).
    /// </summary>
    public static class Example6
    {
        public static bool Run()
        {
            // Generate the input geometry.
            var polygon = new Polygon(8, true);

            // Two intersecting rectangles.
            var A = Generate.Rectangle(0.0, 0.0, 4.0, 4.0, 1);
            var B = Generate.Rectangle(1.0, 1.0, 4.0, 4.0, 2);

            polygon.Add(A);
            polygon.Add(B);

            // Generate mesh.
            var mesh = (Mesh)polygon.Triangulate();

            // Find a seeding triangle (in this case, the point (2, 2) lies in
            // both rectangles).
            var seed = (new TriangleQuadTree(mesh)).Query(2.0, 2.0) as Triangle;

            var iterator = new RegionIterator(mesh);

            iterator.Process(seed, t => t.Label ^= 1, 1);
            iterator.Process(seed, t => t.Label ^= 2, 2);

            // At this point, all triangles will have label 1, 2 or 3 (= 1 xor 2).

            // The intersection of A and B.
            var intersection = mesh.Triangles.Where(t => t.Label == 3);

            // The difference A \ B.
            var difference = mesh.Triangles.Where(t => t.Label == 1);

            // The xor of A and B.
            var xor = mesh.Triangles.Where(t => t.Label == 1 || t.Label == 2);

            return true;
        }
    }
}