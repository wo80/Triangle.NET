
namespace TriangleNet.Rendering
{
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
        IBuffer<uint> Indices { get; }

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
        /// <param name="clear">If true, point buffer will be cleared.</param>
        void Reset(bool clear);

        void SetPoints(IBuffer<float> buffer, bool reset = true);

        void SetIndices(IBuffer<uint> buffer);

        #region Attached data (mesh partitioning and heat map rendering)

        // TODO: better put attached data into a subclass?

        /// <summary>
        /// Gets the mesh partition.
        /// </summary>
        /// <remarks>
        /// Triangle <c>i</c> given by indices <c>[3 * i, 3 * i + 1, 3 * i + 2]</c>
        /// belongs to <c>Partition[i]</c>.
        /// </remarks>
        IBuffer<uint> Partition { get; }

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
        void AttachLayerData(uint[] partition);

        #endregion
    }
}
