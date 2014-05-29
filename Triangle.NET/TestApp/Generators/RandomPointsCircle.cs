// -----------------------------------------------------------------------
// <copyright file="RandomPointsCircle.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.Generators
{
    using System;
    using TriangleNet.Geometry;

    /// <summary>
    /// Simple random points generator (points distributed in a circle).
    /// </summary>
    public class RandomPointsCircle : BaseGenerator
    {
        public RandomPointsCircle()
        {
            name = "Random Points (Circle)";
            description = "";
            parameter = 2;

            descriptions[0] = "Number of points:";
            descriptions[1] = "Distribution:";

            ranges[0] = new int[] { 5, 5000 };
            ranges[1] = new int[] { 0, 1 };
        }

        public override string ParameterDescription(int paramIndex, double paramValue)
        {
            if (paramIndex == 0)
            {
                int numPoints = GetParamValueInt(paramIndex, paramValue);
                numPoints = (numPoints / 10) * 10;

                if (numPoints < 5)
                {
                    numPoints = 5;
                }

                return numPoints.ToString();
            }

            if (paramIndex == 1)
            {
                double exp = (paramValue + 10) / 100;

                if (exp > 1.092)
                {
                    exp = 1.1;
                }

                return exp.ToString("0.00", Util.Nfi);
            }

            return "";
        }

        public override IPolygon Generate(double param0, double param1, double param2)
        {
            int numPoints = GetParamValueInt(0, param0);
            numPoints = (numPoints / 10) * 10;

            if (numPoints < 5)
            {
                numPoints = 5;
            }

            double exp = (param1 + 10) / 100;

            var input = new Polygon(numPoints);

            int i = 0, cNum = 2 * (int)Math.Floor(Math.Sqrt(numPoints));

            double r, phi, radius = 100, step = 2 * Math.PI / cNum;

            // Distrubute points equally on circle border
            for (; i < cNum; i++)
            {
                // Add a little error
                r = Util.Random.NextDouble();

                input.Add(new Vertex((radius + r) * Math.Cos(i * step),
                    (radius + r) * Math.Sin(i * step)));
            }

            for (; i < numPoints; i++)
            {
                // Use sqrt(rand) to get normal distribution right.
                r = Math.Pow(Util.Random.NextDouble(), exp) * radius;
                phi = Util.Random.NextDouble() * Math.PI * 2;

                input.Add(new Vertex(r * Math.Cos(phi), r * Math.Sin(phi)));
            }

            return input;
        }
    }
}
