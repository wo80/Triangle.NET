// -----------------------------------------------------------------------
// <copyright file="Polygon.cs" company="">
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Geometry
{
    using System.Collections.Generic;

    /// <summary>
    /// A polygon represented as a planar straight line graph.
    /// </summary>
    public class Polygon : IPolygon
    {
        List<Vertex> points;
        List<Point> holes;
        List<RegionPointer> regions;

        List<ISegment> segments;

        /// <inheritdoc />
        public List<Vertex> Points => points;

        /// <inheritdoc />
        public List<Point> Holes => holes;

        /// <inheritdoc />
        public List<RegionPointer> Regions => regions;

        /// <inheritdoc />
        public List<ISegment> Segments => segments;

        /// <inheritdoc />
        public bool HasPointMarkers { get; set; }

        /// <inheritdoc />
        public bool HasSegmentMarkers { get; set; }

        /// <inheritdoc />
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
            : this(capacity, false)
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

        /// <inheritdoc />
        public Rectangle Bounds()
        {
            var bounds = new Rectangle();
            bounds.Expand(this.points);

            return bounds;
        }

        /// <inheritdoc />
        public void Add(Vertex vertex)
        {
            this.points.Add(vertex);
        }

        /// <inheritdoc />
        public void Add(ISegment segment, bool insert = false)
        {
            this.segments.Add(segment);

            if (insert)
            {
                this.points.Add(segment.GetVertex(0));
                this.points.Add(segment.GetVertex(1));
            }
        }

        /// <inheritdoc />
        public void Add(ISegment segment, int index)
        {
            this.segments.Add(segment);

            this.points.Add(segment.GetVertex(index));
        }

        /// <inheritdoc />
        public void Add(Contour contour, bool hole = false)
        {
            if (hole)
            {
                this.Add(contour, contour.FindInteriorPoint());
            }
            else
            {
                this.points.AddRange(contour.Points);
                this.segments.AddRange(contour.GetSegments());
            }
        }

        /// <inheritdoc />
        public void Add(Contour contour, Point hole)
        {
            this.points.AddRange(contour.Points);
            this.segments.AddRange(contour.GetSegments());

            this.holes.Add(hole);
        }
    }
}
