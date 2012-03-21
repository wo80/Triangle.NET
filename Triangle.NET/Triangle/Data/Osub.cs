// -----------------------------------------------------------------------
// <copyright file="Osub.cs">
// Original Triangle code by Jonathan Richard Shewchuk, http://www.cs.cmu.edu/~quake/triangle.html
// Triangle.NET code by Christian Woltering, http://home.edo.tu-dortmund.de/~woltering/index.html
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// An oriented subsegment.
    /// </summary>
    /// <remarks>
    /// Iincludes a pointer to a subsegment and an orientation. The orientation
    /// denotes a side of the edge.  Hence, there are two possible orientations.
    /// By convention, the edge is always directed so that the "side" denoted
    /// is the right side of the edge.
    /// </remarks>
    struct Osub
    {
        public Subseg ss;
        public int ssorient; // Ranges from 0 to 1.

        public override string ToString()
        {
            if (ss == null)
            {
                return "O-TID [null]";
            }
            return String.Format("O-SID {0}", ss.Hash);
        }

        #region Osub primitives

        /// <summary>
        /// Reverse the orientation of a subsegment. [sym(ab) -> ba]
        /// </summary>
        /// <remarks>ssym() toggles the orientation of a subsegment.
        /// </remarks>
        public void Sym(ref Osub o2)
        {
            o2.ss = ss;
            o2.ssorient = 1 - ssorient;
        }

        /// <summary>
        /// Reverse the orientation of a subsegment. [sym(ab) -> ba]
        /// </summary>
        public void SymSelf()
        {
            ssorient = 1 - ssorient;
        }

        /// <summary>
        /// Find adjoining subsegment with the same origin. [pivot(ab) -> a*]
        /// </summary>
        /// <remarks>spivot() finds the other subsegment (from the same segment) 
        /// that shares the same origin.
        /// </remarks>
        public void Pivot(ref Osub o2)
        {
            o2 = ss.subsegs[ssorient];
            //sdecode(sptr, o2);
        }

        /// <summary>
        /// Find adjoining subsegment with the same origin. [pivot(ab) -> a*]
        /// </summary>
        public void PivotSelf()
        {
            this = ss.subsegs[ssorient];
            //sdecode(sptr, osub);
        }

        /// <summary>
        /// Find next subsegment in sequence. [next(ab) -> b*]
        /// </summary>
        /// <remarks>snext() finds the next subsegment (from the same segment) in 
        /// sequence; one whose origin is the input subsegment's destination.
        /// </remarks>
        public void Next(ref Osub o2)
        {
            o2 = ss.subsegs[1 - ssorient];
            //sdecode(sptr, o2);
        }

        /// <summary>
        /// Find next subsegment in sequence. [next(ab) -> b*]
        /// </summary>
        public void NextSelf()
        {
            this = ss.subsegs[1 - ssorient];
            //sdecode(sptr, osub);
        }

        /// <summary>
        /// Get the origin of a subsegment
        /// </summary>
        public Vertex Org()
        {
            return ss.vertices[ssorient];
        }

        /// <summary>
        /// Get the destination of a subsegment
        /// </summary>
        public Vertex Dest()
        {
            return ss.vertices[1 - ssorient];
        }

        /// <summary>
        /// Set the origin or destination of a subsegment.
        /// </summary>
        public void SetOrg(Vertex ptr)
        {
            ss.vertices[ssorient] = ptr;
        }

        /// <summary>
        /// Set destination of a subsegment.
        /// </summary>
        public void SetDest(Vertex ptr)
        {
            ss.vertices[1 - ssorient] = ptr;
        }

        /// <summary>
        /// Get the origin of the segment that includes the subsegment.
        /// </summary>
        public Vertex SegOrg()
        {
            return ss.vertices[2 + ssorient];
        }

        /// <summary>
        /// Get the destination of the segment that includes the subsegment.
        /// </summary>
        public Vertex SegDest()
        {
            return ss.vertices[3 - ssorient];
        }

        /// <summary>
        /// Set the origin of the segment that includes the subsegment.
        /// </summary>
        public void SetSegOrg(Vertex ptr)
        {
            ss.vertices[2 + ssorient] = ptr;
        }

        /// <summary>
        /// Set the destination of the segment that includes the subsegment.
        /// </summary>
        public void SetSegDest(Vertex ptr)
        {
            ss.vertices[3 - ssorient] = ptr;
        }

        /// <summary>
        /// Read a boundary marker.
        /// </summary>
        /// <remarks>Boundary markers are used to hold user-defined tags for 
        /// setting boundary conditions in finite element solvers.</remarks>
        public int Mark()
        {
            return ss.boundary;
        }

        /// <summary>
        /// Set a boundary marker.
        /// </summary>
        public void SetMark(int value)
        {
            ss.boundary = value;
        }

        /// <summary>
        /// Bond two subsegments together. [bond(abc, ba)]
        /// </summary>
        public void Bond(ref Osub o2)
        {
            ss.subsegs[ssorient] = o2;
            o2.ss.subsegs[o2.ssorient] = this;
        }

        /// <summary>
        /// Dissolve a subsegment bond (from one side).
        /// </summary>
        /// <remarks>Note that the other subsegment will still think it's 
        /// connected to this subsegment.</remarks>
        public void Dissolve()
        {
            ss.subsegs[ssorient].ss = Mesh.dummysub;
        }

        /// <summary>
        /// Copy a subsegment.
        /// </summary>
        public void Copy(ref Osub o2)
        {
            o2.ss = ss;
            o2.ssorient = ssorient;
        }

        /// <summary>
        /// Test for equality of subsegments.
        /// </summary>
        public bool Equal(Osub o2)
        {
            return ((ss == o2.ss) && (ssorient == o2.ssorient));
        }

        /// <summary>
        /// Check a subsegment's deallocation.
        /// </summary>
        public static bool IsDead(Subseg sub)
        {
            return sub.subsegs[0].ss == null;
        }

        /// <summary>
        /// Set a subsegment's deallocation.
        /// </summary>
        public static void Kill(Subseg sub)
        {
            sub.subsegs[0].ss = null;
            sub.subsegs[1].ss = null;
        }

        /// <summary>
        /// Finds a triangle abutting a subsegment.
        /// </summary>
        public void TriPivot(ref Otri ot)
        {
            ot = ss.triangles[ssorient];
            //decode(ptr, otri)
        }

        /// <summary>
        /// Dissolve a bond (from the subsegment side).
        /// </summary>
        public void TriDissolve()
        {
            ss.triangles[ssorient].triangle = Mesh.dummytri;
        }

        #endregion
    }
}
