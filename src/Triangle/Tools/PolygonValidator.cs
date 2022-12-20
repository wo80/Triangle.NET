// -----------------------------------------------------------------------
// <copyright file="PolygonValidator.cs">
// Triangle.NET Copyright (c) 2012-2022 Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Tools
{
    using System;
    using System.Collections.Generic;
    using TriangleNet.Geometry;

    /// <summary>
    /// Polygon validation helper.
    /// </summary>
    public static class PolygonValidator
    {
        /// <summary>
        /// Test the polygon for consistency.
        /// </summary>
        public static bool IsConsistent(IPolygon poly)
        {
            var logger = Log.Instance;

            var points = poly.Points;

            int horrors = 0;

            int i = 0;
            int count = points.Count;

            if (count < 3)
            {
                logger.Warning("Polygon must have at least 3 vertices.", "PolygonValidator.IsConsistent()");
                return false;
            }

            foreach (var p in points)
            {
                if (p == null)
                {
                    horrors++;
                    logger.Warning(string.Format("Point {0} is null.", i), "PolygonValidator.IsConsistent()");
                }
                else if (double.IsNaN(p.x) || double.IsNaN(p.y))
                {
                    horrors++;
                    logger.Warning(string.Format("Point {0} has invalid coordinates.", i), "PolygonValidator.IsConsistent()");
                }
                else if (double.IsInfinity(p.x) || double.IsInfinity(p.y))
                {
                    horrors++;
                    logger.Warning(string.Format("Point {0} has invalid coordinates.", i), "PolygonValidator.IsConsistent()");
                }

                i++;
            }

            i = 0;

            foreach (var seg in poly.Segments)
            {
                if (seg == null)
                {
                    horrors++;
                    logger.Warning(string.Format("Segment {0} is null.", i), "PolygonValidator.IsConsistent()");

                    // Always abort if a NULL-segment is found.
                    return false;
                }

                var p = seg.GetVertex(0);
                var q = seg.GetVertex(1);

                if ((p.x == q.x) && (p.y == q.y))
                {
                    horrors++;
                    logger.Warning(string.Format("Endpoints of segment {0} are coincident (IDs {1} / {2}).", i, p.id, q.id),
                        "PolygonValidator.IsConsistent()");
                }

                i++;
            }

            if (points[0].id == points[1].id)
            {
                horrors += CheckVertexIDs(poly, count);
            }
            else
            {
                horrors += CheckDuplicateIDs(poly);
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

            VertexSorter.Sort(points);

            for (int i = 1; i < points.Length; i++)
            {
                if (points[i - 1] == points[i])
                {
                    horrors++;
                    logger.Warning(string.Format("Found duplicate point ({0}, {1}).", points[i].x, points[i].y),
                        "PolygonValidator.HasDuplicateVertices()");
                }
            }

            return horrors > 0;
        }

        /// <summary>
        /// Gets the minimum and maximum feature size (segment length) of the input polygon.
        /// </summary>
        /// <param name="poly">The polygon.</param>
        /// <param name="threshold">The ratio threshold (smallest to largest segment length, default = 2e-10).</param>
        /// <remarks>
        /// This method will also report zero-length segments. The method does NOT detect
        /// free vertices lying close to segments.
        /// </remarks>
        public static (double min, double max) GetSegmentRatio(IPolygon poly, double threshold = 2e-10)
        {
            var logger = Log.Instance;

            double min = double.MaxValue;
            double max = 0.0;

            foreach (var seg in poly.Segments)
            {
                var p = seg.GetVertex(0);
                var q = seg.GetVertex(1);

                var dx = p.X - q.X;
                var dy = p.Y - q.Y;

                var length = Math.Sqrt(dx * dx + dy * dy);

                if (length == 0.0)
                {
                    logger.Warning(string.Format("Found zero-length segment (vertex IDs {0} / {1}).", p.id, q.id),
                        "PolygonValidator.GetSegmentRatio()");
                    continue;
                }

                min = Math.Min(min, length);
                max = Math.Max(max, length);
            }

            double ratio = min / max;

            if (ratio < threshold)
            {
                logger.Warning(string.Format("Polygon has tiny segment ratio {0:G2}.", ratio),
                    "PolygonValidator.GetSegmentRatio()");
            }

            return (min, max);
        }

        /// <summary>
        /// Test the polygon for 360 degree angles.
        /// </summary>
        /// <param name="poly">The polygon.</param>
        /// <param name="threshold">The angle threshold.</param>
        /// <remarks>
        /// This method assumes that segments are stored in order. There may be different,
        /// unconnected contours in the polygon, but the segments of each contour have to
        /// be in order to get a meaningful result.
        /// </remarks>
        public static bool HasBadAngles(IPolygon poly, double threshold = 2e-12)
        {
            var logger = Log.Instance;

            int horrors = 0;
            int i = 0;

            Point p0 = null, p1 = null;
            Point q0, q1;

            foreach (var seg in poly.Segments)
            {
                q0 = p0; // Previous segment start point.
                q1 = p1; // Previous segment end point.

                p0 = seg.GetVertex(0); // Current segment start point.
                p1 = seg.GetVertex(1); // Current segment end point.

                if (p0 == p1 || q0 == q1)
                {
                    // Ignore zero-length segments.
                    continue;
                }

                if (q0 != null && q1 != null)
                {
                    // The two segments are connected.
                    if (p0 == q1 && p1 != null)
                    {
                        if (IsBadAngle(q0, p0, p1,threshold))
                        {
                            horrors++;
                            logger.Warning(string.Format("Bad segment angle found at index {0}.", i),
                                "PolygonValidator.HasBadAngles()");
                        }
                    }
                }

                i++;
            }

            return horrors > 0;
        }

        private static bool IsBadAngle(Point a, Point b, Point c, double threshold = 0.0)
        {
            double x = DotProduct(a, b, c);
            double y = CrossProductLength(a, b, c);

            return Math.Abs(Math.Atan2(y, x)) <= threshold;
        }

        //  Returns the dot product <AB, BC>.
        private static double DotProduct(Point a, Point b, Point c)
        {
            //  Calculate the dot product.
            return (a.x - b.x) * (c.x - b.x) + (a.y - b.y) * (c.y - b.y);
        }

        //  Returns the length of cross product AB x BC.
        private static double CrossProductLength(Point a, Point b, Point c)
        {
            //  Calculate the Z coordinate of the cross product.
            return (a.x - b.x) * (c.y - b.y) - (a.y - b.y) * (c.x - b.x);
        }

        private static int CheckVertexIDs(IPolygon poly, int count)
        {
            var logger = Log.Instance;

            int horrors = 0;

            int i = 0;

            Vertex p, q;

            foreach (var seg in poly.Segments)
            {
                p = seg.GetVertex(0);
                q = seg.GetVertex(1);

                if (p.id < 0 || p.id >= count)
                {
                    horrors++;
                    logger.Warning(string.Format("Segment {0} has invalid start point.", i),
                        "PolygonValidator.IsConsistent()");
                }

                if (q.id < 0 || q.id >= count)
                {
                    horrors++;
                    logger.Warning(string.Format("Segment {0} has invalid endpoint.", i),
                        "PolygonValidator.IsConsistent()");
                }

                i++;
            }

            return horrors;
        }

        private static int CheckDuplicateIDs(IPolygon poly)
        {
            var ids = new HashSet<int>();

            // Check for duplicate ids.
            foreach (var p in poly.Points)
            {
                if (!ids.Add(p.id))
                {
                    Log.Instance.Warning("Found duplicate vertex ids.", "PolygonValidator.IsConsistent()");
                    return 1;
                }
            }

            return 0;
        }
    }
}
