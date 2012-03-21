// -----------------------------------------------------------------------
// <copyright file="TriangulateIO.cs">
// Original Triangle code by Jonathan Richard Shewchuk, http://www.cs.cmu.edu/~quake/triangle.html
// Triangle.NET code by Christian Woltering, http://home.edo.tu-dortmund.de/~woltering/index.html
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.IO
{
    /// <summary>
    /// Stores the mesh data in- and output.
    /// </summary>
    public class VoronoiData
    {
        public double[][] PointList;               // In / out
        //public int NumberOfPoints;               // In / out

        public int[][] EdgeList;         // Out only
        //public int NumberOfEdges;      // Out only

        // Stores the direction for infinite voronoi edges
        public double[][] NormList;
    }
}
