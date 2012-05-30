// -----------------------------------------------------------------------
// <copyright file="ITriangle.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Geometry
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Triangle interface.
    /// </summary>
    public interface ITriangle
    {
        /// <summary>
        /// The triangle id.
        /// </summary>
        int ID { get; }

        /// <summary>
        /// First vertex id of the triangle.
        /// </summary>
        int P0 { get; }
        /// <summary>
        /// Second vertex id of the triangle.
        /// </summary>
        int P1 { get; }
        /// <summary>
        /// Third vertex id of the triangle.
        /// </summary>
        int P2 { get; }
        /// <summary>
        /// Gets the specified vertex id.
        /// </summary>
        int this[int index] { get; }

        /// <summary>
        /// True if the triangle implementation contains neighbor information.
        /// </summary>
        bool SupportsNeighbors { get; }

        /// <summary>
        /// First neighbor.
        /// </summary>
        int N0 { get; }
        /// <summary>
        /// Second neighbor.
        /// </summary>
        int N1 { get; }
        /// <summary>
        /// Third neighbor.
        /// </summary>
        int N2 { get; }

        /// <summary>
        /// Triangle area constraint.
        /// </summary>
        double Area { get; }

        /// <summary>
        /// Triangle atributes.
        /// </summary>
        double[] Attributes { get; }
    }
}
