
namespace TriangleNet.Examples
{
    using TriangleNet.Geometry;
    using TriangleNet.Meshing.Algorithm;
    using TriangleNet.Rendering.Text;

    /// <summary>
    /// Simple point set triangulation.
    /// </summary>
    public class Example1 : IExample
    {
        public bool Run(bool print = false)
        {
            // Generate points.
            var points = Generate.RandomPoints(50, new Rectangle(0, 0, 100, 100));

            // Choose triangulator: Incremental, SweepLine or Dwyer.
            var triangulator = new Dwyer();

            // Generate mesh.
            var mesh = triangulator.Triangulate(points, new Configuration());

            if (print) SvgImage.Save(mesh, "example-1.svg", 500);

            return mesh.Triangles.Count > 0;
        }
    }
}
