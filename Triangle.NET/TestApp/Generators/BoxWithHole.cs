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
            int n = GetParamValueInt(1, param1);

            var input = new Polygon(n + 4);

            double r = GetParamValueInt(2, param2);

            // Generate circle (hole)
            input.AddContour(CreateCircle(r, n, 1), 1, new Point(0, 0));

            n = GetParamValueInt(0, param0);

            // Generate box
            input.AddContour(CreateRectangle(new Rectangle(-50, -50, 100, 100), n, 2), 2);

            return input;
        }
    }
}
