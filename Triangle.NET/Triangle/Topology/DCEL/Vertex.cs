// -----------------------------------------------------------------------
// <copyright file="Vertex.cs">
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Topology.DCEL
{
    public class Vertex : TriangleNet.Geometry.Point
    {
        internal HalfEdge leaving;

        /// <summary>
        /// Gets or sets a half-edge leaving the vertex.
        /// </summary>
        public HalfEdge Leaving
        {
            get { return leaving; }
            set { leaving = value; }
        }

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
            this.leaving = leaving;
        }
    }
}
