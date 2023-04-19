
namespace TriangleNet.Rendering
{
    using System.Collections.Generic;
    using Geometry;
    using Meshing;
    using Util;

    public class RenderManager
    {
        // TODO: delete
        public static bool VORONOI_DEBUG = false;

        private IRenderer renderer;
        private Projection zoom;

        public IRenderControl Control { get; private set; }

        public IRenderContext Context { get; private set; }

        public RenderManager()
        {
        }

        public RenderManager(IRenderControl control, IRenderer renderer)
        {
            Initialize(control, renderer);
        }

        public void Initialize(IRenderControl control, IRenderer renderer)
        {
            zoom = new Projection(control.ClientRectangle);

            Context = new RenderContext(zoom, ColorManager.Default());

            this.renderer = renderer;
            this.renderer.Context = Context;

            this.Control = control;
            this.Control.Initialize();
            this.Control.Renderer = renderer;
        }

        public bool TryCreateControl(string assemblyName, IEnumerable<string> dependencies,
            out IRenderControl control)
        {
            return ReflectionHelper.TryCreateControl(assemblyName, dependencies, out control);
        }

        public void Resize()
        {
            Control.HandleResize();
        }

        public void Clear()
        {
            Context.Clear();
            Control.Refresh();
        }

        public void Enable(int layer, bool enabled)
        {
            Context.Enable(layer, enabled);

            Control.Refresh();
        }

        public void Set(IPolygon data, bool refresh = true)
        {
            Context.Add(data);

            if (refresh)
            {
                Control.Refresh();
            }
        }

        public void Set(IMesh data, bool reset, bool refresh = true)
        {
            Context.Add(data, reset);

            if (refresh)
            {
                Control.Refresh();
            }
        }

        /// <summary>
        /// Set data for Voronoi layer.
        /// </summary>
        public void Set(ICollection<Point> points, IEnumerable<IEdge> edges, bool reset, bool refresh = true)
        {
            Context.Add(points, edges, reset);

            if (refresh)
            {
                Control.Refresh();
            }
        }

        public void Update(float[] values)
        {
            Context.Add(values);
            Control.Refresh();
        }

        public void Update(int[] partition)
        {
            Context.Add(partition);
            Control.Refresh();
        }
    }
}
