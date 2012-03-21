// -----------------------------------------------------------------------
// <copyright file="Statistic.cs">
// Original Triangle code by Jonathan Richard Shewchuk, http://www.cs.cmu.edu/~quake/triangle.html
// Triangle.NET code by Christian Woltering, http://home.edo.tu-dortmund.de/~woltering/triangle/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet
{
    using System;
    using System.Text;
    using TriangleNet.Data;

    /// <summary>
    /// Gather mesh statistics.
    /// </summary>
    public class Statistic
    {
        #region Static members

        /// <summary>
        /// Number of incircle tests performed.
        /// </summary>
        public static long InCircleCount = 0;

        /// <summary>
        /// Number of counterclockwise tests performed.
        /// </summary>
        public static long CounterClockwiseCount = 0;

        /// <summary>
        /// Number of 3D orientation tests performed.
        /// </summary>
        public static long Orient3dCount = 0;

        /// <summary>
        /// Number of right-of-hyperbola tests performed.
        /// </summary>
        public static long HyperbolaCount = 0;

        /// <summary>
        /// // Number of circumcenter calculations performed.
        /// </summary>
        public static long CircumcenterCount = 0;

        /// <summary>
        /// Number of circle top calculations performed.
        /// </summary>
        public static long CircleTopCount = 0;

        /// <summary>
        /// Number of vertex relocation.
        /// </summary>
        public static long RelocationCount = 0;

        #endregion

        #region Properties

        double minEdge = 0;
        /// <summary>
        /// Shortest edge
        /// </summary>
        public double ShortestEdge { get { return minEdge; } }

        double maxEdge = 0;
        /// <summary>
        /// Longest edge
        /// </summary>
        public double LongestEdge { get { return maxEdge; } }

        //
        double minAspect = 0;
        /// <summary>
        /// Shortest altitude  
        /// </summary>
        public double ShortestAltitude  { get { return minAspect; } }

        double maxAspect = 0;
        /// <summary>
        /// Largest aspect ratio
        /// </summary>
        public double LargestAspectRatio { get { return maxAspect; } }

        double minArea = 0;
        /// <summary>
        /// Smallest area
        /// </summary>
        public double SmallestArea { get { return minArea; } }

        double maxArea = 0;
        /// <summary>
        /// Largest area
        /// </summary>
        public double LargestArea { get { return maxArea; } }

        double minAngle = 0;
        /// <summary>
        /// Smallest angle
        /// </summary>
        public double SmallestAngle { get { return minAngle; } }

        double maxAngle = 0;
        /// <summary>
        /// Largest angle
        /// </summary>
        public double LargestAngle { get { return maxAngle; } }

        int inVetrices = 0;
        /// <summary>
        /// Input vertices
        /// </summary>
        public int InputVertices { get { return inVetrices; } }

        int inTriangles = 0;
        /// <summary>
        /// Input triangles
        /// </summary>
        public int InputTriangles { get { return inTriangles; } }

        int inSegments = 0;
        /// <summary>
        /// Input segments
        /// </summary>
        public int InputSegments { get { return inSegments; } }

        int inHoles = 0;
        /// <summary>
        /// Input holes
        /// </summary>
        public int InputHoles { get { return inHoles; } }

        int outVertices = 0;
        /// <summary>
        /// Mesh vertices
        /// </summary>
        public int Vertices { get { return outVertices; } }

        int outTriangles = 0;
        /// <summary>
        /// Mesh triangles
        /// </summary>
        public int Triangles { get { return outTriangles; } }

        int outEdges = 0;
        /// <summary>
        /// Mesh edges
        /// </summary>
        public int Edges { get { return outEdges; } }

        int boundaryEdges = 0;
        /// <summary>
        /// Exterior boundary edges
        /// </summary>
        public int BoundaryEdges { get { return boundaryEdges; } }

        int intBoundaryEdges = 0;
        /// <summary>
        /// Interior boundary edges
        /// </summary>
        public int InteriorBoundaryEdges { get { return intBoundaryEdges; } }

        int constrainedEdges = 0;
        /// <summary>
        /// Constrained edges
        /// </summary>
        public int ConstrainedEdges { get { return constrainedEdges; } }

        int[] angleTable;
        /// <summary>
        /// Angle histogram
        /// </summary>
        public int[] AngleHistogram { get { return angleTable; } }

        #endregion

        /// <summary>
        /// detailedHistogram()
        /// </summary>
        /// <param name="m"></param>
        void detailedHistogram(Mesh m)
        {
            Vertex[] p = new Vertex[3];
            double[] cosSquareTable = new double[8];
            double[] dx = new double[3], dy = new double[3];
            double[] edgelength = new double[3];
            double dotproduct;
            double cosSquare;

            int i, ii, j, k;
            double[] cossquaretableHist = new double[89];
            double radconstHist;
            int onedegree;
            int[] angletableHist = new int[180];

            radconstHist = Math.PI / 180.0;
            for (i = 0; i < 89; i++)
            {
                cossquaretableHist[i] = Math.Cos(radconstHist * (i + 1));
                cossquaretableHist[i] = cossquaretableHist[i] * cossquaretableHist[i];
            }
            for (i = 0; i < 180; i++)
            {
                angletableHist[i] = 0;
            }

            foreach (var tri in m.triangles.Values)
            {
                p[0] = tri.vertices[0];
                p[1] = tri.vertices[1];
                p[2] = tri.vertices[2];

                for (i = 0; i < 3; i++)
                {
                    j = plus1Mod3[i];
                    k = minus1Mod3[i];
                    dx[i] = p[j][0] - p[k][0];
                    dy[i] = p[j][1] - p[k][1];
                    edgelength[i] = dx[i] * dx[i] + dy[i] * dy[i];
                }
                for (i = 0; i < 3; i++)
                {
                    j = plus1Mod3[i];
                    k = minus1Mod3[i];
                    dotproduct = dx[j] * dx[k] + dy[j] * dy[k];
                    cosSquare = dotproduct * dotproduct / (edgelength[j] * edgelength[k]);
                    onedegree = 89;
                    for (ii = 88; ii >= 0; ii--)
                    {
                        if (cosSquare > cossquaretableHist[ii])
                        {
                            onedegree = ii;
                        }
                    }
                    if (dotproduct <= 0.0)
                    {
                        angletableHist[onedegree]++;
                    }
                    else
                    {
                        angletableHist[179 - onedegree]++;
                    }
                }
            }
        }

        static readonly int[] plus1Mod3 = { 1, 2, 0 };
        static readonly int[] minus1Mod3 = { 2, 0, 1 };

        /// <summary>
        /// Update statistics about the quality of the mesh.
        /// </summary>
        /// <param name="mesh"></param>
        public void Update(Mesh mesh)
        {
            inVetrices = mesh.invertices;
            inTriangles = mesh.inelements;
            inSegments = mesh.insegments;
            inHoles = mesh.holes.Count;
            outVertices = mesh.vertices.Count - mesh.undeads;
            outTriangles = mesh.triangles.Count;
            outEdges = (int)mesh.edges;
            boundaryEdges = (int)mesh.hullsize;
            intBoundaryEdges = mesh.subsegs.Count - (int)mesh.hullsize;
            constrainedEdges = mesh.subsegs.Count;

            Point2[] p = new Point2[3];

            int k1, k2;
            int tendegree;

            double[] cosSquareTable = new double[8];
            double[] dx = new double[3];
            double[] dy = new double[3];
            double[] edgelength = new double[3];
            double dotproduct;
            double cossquare;
            double triarea;
            double trilongest2;
            double triminaltitude2;
            double triaspect2;

            double radconst = Math.PI / 18.0;
            double degconst = 180.0 / Math.PI;

            // New angle table
            angleTable = new int[18];

            for (int i = 0; i < 8; i++)
            {
                cosSquareTable[i] = Math.Cos(radconst * (i + 1));
                cosSquareTable[i] = cosSquareTable[i] * cosSquareTable[i];
            }
            for (int i = 0; i < 18; i++)
            {
                angleTable[i] = 0;
            }

            minAspect = mesh.xmax - mesh.xmin + mesh.ymax - mesh.ymin;
            minAspect = minAspect * minAspect;
            maxAspect = 0.0;
            minEdge = minAspect;
            maxEdge = 0.0;
            minArea = minAspect;
            maxArea = 0.0;
            minAngle = 0.0;
            maxAngle = 2.0;

            bool acuteBiggest = true;

            foreach (var tri in mesh.triangles.Values)
            {
                p[0] = tri.vertices[0].pt;
                p[1] = tri.vertices[1].pt;
                p[2] = tri.vertices[2].pt;

                trilongest2 = 0.0;

                for (int i = 0; i < 3; i++)
                {
                    k1 = plus1Mod3[i];
                    k2 = minus1Mod3[i];

                    dx[i] = p[k1].X - p[k2].X;
                    dy[i] = p[k1].Y - p[k2].Y;

                    edgelength[i] = dx[i] * dx[i] + dy[i] * dy[i];

                    if (edgelength[i] > trilongest2)
                    {
                        trilongest2 = edgelength[i];
                    }

                    if (edgelength[i] > maxEdge)
                    {
                        maxEdge = edgelength[i];
                    }

                    if (edgelength[i] < minEdge)
                    {
                        minEdge = edgelength[i];
                    }
                }

                //triarea = Primitives.CounterClockwise(p[0], p[1], p[2]);
                triarea = Math.Abs((p[2].X - p[0].X) * (p[1].Y - p[0].Y) -
                    (p[1].X - p[0].X) * (p[2].Y - p[0].Y));
                if (triarea < minArea)
                {
                    minArea = triarea;
                }
                if (triarea > maxArea)
                {
                    maxArea = triarea;
                }
                triminaltitude2 = triarea * triarea / trilongest2;
                if (triminaltitude2 < minAspect)
                {
                    minAspect = triminaltitude2;
                }
                triaspect2 = trilongest2 / triminaltitude2;
                if (triaspect2 > maxAspect)
                {
                    maxAspect = triaspect2;
                }

                for (int i = 0; i < 3; i++)
                {
                    k1 = plus1Mod3[i];
                    k2 = minus1Mod3[i];

                    dotproduct = dx[k1] * dx[k2] + dy[k1] * dy[k2];
                    cossquare = dotproduct * dotproduct / (edgelength[k1] * edgelength[k2]);
                    tendegree = 8;

                    for (int j = 7; j >= 0; j--)
                    {
                        if (cossquare > cosSquareTable[j])
                        {
                            tendegree = j;
                        }
                    }
                    if (dotproduct <= 0.0)
                    {
                        angleTable[tendegree]++;
                        if (cossquare > minAngle)
                        {
                            minAngle = cossquare;
                        }
                        if (acuteBiggest && (cossquare < maxAngle))
                        {
                            maxAngle = cossquare;
                        }
                    }
                    else
                    {
                        angleTable[17 - tendegree]++;
                        if (acuteBiggest || (cossquare > maxAngle))
                        {
                            maxAngle = cossquare;
                            acuteBiggest = false;
                        }
                    }
                }
            }

            minEdge = Math.Sqrt(minEdge);
            maxEdge = Math.Sqrt(maxEdge);
            minAspect = Math.Sqrt(minAspect);
            maxAspect = Math.Sqrt(maxAspect);
            minArea *= 0.5;
            maxArea *= 0.5;
            if (minAngle >= 1.0)
            {
                minAngle = 0.0;
            }
            else
            {
                minAngle = degconst * Math.Acos(Math.Sqrt(minAngle));
            }
            if (maxAngle >= 1.0)
            {
                maxAngle = 180.0;
            }
            else
            {
                if (acuteBiggest)
                {
                    maxAngle = degconst * Math.Acos(Math.Sqrt(maxAngle));
                }
                else
                {
                    maxAngle = 180.0 - degconst * Math.Acos(Math.Sqrt(maxAngle));
                }
            }
        }

        /* Original code with aspect ratio

        int[] aspecttable;
        double[] ratiotable;

        public Statistic()
        {
            angletable = new int[18];
            aspecttable = new int[16];
            ratiotable = new double[] { 
                1.5, 2.0, 2.5, 3.0, 4.0, 6.0, 10.0, 15.0, 25.0, 50.0, 
                100.0, 300.0, 1000.0, 10000.0, 100000.0, 0.0 };
        }

        public void Update(Mesh mesh)
        {
            inVetrices = mesh.invertices;
            inTriangles = mesh.inelements;
            inSegments = mesh.insegments;
            inHoles = mesh.holes;
            outVertices = mesh.vertices.Count - mesh.undeads;
            outTriangles = mesh.triangles.Count;
            outEdges = (int)mesh.edges;
            boundaryEdges = (int)mesh.hullsize;
            intBoundaryEdges = mesh.subsegs.Count - (int)mesh.hullsize;
            constrainedEdges = mesh.subsegs.Count;

            Otri triangleloop = default(Otri);
            Vertex[] p = new Vertex[3];
            double[] cossquaretable = new double[8];
            double[] dx = new double[3], dy = new double[3];
            double[] edgelength = new double[3];
            double dotproduct;
            double cossquare;
            double triarea;
            double trilongest2;
            double triminaltitude2;
            double triaspect2;
            double radconst, degconst;

            int aspectindex;
            int tendegree;
            bool acutebiggest;
            int i, ii, j, k;

            radconst = Math.PI / 18.0;
            degconst = 180.0 / Math.PI;
            for (i = 0; i < 8; i++)
            {
                cossquaretable[i] = Math.Cos(radconst * (double)(i + 1));
                cossquaretable[i] = cossquaretable[i] * cossquaretable[i];
            }
            for (i = 0; i < 18; i++)
            {
                angletable[i] = 0;
            }

            for (i = 0; i < 16; i++)
            {
                aspecttable[i] = 0;
            }

            worstaspect = 0.0;
            minaltitude = mesh.xmax - mesh.xmin + mesh.ymax - mesh.ymin;
            minaltitude = minaltitude * minaltitude;
            shortest = minaltitude;
            longest = 0.0;
            smallestarea = minaltitude;
            biggestarea = 0.0;
            worstaspect = 0.0;
            smallestangle = 0.0;
            biggestangle = 2.0;
            acutebiggest = true;

            triangleloop.orient = 0;
            foreach (var t in mesh.triangles)
            {
                triangleloop.triangle = t;
                p[0] = triangleloop.Org();
                p[1] = triangleloop.Dest();
                p[2] = triangleloop.Apex();
                trilongest2 = 0.0;

                for (i = 0; i < 3; i++)
                {
                    j = plus1Mod3[i];
                    k = minus1Mod3[i];
                    dx[i] = p[j].pt.X - p[k].pt.X;
                    dy[i] = p[j].pt.Y - p[k].pt.Y;
                    edgelength[i] = dx[i] * dx[i] + dy[i] * dy[i];
                    if (edgelength[i] > trilongest2)
                    {
                        trilongest2 = edgelength[i];
                    }
                    if (edgelength[i] > longest)
                    {
                        longest = edgelength[i];
                    }
                    if (edgelength[i] < shortest)
                    {
                        shortest = edgelength[i];
                    }
                }

                //triarea = Primitives.CounterClockwise(p[0], p[1], p[2]);
                triarea = Math.Abs((p[2].pt.X - p[0].pt.X) * (p[1].pt.Y - p[0].pt.Y) -
                    (p[1].pt.X - p[0].pt.X) * (p[2].pt.Y - p[0].pt.Y)) / 2.0;
                if (triarea < smallestarea)
                {
                    smallestarea = triarea;
                }
                if (triarea > biggestarea)
                {
                    biggestarea = triarea;
                }
                triminaltitude2 = triarea * triarea / trilongest2;
                if (triminaltitude2 < minaltitude)
                {
                    minaltitude = triminaltitude2;
                }
                triaspect2 = trilongest2 / triminaltitude2;
                if (triaspect2 > worstaspect)
                {
                    worstaspect = triaspect2;
                }
                aspectindex = 0;
                while ((triaspect2 > ratiotable[aspectindex] * ratiotable[aspectindex]) && (aspectindex < 15))
                {
                    aspectindex++;
                }
                aspecttable[aspectindex]++;

                for (i = 0; i < 3; i++)
                {
                    j = plus1Mod3[i];
                    k = minus1Mod3[i];
                    dotproduct = dx[j] * dx[k] + dy[j] * dy[k];
                    cossquare = dotproduct * dotproduct / (edgelength[j] * edgelength[k]);
                    tendegree = 8;
                    for (ii = 7; ii >= 0; ii--)
                    {
                        if (cossquare > cossquaretable[ii])
                        {
                            tendegree = ii;
                        }
                    }
                    if (dotproduct <= 0.0)
                    {
                        angletable[tendegree]++;
                        if (cossquare > smallestangle)
                        {
                            smallestangle = cossquare;
                        }
                        if (acutebiggest && (cossquare < biggestangle))
                        {
                            biggestangle = cossquare;
                        }
                    }
                    else
                    {
                        angletable[17 - tendegree]++;
                        if (acutebiggest || (cossquare > biggestangle))
                        {
                            biggestangle = cossquare;
                            acutebiggest = false;
                        }
                    }
                }
            }

            shortest = Math.Sqrt(shortest);
            longest = Math.Sqrt(longest);
            minaltitude = Math.Sqrt(minaltitude);
            worstaspect = Math.Sqrt(worstaspect);
            smallestarea *= 0.5;
            biggestarea *= 0.5;
            if (smallestangle >= 1.0)
            {
                smallestangle = 0.0;
            }
            else
            {
                smallestangle = degconst * Math.Acos(Math.Sqrt(smallestangle));
            }
            if (biggestangle >= 1.0)
            {
                biggestangle = 180.0;
            }
            else
            {
                if (acutebiggest)
                {
                    biggestangle = degconst * Math.Acos(Math.Sqrt(biggestangle));
                }
                else
                {
                    biggestangle = 180.0 - degconst * Math.Acos(Math.Sqrt(biggestangle));
                }
            }
        }

         */
    }
}
