
namespace TriangleNet.Rendering
{
    using System.Collections.Generic;
    using System.Linq;
    using TriangleNet.Geometry;
    using TriangleNet.Meshing;
    using TriangleNet.Rendering.Buffer;

    /// <summary>
    /// The RenderContext class brings all the rendering parts together.
    /// </summary>
    public class RenderContext : IRenderContext
    {
        private ColorManager colorManager;
        private Projection zoom;
        private Rectangle bounds;
        private IMesh mesh;

        private List<IRenderLayer> renderLayers;

        public RenderContext(Projection zoom, ColorManager colorManager)
        {
            renderLayers = new List<IRenderLayer>(6);

            renderLayers.Add(new RenderLayer()); // 0 = mesh (filled)
            renderLayers.Add(new RenderLayer()); // 1 = mesh (wireframe)
            renderLayers.Add(new RenderLayer()); // 2 = polygon
            renderLayers.Add(new RenderLayer()); // 3 = points
            renderLayers.Add(new RenderLayer()); // 4 = voronoi overlay
            renderLayers.Add(new RenderLayer()); // 5 = vector field
            renderLayers.Add(new RenderLayer()); // 6 = contour lines

            RenderLayers[1].IsEnabled = true;
            RenderLayers[2].IsEnabled = true;
            RenderLayers[3].IsEnabled = true;

            this.zoom = zoom;
            this.colorManager = colorManager;
        }

        /// <inheritdoc />
        public ColorManager ColorManager => colorManager;

        /// <inheritdoc />
        public IList<IRenderLayer> RenderLayers => renderLayers;

        /// <inheritdoc />
        public Projection Zoom => zoom;

        /// <inheritdoc />
        public IMesh Mesh => mesh;

        /// <inheritdoc />
        public bool HasData => renderLayers.Any(layer => !layer.IsEmpty());

        /// <inheritdoc />
        public void Add(IPolygon data)
        {
            foreach (var layer in RenderLayers)
            {
                layer.Reset(true);
            }

            // Always clear Voronoi layer.
            RenderLayers[4].Reset(true);

            int i = 0;

            // Ensure linear numbering of polygon vertices.
            foreach (var p in data.Points)
            {
                p.ID = i++;
            }

            bounds = data.Bounds();

            zoom.Initialize(bounds);

            RenderLayers[2].SetPoints(VertexBuffer.Create(data.Points, bounds));
            RenderLayers[2].SetIndices(IndexBuffer.Create(data.Segments, 2));

            RenderLayers[3].SetPoints(RenderLayers[2].Points);
        }

        /// <inheritdoc />
        public void Add(IMesh data, bool reset)
        {
            foreach (var layer in RenderLayers)
            {
                layer.Reset(reset);
            }

            // Always clear voronoi layer.
            RenderLayers[4].Reset(true);

            // Save reference to mesh.
            mesh = data;
            bounds = data.Bounds;

            // Ensure linear numbering of vertices.
            mesh.Renumber();

            zoom.Initialize(bounds);

            RenderLayers[1].SetPoints(VertexBuffer.Create(data.Vertices, bounds));
            RenderLayers[1].SetIndices(IndexBuffer.Create(data.Edges, 2));

            RenderLayers[2].SetPoints(RenderLayers[1].Points);
            RenderLayers[2].SetIndices(IndexBuffer.Create(data.Segments, 2));

            RenderLayers[3].SetPoints(RenderLayers[1].Points, false);
        }

        /// <inheritdoc />
        public void Add(ICollection<Point> points, IEnumerable<IEdge> edges, bool reset)
        {
            RenderLayers[4].SetPoints(VertexBuffer.Create(points, bounds));
            RenderLayers[4].SetIndices(IndexBuffer.Create(edges, 2));
            RenderLayers[4].IsEnabled = true;
        }

        /// <inheritdoc />
        public void Add(float[] data)
        {
            // Add function values for filled mesh.
            RenderLayers[0].SetPoints(RenderLayers[1].Points);
            RenderLayers[0].SetIndices(IndexBuffer.Create(mesh.Triangles, 3));
            RenderLayers[0].AttachLayerData(data, colorManager.ColorMap);

            RenderLayers[0].IsEnabled = true;
        }

        /// <inheritdoc />
        public void Add(int[] data)
        {
            // Add partition data for filled mesh.
            RenderLayers[0].SetPoints(RenderLayers[1].Points);
            RenderLayers[0].SetIndices(IndexBuffer.Create(mesh.Triangles, 3));
            RenderLayers[0].AttachLayerData(data);

            RenderLayers[0].IsEnabled = true;
        }

        /// <inheritdoc />
        public void Enable(int layer, bool enabled)
        {
            renderLayers[layer].IsEnabled = enabled;
        }

        /// <inheritdoc />
        public void Clear()
        {
            foreach (var layer in RenderLayers)
            {
                layer.Reset(true);
            }
        }
    }
}
