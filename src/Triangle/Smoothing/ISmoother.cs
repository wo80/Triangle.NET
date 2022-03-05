
namespace TriangleNet.Smoothing
{
    using TriangleNet.Meshing;

    /// <summary>
    /// Interface for mesh smoothers.
    /// </summary>
    public interface ISmoother
    {
        /// <summary>
        /// Smooth mesh with 10 rounds of Voronoi iteration.
        /// </summary>
        /// <param name="mesh">The mesh.</param>
        void Smooth(IMesh mesh);

        /// <summary>
        /// Smooth mesh with 10 rounds of Voronoi iteration.
        /// </summary>
        /// <param name="mesh">The mesh.</param>
        /// <param name="limit">The number of iterations.</param>
        void Smooth(IMesh mesh, int limit);
    }
}