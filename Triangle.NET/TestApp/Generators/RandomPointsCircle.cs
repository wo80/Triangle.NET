// -----------------------------------------------------------------------
// <copyright file="RandomPointsCircle.cs" company="">
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
    /// Simple random points generator (points distributed in a circle).
    /// </summary>
    public class RandomPointsCircle : IGenerator
    {
        public string Name
        {
            get { return "Random Points (Circle)"; }
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
                return "Distribution:";
            }

            return "";
        }

        public string ParameterDescription(int paramIndex, double paramValue)
        {
            if (paramIndex == 1)
            {
                int numPoints = (int)((5000.0 - 5.0) / 100.0 * paramValue + 5.0);
                numPoints = (numPoints / 10) * 10;

                if (numPoints < 5)
                {
                    numPoints = 5;
                }

                return numPoints.ToString();
            }

            if (paramIndex == 2)
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

        public InputGeometry Generate(double param1, double param2, double param3)
        {
            int numPoints = (int)((5000.0 - 5.0) / 100.0 * param1 + 5.0);
            numPoints = (numPoints / 10) * 10;

            if (numPoints < 5)
            {
                numPoints = 5;
            }

            double exp = (param2 + 10) / 100;

            InputGeometry input = new InputGeometry(numPoints);

            double r, phi, radius = 100;

            for (int i = 0; i < numPoints; i++)
            {
                // Use sqrt(rand) to get normal distribution right.
                r = Math.Pow(Util.Random.NextDouble(), exp) * radius; 
                phi = Util.Random.NextDouble() * Math.PI * 2;

                input.AddPoint(r * Math.Cos(phi), r * Math.Sin(phi));
            }

            return input;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
