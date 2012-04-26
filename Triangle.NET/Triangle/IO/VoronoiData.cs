// -----------------------------------------------------------------------
// <copyright file="TriangulateIO.cs">
// Original Triangle code by Jonathan Richard Shewchuk, http://www.cs.cmu.edu/~quake/triangle.html
// Triangle.NET code by Christian Woltering, http://home.edo.tu-dortmund.de/~woltering/index.html
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.IO
{
    /// <summary>
    /// Stores the voronoi data (output only).
    /// </summary>
    public class VoronoiData
    {
        public double[][] InputPoints;
        public double[][] Points;
        public int[][] Edges;

        // Stores the direction for infinite voronoi edges
        public double[][] Directions;
    }
}
