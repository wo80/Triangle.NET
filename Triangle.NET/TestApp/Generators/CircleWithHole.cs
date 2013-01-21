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
    public class CircleWithHole : IGenerator
    {
        // Parameters range
        double[] range1 = { 100, 250 };
        double[] range2 = { 2, 15 };

        public string Name
        {
            get { return "Circle with Hole"; }
        }

        public string Description
        {
            get { return ""; }
        }

        public int ParameterCount
        {
            get { return 2; }
        }

        public string ParameterDescription(int paramIndex)
        {
            if (paramIndex == 1)
            {
                return "Number of points:";
            }

            if (paramIndex == 2)
            {
                return "Outer radius:";
            }

            return "";
        }

        public string ParameterDescription(int paramIndex, double paramValue)
        {
            if (paramIndex == 1)
            {
                int num = (int)((range1[1] - range1[0]) / 100.0 * paramValue + range1[0]);

                return num.ToString();
            }

            if (paramIndex == 2)
            {
                int radius = (int)((range2[1] - range2[0]) / 100.0 * paramValue + range2[0]);

                return radius.ToString();
            }

            return "";
        }

        public InputGeometry Generate(double param1, double param2, double param3)
        {
            // Number of points on the outer circle
            int n = (int)((range1[1] - range1[0]) / 100.0 * param1 + range1[0]);
            int count, npoints;

            double radius = (int)((range2[1] - range2[0]) / 100.0 * param2 + range2[0]);

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

            // Regions: |------|------|---|
            //          r             1   0

            input.AddRegion((r + 3.0) / 4.0, 0, 1);
            input.AddRegion((3 * r + 1.0) / 4.0, 0, 2);

            return input;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
