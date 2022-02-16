
namespace TriangleNet.Tests
{
    using System.Collections.Generic;
    using TriangleNet.Geometry;
    using TriangleNet.Topology;

    static class Helper
    {
        public static Contour Rectangle(double left, double top,
            double right, double bottom, int mark = 0)
        {
            var points = new List<Vertex>(4);

            points.Add(new Vertex(left, top, mark));
            points.Add(new Vertex(right, top, mark));
            points.Add(new Vertex(right, bottom, mark));
            points.Add(new Vertex(left, bottom, mark));

            return new Contour(points, mark, true);
        }

        public static Triangle CreateTriangle(int id, Vertex org, Vertex dest, Vertex apex)
        {
            var t = new Triangle() { id = id, hash = id };

            // Node ordering 'plus 1 mod 3'.
            t.vertices[0] = apex;
            t.vertices[1] = org;
            t.vertices[2] = dest;

            return t;
        }
    }
}
