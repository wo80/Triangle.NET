
namespace TriangleNet.Examples
{
    using System;
    using TriangleNet.Geometry;
    using TriangleNet.Meshing;
    using TriangleNet.Meshing.Iterators;
    using TriangleNet.Rendering.Text;

    /// <summary>
    /// Using a user test function to define a maximum edge length constraint.
    /// </summary>
    public class Example8 : IExample
    {
        const double MAX_EDGE_LENGTH = 0.2;

        public bool Run(bool print = false)
        {
            var poly = new Polygon();

            // Generate the input geometry.
            poly.Add(Generate.Rectangle(0.0, 0.0, 1.0, 1.0));

            // Set user test function.
            var quality = new QualityOptions()
            {
                UserTest = MaxEdgeLength
            };

            // Generate mesh using the polygons Triangulate extension method.
            var mesh = (Mesh)poly.Triangulate(quality);

            // Validate.
            foreach (var e in EdgeIterator.EnumerateEdges(mesh))
            {
                double length = Math.Sqrt(DistSqr(e.GetVertex(0), e.GetVertex(1)));

                if (length > MAX_EDGE_LENGTH)
                {
                    return false;
                }
            }

            if (print) SvgImage.Save(mesh, "example-8.svg", 500);

            return true;
        }

        static bool MaxEdgeLength(ITriangle tri, double area)
        {
            var p0 = tri.GetVertex(0);
            var p1 = tri.GetVertex(1);
            var p2 = tri.GetVertex(2);

            var s1 = DistSqr(p0, p1);
            var s2 = DistSqr(p1, p2);
            var s3 = DistSqr(p2, p0);

            // Comparing against squared max leg length.
            var maxlen = MAX_EDGE_LENGTH * MAX_EDGE_LENGTH;

            return s1 > maxlen || s2 > maxlen || s3 > maxlen;
        }

        static double DistSqr(Vertex a, Vertex b)
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;

            return dx * dx + dy * dy;
        }
    }
}
