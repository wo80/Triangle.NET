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
            : this(mesh, mesh.bounds)
        {
        }

        public StandardVoronoi(Mesh mesh, Rectangle box)
            : base(mesh, true)
        {
            // We explicitly told the base constructor to call the Generate method, so
            // at this point the basic Voronoi diagram is already created.
            PostProcess(box);
        }

        private void PostProcess(Rectangle box)
        {
            // Compute edge intersections with bounding box.
        }
    }
}
