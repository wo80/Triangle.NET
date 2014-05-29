
namespace TriangleNet.Meshing
{
    public class ConstraintOptions
    {
        public static ConstraintOptions Empty
        {
            get { return new ConstraintOptions(); }
        }

        #region Public properties

        /// <summary>
        /// Gets or sets a value indicating wether to use regions.
        /// </summary>
        public bool UseRegions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating wether to create a Conforming
        /// Delaunay triangulation.
        /// </summary>
        public bool ConformingDelaunay { get; set; }

        /// <summary>
        /// Enclose the convex hull with segments.
        /// </summary>
        public bool Convex { get; set; }

        /// <summary>
        /// Suppresses boundary segment splitting.
        /// </summary>
        /// <remarks>
        /// 0 = split segments (default)
        /// 1 = no new vertices on the boundary
        /// 2 = prevent all segment splitting, including internal boundaries
        /// </remarks>
        public int SegmentSplitting { get; set; }

        #endregion
    }
}
