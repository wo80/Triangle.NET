
namespace TriangleNet.Examples
{
    using TriangleNet;
    using TriangleNet.Geometry;
    using TriangleNet.Meshing;
    using TriangleNet.Rendering.Text;
    using TriangleNet.Smoothing;

    /// <summary>
    /// Refine only a part of a polygon mesh be using region pointers and an area constraint.
    /// </summary>
    public class Example4
    {
        public static void Run(bool print = false)
        {
            // Generate the input geometry.
            var poly = CreatePolygon();
            
            // Define regions (first one defines the area constraint).
            poly.Regions.Add(new RegionPointer(1.5, 0.0, 1, 0.01));
            poly.Regions.Add(new RegionPointer(2.5, 0.0, 2));

            // Set quality and constraint options.
            var options = new ConstraintOptions()
            {
                ConformingDelaunay = true
            };

            var quality = new QualityOptions()
            {
                MinimumAngle = 25.0,
                VariableArea = true
            };

            //quality.UserTest = (t, area) => t.Label == 1 && area > 0.01;

            var mesh = poly.Triangulate(options, quality);

            var smoother = new SimpleSmoother();

            smoother.Smooth(mesh, 5);

            if (print) SvgImage.Save(mesh, "example-4.png", 500);
        }

        public static IPolygon CreatePolygon()
        {
            // Generate three concentric circles.
            var poly = new Polygon();

            // Center point.
            var center = new Point(0, 0);

            // Inner contour (hole).
            poly.Add(Generate.Circle(1.0, center, 0.1, 1), center);

            // Internal contour.
            poly.Add(Generate.Circle(2.0, center, 0.1, 2));

            // Outer contour.
            poly.Add(Generate.Circle(3.0, center, 0.3, 3));

            // Note that the outer contour has a larger segment size!

            return poly;
        }
    }
}
