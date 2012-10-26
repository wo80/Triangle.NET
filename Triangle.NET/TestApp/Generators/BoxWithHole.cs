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
    public class BoxWithHole : IGenerator
    {
        public string Name
        {
            get { return "Box with Hole"; }
        }

        public string Description
        {
            get { return ""; }
        }

        public int ParameterCount
        {
            get { return 3; }
        }

        public string ParameterDescription(int paramIndex)
        {
            if (paramIndex == 1)
            {
                return "Points on box sides:";
            }

            if (paramIndex == 2)
            {
                return "Points on hole:";
            }

            if (paramIndex == 3)
            {
                return "Radius:";
            }

            return "";
        }

        public string ParameterDescription(int paramIndex, double paramValue)
        {
            if (paramIndex == 1)
            {
                int numPoints = (int)((50.0 - 5.0) / 100.0 * paramValue + 5.0);

                if (numPoints < 5)
                {
                    numPoints = 5;
                }

                return numPoints.ToString();
            }

            if (paramIndex == 2)
            {
                int numPoints = (int)((100.0 - 10.0) / 100.0 * paramValue + 10.0);
                numPoints = (numPoints / 5) * 5;

                if (numPoints < 10)
                {
                    numPoints = 10;
                }

                return numPoints.ToString();
            }

            if (paramIndex == 3)
            {
                int radius = (int)((20.0 - 5.0) / 100.0 * paramValue + 5.0);

                return radius.ToString();
            }

            return "";
        }

        public InputGeometry Generate(double param1, double param2, double param3)
        {
            int numPoints = (int)((100.0 - 10.0) / 100.0 * param2 + 10.0);

            InputGeometry input = new InputGeometry(numPoints + 4);

            double x, y, step = 2 * Math.PI / numPoints;

            double r = (int)((20.0 - 5.0) / 100.0 * param3 + 5.0);

            // Generate circle
            for (int i = 0; i < numPoints; i++)
            {
                x = r * Math.Cos(i * step);
                y = r * Math.Sin(i * step);

                input.AddPoint(x, y, 2);
                input.AddSegment(i, (i + 1) % numPoints, 2);
            }

            numPoints = input.Count;

            int numPointsB = (int)((50.0 - 5.0) / 100.0 * param1 + 5.0);

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

        public override string ToString()
        {
            return this.Name;
        }
    }
}
