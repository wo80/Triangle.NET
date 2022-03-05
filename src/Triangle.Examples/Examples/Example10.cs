using System;
using System.Collections.Generic;
using System.Linq;
using TriangleNet.Geometry;
using TriangleNet.Meshing;
using TriangleNet.Meshing.Algorithm;
using TriangleNet.Rendering.Text;
using TriangleNet.Tools;

namespace TriangleNet.Examples
{
    /// <summary>
    /// Scattered data interpolation without USE_Z or USE_ATTRIBS.
    /// </summary>
    internal class Example10
    {
        // The function we are sampling.
        private static readonly Func<Point, double> F = p => Math.Sin(p.X) * Math.Cos(p.Y);

        public static bool Run(bool print = false)
        {
            // The input domain.
            var r = new Rectangle(0d, 0d, 10d, 10d);

            var mesh = GetScatteredDataMesh(r, out double[] data);
            //var mesh = GetStructuredDataMesh(r, out double[] data);

            if (print) SvgImage.Save(mesh, "example-10.svg", 500);

            // The points to interpolate.
            var xy = Generate.RandomPoints(50, r);

            var xyData = InterpolateData((Mesh)mesh, data, xy);

            double error = xy.Max(p => Math.Abs(xyData[p.ID] - F(p)));

            // L2 error
           // double error = Math.Sqrt(xy.Sum(p => Math.Pow(xyData[p.ID] - F(p), 2)));

            return error < 0.5;
        }

        private static IMesh GetStructuredDataMesh(Rectangle domain, out double[] data)
        {
            var mesh = GenericMesher.StructuredMesh(domain, 20, 20);

            mesh.Renumber();

            // Generate function values for mesh points.
            data = new double[mesh.Vertices.Count];

            foreach (var item in mesh.Vertices)
            {
                data[item.ID] = F(item);
            }

            return mesh;
        }

        private static IMesh GetScatteredDataMesh(Rectangle domain, out double[] data)
        {
            var r = new Rectangle(domain);

            double h = domain.Width / 20;

            // Generate a rectangle boundary point set (20 points on each side).
            var input = Generate.Rectangle(r, 0.5);

            // Making sure we add some margin to the boundary.
            h = -h / 2;
            r.Resize(h, h);

            // Add more input points (more sampling points, better interpolation).
            input.Points.AddRange(Generate.RandomPoints(350, r));

            var mesher = new GenericMesher(new Dwyer());

            // Generate mesh.
            var mesh = mesher.Triangulate(input.Points);

            mesh.Renumber();

            // Generate function values for mesh points.
            data = new double[mesh.Vertices.Count];

            foreach (var item in mesh.Vertices)
            {
                data[item.ID] = F(item);
            }

            return mesh;
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