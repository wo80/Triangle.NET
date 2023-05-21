
namespace TriangleNet.Examples
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TriangleNet.Geometry;

    /// <summary>
    /// Troubleshooting: finding degenerate boundary triangles.
    /// </summary>
    public class Example10 : IExample
    {
        public bool Run(bool print = false)
        {
            var pts = new List<Vertex>
            {
                // The 4 corners of the rectangle.
                new Vertex(1.5, 1.0),
                new Vertex(1.5, -1.0),
                new Vertex(-1.5, -1.0),
                new Vertex(-1.5, 1.0),

                // The edge midpoints.
                new Vertex(0.0, 1.0),
                new Vertex(0.0, -1.0),
                new Vertex(1.5, 0.0),
                new Vertex(-1.5, 0.0)
            };

            var r = new Random(78403);

            // The original rectangle.
            var poly = Rotate(pts, 0);

            for (int i = 0; i < 10; i++)
            {
                var mesh = poly.Triangulate();

                var list = MeshValidator.GetDegenerateBoundaryTriangles(mesh);

                if (print && list.Any())
                {
                    Console.WriteLine("Iteration {0}: found {1} degenerate triangle(s) of {2}.",
                        i, list.Count(), mesh.Triangles.Count);

                    foreach (var t in list)
                    {
                        Console.WriteLine("   [{0} {1} {2}]",
                            t.GetVertexID(0),
                            t.GetVertexID(1),
                            t.GetVertexID(2));
                    }
                }

                // Random rotation.
                poly = Rotate(pts, Math.PI * r.NextDouble());
            }

            return true;
        }

        /// <summary>
        /// Rotate given point set around the origin.
        /// </summary>
        private static IPolygon Rotate(List<Vertex> points, double radians)
        {
            var poly = new Polygon(points.Count);

            int id = 0;

            foreach (var p in points)
            {
                double x = p.X;
                double y = p.Y;

                double s = Math.Sin(radians);
                double c = Math.Cos(radians);

                double xr = c * x - s * y;
                double yr = s * x + c * y;

                poly.Points.Add(new Vertex(xr, yr) { ID = id++ });
            }

            return poly;
        }
    }
}
