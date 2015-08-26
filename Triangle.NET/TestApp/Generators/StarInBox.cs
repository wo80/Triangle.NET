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

        public override IPolygon Generate(double param0, double param1, double param2)
        {
            int numRays = GetParamValueInt(0, param0);

            var g = new Polygon(numRays + 4);

            g.Add(new Vertex(0, 0)); // Center

            double x, y, r, e, step = 2 * Math.PI / numRays;

            for (int i = 0; i < numRays; i++)
            {
                e = Util.Random.NextDouble() * step * 0.7;
                r = (Util.Random.NextDouble() + 0.7) * 0.5;
                x = r * Math.Cos(i * step + e);
                y = r * Math.Sin(i * step + e);

                g.Add(new Vertex(x, y, 2));
                g.Add(new Segment(g.Points[0], g.Points[i + 1], 2));
            }

            g.Add(new Vertex(-1, -1, 1)); // Box
            g.Add(new Vertex(1, -1, 1));
            g.Add(new Vertex(1, 1, 1));
            g.Add(new Vertex(-1, 1, 1));

            numRays = g.Count;
            g.Add(new Segment(g.Points[numRays - 1], g.Points[numRays - 2], 1));
            g.Add(new Segment(g.Points[numRays - 2], g.Points[numRays - 3], 1));
            g.Add(new Segment(g.Points[numRays - 3], g.Points[numRays - 4], 1));
            g.Add(new Segment(g.Points[numRays - 4], g.Points[numRays - 1], 1));

            return g;
        }
    }
}
