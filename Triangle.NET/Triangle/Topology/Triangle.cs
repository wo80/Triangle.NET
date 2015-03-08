// -----------------------------------------------------------------------
// <copyright file="Triangle.cs" company="">
// Original Triangle code by Jonathan Richard Shewchuk, http://www.cs.cmu.edu/~quake/triangle.html
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Topology
{
    using System;
    using TriangleNet.Geometry;

    /// <summary>
    /// The triangle data structure.
    /// </summary>
    public class Triangle : ITriangle
    {
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

        /// <summary>
        /// Initializes a new instance of the <see cref="Triangle" /> class.
        /// </summary>
        public Triangle()
        {
            // Three NULL vertices.
            vertices = new Vertex[3];

            // Initialize the three adjoining subsegments to be the omnipresent subsegment.
            subsegs = new Osub[3];

            // Initialize the three adjoining triangles to be "outer space".
            neighbors = new Otri[3];

            // area = -1.0;
        }

        #region Public properties

        /// <summary>
        /// Gets or sets the triangle id.
        /// </summary>
        public int ID
        {
            get { return this.id; }
            set { this.id = value; }
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
        /// Gets the third corners vertex id.
        /// </summary>
        public int P2
        {
            get { return this.vertices[2] == null ? -1 : this.vertices[2].id; }
        }

        /// <summary>
        /// Gets the specified corners vertex.
        /// </summary>
        public Vertex GetVertex(int index)
        {
            return this.vertices[index]; // TODO: Check range?
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
            get { return this.neighbors[0].tri.id; }
        }

        /// <summary>
        /// Gets the second neighbors id.
        /// </summary>
        public int N1
        {
            get { return this.neighbors[1].tri.id; }
        }

        /// <summary>
        /// Gets the third neighbors id.
        /// </summary>
        public int N2
        {
            get { return this.neighbors[2].tri.id; }
        }

        /// <summary>
        /// Gets a triangles' neighbor.
        /// </summary>
        /// <param name="index">The neighbor index (0, 1 or 2).</param>
        /// <returns>The neigbbor opposite of vertex with given index.</returns>
        public ITriangle GetNeighbor(int index)
        {
            return neighbors[index].tri.hash == Mesh.DUMMY ? null : neighbors[index].tri;
        }

        /// <summary>
        /// Gets a triangles segment.
        /// </summary>
        /// <param name="index">The vertex index (0, 1 or 2).</param>
        /// <returns>The segment opposite of vertex with given index.</returns>
        public ISegment GetSegment(int index)
        {
            return subsegs[index].seg.hash == Mesh.DUMMY ? null : subsegs[index].seg;
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
