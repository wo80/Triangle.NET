
namespace TriangleNet.Rendering
{
    using System.Collections.Generic;
    using TriangleNet.Geometry;
    using TriangleNet.Meshing;
    using TriangleNet.Rendering.Buffer;
    using TriangleNet.Rendering.Util;

    using Color = System.Drawing.Color;

    /// <summary>
    /// Interface for managing the data of a render layer.
    /// </summary>
    public interface IRenderLayer
    {
        /// <summary>
        /// Gets the number of points in the point buffer.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the points buffer.
        /// </summary>
        IBuffer<float> Points { get; }

        /// <summary>
        /// Gets the indices buffer.
        /// </summary>
        IBuffer<int> Indices { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the layer is enabled.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Indicates whether this layer contains data to render.
        /// </summary>
        /// <returns>Returns true, if the points buffer contains data.</returns>
        bool IsEmpty();

        /// <summary>
        /// Resets this layer to an empty state.
        /// </summary>
        /// <param name="clear">If true, all buffers will be set to <c>null</c>.</param>
        void Reset(bool clear);

        // TODO: add boolean: reset

        /// <summary>
        /// Replaces the current points with the given buffer.
        /// </summary>
        /// <param name="buffer">The new points buffer.</param>
        void SetPoints(IBuffer<float> buffer);

        /// <summary>
        /// Copy the points of the given <see cref="IPolygon"/> to the layers point buffer.
        /// </summary>
        /// <param name="poly">The polygon to render.</param>
        void SetPoints(IPolygon poly);

        /// <summary>
        /// Copy the points of the given <see cref="IMesh"/> to the layers point buffer.
        /// </summary>
        /// <param name="mesh">The mesh to render.</param>
        void SetPoints(IMesh mesh);

        /// <summary>
        /// Copy the points of the given collection to the layers point buffer.
        /// </summary>
        /// <param name="points">The point set to render.</param>
        void SetPoints(ICollection<Point> points);

        /// <summary>
        /// Copy the segment indices of the given polygon to the layers index buffer.
        /// </summary>
        /// <param name="poly">The polygon to render.</param>
        void SetPolygon(IPolygon poly);

        /// <summary>
        /// Copy the segment indices of the given mesh to the layers index buffer.
        /// </summary>
        /// <param name="mesh">The mesh to render.</param>
        void SetPolygon(IMesh mesh);

        /// <summary>
        /// Copy the indices of the given mesh triangles to the layers index buffer.
        /// </summary>
        /// <param name="mesh">The mesh to render.</param>
        /// <param name="elements">If true, all triangle indices are copied. Otherwise,
        /// only edge indices are copied.</param>
        /// <remarks>
        /// Use <c>elements = true</c> for layers rendering filled triangles (3 indices per buffer item).
        /// Use <c>elements = false</c> if only edges are rendered (wireframe, 2 indices per buffer item).
        /// </remarks>
        void SetMesh(IMesh mesh, bool elements);

        /// <summary>
        /// Copy the indices of the given edges to the layers index buffer.
        /// </summary>
        /// <param name="edges">The edges to render.</param>
        void SetMesh(IEnumerable<IEdge> edges);

        #region Attached data (mesh partitioning and heat map rendering)

        // TODO: better put attached data into a subclass?

        /// <summary>
        /// Gets the mesh partition.
        /// </summary>
        /// <remarks>
        /// Triangle <c>i</c> given by indices <c>[3 * i, 3 * i + 1, 3 * i + 2]</c>
        /// belongs to <c>Partition[i]</c>.
        /// </remarks>
        IBuffer<int> Partition { get; }

        /// <summary>
        /// Gets the color attached to a point in the points buffer.
        /// </summary>
        IBuffer<Color> Colors { get; }

        /// <summary>
        /// Attach function values <c>z=f(x,y)</c> for all points <c>(x,y)</c> in the point buffer.
        /// </summary>
        /// <param name="values">The function values.</param>
        /// <param name="colormap">The color map.</param>
        void AttachLayerData(float[] values, ColorMap colormap);

        /// <summary>
        /// Attach partitioning data to each triangle in the index buffer.
        /// </summary>
        /// <param name="partition">The mesh partition.</param>
        void AttachLayerData(int[] partition);

        #endregion
    }
}
