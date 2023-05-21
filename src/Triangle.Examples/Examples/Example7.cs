
namespace TriangleNet.Examples
{
    using System.Linq;
    using TriangleNet;
    using TriangleNet.Geometry;
    using TriangleNet.Meshing.Iterators;
    using TriangleNet.Rendering.Text;
    using TriangleNet.Tools;
    using TriangleNet.Topology;

    /// <summary>
    /// Boolean operations on mesh regions (intersection, difference, xor).
    /// </summary>
    public class Example7 : IExample
    {
        public bool Run(bool print = false)
        {
            // Generate the input geometry.
            var polygon = new Polygon(8, true);

            // Two intersecting rectangles.
            var A = Generate.Rectangle(0d, 0d, 4d, 4d, label: 1);
            var B = Generate.Rectangle(1d, 1d, 4d, 4d, label: 2);

            polygon.Add(A);
            polygon.Add(B);

            // Generate mesh.
            var mesh = (Mesh)polygon.Triangulate();

            if (print) SvgImage.Save(mesh, "example-7.svg", 500);

            // Find a seeding triangle (in this case, the point (2, 2) lies in
            // both rectangles).
            var seed = (new TriangleQuadTree(mesh)).Query(2.0, 2.0) as Triangle;

            var iterator = new RegionIterator();

            iterator.Process(seed, t => t.Label ^= 1, 1);
            iterator.Process(seed, t => t.Label ^= 2, 2);

            // At this point, all triangles will have label 1, 2 or 3 (= 1 xor 2).

            // The intersection of A and B.
            var intersection = mesh.Triangles.Where(t => t.Label == 3);

            // The difference A \ B.
            var difference = mesh.Triangles.Where(t => t.Label == 1);

            // The xor of A and B.
            var xor = mesh.Triangles.Where(t => t.Label == 1 || t.Label == 2);

            return intersection.Any() && difference.Any() && xor.Any();
        }
    }
}