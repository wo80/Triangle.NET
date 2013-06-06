// -----------------------------------------------------------------------
// <copyright file="RandomPoints.cs" company="">
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
    /// Simple random points generator.
    /// </summary>
    public class RandomPoints : BaseGenerator
    {
        public RandomPoints()
        {
            name = "Random Points";
            description = "";
            parameter = 3;

            descriptions[0] = "Number of points:";
            descriptions[1] = "Width:";
            descriptions[2] = "Height:";

            ranges[0] = new int[] { 5, 5000 };
            ranges[1] = new int[] { 10, 200 };
            ranges[2] = new int[] { 10, 200 };
        }

        public override InputGeometry Generate(double param0, double param1, double param2)
        {
            int numPoints = GetParamValueInt(0, param0);
            numPoints = (numPoints / 10) * 10;

            if (numPoints < 5)
            {
                numPoints = 5;
            }

            InputGeometry input = new InputGeometry(numPoints);

            int width = GetParamValueInt(1, param1);
            int height = GetParamValueInt(2, param2);

            for (int i = 0; i < numPoints; i++)
            {
                input.AddPoint(Util.Random.NextDouble() * width,
                        Util.Random.NextDouble() * height);
            }

            return input;
        }
    }
}
