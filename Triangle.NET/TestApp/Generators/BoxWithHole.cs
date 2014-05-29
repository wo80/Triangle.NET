// -----------------------------------------------------------------------
// <copyright file="StarInBox.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.Generators
{
    using System;
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

        public override IPolygon Generate(double param0, double param1, double param2)
        {
            int numPoints = GetParamValueInt(1, param1);

            var input = new Polygon(numPoints + 4);

            double x, y, step = 2 * Math.PI / numPoints;

            double r = GetParamValueInt(2, param2);

            // Generate circle
            for (int i = 0; i < numPoints; i++)
            {
                x = r * Math.Cos(i * step);
                y = r * Math.Sin(i * step);

                input.Add(new Vertex(x, y, 2));
                input.Add(new Edge(i, (i + 1) % numPoints, 2));
            }

            numPoints = input.Points.Count;

            int numPointsB = GetParamValueInt(0, param0);

            // Box sides are 100 units long
            step = 100.0 / numPointsB;

            // Left box boundary points
            for (int i = 0; i < numPointsB; i++)
            {
                input.Add(new Vertex(-50, -50 + i * step, 1));
            }

            // Top box boundary points
            for (int i = 0; i < numPointsB; i++)
            {
                input.Add(new Vertex(-50 + i * step, 50, 1));
            }

            // Right box boundary points
            for (int i = 0; i < numPointsB; i++)
            {
                input.Add(new Vertex(50, 50 - i * step, 1));
            }

            // Bottom box boundary points
            for (int i = 0; i < numPointsB; i++)
            {
                input.Add(new Vertex(50 - i * step, -50, 1));
            }

            // Add box segments
            for (int i = numPoints; i < input.Count - 1; i++)
            {
                input.Add(new Edge(i, i + 1, 1));
            }

            // Add last segments which closes the box
            input.Add(new Edge(input.Count - 1, numPoints, 1));

            // Add hole
            input.Holes.Add(new Point(0, 0));

            return input;
        }
    }
}
