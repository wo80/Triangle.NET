
namespace TriangleNet.Examples
{
    using System;
    using TriangleNet.Geometry;
    using TriangleNet.Meshing;
    using TriangleNet.Rendering.Text;
    using TriangleNet.Smoothing;
    using TriangleNet.Tools;

    /// <summary>
    /// Triangulate a polygon with hole with maximum area constraint, followed by mesh smoothing.
    /// </summary>
    public class Example4 : IExample
    {
        public bool Run(bool print = false)
        {
            // Generate mesh.
            var mesh = CreateMesh();

            // The ideal area if triangles were equilateral.
            var area = Math.Sqrt(3) / 4 * h * h;

            var quality = new QualityMeasure();

            quality.Update(mesh);

            if (print)
            {
                Console.WriteLine($"   Ideal area: {area}");
                Console.WriteLine($"    Min. area: {quality.Area.Minimum}");
                Console.WriteLine($"    Max. area: {quality.Area.Maximum}");
                Console.WriteLine($"    Avg. area: {quality.AreaTotal / mesh.Triangles.Count}");

                SvgImage.Save(mesh, "example-4.svg", 500);
            }

            return quality.Area.Minimum < area && quality.Area.Maximum > area;
        }

        // The boundary segment size of the input geometry.
        const double h = 0.2;

        // Parameter to relax the maximum area constraint.
        const double relax = 1.45;

        public static IMesh CreateMesh()
        {
            // Generate the input geometry.
            var poly = Example3.CreatePolygon(h);

            // Since we want to do CVT smoothing, ensure that the mesh
            // is conforming Delaunay.
            var options = new ConstraintOptions() { ConformingDelaunay = true };

            // Set maximum area quality option (we don't need to set a minimum
            // angle, since smoothing will improve the triangle shapes).
            var quality = new QualityOptions()
            {
                // Given the boundary segment size, we set a maximum
                // area constraint assuming equilateral triangles. The
                // relaxation parameter is chosen to reduce the deviation
                // from this ideal value.
                MaximumArea = (Math.Sqrt(3) / 4 * h * h) * relax
            };

            // Generate mesh using the polygons Triangulate extension method.
            var mesh = poly.Triangulate(options, quality);

            var smoother = new SimpleSmoother();

            // Smooth mesh.
            smoother.Smooth(mesh, 25, .05);

            return mesh;
        }
    }
}
