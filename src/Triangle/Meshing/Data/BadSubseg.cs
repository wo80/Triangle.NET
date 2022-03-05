// -----------------------------------------------------------------------
// <copyright file="BadSubseg.cs" company="">
// Triangle Copyright (c) 1993, 1995, 1997, 1998, 2002, 2005 Jonathan Richard Shewchuk
// Triangle.NET code by Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Meshing.Data
{
    using System;
    using TriangleNet.Geometry;
    using TriangleNet.Topology;

    /// <summary>
    /// A queue used to store encroached subsegments.
    /// </summary>
    /// <remarks>
    /// Each subsegment's vertices are stored so that we can check whether a 
    /// subsegment is still the same.
    /// </remarks>
    class BadSubseg
    {
        public Osub subseg; // An encroached subsegment.
        public Vertex org, dest; // Its two vertices.

        public override int GetHashCode()
        {
            return subseg.seg.hash;
        }

        public override string ToString()
        {
            return String.Format("B-SID {0}", subseg.seg.hash);
        }
    }
}
