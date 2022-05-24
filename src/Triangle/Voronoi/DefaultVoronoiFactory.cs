
namespace TriangleNet.Voronoi
{
    using System;
    using TriangleNet.Topology.DCEL;

    /// <summary>
    /// Default factory for Voronoi / DCEL mesh objects.
    /// </summary>
    public class DefaultVoronoiFactory : IVoronoiFactory
    {
        /// <inheritdoc />
        public void Initialize(int vertexCount, int edgeCount, int faceCount)
        {
        }

        /// <inheritdoc />
        public void Reset()
        {
        }

        /// <inheritdoc />
        public Vertex CreateVertex(double x, double y)
        {
            return new Vertex(x, y);
        }

        /// <inheritdoc />
        public HalfEdge CreateHalfEdge(Vertex origin, Face face)
        {
            return new HalfEdge(origin, face);
        }

        /// <inheritdoc />
        public Face CreateFace(Geometry.Vertex vertex)
        {
            return new Face(vertex);
        }
    }
}
