// -----------------------------------------------------------------------
// <copyright file="IEdge.cs" company="">
// Triangle.NET Copyright (c) 2012-2022 Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Geometry
{
    /// <summary>
    /// Simple edge interface.
    /// </summary>
    public interface IEdge
    {
        /// <summary>
        /// Gets the first endpoints index.
        /// </summary>
        int P0 { get; }

        /// <summary>
        /// Gets the second endpoints index.
        /// </summary>
        int P1 { get; }

        /// <summary>
        /// Gets or sets a general-purpose label.
        /// </summary>
        /// <remarks>
        /// This is used for the segments boundary mark.
        /// </remarks>
        int Label { get; }
    }
}
