
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
            var points = new List<Vertex>(4)
            {
                new Vertex(left, top, mark),
                new Vertex(right, top, mark),
                new Vertex(right, bottom, mark),
                new Vertex(left, bottom, mark)
            };

            return new Contour(points, mark, true);
        }

        public static Polygon SplitRectangle(double left, double top,
            double right, double bottom, int mark = 0)
        {
            double midX = (right - left) / 2;
            double midY = (top - bottom) / 2;

            var midTop = new Vertex(left + midX, top, mark);
            var midBottom = new Vertex(left + midX, bottom, mark);

            var points = new List<Vertex>(4)
            {
                new Vertex(left, top, mark),
                midTop,
                new Vertex(right, top, mark),
                new Vertex(right, bottom, mark),
                midBottom,
                new Vertex(left, bottom, mark)
            };

            var poly = new Polygon();

            poly.Add(new Contour(points, mark, true));
            poly.Add(new Segment(midTop, midBottom));

            poly.Regions.Add(new RegionPointer(left + midX / 2, bottom + midY, 1));
            poly.Regions.Add(new RegionPointer(right - midX / 2, bottom + midY, 2));

            return poly;
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
