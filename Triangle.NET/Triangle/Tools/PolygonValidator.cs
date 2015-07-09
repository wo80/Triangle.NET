// -----------------------------------------------------------------------
// <copyright file="PolygonValidator.cs">
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Tools
{
    using System;
    using TriangleNet.Geometry;

    public static class PolygonValidator
    {
        /// <summary>
        /// Test the polygon for consistency.
        /// </summary>
        public static bool IsConsistent(IPolygon poly)
        {
            var logger = Log.Instance;

            int horrors = 0;

            int i = 0;
            int count = poly.Points.Count;

            foreach (var p in poly.Points)
            {
                if (p == null)
                {
                    horrors++;
                    logger.Warning(String.Format("Point {0} is null.", i), "PolygonValidator.IsConsistent()");
                }
                else if (double.IsNaN(p.x + p.y) || double.IsInfinity(p.x + p.y))
                {
                    horrors++;
                    logger.Warning(String.Format("Point {0} has invalid coordinates.", i), "PolygonValidator.IsConsistent()");
                }
                i++;
            }

            i = 0;

            foreach (var seg in poly.Segments)
            {
                if (seg == null)
                {
                    horrors++;
                    logger.Warning(String.Format("Segment {0} is null.", i), "PolygonValidator.IsConsistent()");
                }
                else
                {
                    if (seg.P0 < 0 || seg.P0 >= count)
                    {
                        horrors++;
                        logger.Warning(String.Format("Segment {0} has invalid startpoint.", i),
                            "PolygonValidator.IsConsistent()");
                    }

                    if (seg.P1 < 0 || seg.P1 >= count)
                    {
                        horrors++;
                        logger.Warning(String.Format("Segment {0} has invalid endpoint.", i),
                            "PolygonValidator.IsConsistent()");
                    }
                }

                i++;
            }

            return horrors == 0;
        }

        /// <summary>
        /// Test the polygon for duplicate vertices.
        /// </summary>
        public static bool HasDuplicateVertices(IPolygon poly)
        {
            var logger = Log.Instance;

            int horrors = 0;

            var points = poly.Points.ToArray();

            (new PointSorter()).Sort(points);

            for (int i = 1; i < points.Length; i++)
            {
                if (points[i - 1] == points[i])
                {
                    horrors++;
                    logger.Warning(String.Format("Found duplicate point {0}.", points[i]),
                        "PolygonValidator.HasDuplicateVertices()");
                }
            }

            return horrors == 0;
        }

        /// <summary>
        /// Test the polygon for 360 degree angles.
        /// </summary>
        public static bool HasBadAngles(IPolygon poly)
        {
            var logger = Log.Instance;

            int horrors = 0;
            int i = 0;

            Point p0 = null, p1 = null;
            Point q0, q1;

            int count = poly.Points.Count;

            foreach (var seg in poly.Segments)
            {
                q0 = p0;
                q1 = p1;

                p0 = (seg.P0 >= 0 && seg.P0 < count) ? poly.Points[seg.P0] : null;
                p1 = (seg.P1 >= 0 && seg.P1 < count) ? poly.Points[seg.P1] : null;

                if (q0 != null && q1 != null)
                {
                    // The two segments are connected.
                    if (p0 == q1 && p1 != null)
                    {
                        if (IsBadAngle(q0, p0, p1))
                        {
                            horrors++;
                            logger.Warning(String.Format("Bad segment angle found at index {0}.", i),
                                "PolygonValidator.HasBadAngles()");
                        }
                    }
                }

                i++;
            }

            return horrors == 0;
        }

        private static bool IsBadAngle(Point a, Point b, Point c, double eps = 0.0)
        {
            double x = DotProduct(a, b, c);
            double y = CrossProductLength(a, b, c);

            return Math.Abs(Math.Atan2(y, x)) <= eps;
        }

        //  Returns the dot product <AB, BC>.
        private static double DotProduct(Point a, Point b, Point c)
        {
            //  Calculate the dot product.
            return (a.X - b.X) * (c.X - b.X) + (a.Y - b.Y) * (c.Y - b.Y);
        }

        //  Returns the length of cross product AB x BC.
        private static double CrossProductLength(Point a, Point b, Point c)
        {
            //  Calculate the Z coordinate of the cross product.
            return (a.X - b.X) * (c.Y - b.Y) - (a.Y - b.Y) * (c.X - b.X);
        }
    }
}
