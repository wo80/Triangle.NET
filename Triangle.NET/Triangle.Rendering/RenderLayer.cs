
namespace TriangleNet.Rendering
{
    using System.Collections.Generic;
    using TriangleNet.Geometry;
    using TriangleNet.Meshing;
    using TriangleNet.Rendering.Buffer;
    using TriangleNet.Rendering.Util;

    using Color = System.Drawing.Color;

    public class RenderLayer : IRenderLayer
    {
        int count;

        protected IBuffer<float> points;
        protected IBuffer<int> indices;

        protected IBuffer<int> partition;
        protected IBuffer<Color> colors;

        public RenderLayer()
        {
            this.IsEnabled = false;
        }

        public int Count
        {
            get { return count; }
        }

        public IBuffer<float> Points
        {
            get { return points; }
            set { points = value; }
        }

        public IBuffer<int> Indices
        {
            get { return indices; }
        }

        public IBuffer<int> Partition
        {
            get { return partition; }
        }

        public IBuffer<Color> Colors
        {
            get { return colors; }
        }

        public bool IsEnabled { get; set; }

        public bool IsEmpty()
        {
            return (points == null || points.Count == 0);
        }

        public void Reset(bool clear)
        {
            if (clear)
            {
                count = 0;
                points = null;
            }

            indices = null;
            partition = null;
            colors = null;
        }

        public BoundingBox SetPoints(IBuffer<float> buffer)
        {
            BoundingBox bounds = new BoundingBox();

            if (points != null && points.Count < buffer.Count)
            {
                count = points.Count / points.Size;
            }
            else
            {
                count = buffer.Count / buffer.Size;
            }

            this.points = buffer;

            return bounds;
        }

        public BoundingBox SetPoints(IPolygon poly)
        {
            BoundingBox bounds = new BoundingBox();

            points = BufferHelper.CreateVertexBuffer(poly.Points, ref bounds);
            count = points.Count / points.Size;

            return bounds;
        }

        public BoundingBox SetPoints(IMesh mesh)
        {
            BoundingBox bounds = new BoundingBox();

            points = BufferHelper.CreateVertexBuffer(mesh.Vertices, ref bounds);
            count = points.Count / points.Size;

            return bounds;
        }

        public BoundingBox SetPoints(ICollection<Point> vertices)
        {
            BoundingBox bounds = new BoundingBox();

            points = BufferHelper.CreateVertexBuffer(vertices, ref bounds);
            count = points.Count / points.Size;

            return bounds;
        }

        public void SetPolygon(IPolygon poly)
        {
            indices = BufferHelper.CreateIndexBuffer(poly.Segments, 2);
        }

        public void SetPolygon(IMesh mesh)
        {
            indices = BufferHelper.CreateIndexBuffer(mesh.Segments, 2);
        }

        public void SetMesh(IEnumerable<IEdge> edges)
        {
            indices = BufferHelper.CreateIndexBuffer(edges, 2);
        }

        public void SetMesh(IMesh mesh, bool elements)
        {
            mesh.Renumber();

            if (!elements)
            {
                indices = BufferHelper.CreateIndexBuffer(mesh.Edges, 2);
            }

            if (elements || indices.Count == 0)
            {
                indices = BufferHelper.CreateIndexBuffer(mesh.Triangles, 3);
            }
        }

        // TODO: remove colormap argument
        public void AttachLayerData(float[] values, ColorMap colormap)
        {
            int length = values.Length;

            Color[] data = new Color[length];

            double min = double.MaxValue;
            double max = double.MinValue;

            // Find min and max of given values.
            for (int i = 0; i < length; i++)
            {
                if (values[i] < min)
                {
                    min = values[i];
                }

                if (values[i] > max)
                {
                    max = values[i];
                }
            }

            for (int i = 0; i < length; i++)
            {
                data[i] = colormap.GetColor(values[i], min, max);
            }

            colors = new ColorBuffer(data, 1);
        }

        public void AttachLayerData(int[] partition)
        {
            this.partition = new IndexBuffer(partition, 1);
        }
    }
}
