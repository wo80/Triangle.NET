// -----------------------------------------------------------------------
// <copyright file="BoundedVoronoi.cs">
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Voronoi
{
    public class BoundedVoronoi : VoronoiBase
    {
        public BoundedVoronoi(Mesh mesh)
            : base(mesh, true)
        {
            // We explicitly told the base constructor to call the Generate method, so
            // at this point the basic Voronoi diagram is already created.
            PostProcess();
        }

        private void PostProcess()
        {
            // Compute edge intersections with mesh boundary edges.
        }
    }
}
