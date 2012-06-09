// -----------------------------------------------------------------------
// <copyright file="RandomPoints.cs" company="">
// TODO: Update copyright text.
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
    /// Simple random points generator.
    /// </summary>
    public class RandomPoints : IGenerator
    {
        public string Name
        {
            get { return "Random Points"; }
        }

        public string Description
        {
            get { return ""; }
        }

        public int ParameterCount
        {
            get { return 1; }
        }

        public string ParameterDescription(int paramIndex)
        {
            if (paramIndex == 1)
            {
                return "Number of points:";
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

            InputGeometry input = new InputGeometry(numPoints);

            int width = Util.Random.Next(100, 200);
            int height = Util.Random.Next(100, 200);

            for (int i = 0; i < numPoints; i++)
            {
                input.AddPoint(Util.Random.NextDouble() * width,
                        Util.Random.NextDouble() * height);
            }

            return input;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
