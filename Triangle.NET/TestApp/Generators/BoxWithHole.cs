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
    public class BoxWithHole : BaseGenerator
    {
        public BoxWithHole()
        {
            name = "Box with Hole";
            description = "";
            parameter = 3;

            descriptions[0] = "Points on box sides:";
            descriptions[1] = "Points on hole:";
            descriptions[2] = "Radius:";

            ranges[0] = new int[] { 5, 50 };
            ranges[1] = new int[] { 10, 200 };
            ranges[2] = new int[] { 5, 20 };
        }

        public override InputGeometry Generate(double param0, double param1, double param2)
        {
            int numPoints = GetParamValueInt(1, param1);

            InputGeometry input = new InputGeometry(numPoints + 4);

            double x, y, step = 2 * Math.PI / numPoints;

            double r = GetParamValueInt(2, param2);

            // Generate circle
            for (int i = 0; i < numPoints; i++)
            {
                x = r * Math.Cos(i * step);
                y = r * Math.Sin(i * step);

                input.AddPoint(x, y, 2);
                input.AddSegment(i, (i + 1) % numPoints, 2);
            }

            numPoints = input.Count;

            int numPointsB = GetParamValueInt(0, param0);

            // Box sides are 100 units long
            step = 100.0 / numPointsB;

            // Left box boundary points
            for (int i = 0; i < numPointsB; i++)
            {
                input.AddPoint(-50, -50 + i * step, 1);
            }

            // Top box boundary points
            for (int i = 0; i < numPointsB; i++)
            {
                input.AddPoint(-50 + i * step, 50, 1);
            }

            // Right box boundary points
            for (int i = 0; i < numPointsB; i++)
            {
                input.AddPoint(50, 50 - i * step, 1);
            }

            // Bottom box boundary points
            for (int i = 0; i < numPointsB; i++)
            {
                input.AddPoint(50 - i * step, -50, 1);
            }

            // Add box segments
            for (int i = numPoints; i < input.Count - 1; i++)
            {
                input.AddSegment(i, i + 1, 1);
            }

            // Add last segments which closes the box
            input.AddSegment(input.Count - 1, numPoints, 1);

            // Add hole
            input.AddHole(0, 0);

            return input;
        }
    }
}
