// -----------------------------------------------------------------------
// <copyright file="Triangle.cs" company="">
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
    /// The triangle data structure.
    /// </summary>
    /// <remarks>
    /// Each triangle contains three pointers to
    /// adjoining triangles, plus three pointers to vertices, plus three
    /// pointers to subsegments (declared below; these pointers are usually
    /// 'dummysub'). It may or may not also contain user-defined attributes
    /// and/or a floating-point "area constraint." It may also contain extra
    /// pointers for nodes, when the user asks for high-order elements.
    /// Because the size and structure of a 'triangle' is not decided until
    /// runtime, I haven't simply declared the type 'triangle' as a struct.
    /// </remarks>
    class Triangle
    {
        // Start at -1, so dummytri has that ID
        private static int hashSeed = -1;
        internal int Hash;

        // The ID is only used for mesh output.
        internal int ID;

        internal Otri[] neighbors;
        internal Vertex[] vertices;
        internal Osub[] subsegs;
        internal double[] attributes;
        internal double area;
        internal bool infected;

        public Triangle(int numAttributes)
        {
            this.Hash = hashSeed++;
            this.ID = this.Hash;

            // Initialize the three adjoining triangles to be "outer space".
            neighbors = new Otri[3];
            neighbors[0].triangle = Mesh.dummytri;
            neighbors[1].triangle = Mesh.dummytri;
            neighbors[2].triangle = Mesh.dummytri;

            // Three NULL vertices.
            vertices = new Vertex[3];
            
            if (Behavior.UseSegments)
            {
                // Initialize the three adjoining subsegments to be the
                // omnipresent subsegment.
                subsegs = new Osub[3];
                subsegs[0].ss = Mesh.dummysub;
                subsegs[1].ss = Mesh.dummysub;
                subsegs[2].ss = Mesh.dummysub;
            }

            if (numAttributes > 0)
            {
                attributes = new double[numAttributes];
            }

            if (Behavior.VarArea)
            {
                area = -1.0;
            }
        }

        /// <summary>
        /// Reset the hash seed.
        /// </summary>
        /// <param name="value">The new has seed value.</param>
        /// <remarks>Reset value will usally 0, if a new triangulation starts, 
        /// or the number of triangles, if refinement is done.</remarks>
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
            return String.Format("TID {0}", Hash);
        }
    }
}
