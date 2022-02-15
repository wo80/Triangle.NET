
namespace TriangleNet
{
    using System;
    using System.Collections.Generic;
    using TriangleNet.Geometry;

    static class Generate
    {
        private const int RANDOM_SEED = 63841;

        public static List<Vertex> RandomPoints(int n, Rectangle bounds,
            int seed = RANDOM_SEED)
        {
            var points = new List<Vertex>(n);

            var xmin = bounds.Left;
            var ymin = bounds.Bottom;

            var width = bounds.Width;
            var height = bounds.Height;

            var random = new Random(seed);

            for (int i = 0; i < n; i++)
            {
                double x = random.NextDouble();
                double y = random.NextDouble();
                points.Add(new Vertex(xmin + x * width, ymin + y * height));
            }

            return points;
        }

        public static Contour Rectangle(double left, double top,
            double right, double bottom, int mark = 0)
        {
            var points = new List<Vertex>(4);

            points.Add(new Vertex(left, top, mark));
            points.Add(new Vertex(right, top, mark));
            points.Add(new Vertex(right, bottom, mark));
            points.Add(new Vertex(left, bottom, mark));

            return new Contour(points, mark, true);
        }

        /// <summary>
        /// Create a circular contour.
        /// </summary>
        /// <param name="r">The radius.</param>
        /// <param name="center">The center point.</param>
        /// <param name="h">The desired segment length.</param>
        /// <param name="label">The boundary label.</param>
        /// <returns>A circular contour.</returns>
        public static Contour Circle(double r, Point center, double h, int label = 0)
        {
            int n = (int)(2 * Math.PI * r / h);

            var points = new List<Vertex>(n);

            double x, y, dphi = 2 * Math.PI / n;

            for (int i = 0; i < n; i++)
            {
                x = center.X + r * Math.Cos(i * dphi);
                y = center.Y + r * Math.Sin(i * dphi);

                points.Add(new Vertex(x, y, label));
            }

            return new Contour(points, label, true);
        }
    }
}
