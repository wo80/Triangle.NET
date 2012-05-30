// -----------------------------------------------------------------------
// <copyright file="InputGeometry.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Geometry
{
    using System;
    using System.Collections.Generic;
    using TriangleNet.Data;

    /// <summary>
    /// The input geometry which will be triangulated. May represent a 
    /// pointset or a planar straight line graph.
    /// </summary>
    public class InputGeometry
    {
        internal List<Vertex> points;
        internal List<Edge> segments;
        internal List<Point> holes;
        internal List<RegionPointer> regions;

        BoundingBox bounds;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputGeometry" /> class.
        /// </summary>
        public InputGeometry()
            : this(3)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputGeometry" /> class. 
        /// The point list will be initialized with a given capacity.
        /// </summary>
        /// <param name="capacity">Point list capacity.</param>
        public InputGeometry(int capacity)
        {
            points = new List<Vertex>(capacity);
            segments = new List<Edge>();
            holes = new List<Point>();
            regions = new List<RegionPointer>();

            bounds = new BoundingBox();
        }

        /// <summary>
        /// Gets the bounding box of the input geometry.
        /// </summary>
        public BoundingBox Bounds
        {
            get { return bounds; }
        }

        /// <summary>
        /// Gets a value indicating whether the geometry should be treated as a PLSG.
        /// </summary>
        public bool HasSegments
        {
            get { return segments.Count > 0; }
        }

        /// <summary>
        /// Gets the number of points.
        /// </summary>
        public int Count
        {
            get { return points.Count; }
        }

        /// <summary>
        /// Gets the list of input points.
        /// </summary>
        public IEnumerable<Point> Points
        {
            get { return null; }
        }

        /// <summary>
        /// Gets the list of input segments.
        /// </summary>
        public IEnumerable<Edge> Segments
        {
            get { return segments; }
        }

        /// <summary>
        /// Gets the list of input holes.
        /// </summary>
        public IEnumerable<Point> Holes
        {
            get { return holes; }
        }

        /// <summary>
        /// Gets the list of input holes.
        /// </summary>
        public IEnumerable<RegionPointer> Regions
        {
            get { return regions; }
        }

        /// <summary>
        /// Clear input geometry.
        /// </summary>
        public void Clear()
        {
            points.Clear();
            segments.Clear();
            holes.Clear();
            regions.Clear();
        }

        /// <summary>
        /// Adds a point to the geometry.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public void AddPoint(double x, double y)
        {
            AddPoint(x, y, 0);
        }

        /// <summary>
        /// Adds a point to the geometry.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="boundary">Boundary marker.</param>
        public void AddPoint(double x, double y, int boundary)
        {
            //points.Add(new Vertex(x, y, boundary));

            bounds.Update(x, y);
        }

        /// <summary>
        /// Adds a hole location to the geometry.
        /// </summary>
        /// <param name="x">X coordinate of the hole.</param>
        /// <param name="y">Y coordinate of the hole.</param>
        public void AddHole(double x, double y)
        {
            holes.Add(new Point(x, y));
        }

        /// <summary>
        /// Adds a hole location to the geometry.
        /// </summary>
        /// <param name="x">X coordinate of the hole.</param>
        /// <param name="y">Y coordinate of the hole.</param>
        /// <param name="area">The regions area constraint.</param>
        /// <param name="attribute">Region attribute.</param>
        public void AddRegion(double x, double y, double area, double attribute)
        {
            regions.Add(new RegionPointer(x, y, area, attribute));
        }

        /// <summary>
        /// Adds a segment to the geometry.
        /// </summary>
        /// <param name="p0">First endpoint.</param>
        /// <param name="p1">Second endpoint.</param>
        public void AddSegment(int p0, int p1)
        {
            AddSegment(p0, p1, 0);
        }

        /// <summary>
        /// Adds a segment to the geometry.
        /// </summary>
        /// <param name="p0">First endpoint.</param>
        /// <param name="p1">Second endpoint.</param>
        /// <param name="boundary">Segment marker.</param>
        public void AddSegment(int p0, int p1, int boundary)
        {
            if (p0 == p1)
            {
                throw new NotSupportedException("Invalid endpoints.");
            }

            segments.Add(new Edge(p0, p1, boundary));
        }
    }
}
