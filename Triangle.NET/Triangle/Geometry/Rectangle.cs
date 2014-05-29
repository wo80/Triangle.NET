// -----------------------------------------------------------------------
// <copyright file="BoundingBox.cs" company="">
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Geometry
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A simple bounding box class.
    /// </summary>
    public class Rectangle
    {
        double xmin, ymin, xmax, ymax;

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle" /> class.
        /// </summary>
        public Rectangle()
            : this(double.MaxValue, double.MaxValue, -double.MaxValue, -double.MaxValue)
        {
        }

        public Rectangle(Rectangle other)
            : this(other.Left, other.Bottom, other.Right, other.Top)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle" /> class
        /// with predefined bounds.
        /// </summary>
        /// <param name="xmin">Minimum x value.</param>
        /// <param name="ymin">Minimum y value.</param>
        /// <param name="xmax">Maximum x value.</param>
        /// <param name="ymax">Maximum y value.</param>
        public Rectangle(double xmin, double ymin, double xmax, double ymax)
        {
            this.xmin = xmin;
            this.xmax = xmax;
            this.ymin = ymin;
            this.ymax = ymax;
        }

        /// <summary>
        /// Gets the minimum x value (left boundary).
        /// </summary>
        public double Left
        {
            get { return xmin; }
        }

        /// <summary>
        /// Gets the maximum x value (right boundary).
        /// </summary>
        public double Right
        {
            get { return xmax; }
        }

        /// <summary>
        /// Gets the minimum y value (bottom boundary).
        /// </summary>
        public double Bottom
        {
            get { return ymin; }
        }

        /// <summary>
        /// Gets the maximum y value (top boundary).
        /// </summary>
        public double Top
        {
            get { return ymax; }
        }

        /// <summary>
        /// Gets the width of the bounding box.
        /// </summary>
        public double Width
        {
            get { return xmax - xmin; }
        }

        /// <summary>
        /// Gets the height of the bounding box.
        /// </summary>
        public double Height
        {
            get { return ymax - ymin; }
        }

        /// <summary>
        /// Scale bounds.
        /// </summary>
        /// <param name="dx">Add dx to left and right bounds.</param>
        /// <param name="dy">Add dy to top and bottom bounds.</param>
        public void Resize(double dx, double dy)
        {
            xmin -= dx;
            xmax += dx;
            ymin -= dy;
            ymax += dy;
        }

        /// <summary>
        /// Expand rectangle to include given point.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="p">Y coordinate.</param>
        public void Expand(Point p)
        {
            xmin = Math.Min(xmin, p.x);
            ymin = Math.Min(ymin, p.y);
            xmax = Math.Max(xmax, p.x);
            ymax = Math.Max(ymax, p.y);
        }

        /// <summary>
        /// Expand rectangle to include a list of points.
        /// </summary>
        public void Expand(IEnumerable<Point> points)
        {
            foreach (var p in points)
            {
                Expand(p);
            }
        }

        /// <summary>
        /// Expand rectangle to include given rectangle.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public void Expand(Rectangle other)
        {
            xmin = Math.Min(xmin, other.xmin);
            ymin = Math.Min(ymin, other.ymin);
            xmax = Math.Max(xmax, other.xmax);
            ymax = Math.Max(ymax, other.ymax);
        }

        /// <summary>
        /// Check if given point is inside bounding box.
        /// </summary>
        /// <param name="pt">Point to check.</param>
        /// <returns>Return true, if bounding box contains given point.</returns>
        public bool Contains(Point pt)
        {
            return ((pt.x >= xmin) && (pt.x <= xmax) && (pt.y >= ymin) && (pt.y <= ymax));
        }

        public bool Contains(Rectangle other)
        {
            return (xmin <= other.Left && other.Right <= xmax
                && ymin <= other.Bottom && other.Top <= ymax);
        }

        public bool Intersects(Rectangle other)
        {
            return (other.Left < xmax && xmin < other.Right
                && other.Bottom < ymax && ymin < other.Top);
        }
    }
}
