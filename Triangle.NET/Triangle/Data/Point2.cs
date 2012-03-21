// -----------------------------------------------------------------------
// <copyright file="Point2.cs" company="">
// Original Triangle code by Jonathan Richard Shewchuk, http://www.cs.cmu.edu/~quake/triangle.html
// Triangle.NET code by Christian Woltering, http://home.edo.tu-dortmund.de/~woltering/triangle/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents a 2D point.
    /// </summary>
    public struct Point2
    {
        public double X;
        public double Y;

        public Point2(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public bool Equals(Point2 p)
        {
            // Return true if the fields match:
            return (X == p.X) && (Y == p.Y);
        }
    }
}
