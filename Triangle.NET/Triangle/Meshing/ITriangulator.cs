// -----------------------------------------------------------------------
// <copyright file="ITriangulator.cs" company="">
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Meshing
{
    using System.Collections.Generic;
    using TriangleNet.Geometry;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ITriangulator
    {
        Mesh Triangulate(ICollection<Vertex> points);
    }
}
