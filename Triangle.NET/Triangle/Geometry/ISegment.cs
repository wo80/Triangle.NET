// -----------------------------------------------------------------------
// <copyright file="Segment.cs" company="">
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Geometry
{
    using TriangleNet.Data;

    /// <summary>
    /// Interface for segment geometry.
    /// </summary>
    public interface ISegment : IEdge
    {
        #region Public properties

        /// <summary>
        /// Gets the segments endpoint.
        /// </summary>
        /// <param name="index">The vertex index (0 or 1).</param>
        Vertex GetVertex(int index);

        /// <summary>
        /// Gets an adjoining triangle.
        /// </summary>
        /// <param name="index">The triangle index (0 or 1).</param>
        ITriangle GetTriangle(int index);

        #endregion
    }
}
