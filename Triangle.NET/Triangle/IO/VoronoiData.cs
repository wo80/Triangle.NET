// -----------------------------------------------------------------------
// <copyright file="TriangulateIO.cs">
// Original Triangle code by Jonathan Richard Shewchuk, http://www.cs.cmu.edu/~quake/triangle.html
// Triangle.NET code by Christian Woltering, http://home.edo.tu-dortmund.de/~woltering/index.html
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.IO
{
    using TriangleNet.Geometry;

    /// <summary>
    /// Stores the voronoi data (output only).
    /// </summary>
    public class VoronoiData
    {
        public Point[] InputPoints;
        public Point[] Points;
        public Edge[] Edges;

        // Stores the direction for infinite voronoi edges
        public double[][] Directions;
    }
}