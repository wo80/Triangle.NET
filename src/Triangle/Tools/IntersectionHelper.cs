﻿// -----------------------------------------------------------------------
// <copyright file="IntersectionHelper.cs" company="">
// Triangle.NET Copyright (c) 2012-2022 Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Tools
{
    using System;
    using Geometry;

    /// <summary>
    /// Segment intersection helper.
    /// </summary>
    public static class IntersectionHelper
    {
        /// <summary>
        /// Check if a given test point lies on a segment.
        /// </summary>
        /// <param name="a">The segment start point.</param>
        /// <param name="b">The segment end point.</param>
        /// <param name="test">The point to test.</param>
        /// <param name="eps">Threshold to test collinearity (default = 1e-12).</param>
        /// <returns></returns>
        public static bool IsPointOnSegment(Point a, Point b, Point test, double eps = 1e-12)
        {
            // The cross product.
            var cross = (test.Y - a.Y) * (b.X - a.X) - (test.X - a.X) * (b.Y - a.Y);

            // Check if points are collinear.
            if (Math.Abs(cross) > eps) return false;

            // The dot product (projection of test point onto segment).
            var dot = (test.X - a.X) * (b.X - a.X) + (test.Y - a.Y) * (b.Y - a.Y);

            // Check if test point is actually between a and b (left of a).
            if (dot < 0) return false;

            // Length of the segment.
            var ab = (b.X - a.X) * (b.X - a.X) + (b.Y - a.Y) * (b.Y - a.Y);

            // Ignore duplicate input points.
            if (ab == 0) return false;

            // Check if test point is actually between a and b (right of b).
            if (dot > ab) return false;

            return true;
        }

        /// <summary>
        /// Compute intersection of two segments.
        /// </summary>
        /// <param name="p0">Segment 1 start point.</param>
        /// <param name="p1">Segment 1 end point.</param>
        /// <param name="q0">Segment 2 start point.</param>
        /// <param name="q1">Segment 2 end point.</param>
        /// <param name="c0">The intersection point.</param>
        /// <remarks>
        /// This is a special case of segment intersection. Since the calling algorithm assures
        /// that a valid intersection exists, there's no need to check for any special cases.
        /// </remarks>
        public static void IntersectSegments(Point p0, Point p1, Point q0, Point q1, ref Point c0)
        {
            var ux = p1.x - p0.x;
            var uy = p1.y - p0.y;
            var vx = q1.x - q0.x;
            var vy = q1.y - q0.y;
            var wx = p0.x - q0.x;
            var wy = p0.y - q0.y;

            var d = (ux * vy - uy * vx);
            var s = (vx * wy - vy * wx) / d;

            // Intersection point
            c0.x = p0.X + s * ux;
            c0.y = p0.Y + s * uy;
        }

        /// <summary>
        /// Intersect segment with a bounding box.
        /// </summary>
        /// <param name="rect">The clip rectangle.</param>
        /// <param name="p0">Segment endpoint.</param>
        /// <param name="p1">Segment endpoint.</param>
        /// <param name="c0">The new location of p0.</param>
        /// <param name="c1">The new location of p1.</param>
        /// <returns>Returns true, if segment is clipped.</returns>
        /// <remarks>
        /// Based on Liang-Barsky function by Daniel White:
        /// http://www.skytopia.com/project/articles/compsci/clipping.html
        /// </remarks>
        public static bool LiangBarsky(Rectangle rect, Point p0, Point p1, ref Point c0, ref Point c1)
        {
            // Define the x/y clipping values for the border.
            var xmin = rect.Left;
            var xmax = rect.Right;
            var ymin = rect.Bottom;
            var ymax = rect.Top;

            // Define the start and end points of the line.
            var x0 = p0.X;
            var y0 = p0.Y;
            var x1 = p1.X;
            var y1 = p1.Y;

            var t0 = 0.0;
            var t1 = 1.0;

            var dx = x1 - x0;
            var dy = y1 - y0;

            double p = 0.0, q = 0.0, r;

            for (var edge = 0; edge < 4; edge++)
            {
                // Traverse through left, right, bottom, top edges.
                if (edge == 0) { p = -dx; q = -(xmin - x0); }
                if (edge == 1) { p = dx; q = (xmax - x0); }
                if (edge == 2) { p = -dy; q = -(ymin - y0); }
                if (edge == 3) { p = dy; q = (ymax - y0); }
                r = q / p;
                if (p == 0 && q < 0) return false; // Don't draw line at all. (parallel line outside)
                if (p < 0)
                {
                    if (r > t1) return false; // Don't draw line at all.
                    else if (r > t0) t0 = r; // Line is clipped!
                }
                else if (p > 0)
                {
                    if (r < t0) return false; // Don't draw line at all.
                    else if (r < t1) t1 = r; // Line is clipped!
                }
            }

            c0.X = x0 + t0 * dx;
            c0.Y = y0 + t0 * dy;
            c1.X = x0 + t1 * dx;
            c1.Y = y0 + t1 * dy;

            return true; // (clipped) line is drawn
        }

        /// <summary>
        /// Intersect a ray with a bounding box.
        /// </summary>
        /// <param name="rect">The clip rectangle.</param>
        /// <param name="p0">The ray start point (inside the box).</param>
        /// <param name="p1">Any point in ray direction (NOT the direction vector).</param>
        /// <param name="c1">The intersection point.</param>
        /// <returns>Returns false, if start point is outside the box.</returns>
        public static bool BoxRayIntersection(Rectangle rect, Point p0, Point p1, ref Point c1)
        {
            return BoxRayIntersection(rect, p0, p1.x - p0.x, p1.y - p0.y, ref c1);
        }

        /// <summary>
        /// Intersect a ray with a bounding box.
        /// </summary>
        /// <param name="rect">The clip rectangle.</param>
        /// <param name="p">The ray start point (inside the box).</param>
        /// <param name="dx">X direction.</param>
        /// <param name="dy">Y direction.</param>
        /// <returns>Returns false, if start point is outside the box.</returns>
        public static Point BoxRayIntersection(Rectangle rect, Point p, double dx, double dy)
        {
            var intersection = new Point();

            if (BoxRayIntersection(rect, p, dx, dy, ref intersection))
            {
                return intersection;
            }

            return null;
        }

        /// <summary>
        /// Intersect a ray with a bounding box.
        /// </summary>
        /// <param name="rect">The clip rectangle.</param>
        /// <param name="p">The ray startpoint (inside the box).</param>
        /// <param name="dx">X direction.</param>
        /// <param name="dy">Y direction.</param>
        /// <param name="c">The intersection point.</param>
        /// <returns>Returns false, if startpoint is outside the box.</returns>
        public static bool BoxRayIntersection(Rectangle rect, Point p, double dx, double dy, ref Point c)
        {
            var x = p.X;
            var y = p.Y;

            double t1, x1, y1, t2, x2, y2;

            // Bounding box
            var xmin = rect.Left;
            var xmax = rect.Right;
            var ymin = rect.Bottom;
            var ymax = rect.Top;

            // Check if point is inside the bounds
            if (x < xmin || x > xmax || y < ymin || y > ymax)
            {
                return false;
            }

            // Calculate the cut through the vertical boundaries
            if (dx < 0)
            {
                // Line going to the left: intersect with x = minX
                t1 = (xmin - x) / dx;
                x1 = xmin;
                y1 = y + t1 * dy;
            }
            else if (dx > 0)
            {
                // Line going to the right: intersect with x = maxX
                t1 = (xmax - x) / dx;
                x1 = xmax;
                y1 = y + t1 * dy;
            }
            else
            {
                // Line going straight up or down: no intersection possible
                t1 = double.MaxValue;
                x1 = y1 = 0;
            }

            // Calculate the cut through upper and lower boundaries
            if (dy < 0)
            {
                // Line going downwards: intersect with y = minY
                t2 = (ymin - y) / dy;
                x2 = x + t2 * dx;
                y2 = ymin;
            }
            else if (dy > 0)
            {
                // Line going upwards: intersect with y = maxY
                t2 = (ymax - y) / dy;
                x2 = x + t2 * dx;
                y2 = ymax;
            }
            else
            {
                // Horizontal line: no intersection possible
                t2 = double.MaxValue;
                x2 = y2 = 0;
            }

            if (t1 < t2)
            {
                c.x = x1;
                c.y = y1;
            }
            else
            {
                c.x = x2;
                c.y = y2;
            }

            return true;
        }
    }
}
