// -----------------------------------------------------------------------
// <copyright file="Segment.cs" company="">
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Geometry
{
    /// <summary>
    /// Interface for segment geometry.
    /// </summary>
    public interface ISegment : IEdge
    {
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
    }
}
