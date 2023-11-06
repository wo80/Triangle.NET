
namespace TriangleNet.Rendering
{
    using System.Collections.Generic;
    using TriangleNet.Geometry;
    using TriangleNet.Meshing;
    using TriangleNet.Rendering.Util;

    /// <summary>
    /// A helper class to handle <see cref="IRenderControl"/> and <see cref="IRenderContext"/>.
    /// </summary>
    public class RenderManager
    {
        // TODO: delete
        public static bool VORONOI_DEBUG = false;

        IRenderControl control;
        IRenderContext context;
        IRenderer renderer;
        Projection zoom;

        public IRenderControl Control => control; 

        public IRenderContext Context => context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderManager"/> class.
        /// </summary>
        public RenderManager()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderManager"/> class.
        /// </summary>
        /// <param name="control">The <see cref="IRenderControl"/> instance.</param>
        /// <param name="renderer">The <see cref="IRenderer"/> instance.</param>
        public RenderManager(IRenderControl control, IRenderer renderer)
        {
            Initialize(control, renderer);
        }

        /// <summary>
        /// Initializes the <see cref="RenderManager"/> instance.
        /// </summary>
        /// <param name="control">The <see cref="IRenderControl"/> instance.</param>
        /// <param name="renderer">The <see cref="IRenderer"/> instance.</param>
        public void Initialize(IRenderControl control, IRenderer renderer)
        {
            zoom = new Projection(control.ClientRectangle);

            context = new RenderContext(zoom, ColorManager.Default());

            this.renderer = renderer;
            this.renderer.Context = context;

            this.control = control;
            this.control.Initialize();
            this.control.Renderer = renderer;
        }

        /// <summary>
        /// Try to create a <see cref="IRenderControl"/> form the given assembly name.
        /// </summary>
        /// <param name="assemblyName">The name of the assembly that contains the control.</param>
        /// <param name="dependencies">A list of (local) assembly dependencies.</param>
        /// <param name="control">The <see cref="IRenderControl"/> (output).</param>
        /// <returns>True if control was created successfully.</returns>
        public bool TryCreateControl(string assemblyName, IEnumerable<string> dependencies,
            out IRenderControl control)
        {
            return ReflectionHelper.TryCreateControl(assemblyName, dependencies, out control);
        }

        /// <summary>
        /// Update render control to reflect size changes of the window.
        /// </summary>
        public void Resize()
        {
            control.HandleResize();
        }

        /// <summary>
        /// Clear all data.
        /// </summary>
        public void Clear()
        {
            context.Clear();
            control.Refresh();
        }

        /// <summary>
        /// Enable or disable the given layer.
        /// </summary>
        public void Enable(int layer, bool enabled)
        {
            context.Enable(layer, enabled);

            control.Refresh();
        }

        /// <summary>
        /// Add polygon data.
        /// </summary>
        public void Set(IPolygon data, bool refresh = true)
        {
            context.Add(data);

            if (refresh)
            {
                control.Refresh();
            }
        }

        /// <summary>
        /// Add mesh data.
        /// </summary>
        public void Set(IMesh data, bool reset, bool refresh = true)
        {
            context.Add(data, reset);

            if (refresh)
            {
                control.Refresh();
            }
        }

        /// <summary>
        /// Add data for Voronoi layer.
        /// </summary>
        public void Set(ICollection<Point> points, IEnumerable<IEdge> edges, bool reset, bool refresh = true)
        {
            context.Add(points, edges, reset);

            if (refresh)
            {
                control.Refresh();
            }
        }

        /// <summary>
        /// Update data for function values.
        /// </summary>
        public void Update(float[] values)
        {
            context.Add(values);
            control.Refresh();
        }

        /// <summary>
        /// Update data for mesh partitioning.
        /// </summary>
        public void Update(uint[] partition)
        {
            context.Add(partition);
            control.Refresh();
        }
    }
}
