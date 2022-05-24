
namespace TriangleNet.Voronoi
{
    using TriangleNet.Topology.DCEL;

    /// <summary>
    /// Factory for Voronoi DCEL datastructure.
    /// </summary>
    public interface IVoronoiFactory
    {
        /// <summary>
        /// Initialize object pool.
        /// </summary>
        /// <param name="vertexCount"></param>
        /// <param name="edgeCount"></param>
        /// <param name="faceCount"></param>
        void Initialize(int vertexCount, int edgeCount, int faceCount);

        /// <summary>
        /// Reset object pool.
        /// </summary>
        void Reset();

        /// <summary>
        /// Return a <see cref="Vertex" />.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        Vertex CreateVertex(double x, double y);

        /// <summary>
        /// Return a <see cref="HalfEdge" />.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="face"></param>
        /// <returns></returns>
        HalfEdge CreateHalfEdge(Vertex origin, Face face);

        /// <summary>
        /// Return a <see cref="Face" />.
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        Face CreateFace(Geometry.Vertex vertex);
    }
}
