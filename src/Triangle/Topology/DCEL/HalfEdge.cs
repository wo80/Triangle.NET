// -----------------------------------------------------------------------
// <copyright file="HalfEdge.cs">
// Triangle.NET Copyright (c) 2012-2022 Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Topology.DCEL
{
    /// <summary>
    /// A half-edge of the DCEL datastructure.
    /// </summary>
    public class HalfEdge
    {
        internal int id;
        internal int mark;

        internal Vertex? origin;
        internal Face? face;
        internal HalfEdge? twin;
        internal HalfEdge? next;

        /// <summary>
        /// Gets or sets the half-edge id.
        /// </summary>
        public int ID
        {
            get => id;
            set => id = value;
        }

        /// <summary>
        /// Gets or sets a boundary marker.
        /// </summary>
        public int Boundary
        {
            get => mark;
            set => mark = value;
        }

        /// <summary>
        /// Gets or sets the origin of the half-edge.
        /// </summary>
        public Vertex? Origin
        {
            get => origin;
            set => origin = value;
        }

        /// <summary>
        /// Gets or sets the face connected to the half-edge.
        /// </summary>
        public Face? Face
        {
            get => face;
            set => face = value;
        }

        /// <summary>
        /// Gets or sets the twin of the half-edge.
        /// </summary>
        public HalfEdge? Twin
        {
            get => twin;
            set => twin = value;
        }

        /// <summary>
        /// Gets or sets the next pointer of the half-edge.
        /// </summary>
        public HalfEdge? Next
        {
            get => next;
            set => next = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HalfEdge" /> class.
        /// </summary>
        /// <param name="origin">The origin of this half-edge.</param>
        public HalfEdge(Vertex origin)
        {
            this.origin = origin;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HalfEdge" /> class.
        /// </summary>
        /// <param name="origin">The origin of this half-edge.</param>
        /// <param name="face">The face connected to this half-edge.</param>
        public HalfEdge(Vertex origin, Face? face)
        {
            this.origin = origin;
            this.face = face;

            // IMPORTANT: do not remove the (face.edge == null) check!
            if (face != null && face.edge == null)
            {
                face.edge = this;
            }
        }

        /// <inheritdoc />
        public override string ToString() => $"HE-ID {id} (Origin = VID-{origin?.id})";
    }
}
