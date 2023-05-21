
namespace TriangleNet.Examples
{
    using TriangleNet.Geometry;
    using TriangleNet.Meshing;
    using TriangleNet.Rendering.Text;

    /// <summary>
    /// Triangulate a polygon with hole and set minimum angle constraint.
    /// </summary>
    public class Example3 : IExample
    {
        public bool Run(bool print = false)
        {
            // Generate the input geometry.
            var poly = CreatePolygon();

            // Set minimum angle quality option.
            var quality = new QualityOptions() { MinimumAngle = 30.0 };

            // Generate mesh using the polygons Triangulate extension method.
            var mesh = poly.Triangulate(quality);

            if (print) SvgImage.Save(mesh, "example-3.svg", 500);

            return mesh.Triangles.Count > 0;
        }

        public static IPolygon CreatePolygon(double h = 0.2)
        {
            // Generate the input geometry.
            var poly = new Polygon();

            // Center point.
            var center = new Point(0, 0);

            // Inner contour (hole).
            poly.Add(Generate.Circle(1.0, center, h, 1), center);

            // Internal contour.
            poly.Add(Generate.Circle(2.0, center, h, 2));

            // Outer contour.
            poly.Add(Generate.Circle(3.0, center, h, 3));

            return poly;
        }
    }
}