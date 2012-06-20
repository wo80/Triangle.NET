// -----------------------------------------------------------------------
// <copyright file="Vertex.cs" company="">
// Original Triangle code by Jonathan Richard Shewchuk, http://www.cs.cmu.edu/~quake/triangle.html
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TriangleNet.Geometry;

    /// <summary>
    /// The vertex data structure.
    /// </summary>
    public class Vertex : Point
    {
        // Hash for dictionary. Will be set by mesh instance.
        internal int hash;

        // The ID is only used for mesh output.
        internal int id;

        internal VertexType type;
        internal Otri tri;

        public Vertex()
            : this(0, 0, 0)
        { }

        public Vertex(double x, double y)
            : this(x, y, 0)
        { }

        public Vertex(double x, double y, int mark)
            : base(x, y, mark)
        {
            this.type = VertexType.InputVertex;
        }

        #region Public properties

        /// <summary>
        /// Gets the vertex id.
        /// </summary>
        public int ID
        {
            get { return this.id; }
        }

        /// <summary>
        /// Gets the vertex type.
        /// </summary>
        public VertexType Type
        {
            get { return this.type; }
        }

        /// <summary>
        /// Gets the specified coordinate of the vertex.
        /// </summary>
        /// <param name="i">Coordinate index.</param>
        /// <returns>X coordinate, if index is 0, Y coordinate, if index is 1.</returns>
        public double this[int i]
        {
            get
            {
                if (i == 0)
                {
                    return x;
                }

                if (i == 1)
                {
                    return y;
                }

                throw new ArgumentOutOfRangeException("Index must be 0 or 1.");
            }
        }

        #endregion

        public override int GetHashCode()
        {
            return this.hash;
        }
    }
}
