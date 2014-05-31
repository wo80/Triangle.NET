
namespace TriangleNet.Meshing
{
    using System.Collections.Generic;
    using TriangleNet.Data;
    using TriangleNet.Geometry;

    public interface IMesh
    {
        ICollection<Vertex> Vertices { get; }
        IEnumerable<Edge> Edges { get; }
        ICollection<Segment> Segments { get; }
        ICollection<Triangle> Triangles { get; }
        IList<Point> Holes { get; }

        Rectangle Bounds { get; }

        void Renumber();
        void Refine(QualityOptions quality);
    }
}
