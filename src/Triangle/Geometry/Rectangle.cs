// -----------------------------------------------------------------------
// <copyright file="Rectangle.cs" company="">
// Triangle.NET Copyright (c) 2012-2022 Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Geometry
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A simple rectangle class.
    /// </summary>
    public class Rectangle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle" /> class.
        /// </summary>
        public Rectangle()
        {
            Left = Bottom = double.MaxValue;
            Right = Top = -double.MaxValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle" /> class.
        /// </summary>
        /// <param name="other"></param>
        public Rectangle(Rectangle other)
            : this(other.Left, other.Bottom, other.Right, other.Top)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle" /> class
        /// with predefined bounds.
        /// </summary>
        /// <param name="x">Minimum x value (left).</param>
        /// <param name="y">Minimum y value (bottom).</param>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        public Rectangle(double x, double y, double width, double height)
        {
            Left = x;
            Bottom = y;
            Right = x + width;
            Top = y + height;
        }

        /// <summary>
        /// Gets the minimum x value (left boundary).
        /// </summary>
        public double Left { get; private set; }

        /// <summary>
        /// Gets the maximum x value (right boundary).
        /// </summary>
        public double Right { get; private set; }

        /// <summary>
        /// Gets the minimum y value (bottom boundary).
        /// </summary>
        public double Bottom { get; private set; }

        /// <summary>
        /// Gets the maximum y value (top boundary).
        /// </summary>
        public double Top { get; private set; }

        /// <summary>
        /// Gets the minimum x value (left boundary).
        /// </summary>
        public double X => Left;

        /// <summary>
        /// Gets the minimum y value (bottom boundary).
        /// </summary>
        public double Y => Bottom;

        /// <summary>
        /// Gets the width of the rectangle.
        /// </summary>
        public double Width => Right - Left;

        /// <summary>
        /// Gets the height of the rectangle.
        /// </summary>
        public double Height => Top - Bottom;

        /// <summary>
        /// Update bounds.
        /// </summary>
        /// <param name="dx">Add dx to left and right bounds.</param>
        /// <param name="dy">Add dy to top and bottom bounds.</param>
        public void Resize(double dx, double dy)
        {
            Left -= dx;
            Right += dx;
            Bottom -= dy;
            Top += dy;
        }

        /// <summary>
        /// Expand rectangle to include given point.
        /// </summary>
        /// <param name="p">Point.</param>
        public void Expand(Point p)
        {
            Left = Math.Min(Left, p.x);
            Bottom = Math.Min(Bottom, p.y);
            Right = Math.Max(Right, p.x);
            Top = Math.Max(Top, p.y);
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
        /// <param name="other">The other rectangle.</param>
        public void Expand(Rectangle other)
        {
            Left = Math.Min(Left, other.Left);
            Bottom = Math.Min(Bottom, other.Bottom);
            Right = Math.Max(Right, other.Right);
            Top = Math.Max(Top, other.Top);
        }

        /// <summary>
        /// Check if given point is inside rectangle.
        /// </summary>
        /// <param name="x">Point to check.</param>
        /// <param name="y">Point to check.</param>
        /// <returns>Return true, if rectangle contains given point.</returns>
        public bool Contains(double x, double y)
        {
            return (x >= Left) && (x <= Right) && (y >= Bottom) && (y <= Top);
        }

        /// <summary>
        /// Check if given point is inside rectangle.
        /// </summary>
        /// <param name="pt">Point to check.</param>
        /// <returns>Return true, if rectangle contains given point.</returns>
        public bool Contains(Point pt)
        {
            return Contains(pt.x, pt.y);
        }

        /// <summary>
        /// Check if this rectangle contains other rectangle.
        /// </summary>
        /// <param name="other">Rectangle to check.</param>
        /// <returns>Return true, if this rectangle contains given rectangle.</returns>
        public bool Contains(Rectangle other)
        {
            return Left <= other.Left && other.Right <= Right
                && Bottom <= other.Bottom && other.Top <= Top;
        }

        /// <summary>
        /// Check if this rectangle intersects other rectangle.
        /// </summary>
        /// <param name="other">Rectangle to check.</param>
        /// <returns>Return true, if given rectangle intersects this rectangle.</returns>
        public bool Intersects(Rectangle other)
        {
            return other.Left < Right && Left < other.Right
                && other.Bottom < Top && Bottom < other.Top;
        }
    }
}
