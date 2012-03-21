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
        public static bool Poly { get; set; }
        public static bool Quality { get; set; }
        public static bool VarArea { get; set; }
        public static bool FixedArea { get; set; }
        public static bool Usertest { get; set; }
        public static bool RegionAttrib { get; set; }
        public static bool Convex { get; set; }
        public static bool Jettison { get; set; }
        public static bool UseBoundaryMarkers { get; set; }
        public static bool NoHoles { get; set; }
        public static bool NoExact { get; set; }
        public static bool ConformDel { get; set; }
        public static TriangulationAlgorithm Algorithm { get; set; }
        public static bool Verbose { get; set; }
        public static bool UseSegments { get; set; }

        public static int NoBisect { get; set; } // <- int
        public static int Steiner { get; set; }

        public static double MinAngle { get; set; }
        public static double GoodAngle { get; set; }
        public static double Offconstant { get; set; }
        public static double MaxArea { get; set; }
        public static double MaxAngle { get; set; }
        public static double MaxGoodAngle { get; set; }

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
            UseBoundaryMarkers = false;
            NoHoles = false;
            NoExact = false;
            ConformDel = false;
            Algorithm = TriangulationAlgorithm.Dwyer;
            Verbose = false;
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
