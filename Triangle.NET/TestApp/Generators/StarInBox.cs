// -----------------------------------------------------------------------
// <copyright file="StarInBox.cs" company="">
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
    /// Generates a star contained in a box.
    /// </summary>
    public class StarInBox : IGenerator
    {
        public string Name
        {
            get { return "Star in Box"; }
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
                return "Number of rays:";
            }

            return "";
        }

        public string ParameterDescription(int paramIndex, double paramValue)
        {
            if (paramIndex == 1)
            {
                int numRays = (int)((61.0 - 3.0) / 100.0 * paramValue + 3.0);

                return numRays.ToString();
            }

            return "";
        }

        public InputGeometry Generate(double param1, double param2, double param3)
        {
            int numRays = (int)((61.0 - 3.0) / 100.0 * param1 + 3.0);

            InputGeometry input = new InputGeometry(numRays + 4);

            input.AddPoint(0, 0); // Center

            double x, y, r, e, step = 2 * Math.PI / numRays;

            for (int i = 0; i < numRays; i++)
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

            numRays = input.Count;
            input.AddSegment(numRays - 1, numRays - 2);
            input.AddSegment(numRays - 2, numRays - 3);
            input.AddSegment(numRays - 3, numRays - 4);
            input.AddSegment(numRays - 4, numRays - 1);

            return input;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
