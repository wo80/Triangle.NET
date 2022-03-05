
namespace TriangleNet.Examples
{
    using System;
    using TriangleNet.Geometry;
    using TriangleNet.Meshing;
    using TriangleNet.Rendering.Text;
    using TriangleNet.Smoothing;

    /// <summary>
    /// Triangulate a polygon with hole with maximum area constraint, followed by mesh smoothing.
    /// </summary>
    public class Example3
    {
        public static bool Run(bool print = false)
        {
            // Generate mesh.
            var mesh = CreateMesh();

            if (print) SvgImage.Save(mesh, "example-3.svg", 500);

            return true;
        }

        public static IMesh CreateMesh()
        {
            // Generate the input geometry.
            var poly = Example2.CreatePolygon();

            // Since we want to do CVT smoothing, ensure that the mesh
            // is conforming Delaunay.
            var options = new ConstraintOptions() { ConformingDelaunay = true };

            // Set maximum area quality option (we don't need to set a minimum
            // angle, since smoothing will improve the triangle shapes).
            var quality = new QualityOptions()
            {
                // The boundary segments have a length of 0.2, so we set a
                // maximum area constraint assuming equilateral triangles.
                MaximumArea = (Math.Sqrt(3) / 4 * 0.2 * 0.2) * 1.45
            };

            // Generate mesh using the polygons Triangulate extension method.
            var mesh = poly.Triangulate(options, quality);

            var smoother = new SimpleSmoother();

            // Smooth mesh.
            smoother.Smooth(mesh, 25);

            return mesh;
        }
    }
}
