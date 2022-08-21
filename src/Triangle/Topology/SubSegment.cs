// -----------------------------------------------------------------------
// <copyright file="SubSegment.cs" company="">
// Triangle Copyright (c) 1993, 1995, 1997, 1998, 2002, 2005 Jonathan Richard Shewchuk
// Triangle.NET code by Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Topology
{
    using System;
    using TriangleNet.Geometry;

    /// <summary>
    /// The subsegment data structure.
    /// </summary>
    public class SubSegment : ISegment
    {
        // Hash for dictionary. Will be set by mesh instance.
        internal int hash;

        internal Osub[] subsegs;
        internal Vertex[] vertices;
        internal Otri[] triangles;
        internal int boundary;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubSegment" /> class.
        /// </summary>
        public SubSegment()
        {
            // Four NULL vertices.
            vertices = new Vertex[4];

            // Set the boundary marker to zero.
            boundary = 0;

            // Initialize the two adjoining subsegments to be the omnipresent
            // subsegment.
            subsegs = new Osub[2];

            // Initialize the two adjoining triangles to be "outer space."
            triangles = new Otri[2];
        }

        #region Public properties

        /// <summary>
        /// Gets the first endpoints vertex id.
        /// </summary>
        public int P0 => vertices[0].id;

        /// <summary>
        /// Gets the seconds endpoints vertex id.
        /// </summary>
        public int P1 => vertices[1].id;

        /// <summary>
        /// Gets the segment boundary mark.
        /// </summary>
        public int Label => boundary;

        #endregion

        /// <summary>
        /// Gets the segments endpoint.
        /// </summary>
        public Vertex GetVertex(int index)
        {
            return vertices[index]; // TODO: Check range?
        }

        /// <summary>
        /// Gets an adjoining triangle.
        /// </summary>
        public ITriangle GetTriangle(int index)
        {
            return triangles[index].tri.hash == Mesh.DUMMY ? null : triangles[index].tri;
        }

        /// <summary>
        /// Gets an adjoining triangle.
        /// </summary>
        public void GetTriangle(int index, ref Otri otri)
        {
            otri = triangles[index];
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return hash;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("SID {0}", hash);
        }
    }
}
