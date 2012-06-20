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
    public class RingPolygon : IGenerator
    {
        public string Name
        {
            get { return "Ring"; }
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
                return "Variation:";
            }

            return "";
        }

        public string ParameterDescription(int paramIndex, double paramValue)
        {
            if (paramIndex == 1)
            {
                int numRays = (int)((250.0 - 50.0) / 100.0 * paramValue + 50.0);

                return numRays.ToString();
            }

            if (paramIndex == 2)
            {
                double variation = (paramValue + 1) / 100;

                return variation.ToString("0.0", Util.Nfi);
            }

            return "";
        }

        public InputGeometry Generate(double param1, double param2, double param3)
        {
            int n = (int)((250.0 - 50.0) / 100.0 * param1 + 50.0);
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
                    ro = r + r * Util.Random.NextDouble() * (param2 / 100);
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
