// -----------------------------------------------------------------------
// <copyright file="RingPolygon.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TriangleNet.Geometry;

    /// <summary>
    /// Generates a ring polygon.
    /// </summary>
    public class CircleWithHole : BaseGenerator
    {
        public CircleWithHole()
        {
            name = "Circle with Hole";
            description = "";
            parameter = 2;

            descriptions[0] = "Number of points:";
            descriptions[1] = "Outer radius:";

            ranges[0] = new int[] { 100, 250 };
            ranges[1] = new int[] { 2, 15 };
        }

        public override InputGeometry Generate(double param0, double param1, double param2)
        {
            // Number of points on the outer circle
            int n = GetParamValueInt(0, param0);
            int count, npoints;

            double radius = GetParamValueInt(1, param1);

            // Step size on the outer circle
            double h = 2 * Math.PI * radius / n;

            // Current radius and step size
            double r, dphi;

            InputGeometry input = new InputGeometry(n + 1);

            // Inner cirlce (radius = 1)
            r = 1;
            npoints = (int)(2 * Math.PI * r / h);
            dphi = 2 * Math.PI / npoints;
            for (int i = 0; i < npoints; i++)
            {
                input.AddPoint(r * Math.Cos(i * dphi), r * Math.Sin(i * dphi), 1);
                input.AddSegment(i, (i + 1) % npoints, 1);
            }

            count = input.Count;

            // Center cirlce
            r = (radius + 1) / 2.0;
            npoints = (int)(2 * Math.PI * r / h);
            dphi = 2 * Math.PI / npoints;
            for (int i = 0; i < npoints; i++)
            {
                input.AddPoint(r * Math.Cos(i * dphi), r * Math.Sin(i * dphi), 2);
                input.AddSegment(count + i, count + (i + 1) % npoints, 2);
            }

            count = input.Count;

            // Outer cirlce
            r = radius;
            npoints = (int)(2 * Math.PI * r / h);
            dphi = 2 * Math.PI / npoints;
            for (int i = 0; i < npoints; i++)
            {
                input.AddPoint(r * Math.Cos(i * dphi), r * Math.Sin(i * dphi), 3);
                input.AddSegment(count + i, count + (i + 1) % npoints, 3);
            }

            input.AddHole(0, 0);

            // Regions: |++++++|++++++|---|
            //          r             1   0

            input.AddRegion((r + 3.0) / 4.0, 0, 1);
            input.AddRegion((3 * r + 1.0) / 4.0, 0, 2);

            return input;
        }
    }
}
