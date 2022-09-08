// -----------------------------------------------------------------------
// <copyright file="Polygon.cs" company="">
// Triangle.NET Copyright (c) 2012-2022 Christian Woltering
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
        private readonly List<Vertex> points;
        private readonly List<ISegment> segments;
        private readonly List<Point> holes;
        private readonly List<RegionPointer> regions;

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
            bounds.Expand(points);

            return bounds;
        }

        /// <inheritdoc />
        public void Add(Vertex vertex)
        {
            points.Add(vertex);
        }

        /// <inheritdoc />
        public void Add(ISegment segment, bool insert = false)
        {
            segments.Add(segment);

            if (insert)
            {
                points.Add(segment.GetVertex(0));
                points.Add(segment.GetVertex(1));
            }
        }

        /// <inheritdoc />
        public void Add(ISegment segment, int index)
        {
            segments.Add(segment);

            points.Add(segment.GetVertex(index));
        }

        private void AddContourPointsAndSegments(Contour contour)
        {
            points.AddRange(contour.Points);
            segments.AddRange(contour.GetSegments());
        }

        /// <inheritdoc />
        public void Add(Contour contour, int regionlabel )
        {
            if (regionlabel != 0)
            {
                var interiorPoint = contour.FindInteriorPoint();
                Regions.Add(new RegionPointer(interiorPoint.X, interiorPoint.Y, regionlabel));
            }
            AddContourPointsAndSegments(contour);
        }

        /// <inheritdoc />
        public void Add(Contour contour, bool hole = false)
        {
            if (hole)
            {
                Add(contour, contour.FindInteriorPoint());
            }
            else
            {
                AddContourPointsAndSegments(contour);
            }
        }

        /// <inheritdoc />
        public void Add(Contour contour, Point hole)
        {
            AddContourPointsAndSegments(contour);
            holes.Add(hole);
        }
    }
}
