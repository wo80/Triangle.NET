﻿// -----------------------------------------------------------------------
// <copyright file="Contour.cs" company="">
// Triangle.NET Copyright (c) 2012-2022 Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Geometry
{
    using System;
    using System.Collections.Generic;
    using Tools;

    /// <summary>
    /// Represents a contour of a polygon (outer boundary or internal holes).
    /// </summary>
    public class Contour
    {
        private int marker;

        private bool convex;

        /// <summary>
        /// Gets or sets the list of points making up the contour.
        /// </summary>
        public List<Vertex> Points { get; } = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="Contour" /> class.
        /// </summary>
        /// <param name="points">The points that make up the contour.</param>
        public Contour(IEnumerable<Vertex> points)
            : this(points, 0, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Contour" /> class.
        /// </summary>
        /// <param name="points">The points that make up the contour.</param>
        /// <param name="marker">Contour marker.</param>
        public Contour(IEnumerable<Vertex> points, int marker)
            : this(points, marker, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Contour" /> class.
        /// </summary>
        /// <param name="points">The points that make up the contour.</param>
        /// <param name="marker">Contour marker.</param>
        /// <param name="convex">The hole is convex.</param>
        public Contour(IEnumerable<Vertex> points, int marker, bool convex)
        {
            AddPoints(points);

            this.marker = marker;
            this.convex = convex;
        }

        /// <summary>
        /// Gets the list of segments of the contour.
        /// </summary>
        /// <returns></returns>
        public List<ISegment> GetSegments()
        {
            var segments = new List<ISegment>();

            var p = Points;

            var count = p.Count - 1;

            for (var i = 0; i < count; i++)
            {
                // Add segments to polygon.
                segments.Add(new Segment(p[i], p[i + 1], marker));
            }

            // Close the contour.
            segments.Add(new Segment(p[count], p[0], marker));

            return segments;
        }

        /// <summary>
        /// Try to find a point inside the contour.
        /// </summary>
        /// <param name="limit">The number of iterations on each segment (default = 5).</param>
        /// <param name="eps">Threshold for co-linear points (default = 2e-5).</param>
        /// <returns>Point inside the contour</returns>
        /// <exception cref="Exception">Throws if no point could be found.</exception>
        /// <remarks>
        /// For each corner (index i) of the contour, the 3 points with indices i-1, i and i+1
        /// are considered and a search on the line through the corner vertex is started (either
        /// on the bisecting line, or, if <see cref="IPredicates.CounterClockwise"/> is less than
        /// eps, on the perpendicular line.
        /// A given number of points will be tested (limit), while the distance to the contour
        /// boundary will be reduced in each iteration (with a factor 1 / 2^i, i = 1 ... limit).
        /// </remarks>
        public Point FindInteriorPoint(int limit = 5, double eps = 2e-5)
        {
            if (convex)
            {
                var p = Points;

                var count = p.Count;

                var point = new Point(0.0, 0.0);

                for (var i = 0; i < count; i++)
                {
                    point.x += p[i].x;
                    point.y += p[i].y;
                }

                // If the contour is convex, use its centroid.
                point.x /= count;
                point.y /= count;

                return point;
            }

            return FindPointInPolygon(Points, limit, eps);
        }

        private void AddPoints(IEnumerable<Vertex> points)
        {
            Points.AddRange(points);

            var count = Points.Count - 1;

            // Check if first vertex equals last vertex.
            if (Points[0] == Points[count])
            {
                Points.RemoveAt(count);
            }
        }

        #region Helper methods

        private static Point FindPointInPolygon(List<Vertex> contour, int limit, double eps)
        {
            var bounds = new Rectangle();
            bounds.Expand(contour);

            var length = contour.Count;

            var test = new Point();

            Point a, b, c; // Current corner points.

            double bx, by;
            double dx, dy;
            double h;

            var predicates = RobustPredicates.Default;

            a = contour[0];
            b = contour[1];

            for (var i = 0; i < length; i++)
            {
                c = contour[(i + 2) % length];

                // Corner point.
                bx = b.x;
                by = b.y;

                // NOTE: if we knew the contour points were in counterclockwise order, we
                // could skip concave corners and search only in one direction.

                h = predicates.CounterClockwise(a, b, c);

                if (Math.Abs(h) < eps)
                {
                    // Points are nearly co-linear. Use perpendicular direction.
                    dx = (c.y - a.y) / 2;
                    dy = (a.x - c.x) / 2;
                }
                else
                {
                    // Direction [midpoint(a-c) -> corner point]
                    dx = (a.x + c.x) / 2 - bx;
                    dy = (a.y + c.y) / 2 - by;
                }

                // Move around the contour.
                a = b;
                b = c;

                h = 1.0;

                for (var j = 0; j < limit; j++)
                {
                    // Search in direction.
                    test.x = bx + dx * h;
                    test.y = by + dy * h;

                    if (bounds.Contains(test) && IsPointInPolygon(test, contour) && !IsPointOnSegment(test, contour))
                    {
                        return test;
                    }

                    // Search in opposite direction (see NOTE above).
                    test.x = bx - dx * h;
                    test.y = by - dy * h;

                    if (bounds.Contains(test) && IsPointInPolygon(test, contour) && !IsPointOnSegment(test, contour))
                    {
                        return test;
                    }

                    h = h / 2;
                }
            }

            throw new Exception();
        }

        /// <summary>
        /// Return true if the given point is inside the polygon, or false if it is not.
        /// </summary>
        /// <param name="point">The point to check.</param>
        /// <param name="poly">The polygon (list of contour points).</param>
        /// <returns></returns>
        /// <remarks>
        /// WARNING: If the point is exactly on the edge of the polygon, then the function
        /// may return true or false.
        /// 
        /// See http://alienryderflex.com/polygon/
        /// </remarks>
        private static bool IsPointInPolygon(Point point, List<Vertex> poly)
        {
            var inside = false;

            var x = point.x;
            var y = point.y;

            var count = poly.Count;

            for (int i = 0, j = count - 1; i < count; i++)
            {
                if (((poly[i].y < y && poly[j].y >= y) || (poly[j].y < y && poly[i].y >= y))
                    && (poly[i].x <= x || poly[j].x <= x))
                {
                    inside ^= (poly[i].x + (y - poly[i].y) / (poly[j].y - poly[i].y) * (poly[j].x - poly[i].x) < x);
                }

                j = i;
            }

            return inside;
        }


        /// <summary>
        /// Work around IsPointInPolygon() failing for points on segments.
        /// </summary>
        private static bool IsPointOnSegment(Point test, List<Vertex> contour, double eps = 1e-12)
        {
            var count = contour.Count;

            var i = count - 1;

            for (var j = 0; j < count; j++)
            {
                if (IntersectionHelper.IsPointOnSegment(contour[i], contour[j], test, eps))
                {
                    return true;
                }

                i = j;
            }

            return false;
        }

        #endregion
    }
}
