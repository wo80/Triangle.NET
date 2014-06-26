// -----------------------------------------------------------------------
// <copyright file="Segment.cs" company="">
// Original Triangle code by Jonathan Richard Shewchuk, http://www.cs.cmu.edu/~quake/triangle.html
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Data
{
    using System;
    using TriangleNet.Geometry;

    /// <summary>
    /// The subsegment data structure.
    /// </summary>
    public class Segment : ISegment
    {
        #region Static initialization of "omnipresent" subsegment

        // Set up 'dummysub', the omnipresent subsegment pointed to by any
        // triangle side or subsegment end that isn't attached to a real
        // subsegment.

        internal static Segment Empty;

        static Segment()
        {
            Empty = new Segment();
            Empty.hash = -1;

            // Initialize the two adjoining subsegments to be the omnipresent
            // subsegment. These will eventually be changed by various bonding
            // operations, but their values don't really matter, as long as they
            // can legally be dereferenced.
            Empty.subsegs[0].seg = Empty;
            Empty.subsegs[1].seg = Empty;

            Triangle.Initialize();
        }

        #endregion

        // Hash for dictionary. Will be set by mesh instance.
        internal int hash;

        internal Osub[] subsegs;
        internal Vertex[] vertices;
        internal Otri[] triangles;
        internal int boundary;

        public Segment()
        {
            // Initialize the two adjoining subsegments to be the omnipresent
            // subsegment.
            subsegs = new Osub[2];
            subsegs[0].seg = Empty;
            subsegs[1].seg = Empty;

            // Four NULL vertices.
            vertices = new Vertex[4];

            // Initialize the two adjoining triangles to be "outer space."
            triangles = new Otri[2];
            triangles[0].triangle = Triangle.Empty;
            triangles[1].triangle = Triangle.Empty;

            // Set the boundary marker to zero.
            boundary = 0;
        }

        #region Public properties

        /// <summary>
        /// Gets the first endpoints vertex id.
        /// </summary>
        public int P0
        {
            get { return this.vertices[0].id; }
        }

        /// <summary>
        /// Gets the seconds endpoints vertex id.
        /// </summary>
        public int P1
        {
            get { return this.vertices[1].id; }
        }

        /// <summary>
        /// Gets the segment boundary mark.
        /// </summary>
        public int Boundary
        {
            get { return this.boundary; }
        }

        #endregion

        /// <summary>
        /// Gets the segments endpoint.
        /// </summary>
        public Vertex GetVertex(int index)
        {
            return this.vertices[index]; // TODO: Check range?
        }

        /// <summary>
        /// Gets an adjoining triangle.
        /// </summary>
        public ITriangle GetTriangle(int index)
        {
            return triangles[index].triangle.id == Triangle.EmptyID ? null : triangles[index].triangle;
        }

        public override int GetHashCode()
        {
            return this.hash;
        }

        public override string ToString()
        {
            return String.Format("SID {0}", hash);
        }
    }
}
