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
    public class RingPolygon : BaseGenerator
    {
        public RingPolygon()
        {
            name = "Ring";
            description = "";
            parameter = 2;

            descriptions[0] = "Number of points:";
            descriptions[1] = "Variation:";

            ranges[0] = new int[] { 50, 250 };
            ranges[1] = new int[] { 0, 1 };
        }

        public override string ParameterDescription(int paramIndex, double paramValue)
        {
            if (paramIndex == 0)
            {
                int numRays = GetParamValueInt(paramIndex, paramValue);
                return numRays.ToString();
            }

            if (paramIndex == 1)
            {
                double variation = GetParamValueDouble(paramIndex, paramValue);
                return variation.ToString("0.0", Util.Nfi);
            }

            return "";
        }

        public override InputGeometry Generate(double param0, double param1, double param2)
        {
            int n = GetParamValueInt(0, param0);
            int m = n / 2;

            InputGeometry input = new InputGeometry(n + 1);

            double ro, r = 10;
            double step = 2 * Math.PI / m;

            // Inner ring
            for (int i = 0; i < m; i++)
            {
                input.AddPoint(r * Math.Cos(i * step), r * Math.Sin(i * step));
                input.AddSegment(i, (i + 1) % m);
            }

            r = 1.5 * r;


            step = 2 * Math.PI / n;
            double offset = step / 2;

            // Outer ring
            for (int i = 0; i < n; i++)
            {
                ro = r;

                if (i % 2 == 0)
                {
                    ro = r + r * Util.Random.NextDouble() * (param1 / 100);
                }

                input.AddPoint(ro * Math.Cos(i * step + offset), ro * Math.Sin(i * step + offset));
                input.AddSegment(m + i, m + ((i + 1) % n));
            }

            input.AddHole(0, 0);

            return input;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
