// -----------------------------------------------------------------------
// <copyright file="Contour.cs" company="">
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Geometry
{
    using System;
    using System.Collections.Generic;
    
    public class Contour
    {
        int marker;

        bool convex;

        /// <summary>
        /// Gets or sets the list of points making up the contour.
        /// </summary>
        public List<Vertex> Points { get; set; }

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

        public List<ISegment> GetSegments()
        {
            var segments = new List<ISegment>();

            var p = this.Points;

            int count = p.Count - 1;

            for (int i = 0; i < count; i++)
            {
                // Add segments to polygon.
                segments.Add(new Segment(p[i], p[i + 1], marker));
            }

            // Close the contour.
            segments.Add(new Segment(p[count], p[0], marker));

            return segments;
        }

        public Point FindInteriorPoint()
        {
            if (convex)
            {
                int count = this.Points.Count;

                var centroid = new Point(0.0, 0.0);

                for (int i = 0; i < count; i++)
                {
                    centroid.x += this.Points[i].x;
                    centroid.y += this.Points[i].y;
                }

                // If the hole is convex, use its centroid.
                centroid.x /= count;
                centroid.y /= count;

                return centroid;
            }

            return FindPointInPolygon(this.Points);
        }

        private void AddPoints(IEnumerable<Vertex> points)
        {
            this.Points = new List<Vertex>(points);

            int count = Points.Count - 1;

            // Check if first vertex equals last vertex.
            if (Points[0] == Points[count])
            {
                Points.RemoveAt(count);
            }
        }

        private static Point FindPointInPolygon(List<Vertex> contour)
        {
            var bounds = new Rectangle();
            bounds.Expand(contour);

            int length = contour.Count;
            int limit = 8;

            var test = new Point();

            Point a, b; // Current edge.
            double cx, cy; // Center of current edge.
            double dx, dy; // Direction perpendicular to edge.

            for (int i = 0; i < length; i++)
            {
                a = contour[i];
                b = contour[(i + 1) % length];

                cx = (a.x + b.x) / 2;
                cy = (a.y + b.y) / 2;

                dx = (b.y - a.y) / 1.374;
                dy = (a.x - b.x) / 1.374;

                for (int j = 1; j <= limit; j++)
                {
                    // Search to the right of the segment.
                    test.x = cx + dx / j;
                    test.y = cy + dy / j;

                    if (bounds.Contains(test) && IsPointInPolygon(test, contour))
                    {
                        return test;
                    }

                    // Search on the other side of the segment.
                    test.x = cx - dx / j;
                    test.y = cy - dy / j;

                    if (bounds.Contains(test) && IsPointInPolygon(test, contour))
                    {
                        return test;
                    }
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
            bool inside = false;

            double x = point.x;
            double y = point.y;

            int count = poly.Count;

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
    }
}
