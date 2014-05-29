
namespace TriangleNet.Geometry
{
    public interface IEdge
    {
        /// <summary>
        /// Gets the first endpoints index.
        /// </summary>
        int P0 { get; }

        /// <summary>
        /// Gets the second endpoints index.
        /// </summary>
        int P1 { get; }

        /// <summary>
        /// Gets the segments boundary mark.
        /// </summary>
        int Boundary { get; }
    }
}
