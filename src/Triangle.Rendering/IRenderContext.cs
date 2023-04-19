﻿
namespace TriangleNet.Rendering
{
    using System.Collections.Generic;
    using Geometry;
    using Meshing;

    public interface IRenderContext
    {
        /// <summary>
        /// Gets the color manager.
        /// </summary>
        ColorManager ColorManager { get; }

        /// <summary>
        /// Gets the list of <see cref="IRenderLayer"/>s.
        /// </summary>
        IList<IRenderLayer> RenderLayers { get; }

        /// <summary>
        /// Gets the <see cref="Projection"/>.
        /// </summary>
        Projection Zoom { get; }

        /// <summary>
        /// Gets the <see cref="IMesh"/>.
        /// </summary>
        IMesh Mesh { get; }

        /// <summary>
        /// Gets a value indicating whether the context has data to render. 
        /// </summary>
        bool HasData { get; }

        /// <summary>
        /// Add polygon data.
        /// </summary>
        /// <param name="data"></param>
        void Add(IPolygon data);

        /// <summary>
        /// Add mesh data.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="reset"></param>
        void Add(IMesh data, bool reset);

        /// <summary>
        /// Add edge data (used for Voronoi).
        /// </summary>
        /// <param name="points"></param>
        /// <param name="edges"></param>
        /// <param name="reset"></param>
        void Add(ICollection<Point> points, IEnumerable<IEdge> edges, bool reset);

        /// <summary>
        /// Add mesh function values <c>z=f(x,y)</c>.
        /// </summary>
        /// <param name="values"></param>
        void Add(float[] values);

        /// <summary>
        /// Add mesh partitioning data.
        /// </summary>
        /// <param name="partition"></param>
        void Add(int[] partition);

        /// <summary>
        /// Enable or disable a layer for rendering.
        /// </summary>
        /// <param name="layer">The layer index.</param>
        /// <param name="enabled">If true, enable layer, otherwise disable.</param>
        /// <remarks>
        ///  0 = mesh (filled)
        ///  1 = mesh (wireframe)
        ///  2 = polygon
        ///  3 = points
        ///  4 = voronoi overlay
        ///  5 = vector field
        ///  6 = contour lines
        /// </remarks>
        void Enable(int layer, bool enabled);

        /// <summary>
        /// Clear data from all layers.
        /// </summary>
        void Clear();
    }
}
