﻿// -----------------------------------------------------------------------
// <copyright file="BadTriangle.cs" company="">
// Triangle Copyright (c) 1993, 1995, 1997, 1998, 2002, 2005 Jonathan Richard Shewchuk
// Triangle.NET code by Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Meshing.Data
{
    using System;
    using Geometry;
    using Topology;

    /// <summary>
    /// A queue used to store bad triangles.
    /// </summary>
    /// <remarks>
    /// The key is the square of the cosine of the smallest angle of the triangle.
    /// Each triangle's vertices are stored so that one can check whether a
    /// triangle is still the same.
    /// </remarks>
    internal class BadTriangle
    {
        public Otri poortri; // A skinny or too-large triangle.
        public double key; // cos^2 of smallest (apical) angle.
        public Vertex org, dest, apex; // Its three vertices.
        public BadTriangle next; // Pointer to next bad triangle.

        public override string ToString()
        {
            return String.Format("B-TID {0}", poortri.tri.hash);
        }
    }
}
