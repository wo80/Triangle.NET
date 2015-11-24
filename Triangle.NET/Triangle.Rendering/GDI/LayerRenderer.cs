
namespace TriangleNet.Rendering.GDI
{
    using System.Drawing;

    public class LayerRenderer : IRenderer
    {
        MeshRenderer meshRenderer;
        FunctionRenderer functionRenderer;

        public IRenderContext Context { get; set; }

        public Graphics RenderTarget { get; set; }

        public LayerRenderer()
        {
            meshRenderer = new MeshRenderer();
            functionRenderer = new FunctionRenderer();
        }

        public void Render()
        {
            meshRenderer.Context = Context;
            meshRenderer.RenderTarget = RenderTarget;

            functionRenderer.Context = Context;
            functionRenderer.RenderTarget = RenderTarget;

            // 0 = mesh (filled)
            // 1 = mesh (wireframe)
            // 2 = polygon
            // 3 = points
            // 4 = voronoi overlay
            // 5 = vector field
            // 6 = contour lines

            int i = 0;

            foreach (var layer in this.Context.RenderLayers)
            {
                if (!layer.IsEmpty() && layer.IsEnabled)
                {
                    switch (i)
                    {
                        case 0:
                            RenderFilledMesh(layer);
                            break;
                        case 1:
                            RenderMesh(layer);
                            break;
                        case 2:
                            RenderPolygon(layer);
                            break;
                        case 3:
                            RenderPoints(layer);
                            break;
                        case 4:
                            RenderVoronoi(layer);
                            break;
                        case 5:
                        case 6:
                        default:
                            break;
                    }
                }

                i++;
            }
        }

        private void RenderFilledMesh(IRenderLayer layer)
        {
            if (layer.Partition != null)
            {
                meshRenderer.RenderElements(layer.Points.Data, layer.Indices.Data, 3, layer.Partition.Data);
            }
            else if (layer.Colors != null)
            {
                functionRenderer.Render(layer);
            }
        }

        private void RenderMesh(IRenderLayer layer)
        {
            if (layer.Indices.Size == 3)
            {
                meshRenderer.RenderElements(layer.Points.Data, layer.Indices.Data, 3, null);
            }
            else
            {
                meshRenderer.RenderEdges(layer.Points.Data, layer.Indices.Data, Context.ColorManager.Line);
            }
        }

        private void RenderPolygon(IRenderLayer layer)
        {
            meshRenderer.RenderSegments(layer.Points.Data, layer.Indices.Data, Context.ColorManager.Segment);
        }

        private void RenderPoints(IRenderLayer layer)
        {
            meshRenderer.RenderPoints(layer.Points.Data, layer.Points.Size, layer.Count);
        }

        private void RenderVoronoi(IRenderLayer layer)
        {
            if (RenderManager.VORONOI_DEBUG)
            {
                meshRenderer.RenderEdges(layer.Points.Data, layer.Indices.Data, Pens.Purple);
                meshRenderer.RenderPoints(layer.Points.Data, layer.Points.Size, 0, layer.Count, Brushes.Red);
            }
            else
            {
                meshRenderer.RenderEdges(layer.Points.Data, layer.Indices.Data, Context.ColorManager.VoronoiLine);
            }
        }
    }
}
