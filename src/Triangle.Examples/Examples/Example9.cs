﻿
namespace TriangleNet.Examples
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TriangleNet.Geometry;

    /// <summary>
    /// Troubleshooting: finding degenerate boundary triangles.
    /// </summary>
    public class Example9
    {
        public static void Run()
        {
            var pts = new List<Vertex>
            {
                // The 4 corners of the square.
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

                if (list.Any())
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

                double xr = Math.Cos(radians) * x - Math.Sin(radians) * y;
                double yr = Math.Sin(radians) * x + Math.Cos(radians) * y;

                poly.Points.Add(new Vertex(xr, yr) { ID = id++ });
            }

            return poly;
        }
    }
}