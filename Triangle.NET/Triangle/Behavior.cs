// -----------------------------------------------------------------------
// <copyright file="Behavior.cs">
// Original Triangle code by Jonathan Richard Shewchuk, http://www.cs.cmu.edu/~quake/triangle.html
// Triangle.NET code by Christian Woltering, http://home.edo.tu-dortmund.de/~woltering/triangle/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet
{
    using System;

    /// <summary>
    /// Controls the behavior of the meshing software.
    /// </summary>
    class Behavior
    {
        /// <summary>
        /// Load behavior defaults.
        /// </summary>
        public Behavior()
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
            ConformDel = false;
            Algorithm = TriangulationAlgorithm.Dwyer;
            UseSegments = true;

            NoBisect = 0;
            Steiner = -1;

            MinAngle = 0.0;
            GoodAngle = 0.0;
            MaxAngle = 0.0;
            MaxGoodAngle = 0.0;
            MaxArea = -1.0;
            Offconstant = 0.0;

            Verbose = true;
            NoExact = false;
        }

        #region Static properties

        /// <summary>
        /// No exact arithmetic.
        /// </summary>
        public static bool NoExact { get; set; }

        /// <summary>
        /// Log detailed information.
        /// </summary>
        public static bool Verbose { get; set; }

        #endregion

        #region Public properties

        /// <summary>
        /// Input is a Planar Straight Line Graph.
        /// </summary>
        public bool Poly { get; set; }
        /// <summary>
        /// Quality mesh generation.
        /// </summary>
        public bool Quality { get; set; }
        /// <summary>
        /// Apply a maximum triangle area constraint.
        /// </summary>
        public bool VarArea { get; set; }
        /// <summary>
        /// Apply a maximum triangle area constraint.
        /// </summary>
        public bool FixedArea { get; set; }
        /// <summary>
        /// Apply a user-defined triangle constraint.
        /// </summary>
        public bool Usertest { get; set; }
        /// <summary>
        /// Apply attributes to identify triangles in certain regions.
        /// </summary>
        public bool RegionAttrib { get; set; }
        /// <summary>
        /// Enclose the convex hull with segments.
        /// </summary>
        public bool Convex { get; set; }
        /// <summary>
        /// Jettison unused vertices from output.
        /// </summary>
        public bool Jettison { get; set; }
        /// <summary>
        /// Compute boundary information.
        /// </summary>
        public bool UseBoundaryMarkers { get; set; }
        /// <summary>
        /// Ignores holes in polygons.
        /// </summary>
        public bool NoHoles { get; set; }
        /// <summary>
        /// Conforming Delaunay (all triangles are truly Delaunay).
        /// </summary>
        public bool ConformDel { get; set; }
        /// <summary>
        /// Algorithm to use for triangulation.
        /// </summary>
        public TriangulationAlgorithm Algorithm { get; set; }
        /// <summary>
        /// Use segments (should not be set manually)
        /// </summary>
        public bool UseSegments { get; set; } // TODO: internal set

        /// <summary>
        /// Suppresses boundary segment splitting.
        /// </summary>
        public int NoBisect { get; set; } // <- int !
        /// <summary>
        /// Use maximum number of added Steiner points.
        /// </summary>
        public int Steiner { get; set; }

        /// <summary>
        /// Minimum angle constraint.
        /// </summary>
        public double MinAngle { get; set; }
        /// <summary>
        /// (should not be set manually)
        /// </summary>
        public double GoodAngle { get; set; }
        /// <summary>
        /// (should not be set manually)
        /// </summary>
        public double Offconstant { get; set; }
        /// <summary>
        /// Maximum area constraint.
        /// </summary>
        public double MaxArea { get; set; }
        /// <summary>
        /// Maximum angle constraint.
        /// </summary>
        public double MaxAngle { get; set; }
        /// <summary>
        /// (should not be set manually)
        /// </summary>
        public double MaxGoodAngle { get; set; }

        #endregion
    }
}
