﻿// -----------------------------------------------------------------------
// <copyright file="Face.cs">
// Triangle.NET Copyright (c) 2012-2022 Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Topology.DCEL
{
    using System.Collections.Generic;
    using Geometry;

    /// <summary>
    /// A face of the DCEL datastructure.
    /// </summary>
    public class Face
    {
        #region Static initialization of "Outer Space" face

        /// <summary>
        /// A face representing "outer space".
        /// </summary>
        public static readonly Face Empty;

        static Face()
        {
            Empty = new Face(null);
            Empty.id = -1;
        }

        #endregion

        internal int id;
        internal int mark;

        // If the face is a Voronoi cell, this is the point that generates the cell.
        internal Point? generator;

        internal HalfEdge? edge;
        internal bool bounded;

        /// <summary>
        /// If part of a Voronoi diagram, returns the generator vertex
        /// of the face. Otherwise <c>null</c>.
        /// </summary>
        public Point Generator => generator;

        /// <summary>
        /// Gets or sets the face id.
        /// </summary>
        public int ID
        {
            get => id;
            set => id = value;
        }

        /// <summary>
        /// Gets or sets a half-edge connected to the face.
        /// </summary>
        public HalfEdge? Edge
        {
            get => edge;
            set => edge = value;
        }

        /// <summary>
        /// Gets or sets a value, indicating if the face is bounded (for Voronoi diagram).
        /// </summary>
        public bool Bounded
        {
            get => bounded;
            set => bounded = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Face" /> class.
        /// </summary>
        /// <param name="generator">The generator of this face (for Voronoi diagram)</param>
        public Face(Point generator)
            : this(generator, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Face" /> class.
        /// </summary>
        /// <param name="generator">The generator of this face (for Voronoi diagram)</param>
        /// <param name="edge">The half-edge connected to this face.</param>
        public Face(Point? generator, HalfEdge? edge)
        {
            this.generator = generator;
            this.edge = edge;

            bounded = true;

            if (generator is not null)
            {
                id = generator.ID;
            }
        }

        /// <summary>
        /// Enumerates all half-edges of the face boundary.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HalfEdge> EnumerateEdges()
        {
            var edge = Edge;
            var first = edge.ID;

            do
            {
                yield return edge;

                edge = edge.Next;
            } while (edge.ID != first);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("F-ID {0}", id);
        }
    }
}
