// -----------------------------------------------------------------------
// <copyright file="Vertex.cs">
// Triangle.NET Copyright (c) 2012-2022 Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Topology.DCEL
{
    using System.Collections.Generic;

    /// <summary>
    /// A vertex of the DCEL datastructure.
    /// </summary>
    public class Vertex : Geometry.Point
    {
        /// <summary>
        /// Gets or sets a half-edge leaving the vertex.
        /// </summary>
        public HalfEdge? Leaving { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vertex" /> class.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public Vertex(double x, double y)
            : base(x, y)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vertex" /> class.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="leaving">A half-edge leaving this vertex.</param>
        public Vertex(double x, double y, HalfEdge leaving)
            : base(x, y)
        {
            Leaving = leaving;
        }

        /// <summary>
        /// Enumerates all half-edges leaving this vertex.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HalfEdge> EnumerateEdges()
        {
            var edge = Leaving;
            if (edge is null) yield break;
            var first = edge.ID;

            do
            {
                yield return edge;

                edge = edge.Twin.Next;
            } while (edge.ID != first);
        }

        /// <inheritdoc />
        public override string ToString() => $"V-ID {id}";
    }
}
