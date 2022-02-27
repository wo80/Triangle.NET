
namespace TriangleNet.Examples
{
    using TriangleNet.Geometry;
    using TriangleNet.Meshing;
    using TriangleNet.Meshing.Algorithm;
    using TriangleNet.Rendering.Text;

    /// <summary>
    /// Simple point set triangulation.
    /// </summary>
    public class Example1
    {
        public static void Run(bool print = false)
        {
            // Generate points.
            var points = Generate.RandomPoints(50, new Rectangle(0, 0, 100, 100));

            // Choose triangulator: Incremental, SweepLine or Dwyer.
            var triangulator = new Dwyer();

            // Generate a default mesher.
            var mesher = new GenericMesher(triangulator);
            
            // Generate mesh.
            var mesh = mesher.Triangulate(points);

            if (print) SvgImage.Save(mesh, "example-1.svg", 500);
        }
    }
}
