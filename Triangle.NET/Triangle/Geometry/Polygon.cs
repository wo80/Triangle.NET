// -----------------------------------------------------------------------
// <copyright file="Polygon.cs" company="">
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Geometry
{
    using System;
    using System.Collections.Generic;
    using TriangleNet.Topology;

    /// <summary>
    /// A polygon represented as a planar straight line graph.
    /// </summary>
    public class Polygon : IPolygon
    {
        List<Vertex> points;
        List<Point> holes;
        List<RegionPointer> regions;

        List<ISegment> segments;

        /// <inherit />
        public List<Vertex> Points
        {
            get { return points; }
        }

        /// <inherit />
        public List<Point> Holes
        {
            get { return holes; }
        }

        /// <inherit />
        public List<RegionPointer> Regions
        {
            get { return regions; }
        }

        /// <inherit />
        public List<ISegment> Segments
        {
            get { return segments; }
        }

        /// <inherit />
        public bool HasPointMarkers { get; set; }

        /// <inherit />
        public bool HasSegmentMarkers { get; set; }

        /// <inherit />
        public int Count
        {
            get { return points.Count; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon" /> class.
        /// </summary>
        public Polygon()
            : this(3, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon" /> class.
        /// </summary>
        /// <param name="capacity">The default capacity for the points list.</param>
        public Polygon(int capacity)
            : this(3, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon" /> class.
        /// </summary>
        /// <param name="capacity">The default capacity for the points list.</param>
        /// <param name="markers">Use point and segment markers.</param>
        public Polygon(int capacity, bool markers)
        {
            points = new List<Vertex>(capacity);
            holes = new List<Point>();
            regions = new List<RegionPointer>();

            segments = new List<ISegment>();

            HasPointMarkers = markers;
            HasSegmentMarkers = markers;
        }

        /// <inherit />
        public void AddContour(IEnumerable<Vertex> points, int marker = 0,
            bool hole = false, bool convex = false)
        {
            var c = new Contour(points, marker);

            this.points.AddRange(c.Points);

            this.segments.AddRange(c.GetSegments());

            if (hole)
            {
                this.holes.Add(c.FindInteriorPoint(convex));
            }
        }

        /// <inherit />
        public void AddContour(IEnumerable<Vertex> points, int marker, Point hole)
        {
            var c = new Contour(points, marker);

            this.points.AddRange(c.Points);

            this.segments.AddRange(c.GetSegments());

            this.holes.Add(hole);
        }

        /// <inherit />
        public Rectangle Bounds()
        {
            var bounds = new Rectangle();
            bounds.Expand(this.points);

            return bounds;
        }

        /// <summary>
        /// Add a vertex to the polygon.
        /// </summary>
        public void Add(Vertex vertex)
        {
            this.points.Add(vertex);
        }

        /// <summary>
        /// Add a segment to the polygon.
        /// </summary>
        public void Add(ISegment segment)
        {
            this.segments.Add(segment);
        }
    }
}
