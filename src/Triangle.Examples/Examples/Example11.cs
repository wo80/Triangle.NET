
namespace TriangleNet.Examples
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TriangleNet.Geometry;
    using TriangleNet.Meshing;
    using TriangleNet.Meshing.Algorithm;
    using TriangleNet.Rendering.Text;
    using TriangleNet.Tools;

    /// <summary>
    /// Scattered data interpolation without USE_Z or USE_ATTRIBS.
    /// </summary>
    public class Example11 : IExample
    {
        // The function we are sampling.
        private static readonly Func<Point, double> F = p => Math.Sin(p.X) * Math.Cos(p.Y);

        // The mesh size, for a structured grid (SIZE x SIZE) points.
        private const int SIZE = 20;

        public bool Run(bool print = false)
        {
            // The input domain.
            var r = new Rectangle(0d, 0d, 10d, 10d);

            var mesh = GetScatteredDataMesh(r);
            //var mesh = GetStructuredDataMesh(r);

            // Generate function values for mesh points.
            double[] data = GetFunctionValues(mesh.Vertices);

            if (print) SvgImage.Save(mesh, "example-11.svg", 500);

            // The points to interpolate.
            var xy = Generate.RandomPoints(50, r);

            var xyData = InterpolateData((Mesh)mesh, data, xy);

            double error = xy.Max(p => Math.Abs(xyData[p.ID] - F(p)));

            // L2 error
            //double error = Math.Sqrt(xy.Sum(p => Math.Pow(xyData[p.ID] - F(p), 2)));

            // Define tolerance dependent on mesh dimensions and size.
            double tolerance = 0.5 * Math.Max(r.Width, r.Height) / SIZE;

            return error < tolerance;
        }

        private static IMesh GetStructuredDataMesh(Rectangle domain)
        {
            var mesh = GenericMesher.StructuredMesh(domain, SIZE, SIZE);

            mesh.Renumber();

            return mesh;
        }

        private static IMesh GetScatteredDataMesh(Rectangle domain)
        {
            var r = new Rectangle(domain);

            double h = domain.Width / SIZE;

            // Generate a rectangle boundary point set (SIZE points on each side).
            var input = Generate.Rectangle(r, h);

            // Making sure we add some margin to the boundary.
            h = -h / 2;
            r.Resize(h, h);

            int n = Math.Max(1, SIZE * SIZE - input.Points.Count);

            // Add more input points (more sampling points, better interpolation).
            input.Points.AddRange(Generate.RandomPoints(n, r));

            var mesher = new GenericMesher(new Dwyer());

            // Generate mesh.
            var mesh = mesher.Triangulate(input.Points);

            mesh.Renumber();

            return mesh;
        }

        private static double[] GetFunctionValues(ICollection<Vertex> vertices)
        {
            var data = new double[vertices.Count];

            foreach (var item in vertices)
            {
                data[item.ID] = F(item);
            }

            return data;
        }

        private static double[] InterpolateData(Mesh mesh, double[] data, IEnumerable<Point> xy)
        {
            // The interpolated values.
            var values = new double[xy.Count()];

            var qtree = new TriangleQuadTree(mesh);

            int i = 0;

            foreach (var p in xy)
            {
                var tri = qtree.Query(p.X, p.Y);

                // For easy access of the interpolated values.
                p.ID = i;

                if (tri == null)
                {
                    values[i] = float.NaN;
                }
                else
                {
                    values[i] = Interpolation.InterpolatePoint(tri, p, data);
                }

                i++;
            }

            return values;
        }
    }
}