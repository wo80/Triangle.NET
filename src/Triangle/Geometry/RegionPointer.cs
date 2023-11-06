// -----------------------------------------------------------------------
// <copyright file="RegionPointer.cs" company="">
// Triangle.NET Copyright (c) 2012-2022 Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Geometry
{
    /// <summary>
    /// Pointer to a region in the mesh geometry. A region is a well-defined
    /// subset of the geometry (enclosed by subsegments).
    /// </summary>
    public class RegionPointer
    {
        /// <summary>
        /// Gets the x and y coordinates of the region pointer.
        /// </summary>
        public Point Point { get; private set; }

        /// <summary>
        /// Gets the label of the region.
        /// </summary>
        public int Label { get; private set; }

        /// <summary>
        /// Gets the area constraint of the region.
        /// </summary>
        public double Area { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionPointer" /> class.
        /// </summary>
        /// <param name="x">X coordinate of the region.</param>
        /// <param name="y">Y coordinate of the region.</param>
        /// <param name="label">Region label.</param>
        public RegionPointer(double x, double y, int label)
            : this(x, y, label, 0.0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionPointer" /> class.
        /// </summary>
        /// <param name="x">X coordinate of the region.</param>
        /// <param name="y">Y coordinate of the region.</param>
        /// <param name="label">Region label.</param>
        /// <param name="area">Area constraint.</param>
        public RegionPointer(double x, double y, int label, double area)
        {
            Point = new Point(x, y);
            Label = label;
            Area = area;
        }
    }
}
