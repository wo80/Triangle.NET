// -----------------------------------------------------------------------
// <copyright file="Behavior.cs">
// Original Triangle code by Jonathan Richard Shewchuk, http://www.cs.cmu.edu/~quake/triangle.html
// Triangle.NET code by Christian Woltering, http://home.edo.tu-dortmund.de/~woltering/triangle/
// </copyright>
// -----------------------------------------------------------------------

using System;
namespace TriangleNet
{
    // TODO: Make Behavior non-static and an instance member of mesh class.

    /// <summary>
    /// Controls the behavior of the meshing software.
    /// </summary>
    static class Behavior
    {
        /// <summary>
        /// Input is a Planar Straight Line Graph.
        /// </summary>
        public static bool Poly { get; set; }
        /// <summary>
        /// Quality mesh generation.
        /// </summary>
        public static bool Quality { get; set; }
        /// <summary>
        /// Apply a maximum triangle area constraint.
        /// </summary>
        public static bool VarArea { get; set; }
        /// <summary>
        /// Apply a maximum triangle area constraint.
        /// </summary>
        public static bool FixedArea { get; set; }
        /// <summary>
        /// Apply a user-defined triangle constraint.
        /// </summary>
        public static bool Usertest { get; set; }
        /// <summary>
        /// Apply attributes to identify triangles in certain regions.
        /// </summary>
        public static bool RegionAttrib { get; set; }
        /// <summary>
        /// Enclose the convex hull with segments.
        /// </summary>
        public static bool Convex { get; set; }
        /// <summary>
        /// Jettison unused vertices from output.
        /// </summary>
        public static bool Jettison { get; set; }
        /// <summary>
        /// Compute boundary information.
        /// </summary>
        public static bool UseBoundaryMarkers { get; set; }
        /// <summary>
        /// Ignores holes in polygons.
        /// </summary>
        public static bool NoHoles { get; set; }
        /// <summary>
        /// No exact arithmetic.
        /// </summary>
        public static bool NoExact { get; set; }
        /// <summary>
        /// Conforming Delaunay (all triangles are truly Delaunay).
        /// </summary>
        public static bool ConformDel { get; set; }
        /// <summary>
        /// Algorithm to use for triangulation.
        /// </summary>
        public static TriangulationAlgorithm Algorithm { get; set; }
        /// <summary>
        /// Log detailed information.
        /// </summary>
        public static bool Verbose { get; set; }
        /// <summary>
        /// Use segments (should not be set manually)
        /// </summary>
        public static bool UseSegments { get; set; } // TODO: internal set

        /// <summary>
        /// Suppresses boundary segment splitting.
        /// </summary>
        public static int NoBisect { get; set; } // <- int !
        /// <summary>
        /// Use maximum number of added Steiner points.
        /// </summary>
        public static int Steiner { get; set; }

        /// <summary>
        /// Minimum angle constraint.
        /// </summary>
        public static double MinAngle { get; set; }
        /// <summary>
        /// (should not be set manually)
        /// </summary>
        public static double GoodAngle { get; set; }
        /// <summary>
        /// (should not be set manually)
        /// </summary>
        public static double Offconstant { get; set; }
        /// <summary>
        /// Maximum area constraint.
        /// </summary>
        public static double MaxArea { get; set; }
        /// <summary>
        /// Maximum angle constraint.
        /// </summary>
        public static double MaxAngle { get; set; }
        /// <summary>
        /// (should not be set manually)
        /// </summary>
        public static double MaxGoodAngle { get; set; }

        /// <summary>
        /// Load behavior defaults.
        /// </summary>
        public static void Init()
        {
            Poly = false;
            Quality = false;
            VarArea = false;
            FixedArea = false;
            Usertest = false;
            RegionAttrib = false;
            Convex = false;
            Jettison = false;
            UseBoundaryMarkers = true;
            NoHoles = false;
            NoExact = false;
            ConformDel = false;
            Algorithm = TriangulationAlgorithm.Dwyer;
            Verbose = true;
            UseSegments = true;

            NoBisect = 0;
            Steiner = -1;

            MinAngle = 0.0;
            GoodAngle = 0.0;
            MaxAngle = 0.0;
            MaxGoodAngle = 0.0;
            MaxArea = -1.0;
            Offconstant = 0.0;
        }
    }
}
