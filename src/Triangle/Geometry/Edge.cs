// -----------------------------------------------------------------------
// <copyright file="Edge.cs" company="">
// Triangle.NET Copyright (c) 2012-2022 Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Geometry
{
    /// <summary>
    /// Represents a straight line segment in 2D space.
    /// </summary>
    public class Edge : IEdge
    {
        /// <summary>
        /// Gets the first endpoints index.
        /// </summary>
        public int P0 { get; private set; }

        /// <summary>
        /// Gets the second endpoints index.
        /// </summary>
        public int P1 { get; private set; }

        /// <summary>
        /// Gets the segments boundary mark.
        /// </summary>
        public int Label { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Edge" /> class.
        /// </summary>
        public Edge(int p0, int p1)
            : this(p0, p1, 0)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Edge" /> class.
        /// </summary>
        public Edge(int p0, int p1, int label)
        {
            P0 = p0;
            P1 = p1;
            Label = label;
        }
    }
}
