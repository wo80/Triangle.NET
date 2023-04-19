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
        /// <inheritdoc />
        public List<Vertex> Points { get; }

        /// <inheritdoc />
        public List<Point> Holes { get; }

        /// <inheritdoc />
        public List<RegionPointer> Regions { get; }

        /// <inheritdoc />
        public List<ISegment> Segments { get; }

        /// <inheritdoc />
        public bool HasPointMarkers { get; set; }

        /// <inheritdoc />
        public bool HasSegmentMarkers { get; set; }

        /// <inheritdoc />
        public int Count => Points.Count;

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
            Points = new List<Vertex>(capacity);
            Holes = new List<Point>();
            Regions = new List<RegionPointer>();

            Segments = new List<ISegment>();

            HasPointMarkers = markers;
            HasSegmentMarkers = markers;
        }

        /// <inheritdoc />
        public Rectangle Bounds()
        {
            var bounds = new Rectangle();
            bounds.Expand(Points);

            return bounds;
        }

        /// <inheritdoc />
        public void Add(Vertex vertex)
        {
            Points.Add(vertex);
        }

        /// <inheritdoc />
        public void Add(ISegment segment, bool insert = false)
        {
            Segments.Add(segment);

            if (insert)
            {
                Points.Add(segment.GetVertex(0));
                Points.Add(segment.GetVertex(1));
            }
        }

        /// <inheritdoc />
        public void Add(ISegment segment, int index)
        {
            Segments.Add(segment);

            Points.Add(segment.GetVertex(index));
        }

        private void AddContourPointsAndSegments(Contour contour)
        {
            Points.AddRange(contour.Points);
            Segments.AddRange(contour.GetSegments());
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
            Holes.Add(hole);
        }
    }
}
