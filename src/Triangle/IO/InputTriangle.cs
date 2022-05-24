// -----------------------------------------------------------------------
// <copyright file="InputTriangle.cs" company="">
// Triangle.NET Copyright (c) 2012-2022 Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.IO
{
    using System;
    using TriangleNet.Geometry;

    /// <summary>
    /// Simple triangle class for input.
    /// </summary>
    public class InputTriangle : ITriangle
    {
        internal int[] vertices;
        internal int label;
        internal double area;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputTriangle" /> class.
        /// </summary>
        public InputTriangle(int p0, int p1, int p2)
        {
            vertices = new int[] { p0, p1, p2 };
        }

        #region Public properties

        /// <inheritdoc/>
        public int ID
        {
            get { return 0; }
            set { }
        }

        /// <inheritdoc/>
        public int Label
        {
            get { return label; }
            set { label = value; }
        }

        /// <inheritdoc/>
        public double Area
        {
            get { return area; }
            set { area = value; }
        }

        /// <summary>
        /// WARNING: not implemented.
        /// </summary>
        public Vertex GetVertex(int index)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetVertexID(int index)
        {
            return vertices[index];
        }

        /// <summary>
        /// WARNING: not implemented.
        /// </summary>
        public ITriangle GetNeighbor(int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// WARNING: not implemented.
        /// </summary>
        public int GetNeighborID(int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// WARNING: not implemented.
        /// </summary>
        public ISegment GetSegment(int index)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
