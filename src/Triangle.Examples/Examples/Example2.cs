
namespace TriangleNet.Examples
{
    using System.Collections.Generic;
    using TriangleNet.Geometry;
    using TriangleNet.Meshing;
    using TriangleNet.Rendering.Text;

    /// <summary>
    /// Simple point set triangulation with convex hull.
    /// </summary>
    public class Example2 : IExample
    {
        public bool Run(bool print = false)
        {
            const int N = 50;

            // Generate point set.
            var points = Generate.RandomPoints(N, new Rectangle(0, 0, 100, 100));

            // We use a polygon as input to enable segment insertion on the convex hull.
            var poly = new Polygon(N);

            poly.Points.AddRange(points);

            // Set the 'convex' option to enclose the convex hull with segments.
            var options = new ConstraintOptions() { Convex = true };

            // Generate mesh.
            var mesh = poly.Triangulate(options);

            if (print) SvgImage.Save(mesh, "example-2.svg", 500);

            return CheckConvexHull(mesh.Segments);
        }

        private static bool CheckConvexHull(IEnumerable<ISegment> segments)
        {
            int first = -1, prev = -1;

            Point a = null, b, c;

            var p = RobustPredicates.Default;

            foreach (var s in segments)
            {
                // If first loop ...
                if (first < 0)
                {
                    // initialize vertex ids of first segment and ...
                    first = s.P1;
                    prev = s.P0;

                    // initialize first segment endpoint.
                    a = s.GetVertex(1);

                    continue;
                }

                // Check whether segments are returned in consecutive order.
                if (prev != s.P1)
                {
                    return false;
                }

                b = s.GetVertex(1);
                c = s.GetVertex(0);

                // Check whether the convex hull is traversed in counterclockwise.
                if (p.CounterClockwise(a, b, c) < 0)
                {
                    return false;
                }

                prev = s.P0;
                a = b;
            }

            // Check whether the last segment connects to the first.
            return prev == first;
        }
    }
}
