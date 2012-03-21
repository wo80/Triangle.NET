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
    public class MeshData
    {
        public double[][] Points;             // In / out
        public double[][] PointAttributes;    // In / out
        public int[] PointMarkers;            // In / out

        public int[][] Triangles;             // In / out
        public double[][] TriangleAttributes; // In / out
        public double[] TriangleAreas;        // In only
        public int[][] Neighbors;             // Out only

        public int[][] Segments;              // In / out
        public int[] SegmentMarkers;          // In / out

        public double[][] Holes;      // In / pointer to array copied out
        public double[][] Regions;    // In / pointer to array copied out

        public int[][] Edges;         // Out only
        public int[] EdgeMarkers;     // Out only
    }
}
