
namespace TriangleNet.Rendering
{
    using System.Collections.Generic;
    using System.Linq;
    using TriangleNet.Geometry;
    using TriangleNet.Meshing;
    using TriangleNet.Voronoi.Legacy;

    /// <summary>
    /// The RenderContext class brings all the rendering parts together.
    /// </summary>
    public class RenderContext : IRenderContext
    {
        private ColorManager colorManager;
        private BoundingBox bounds;
        private Projection zoom;
        private IMesh mesh;

        private List<IRenderLayer> renderLayers;

        public RenderContext(Projection zoom, ColorManager colorManager)
        {
            bounds = new BoundingBox();

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

        public ColorManager ColorManager
        {
            get { return colorManager; }
        }

        public BoundingBox Bounds
        {
            get { return bounds; }
        }

        public IList<IRenderLayer> RenderLayers
        {
            get { return renderLayers; }
        }

        public Projection Zoom
        {
            get { return zoom; }
        }

        public IMesh Mesh
        {
            get { return mesh; }
        }

        public bool HasData
        {
            get
            {
                return renderLayers.Any(layer => !layer.IsEmpty());
            }
        }

        public void Add(IPolygon data)
        {
            foreach (var layer in RenderLayers)
            {
                layer.Reset(true);
            }

            // Always clear voronoi layer.
            RenderLayers[4].Reset(true);

            int i = 0;

            // Ensure linear numbering of polygon vertices.
            foreach (var p in data.Points)
            {
                p.ID = i++;
            }

            this.bounds = RenderLayers[2].SetPoints(data);
            this.zoom.Initialize(bounds);

            RenderLayers[2].SetPolygon(data);
            RenderLayers[3].SetPoints(RenderLayers[2].Points);
        }

        public void Add(IMesh data, bool reset)
        {
            foreach (var layer in RenderLayers)
            {
                layer.Reset(reset);
            }

            // Always clear voronoi layer.
            RenderLayers[4].Reset(true);

            // Save reference to mesh.
            this.mesh = data;

            this.bounds = RenderLayers[1].SetPoints(data);
            this.zoom.Initialize(bounds);

            RenderLayers[1].SetMesh(data, false);

            RenderLayers[2].SetPoints(RenderLayers[1].Points);
            RenderLayers[2].SetPolygon(data);

            RenderLayers[3].SetPoints(RenderLayers[1].Points);
        }

        public void Add(ICollection<Point> points, IEnumerable<IEdge> edges, bool reset)
        {
            RenderLayers[4].SetPoints(points);
            RenderLayers[4].SetMesh(edges);
            RenderLayers[4].IsEnabled = true;
        }

        public void Add(float[] data)
        {
            // Add function values for filled mesh.
            RenderLayers[0].SetPoints(RenderLayers[1].Points);
            RenderLayers[0].SetMesh(this.mesh, true);
            RenderLayers[0].AttachLayerData(data, colorManager.ColorMap);

            RenderLayers[0].IsEnabled = true;
        }

        public void Add(int[] data)
        {
            // Add partition data for filled mesh.
            RenderLayers[0].SetPoints(RenderLayers[1].Points);
            RenderLayers[0].SetMesh(this.mesh, true);
            RenderLayers[0].AttachLayerData(data);

            RenderLayers[0].IsEnabled = true;
        }

        public void Enable(int layer, bool enabled)
        {
            renderLayers[layer].IsEnabled = enabled;
        }

        public void Clear()
        {
        }
    }
}
