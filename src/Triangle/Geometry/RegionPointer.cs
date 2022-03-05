// -----------------------------------------------------------------------
// <copyright file="RegionPointer.cs" company="">
// Triangle.NET Copyright (c) 2012-2022 Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Geometry
{
    using System;

    /// <summary>
    /// Pointer to a region in the mesh geometry. A region is a well-defined
    /// subset of the geometry (enclosed by subsegments).
    /// </summary>
    public class RegionPointer
    {
        internal Point point;
        internal int id;
        internal double area;

        /// <summary>
        /// Gets or sets a region area constraint.
        /// </summary>
        public double Area
        {
            get { return area; }
            set
            {
                if (value < 0.0)
                {
                    throw new ArgumentException("Area constraints must not be negative.");
                }
                area = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionPointer" /> class.
        /// </summary>
        /// <param name="x">X coordinate of the region.</param>
        /// <param name="y">Y coordinate of the region.</param>
        /// <param name="id">Region id.</param>
        public RegionPointer(double x, double y, int id)
            : this(x, y, id, 0.0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionPointer" /> class.
        /// </summary>
        /// <param name="x">X coordinate of the region.</param>
        /// <param name="y">Y coordinate of the region.</param>
        /// <param name="id">Region id.</param>
        /// <param name="area">Area constraint.</param>
        public RegionPointer(double x, double y, int id, double area)
        {
            this.point = new Point(x, y);
            this.id = id;
            this.area = area;
        }
    }
}
