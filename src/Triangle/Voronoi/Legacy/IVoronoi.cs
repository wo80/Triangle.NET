// -----------------------------------------------------------------------
// <copyright file="IVoronoi.cs" company="">
// Triangle.NET Copyright (c) 2012-2022 Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Voronoi.Legacy
{
    using System.Collections.Generic;
    using TriangleNet.Geometry;

    /// <summary>
    /// Voronoi diagram interface.
    /// </summary>
    public interface IVoronoi
    {
        /// <summary>
        /// Gets the list of Voronoi vertices.
        /// </summary>
        Point[] Points { get; }

        /// <summary>
        /// Gets the list of Voronoi regions.
        /// </summary>
        ICollection<VoronoiRegion> Regions { get; }

        /// <summary>
        /// Gets the list of edges.
        /// </summary>
        IEnumerable<IEdge> Edges { get; }
    }
}
