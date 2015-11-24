
namespace TriangleNet.Rendering
{
    using System.Collections.Generic;
    using TriangleNet.Geometry;
    using TriangleNet.Meshing;
    using TriangleNet.Rendering.Buffer;
    using TriangleNet.Rendering.Util;

    using Color = System.Drawing.Color;

    public interface IRenderLayer
    {
        int Count { get; }

        // Points can be set, because layers may share vertices.
        IBuffer<float> Points { get; }
        IBuffer<int> Indices { get; }

        bool IsEnabled { get; set; }

        bool IsEmpty();

        void Reset(bool clear);

        // TODO: add boolean: reset
        BoundingBox SetPoints(IBuffer<float> buffer);
        BoundingBox SetPoints(IPolygon poly);
        BoundingBox SetPoints(IMesh mesh);
        BoundingBox SetPoints(ICollection<Point> points);
        void SetPolygon(IPolygon poly);
        void SetPolygon(IMesh mesh);
        void SetMesh(IMesh mesh, bool elements);
        void SetMesh(IEnumerable<IEdge> edges);


        // TODO: better put these into a subclass.
        IBuffer<int> Partition { get; }
        IBuffer<Color> Colors { get; }

        void AttachLayerData(float[] values, ColorMap colormap);
        void AttachLayerData(int[] partition);
    }
}
