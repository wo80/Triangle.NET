// -----------------------------------------------------------------------
// <copyright file="StandardVoronoi.cs">
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Voronoi
{
    using TriangleNet.Geometry;

    public class StandardVoronoi : VoronoiBase
    {
        public StandardVoronoi(Mesh mesh)
            : base(mesh, true)
        {
        }

        private void Intersect(Rectangle box)
        {
            // TODO: compute edge intersections with bounding box.
        }
    }
}
