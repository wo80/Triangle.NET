
namespace TriangleNet.Meshing
{
    using System.Collections.Generic;
    using TriangleNet.Geometry;

    /// <summary>
    /// Interface for point set triangulation.
    /// </summary>
    public interface ITriangulator
    {
        /// <summary>
        /// Triangulates a point set.
        /// </summary>
        /// <param name="points">Collection of points.</param>
        /// <param name="config"></param>
        /// <returns>Mesh</returns>
        IMesh Triangulate(IList<Vertex> points, Configuration config);
    }
}
