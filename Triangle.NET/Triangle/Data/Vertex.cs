// -----------------------------------------------------------------------
// <copyright file="Vertex.cs" company="">
// Original Triangle code by Jonathan Richard Shewchuk, http://www.cs.cmu.edu/~quake/triangle.html
// Triangle.NET code by Christian Woltering, http://home.edo.tu-dortmund.de/~woltering/triangle/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;


    /// <summary>
    /// The vertex data structure.
    /// </summary>
    /// <remarks>
    /// Each vertex is actually an array of doubles. An integer boundary marker,
    /// and sometimes a to a triangle, is appended after the doubles.
    /// </remarks>
    class Vertex : IComparable<Vertex>, IEquatable<Vertex>
    {
        private static int hashSeed = 0;
        internal int Hash;

        // The ID is only used for mesh output.
        internal int ID;

        internal Point2 pt;
        internal int mark;
        internal VertexType type;
        internal Otri tri;
        internal double[] attributes;

        public Vertex()
            : this(0)
        { }

        public Vertex(int numAttributes)
        {
            this.Hash = hashSeed++;
            
            pt = default(Point2);

            if (numAttributes > 0)
            {
                attributes = new double[numAttributes];
            }
        }

        public static bool operator ==(Vertex a, Vertex b)
        {
            // If vertex is null return false.
            if ((object)a == null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Vertex a, Vertex b)
        {
            // If vertex is null return false.
            if ((object)a == null)
            {
                return false;
            }

            return !a.Equals(b);
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
                    return pt.X;
                }

                if (i == 1)
                {
                    return pt.Y;
                }

                throw new ArgumentOutOfRangeException("Index must be 0 or 1.");
            }
        }

        /// <summary>
        /// Reset the hash seed.
        /// </summary>
        /// <param name="value">The new has seed value.</param>
        /// <remarks>Reset value will usally 0, if a new triangulation starts, 
        /// or the number of points, if refinement is done.</remarks>
        internal static void ResetHashSeed(int value)
        {
            if (value < 0)
            {
                throw new ArgumentException("A hash seed must be non negative.");
            }
            hashSeed = value;
        }

        public override int GetHashCode()
        {
            return this.Hash;
        }

        public override bool Equals(object obj)
        {
            Vertex v = obj as Vertex;

            if (v == null)
            {
                return false;
            }

            return this.Equals(v);
        }

        public bool Equals(Vertex v)
        {
            // If both are null, or both are same instance, return true.
            if (object.ReferenceEquals(this, v))
            {
                return true;
            }

            // If vertex is null return false.
            if ((object)v == null)
            {
                return false;
            }

            // Return true if the fields match:
            return this.pt.Equals(v.pt);
        }


        public override string ToString()
        {
            return String.Format("[{0},{1}]", pt.X, pt.Y);
        }

        public int CompareTo(Vertex other)
        {
            if (pt.X == other.pt.X && pt.Y == other.pt.Y)
            {
                return 0;
            }

            return (pt.X < other.pt.X || (pt.X == other.pt.X && pt.Y < other.pt.Y)) ? -1 : 1;
        }
    }
}
