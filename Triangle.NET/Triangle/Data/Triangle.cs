// -----------------------------------------------------------------------
// <copyright file="Triangle.cs" company="">
// Original Triangle code by Jonathan Richard Shewchuk, http://www.cs.cmu.edu/~quake/triangle.html
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Data
{
    using System;
    using TriangleNet.Geometry;

    /// <summary>
    /// The triangle data structure.
    /// </summary>
    public class Triangle : ITriangle
    {
        #region Static initialization of "Outer Space" triangle

        // The triangle that fills "outer space," called 'dummytri', is pointed to
        // by every triangle and subsegment on a boundary (be it outer or inner) of
        // the triangulation. Also, 'dummytri' points to one of the triangles on
        // the convex hull (until the holes and concavities are carved), making it
        // possible to find a starting triangle for point location.

        // 'dummytri' and 'dummysub' are generally required to fulfill only a few
        // invariants: their vertices must remain NULL and 'dummytri' must always
        // be bonded (at offset zero) to some triangle on the convex hull of the
        // mesh, via a boundary edge. Otherwise, the connections of 'dummytri' and
        // 'dummysub' may change willy-nilly. This makes it possible to avoid
        // writing a good deal of special-case code (in the edge flip, for example)
        // for dealing with the boundary of the mesh, places where no subsegment is
        // present, and so forth.  Other entities are frequently bonded to
        // 'dummytri' and 'dummysub' as if they were real mesh entities, with no
        // harm done.

        internal const int EmptyID = -1;

        internal static Triangle Empty;

        /// <summary>
        /// Initializes the dummytri (Triangle.Empty). The method is called by the static Segment
        /// constructor (which ensures that dummysub (Segment.Empty) will not be null).
        /// </summary>
        internal static void Initialize()
        {
            // Set up 'dummytri', the 'triangle' that occupies "outer space."
            Empty = new Triangle();
            Empty.hash = EmptyID;
            Empty.id = EmptyID;

            // Initialize the three adjoining triangles to be "outer space." These
            // will eventually be changed by various bonding operations, but their
            // values don't really matter, as long as they can legally be
            // dereferenced.
            Empty.neighbors[0].triangle = Empty;
            Empty.neighbors[1].triangle = Empty;
            Empty.neighbors[2].triangle = Empty;

            // Initialize the three adjoining subsegments of 'dummytri' to be
            // the omnipresent subsegment.
            Empty.subsegs[0].seg = Segment.Empty;
            Empty.subsegs[1].seg = Segment.Empty;
            Empty.subsegs[2].seg = Segment.Empty;
        }

        #endregion

        // Hash for dictionary. Will be set by mesh instance.
        internal int hash;

        // The ID is only used for mesh output.
        internal int id;

        internal Otri[] neighbors;
        internal Vertex[] vertices;
        internal Osub[] subsegs;
        internal int region;
        internal double area;
        internal bool infected;

        public Triangle()
        {
            // Three NULL vertices.
            vertices = new Vertex[3];

            // Initialize the three adjoining subsegments to be the omnipresent subsegment.
            subsegs = new Osub[3];
            subsegs[0].seg = Segment.Empty;
            subsegs[1].seg = Segment.Empty;
            subsegs[2].seg = Segment.Empty;

            // Initialize the three adjoining triangles to be "outer space".
            neighbors = new Otri[3];
            neighbors[0].triangle = Empty;
            neighbors[1].triangle = Empty;
            neighbors[2].triangle = Empty;

            // area = -1.0;
        }

        #region Public properties

        /// <summary>
        /// Gets the triangle id.
        /// </summary>
        public int ID
        {
            get { return this.id; }
        }

        /// <summary>
        /// Gets the first corners vertex id.
        /// </summary>
        public int P0
        {
            get { return this.vertices[0] == null ? -1 : this.vertices[0].id; }
        }

        /// <summary>
        /// Gets the seconds corners vertex id.
        /// </summary>
        public int P1
        {
            get { return this.vertices[1] == null ? -1 : this.vertices[1].id; }
        }

        /// <summary>
        /// Gets the specified corners vertex.
        /// </summary>
        public Vertex GetVertex(int index)
        {
            return this.vertices[index]; // TODO: Check range?
        }

        /// <summary>
        /// Gets the third corners vertex id.
        /// </summary>
        public int P2
        {
            get { return this.vertices[2] == null ? -1 : this.vertices[2].id; }
        }

        public bool SupportsNeighbors
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the first neighbors id.
        /// </summary>
        public int N0
        {
            get { return this.neighbors[0].triangle.id; }
        }

        /// <summary>
        /// Gets the second neighbors id.
        /// </summary>
        public int N1
        {
            get { return this.neighbors[1].triangle.id; }
        }

        /// <summary>
        /// Gets the third neighbors id.
        /// </summary>
        public int N2
        {
            get { return this.neighbors[2].triangle.id; }
        }

        /// <summary>
        /// Gets a triangles' neighbor.
        /// </summary>
        /// <param name="index">The neighbor index (0, 1 or 2).</param>
        /// <returns>The neigbbor opposite of vertex with given index.</returns>
        public ITriangle GetNeighbor(int index)
        {
            return neighbors[index].triangle.id == EmptyID ? null : neighbors[index].triangle;
        }

        /// <summary>
        /// Gets a triangles segment.
        /// </summary>
        /// <param name="index">The vertex index (0, 1 or 2).</param>
        /// <returns>The segment opposite of vertex with given index.</returns>
        public ISegment GetSegment(int index)
        {
            return subsegs[index].seg == Segment.Empty ? null : subsegs[index].seg;
        }

        /// <summary>
        /// Gets the triangle area constraint.
        /// </summary>
        public double Area
        {
            get { return this.area; }
            set { this.area = value; }
        }

        /// <summary>
        /// Region ID the triangle belongs to.
        /// </summary>
        public int Region
        {
            get { return this.region; }
        }

        #endregion

        public override int GetHashCode()
        {
            return this.hash;
        }

        public override string ToString()
        {
            return String.Format("TID {0}", hash);
        }
    }
}
