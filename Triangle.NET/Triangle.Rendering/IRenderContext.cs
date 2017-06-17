
namespace TriangleNet.Rendering
{
    using System.Collections.Generic;
    using TriangleNet.Geometry;
    using TriangleNet.Meshing;
    using TriangleNet.Voronoi.Legacy;

    public interface IRenderContext
    {
        ColorManager ColorManager { get; }

        BoundingBox Bounds { get; }

        IList<IRenderLayer> RenderLayers { get; }

        Projection Zoom { get; }

        IMesh Mesh { get; }

        bool HasData { get; }

        void Add(IPolygon data);
        void Add(IMesh data, bool reset);
        void Add(ICollection<Point> points, IEnumerable<IEdge> edges, bool reset);

        void Add(float[] values);
        void Add(int[] partition);

        void Enable(int layer, bool enabled);
    }
}
