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
    public class StarInBox : BaseGenerator
    {
        public StarInBox()
        {
            name = "Star in Box";
            description = "";
            parameter = 1;

            descriptions[0] = "Number of rays:";

            ranges[0] = new int[] { 3, 61 };
        }

        public override InputGeometry Generate(double param0, double param1, double param2)
        {
            int numRays = GetParamValueInt(0, param0);

            InputGeometry input = new InputGeometry(numRays + 4);

            input.AddPoint(0, 0); // Center

            double x, y, r, e, step = 2 * Math.PI / numRays;

            for (int i = 0; i < numRays; i++)
            {
                e = Util.Random.NextDouble() * step * 0.7;
                r = (Util.Random.NextDouble() + 0.7) * 0.5;
                x = r * Math.Cos(i * step + e);
                y = r * Math.Sin(i * step + e);

                input.AddPoint(x, y, 2);
                input.AddSegment(0, i + 1, 2);
            }

            input.AddPoint(-1, -1, 1); // Box
            input.AddPoint(1, -1, 1);
            input.AddPoint(1, 1, 1);
            input.AddPoint(-1, 1, 1);

            numRays = input.Count;
            input.AddSegment(numRays - 1, numRays - 2, 1);
            input.AddSegment(numRays - 2, numRays - 3, 1);
            input.AddSegment(numRays - 3, numRays - 4, 1);
            input.AddSegment(numRays - 4, numRays - 1, 1);

            return input;
        }
    }
}
