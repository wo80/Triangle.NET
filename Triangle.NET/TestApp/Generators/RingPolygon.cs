// -----------------------------------------------------------------------
// <copyright file="RingPolygon.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.Generators
{
    using System;
    using System.Collections.Generic;
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

        public override IPolygon Generate(double param0, double param1, double param2)
        {
            int n = GetParamValueInt(0, param0);
            int m = n / 2;

            var input = new Polygon(n + 1);

            double ro, r = 10;
            double step = 2 * Math.PI / m;

            var inner = new List<Vertex>(m);

            // Inner ring
            for (int i = 0; i < m; i++)
            {
                inner.Add(new Vertex(r * Math.Cos(i * step), r * Math.Sin(i * step)));
            }

            input.AddContour(inner, 1);

            r = 1.5 * r;

            var outer = new List<Vertex>(n);

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

                outer.Add(new Vertex(ro * Math.Cos(i * step + offset), ro * Math.Sin(i * step + offset)));
            }

            input.AddContour(outer, 2);

            input.Holes.Add(new Point(0, 0));

            return input;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
