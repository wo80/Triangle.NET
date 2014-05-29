
namespace TriangleNet.Geometry
{
    using System.Collections.Generic;
    using TriangleNet.Data;

    public interface IPolygon
    {
        List<Vertex> Points { get; }
        List<IEdge> Segments { get; }

        List<Point> Holes { get; }
        //List<Point> Regions { get; }
        List<RegionPointer> Regions { get; }

        bool HasPointMarkers { get; set; }
        bool HasSegmentMarkers { get; set; }

        void AddContour(IEnumerable<Vertex> points, int marker, bool hole, bool convex);
        void AddContour(IEnumerable<Vertex> points, int marker, Point hole);

        Rectangle Bounds();
    }
}
