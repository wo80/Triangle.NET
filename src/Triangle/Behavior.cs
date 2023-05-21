// -----------------------------------------------------------------------
// <copyright file="Behavior.cs">
// Triangle Copyright (c) 1993, 1995, 1997, 1998, 2002, 2005 Jonathan Richard Shewchuk
// Triangle.NET code by Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet
{
    using System;
    using TriangleNet.Geometry;

    /// <summary>
    /// Controls the behavior of the meshing software.
    /// </summary>
    class Behavior
    {
        private bool quality;

        private int noBisect;

        private double minAngle;
        private double maxAngle;
        private double maxArea = -1.0;

        internal bool fixedArea;
        internal bool useSegments = true;
        internal bool useRegions = false;
        internal double goodAngle;
        internal double maxGoodAngle;
        internal double offconstant;

        /// <summary>
        /// Creates an instance of the Behavior class.
        /// </summary>
        public Behavior(bool quality = false, double minAngle = 20.0)
        {
            if (quality)
            {
                this.quality = true;
                this.minAngle = minAngle;

                Update();
            }
        }

        /// <summary>
        /// Update quality options dependencies.
        /// </summary>
        private void Update()
        {
            quality = true;

            if (minAngle < 0 || minAngle > 60)
            {
                minAngle = 0;
                quality = false;

                Log.Instance.Warning("Invalid quality option (minimum angle).", "Mesh.Behavior");
            }

            if ((maxAngle != 0.0) && (maxAngle < 60 || maxAngle > 180))
            {
                maxAngle = 0;
                quality = false;

                Log.Instance.Warning("Invalid quality option (maximum angle).", "Mesh.Behavior");
            }

            useSegments = Poly || Quality || Convex;
            goodAngle = Math.Cos(MinAngle * Math.PI / 180.0);
            maxGoodAngle = Math.Cos(MaxAngle * Math.PI / 180.0);

            if (goodAngle == 1.0)
            {
                offconstant = 0.0;
            }
            else
            {
                offconstant = 0.475 * Math.Sqrt((1.0 + goodAngle) / (1.0 - goodAngle));
            }

            goodAngle *= goodAngle;
        }

        #region Static properties

        /// <summary>
        /// No exact arithmetic.
        /// </summary>
        public static bool NoExact { get; set; }

        #endregion

        #region Public properties

        /// <summary>
        /// Quality mesh generation.
        /// </summary>
        public bool Quality
        {
            get => quality;
            set
            {
                quality = value;
                if (quality)
                {
                    Update();
                }
            }
        }

        /// <summary>
        /// Minimum angle constraint.
        /// </summary>
        public double MinAngle
        {
            get => minAngle;
            set { minAngle = value; Update(); }
        }

        /// <summary>
        /// Maximum angle constraint.
        /// </summary>
        public double MaxAngle
        {
            get => maxAngle;
            set { maxAngle = value; Update(); }
        }

        /// <summary>
        /// Maximum area constraint.
        /// </summary>
        public double MaxArea
        {
            get => maxArea;
            set
            {
                maxArea = value;
                fixedArea = value > 0.0;
            }
        }

        /// <summary>
        /// Apply a maximum triangle area constraint.
        /// </summary>
        public bool VarArea { get; set; } = false;

        /// <summary>
        /// Input is a Planar Straight Line Graph.
        /// </summary>
        public bool Poly { get; set; }

        /// <summary>
        /// Apply a user-defined triangle constraint.
        /// </summary>
        public Func<ITriangle, double, bool> UserTest { get; set; }

        /// <summary>
        /// Exclude triangles from being refined.
        /// </summary>
        public Func<ITriangle, bool> Exclude { get; set; }

        /// <summary>
        /// Enclose the convex hull with segments.
        /// </summary>
        public bool Convex { get; set; } = false;

        /// <summary>
        /// Conforming Delaunay (all triangles are truly Delaunay).
        /// </summary>
        public bool ConformingDelaunay { get; set; } = false;

        /// <summary>
        /// Suppresses boundary segment splitting.
        /// </summary>
        /// <remarks>
        /// 0 = split segments
        /// 1 = no new vertices on the boundary
        /// 2 = prevent all segment splitting, including internal boundaries
        /// </remarks>
        public int NoBisect
        {
            get => noBisect;
            set
            {
                noBisect = value;
                if (noBisect < 0 || noBisect > 2)
                {
                    noBisect = 0;
                }
            }
        }

        /// <summary>
        /// Compute boundary information.
        /// </summary>
        public bool UseBoundaryMarkers { get; set; } = true;

        /// <summary>
        /// Ignores holes in polygons.
        /// </summary>
        public bool NoHoles { get; set; } = false;

        /// <summary>
        /// Jettison unused vertices from output.
        /// </summary>
        public bool Jettison { get; set; } = false;

        #endregion
    }
}
