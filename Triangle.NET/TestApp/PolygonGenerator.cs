// -----------------------------------------------------------------------
// <copyright file="PolygonGenerator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using MeshExplorer.IO;
    using TriangleNet.IO;
    using MeshExplorer.Rendering;
    using TriangleNet.Geometry;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class PolygonGenerator
    {
        public static InputGeometry StarInBox(int n)
        {
            InputGeometry input = new InputGeometry(n + 4);

            input.AddPoint(0, 0); // Center

            double x, y, r, e, step = 2 * Math.PI / n;

            for (int i = 0; i < n; i++)
            {
                e = Util.Random.NextDouble() * step * 0.7;
                r = (Util.Random.NextDouble() + 0.7) * 0.5;
                x = r * Math.Cos(i * step + e);
                y = r * Math.Sin(i * step + e);

                input.AddPoint(x, y);
                input.AddSegment(0, i + 1);
            }

            input.AddPoint(-1, -1); // Box
            input.AddPoint(1, -1);
            input.AddPoint(1, 1);
            input.AddPoint(-1, 1);

            n = input.Count;
            input.AddSegment(n - 1, n - 2);
            input.AddSegment(n - 2, n - 3);
            input.AddSegment(n - 3, n - 4);
            input.AddSegment(n - 4, n - 1);

            return input;
        }


        public static InputGeometry CreateRandomPoints(int numPoints, int width, int height)
        {
            InputGeometry input = new InputGeometry(numPoints);

            for (int i = 0; i < numPoints; i++)
            {
                input.AddPoint(Util.Random.NextDouble() * width,
                        Util.Random.NextDouble() * height);
            }

            return input;
        }

        public static InputGeometry CreateCirclePoints(double x, double y, double r, int n)
        {
            InputGeometry input = new InputGeometry(n + 1);

            // Add center
            input.AddPoint(x, y);

            double angle = 0, step = 2 * Math.PI / n;

            while (angle < 2 * Math.PI)
            {
                input.AddPoint(r * Math.Cos(angle), r * Math.Sin(angle));
                angle += step;
            }

            return input;
        }

        public static InputGeometry CreateStarPoints(double x, double y, double r, int n)
        {
            InputGeometry input = new InputGeometry(n + 1);

            // Add center
            input.AddPoint(x, y);

            double angle = 0, step = 2 * Math.PI / n;

            while (angle < 2 * Math.PI)
            {
                input.AddPoint(r * Math.Cos(angle), r * Math.Sin(angle));
                angle += step;
            }

            angle = step / 2;
            r /= 1.5;

            while (angle < 2 * Math.PI)
            {
                input.AddPoint(r * Math.Cos(angle), r * Math.Sin(angle));
                angle += step;
            }

            return input;
        }
    }
}
