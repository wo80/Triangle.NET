// -----------------------------------------------------------------------
// <copyright file="Subseg.cs" company="">
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
    /// The subsegment data structure.
    /// </summary>
    /// <remarks>
    /// Each subsegment contains two pointers to adjoining subsegments, plus
    /// four pointers to vertices, plus two pointers to adjoining triangles,
    /// plus one boundary marker, plus one segment number.
    /// </remarks>
    class Subseg
    {
        // Start at -1, so dummysub has that ID
        private static int hashSeed = -1;
        internal int Hash;

        // The ID is only used for mesh output.
        //public int ID;

        public Osub[] subsegs;
        public Vertex[] vertices;
        public Otri[] triangles;
        public int boundary;
        //public int segment;

        public Subseg()
        {
            Hash = hashSeed++;

            // Initialize the two adjoining subsegments to be the omnipresent
            //   subsegment.
            subsegs = new Osub[2];
            subsegs[0].ss = Mesh.dummysub;
            subsegs[1].ss = Mesh.dummysub;

            // Four NULL vertices.
            vertices = new Vertex[4];

            // Initialize the two adjoining triangles to be "outer space."
            triangles = new Otri[2];
            triangles[0].triangle = Mesh.dummytri;
            triangles[1].triangle = Mesh.dummytri;

            // Set the boundary marker to zero.
            boundary = 0;
        }

        /// <summary>
        /// Reset the hash seed.
        /// </summary>
        /// <param name="value">The new has seed value.</param>
        /// <remarks>Reset value will usally 0, if a new triangulation starts, 
        /// or the number of subsegments, if refinement is done.</remarks>
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

        public override string ToString()
        {
            return String.Format("SID {0}", Hash);
        }
    }
}
